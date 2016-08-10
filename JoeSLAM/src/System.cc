/**
* This file is part of ORB-SLAM2.
*
* Copyright (C) 2014-2016 Raúl Mur-Artal <raulmur at unizar dot es> (University of Zaragoza)
* For more information see <https://github.com/raulmur/ORB_SLAM2>
*
* ORB-SLAM2 is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* ORB-SLAM2 is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with ORB-SLAM2. If not, see <http://www.gnu.org/licenses/>.
*/



#include "System.h"
#include "Converter.h"
#include <thread>
#include <pangolin/pangolin.h>
#include <iomanip>

namespace ORB_SLAM2
{

System::System(const string &strVocFile, const string &strSettingsFile, const eSensor sensor,
               const bool bUseViewer, const string &dataDir):mSensor(sensor),mbReset(false),mbActivateLocalizationMode(false),
        mbDeactivateLocalizationMode(false)
{
    // Output welcome message
    cout << endl <<
    "==================================================" << endl <<
    "================ Reconstruct-SLAM ================" << endl <<
    "==================================================" << endl << endl;

    cout << "Input sensor was set to: ";

    if(mSensor==MONOCULAR)
        cout << "Monocular" << endl;
    else if(mSensor==STEREO)
        cout << "Stereo" << endl;
    else if(mSensor==RGBD)
        cout << "RGB-D" << endl;

    //Check settings file
    cv::FileStorage fsSettings(strSettingsFile.c_str(), cv::FileStorage::READ);
    if(!fsSettings.isOpened())
    {
       cerr << "Failed to open settings file at: " << strSettingsFile << endl;
       exit(-1);
    }


    //Load ORB Vocabulary
    cout << endl << "Loading ORB Vocabulary. This could take a while..." << endl;

    mpVocabulary = new ORBVocabulary();
    bool bVocLoad = mpVocabulary->loadFromTextFile(strVocFile);
    if(!bVocLoad)
    {
        cerr << "Wrong path to vocabulary. " << endl;
        cerr << "Falied to open at: " << strVocFile << endl;
        exit(-1);
    }
    cout << "Vocabulary loaded!" << endl << endl;

    //Create KeyFrame Database
    mpKeyFrameDatabase = new KeyFrameDatabase(*mpVocabulary);

    //Create the Map
    mpMap = new Map();

    //Create Drawers. These are used by the Viewer
    mpFrameDrawer = new FrameDrawer(mpMap);
    mpMapDrawer = new MapDrawer(mpMap, strSettingsFile);

    //Initialize the Tracking thread
    //(it will live in the main thread of execution, the one that called this constructor)
    mpTracker = new Tracking(this, mpVocabulary, mpFrameDrawer, mpMapDrawer,
                             mpMap, mpKeyFrameDatabase, strSettingsFile, mSensor);

    //Initialize the Local Mapping thread and launch
    mpLocalMapper = new LocalMapping(mpMap, mSensor==MONOCULAR);
    mptLocalMapping = new thread(&ORB_SLAM2::LocalMapping::Run,mpLocalMapper);

    //Initialize the Loop Closing thread and launch
    mpLoopCloser = new LoopClosing(mpMap, mpKeyFrameDatabase, mpVocabulary, mSensor!=MONOCULAR);
    mptLoopClosing = new thread(&ORB_SLAM2::LoopClosing::Run, mpLoopCloser);

    //Initialize the Viewer thread and launch
    mpViewer = new Viewer(this, mpFrameDrawer,mpMapDrawer,mpTracker,strSettingsFile);
    if(bUseViewer)
        mptViewer = new thread(&Viewer::Run, mpViewer);

    mpTracker->SetViewer(mpViewer);

    //Set pointers between threads
    mpTracker->SetLocalMapper(mpLocalMapper);
    mpTracker->SetLoopClosing(mpLoopCloser);

    mpLocalMapper->SetTracker(mpTracker);
    mpLocalMapper->SetLoopCloser(mpLoopCloser);

    mpLoopCloser->SetTracker(mpTracker);
    mpLoopCloser->SetLocalMapper(mpLocalMapper);
}

cv::Mat System::TrackStereo(const cv::Mat &imLeft, const cv::Mat &imRight, const double &timestamp)
{
    if(mSensor!=STEREO)
    {
        cerr << "ERROR: you called TrackStereo but input sensor was not set to STEREO." << endl;
        exit(-1);
    }   

    // Check mode change
    {
        unique_lock<mutex> lock(mMutexMode);
        if(mbActivateLocalizationMode)
        {
            mpLocalMapper->RequestStop();

            // Wait until Local Mapping has effectively stopped
            while(!mpLocalMapper->isStopped())
            {
                usleep(1000);
            }

            mpTracker->InformOnlyTracking(true);
            mbActivateLocalizationMode = false;
        }
        if(mbDeactivateLocalizationMode)
        {
            mpTracker->InformOnlyTracking(false);
            mpLocalMapper->Release();
            mbDeactivateLocalizationMode = false;
        }
    }

    // Check reset
    {
    unique_lock<mutex> lock(mMutexReset);
    if(mbReset)
    {
        mpTracker->Reset();
        mbReset = false;
    }
    }

    return mpTracker->GrabImageStereo(imLeft,imRight,timestamp);
}

cv::Mat System::TrackRGBD(const cv::Mat &im, const cv::Mat &depthmap, const double &timestamp)
{
    if(mSensor!=RGBD)
    {
        cerr << "ERROR: you called TrackRGBD but input sensor was not set to RGBD." << endl;
        exit(-1);
    }    

    // Check mode change
    {
        unique_lock<mutex> lock(mMutexMode);
        if(mbActivateLocalizationMode)
        {
            mpLocalMapper->RequestStop();

            // Wait until Local Mapping has effectively stopped
            while(!mpLocalMapper->isStopped())
            {
                usleep(1000);
            }

            mpTracker->InformOnlyTracking(true);
            mbActivateLocalizationMode = false;
        }
        if(mbDeactivateLocalizationMode)
        {
            mpTracker->InformOnlyTracking(false);
            mpLocalMapper->Release();
            mbDeactivateLocalizationMode = false;
        }
    }

    // Check reset
    {
    unique_lock<mutex> lock(mMutexReset);
    if(mbReset)
    {
        mpTracker->Reset();
        mbReset = false;
    }
    }

    return mpTracker->GrabImageRGBD(im,depthmap,timestamp);
}

cv::Mat System::TrackMonocular(const cv::Mat &im, const double &timestamp, const string frame_img_path)
{
    if(mSensor!=MONOCULAR)
    {
        cerr << "ERROR: you called TrackMonocular but input sensor was not set to Monocular." << endl;
        exit(-1);
    }

    // Check mode change
    {
        unique_lock<mutex> lock(mMutexMode);
        if(mbActivateLocalizationMode)
        {
            mpLocalMapper->RequestStop();

            // Wait until Local Mapping has effectively stopped
            while(!mpLocalMapper->isStopped())
            {
                usleep(1000);
            }

            mpTracker->InformOnlyTracking(true);
            mbActivateLocalizationMode = false;
        }
        if(mbDeactivateLocalizationMode)
        {
            mpTracker->InformOnlyTracking(false);
            mpLocalMapper->Release();
            mbDeactivateLocalizationMode = false;
        }
    }

    // Check reset
    {
    unique_lock<mutex> lock(mMutexReset);
    if(mbReset)
    {
        mpTracker->Reset();
        mbReset = false;
    }
    }

    return mpTracker->GrabImageMonocular(im,timestamp,frame_img_path);
}

void System::ActivateLocalizationMode()
{
    unique_lock<mutex> lock(mMutexMode);
    mbActivateLocalizationMode = true;
}

void System::DeactivateLocalizationMode()
{
    unique_lock<mutex> lock(mMutexMode);
    mbDeactivateLocalizationMode = true;
}

void System::Reset()
{
    unique_lock<mutex> lock(mMutexReset);
    mbReset = true;
}

void System::Shutdown()
{
    mpLocalMapper->RequestFinish();
    mpLoopCloser->RequestFinish();
    mpViewer->RequestFinish();

    // Wait until all thread have effectively stopped
    while(!mpLocalMapper->isFinished() || !mpLoopCloser->isFinished()  ||
          !mpViewer->isFinished()      || mpLoopCloser->isRunningGBA())
    {
        usleep(5000);
    }

    pangolin::BindToContext("ORB-SLAM2: Map Viewer");
}

void System::SaveTrajectoryTUM(const string &filename)
{
    cout << endl << "Saving camera trajectory to " << filename << " ..." << endl;

    vector<KeyFrame*> vpKFs = mpMap->GetAllKeyFrames();
    sort(vpKFs.begin(),vpKFs.end(),KeyFrame::lId);

    // Transform all keyframes so that the first keyframe is at the origin.
    // After a loop closure the first keyframe might not be at the origin.
    cv::Mat Two = vpKFs[0]->GetPoseInverse();

    ofstream f;
    f.open(filename.c_str());
    f << fixed;

    // Frame pose is stored relative to its reference keyframe (which is optimized by BA and pose graph).
    // We need to get first the keyframe pose and then concatenate the relative transformation.
    // Frames not localized (tracking failure) are not saved.

    // For each frame we have a reference keyframe (lRit), the timestamp (lT) and a flag
    // which is true when tracking failed (lbL).
    list<ORB_SLAM2::KeyFrame*>::iterator lRit = mpTracker->mlpReferences.begin();
    list<double>::iterator lT = mpTracker->mlFrameTimes.begin();
    list<bool>::iterator lbL = mpTracker->mlbLost.begin();
    for(list<cv::Mat>::iterator lit=mpTracker->mlRelativeFramePoses.begin(),
        lend=mpTracker->mlRelativeFramePoses.end();lit!=lend;lit++, lRit++, lT++, lbL++)
    {
        if(*lbL)
            continue;

        KeyFrame* pKF = *lRit;

        cv::Mat Trw = cv::Mat::eye(4,4,CV_32F);

        // If the reference keyframe was culled, traverse the spanning tree to get a suitable keyframe.
        while(pKF->isBad())
        {
            Trw = Trw*pKF->mTcp;
            pKF = pKF->GetParent();
        }

        Trw = Trw*pKF->GetPose()*Two;

        cv::Mat Tcw = (*lit)*Trw;
        cv::Mat Rwc = Tcw.rowRange(0,3).colRange(0,3).t();
        cv::Mat twc = -Rwc*Tcw.rowRange(0,3).col(3);

        vector<float> q = Converter::toQuaternion(Rwc);

        f << setprecision(6) << *lT << " " <<  setprecision(9) << twc.at<float>(0) << " " << twc.at<float>(1) << " " << twc.at<float>(2) << " " << q[0] << " " << q[1] << " " << q[2] << " " << q[3] << endl;
    }
    f.close();
    cout << endl << "trajectory saved!" << endl;
}


void System::SaveKeyFrameTrajectoryTUM(const string &filename)
{
    cout << endl << "Saving keyframe trajectory to " << filename << " ..." << endl;

    vector<KeyFrame*> vpKFs = mpMap->GetAllKeyFrames();
    sort(vpKFs.begin(),vpKFs.end(),KeyFrame::lId);

    // Transform all keyframes so that the first keyframe is at the origin.
    // After a loop closure the first keyframe might not be at the origin.
    //cv::Mat Two = vpKFs[0]->GetPoseInverse();

    ofstream f;
    f.open(filename.c_str());
    f << fixed;

    for(size_t i=0; i<vpKFs.size(); i++)
    {
        KeyFrame* pKF = vpKFs[i];

       // pKF->SetPose(pKF->GetPose()*Two);

        if(pKF->isBad())
            continue;

        cv::Mat R = pKF->GetRotation().t();
        vector<float> q = Converter::toQuaternion(R);
        cv::Mat t = pKF->GetCameraCenter();
        f << setprecision(6) << pKF->mTimeStamp << setprecision(7) << " " << t.at<float>(0) << " " << t.at<float>(1) << " " << t.at<float>(2)
          << " " << q[0] << " " << q[1] << " " << q[2] << " " << q[3] << endl;

    }

    f.close();
    cout << endl << "trajectory saved!" << endl;
}

void System::SaveTrajectoryKITTI(const string &filename)
{
    cout << endl << "Saving camera trajectory to " << filename << " ..." << endl;

    vector<KeyFrame*> vpKFs = mpMap->GetAllKeyFrames();
    sort(vpKFs.begin(),vpKFs.end(),KeyFrame::lId);

    // Transform all keyframes so that the first keyframe is at the origin.
    // After a loop closure the first keyframe might not be at the origin.
    cv::Mat Two = vpKFs[0]->GetPoseInverse();

    ofstream f;
    f.open(filename.c_str());
    f << fixed;

    // Frame pose is stored relative to its reference keyframe (which is optimized by BA and pose graph).
    // We need to get first the keyframe pose and then concatenate the relative transformation.
    // Frames not localized (tracking failure) are not saved.

    // For each frame we have a reference keyframe (lRit), the timestamp (lT) and a flag
    // which is true when tracking failed (lbL).
    list<ORB_SLAM2::KeyFrame*>::iterator lRit = mpTracker->mlpReferences.begin();
    list<double>::iterator lT = mpTracker->mlFrameTimes.begin();
    for(list<cv::Mat>::iterator lit=mpTracker->mlRelativeFramePoses.begin(), lend=mpTracker->mlRelativeFramePoses.end();lit!=lend;lit++, lRit++, lT++)
    {
        ORB_SLAM2::KeyFrame* pKF = *lRit;

        cv::Mat Trw = cv::Mat::eye(4,4,CV_32F);

        while(pKF->isBad())
        {
          //  cout << "bad parent" << endl;
            Trw = Trw*pKF->mTcp;
            pKF = pKF->GetParent();
        }

        Trw = Trw*pKF->GetPose()*Two;

        cv::Mat Tcw = (*lit)*Trw;
        cv::Mat Rwc = Tcw.rowRange(0,3).colRange(0,3).t();
        cv::Mat twc = -Rwc*Tcw.rowRange(0,3).col(3);

        f << setprecision(9) << Rwc.at<float>(0,0) << " " << Rwc.at<float>(0,1)  << " " << Rwc.at<float>(0,2) << " "  << twc.at<float>(0) << " " <<
             Rwc.at<float>(1,0) << " " << Rwc.at<float>(1,1)  << " " << Rwc.at<float>(1,2) << " "  << twc.at<float>(1) << " " <<
             Rwc.at<float>(2,0) << " " << Rwc.at<float>(2,1)  << " " << Rwc.at<float>(2,2) << " "  << twc.at<float>(2) << endl;
    }
    f.close();
    cout << endl << "trajectory saved!" << endl;
}

void System::SaveMap_PLY_JMD(const string &filename)
{
    cout << endl << "Saving Map as a ply file" << filename << " ... ";
    
    //open file
    ofstream f;
    f.open(filename.c_str());
    
    //get map points
    std::vector<MapPoint*> mapPts = mpMap->GetAllMapPoints();
    
    //write ply header
    f << "ply" << endl;
    f << "format ascii 1.0" << endl;
    f << "comment Reconstruct SLAM ply output" << endl;
    f << "element vertex " << mapPts.size() << endl;
    f << "property float x" << endl;
    f << "property float y" << endl;
    f << "property float z" << endl;
    f << "end_header" << endl;
    
    //write vertices
    for(std::vector<MapPoint*>::iterator it = mapPts.begin(); it != mapPts.end(); ++it)
    {
        //map point
        MapPoint* mpt = *it;
        cv::Mat pt = mpt->GetWorldPos();
        
        //write pt to file
        f << pt.at<float>(0,0) << " " << pt.at<float>(0,1) << " " << pt.at<float>(0,2) << endl;
    }
    
    //close file
    f.close();
    
    cout << "Done." << endl;
}

// Saves the SLAM scene to a format Visual SFM can
// read (*.nvm).
//  1. the cameras used are the keyframes (not every frame of video)
void System::SaveScene2NVM()
{
    //prompt
    cout << endl << "Saving Scene to NVM Format" << endl;
    
    /*---------- Setup ----------*/
    
    // key frames vector
    vector<KeyFrame*> keyFrames = mpMap->GetAllKeyFrames();
    sort(keyFrames.begin(),keyFrames.end(),KeyFrame::lId);
    
    // get map points
    std::vector<MapPoint*> mapPts = mpMap->GetAllMapPoints();
    
    // get calibration information
    cv::Mat K = mpTracker->Get_K();
    cv::Mat Dist = mpTracker->Get_Dist();
    float f;
    
    // parameters for image writing
    vector<int> compression_params;
    compression_params.push_back(CV_IMWRITE_JPEG_QUALITY);
    compression_params.push_back(100);
    compression_params.push_back(CV_IMWRITE_PNG_COMPRESSION);
    compression_params.push_back(9);
    
    // go through the keyframes and figure out what their id is relative to the map
    for(std::vector<MapPoint*>::iterator it = mapPts.begin(); it != mapPts.end(); ++it)
    {
        //map point
        MapPoint* mpt = *it;
        
        // get observations data
        // this is a map of keyframes and the index of the point in that keyframe
        std::map<KeyFrame*,size_t> obs = mpt->GetObservations();
        
        // for each observation
        for(std::map<KeyFrame*,size_t>::iterator it = obs.begin(); it != obs.end(); ++it)
        {
            //need the view id: i.e. the number the keyframe is in the map. Unfortunately, it seems
            //that mnId, mnFrameId, and nNextId are all not what we need, so the solution is instead
            //to search the keyframes vector for the matching pointer and keep that index into the vector
            //which will match the order that the keyframes were output.
            KeyFrame* currKF = it->first;
            for(unsigned int i = 0; i < keyFrames.size(); i++)
            {
                //match found, set ID and leave loop
                if(currKF->mnId == keyFrames[i]->mnId)
                {
                    currKF->mnViewId = i;
                    break;
                }
            }
        }
    }
    
    /*-------- End Setup --------*/
    
    
    /*---------- Write Cameras ----------*/
    // 
    cout << "    Writing Camera Info to reconstruct.nvm" << endl;
    
    // create file
    ofstream nvm_f;
    nvm_f.open("./reconstruct.nvm");
    
    // write header
    nvm_f << "NVM_V3" << endl << endl;
    
    // write number of cameras
    nvm_f << keyFrames.size() << endl;
    
    // write camera info
    nvm_f << setprecision(7);
    for(size_t i=0; i<keyFrames.size(); i++)
    {
        //skip bad frames
        KeyFrame* pKF = keyFrames[i];
        if(pKF->isBad()) {
            continue;
        }
        cout << "        camera " << i << endl;

        //extract R and t
        cv::Mat R = pKF->GetRotation().t();
        cv::Mat t = pKF->GetCameraCenter();
        
        // as an initial test, I am just taking f to be the average of fx and fy
        // and using that with k1 and k2 from the distortion coeffs. An alternative
        // is to set f to 1 and multiply the correct intrinsic matrix through the
        // rotation matrix. This might prove to be the better way, at which point
        // we may want to remove the "nieve" method.
        cv::Mat M;
        if(1)
        {   
            //flen = (fx + fy) / 2;
            f = (K.at<float>(0,0) + K.at<float>(1,1)) / 2.0f;
            
            //M = [R | t]
            M = (cv::Mat_<float>(4,4) << 
                    R.at<float>(0,0),R.at<float>(0,1),R.at<float>(0,2),t.at<float>(0),
                    R.at<float>(1,0),R.at<float>(1,1),R.at<float>(1,2),t.at<float>(1),
                    R.at<float>(2,0),R.at<float>(2,1),R.at<float>(2,2),t.at<float>(2),
                    0,0,0,1 );
        }
        else
        {
            //flen = 1
            f = 1.0;
            
            // make a homogeneous extrinsic matrix first
            cv::Mat P = (cv::Mat_<float>(4,4) << 
                             R.at<float>(0,0),R.at<float>(0,1),R.at<float>(0,2),t.at<float>(0),
                             R.at<float>(1,0),R.at<float>(1,1),R.at<float>(1,2),t.at<float>(1),
                             R.at<float>(2,0),R.at<float>(2,1),R.at<float>(2,2),t.at<float>(2),
                             0,0,0,1 );
                             
            // make a zero padded K Matrix
            cv::Mat KH = (cv::Mat_<float>(3,4) <<
                            K.at<float>(0,0),K.at<float>(0,1),K.at<float>(0,2),0,
                            K.at<float>(1,0),K.at<float>(1,1),K.at<float>(1,2),0,
                            K.at<float>(2,0),K.at<float>(2,1),K.at<float>(2,2),0);
                            
            // M = intrinsics * extrinsics
            M = KH * P;  
        }
        
        /*--- Write Camera ---*/
        // <File name> <focal length> <quaternion WXYZ> <camera center> <radial distortion> 0
        
        // <File name>
        nvm_f << pKF->mnFrameImgPath << " ";
            
        // <focal length>
        nvm_f << f << " ";
        
        // <quaternion WXYZ>
        // based on the methods outlined here:
        //     http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
        float QW, QX, QY, QZ, Tr;
        Tr = M.at<float>(0,0) + M.at<float>(1,1) + M.at<float>(2,2);
        if( Tr > 0 ) 
        {
            float s = 0.5f / sqrtf(Tr + 1.0f);
            QW = 0.25f / s;
            QX = ( M.at<float>(2,1) - M.at<float>(1,2) ) * s;
            QY = ( M.at<float>(0,2) - M.at<float>(2,0) ) * s;
            QZ = ( M.at<float>(1,0) - M.at<float>(0,1) ) * s;
        } 
        else 
        {
            if ( M.at<float>(0,0) > M.at<float>(1,1) && M.at<float>(0,0) > M.at<float>(2,2) ) 
            {
                float s = 2.0f * sqrtf( 1.0f + M.at<float>(0,0) - M.at<float>(1,1) - M.at<float>(2,2) );
                QW = (M.at<float>(2,1) - M.at<float>(1,2) ) / s;
                QX = 0.25f * s;
                QY = (M.at<float>(0,1) + M.at<float>(1,0) ) / s;
                QZ = (M.at<float>(0,2) + M.at<float>(2,0) ) / s;
            } 
            else if ( M.at<float>(1,1) > M.at<float>(2,2) ) 
            {
                float s = 2.0f * sqrtf( 1.0f + M.at<float>(1,1) - M.at<float>(0,0) - M.at<float>(2,2) );
                QW = (M.at<float>(0,2) - M.at<float>(2,0) ) / s;
                QX = (M.at<float>(0,1) + M.at<float>(1,0) ) / s;
                QY = 0.25f * s;
                QZ = (M.at<float>(1,2) + M.at<float>(2,1) ) / s;
            } 
            else 
            {
                float s = 2.0f * sqrtf( 1.0f + M.at<float>(2,2) - M.at<float>(0,0) - M.at<float>(1,1) );
                QW = (M.at<float>(1,0) - M.at<float>(0,1) ) / s;
                QX = (M.at<float>(0,2) + M.at<float>(2,0) ) / s;
                QY = (M.at<float>(1,2) + M.at<float>(2,1) ) / s;
                QZ = 0.25f * s;
            }
        }
        nvm_f << QW << " " << QX << " " << QY << " " << QZ << " ";
        
        // <camera center>
        nvm_f << M.at<float>(0,3) << " " << M.at<float>(1,3) << " " << M.at<float>(2,3) << " ";
        
        // <radial distortion>
        // Could be three radial distortion coefficients:
        // k1 - Dist.at<float>(0) 
        // k2 - Dist.at<float>(1)
        // k3 - Dist.at<float>(4)
        nvm_f << Dist.at<float>(0) << " ";
        
        // 0 <end line>
        nvm_f << 0 << endl;
                
        /*- End Write Camera -*/
        
    }
    nvm_f << endl;
    /*-------- End Write Cameras --------*/
    
    /*---------- Write Points ----------*/
    // <Point>  = <XYZ> <RGB> <number of measurements> <List of Measurements>
    // <Measurement> = <Image index> <Feature Index> <xy>
    cout << "        points" << endl;
    
    //write number of points
    nvm_f << mapPts.size() << endl;
    
    //for each point
    for(std::vector<MapPoint*>::iterator it = mapPts.begin(); it != mapPts.end(); ++it)
    {
        //map point
        MapPoint* mpt = *it;
        
        // <XYZ>
        cv::Mat pt = mpt->GetWorldPos();
        nvm_f << pt.at<float>(0,0) << " " << pt.at<float>(0,1) << " " << pt.at<float>(0,2) << " ";
        
        //write fake color <RGB>
        nvm_f << 250 << " " << 100 << " " << 150 << " ";
        
        /*----- Ref Views -----*/
        // for each point, we write which views observed the point in terms
        // of which keyframe saw the point and what is the id of the point
        // in that keyframe
        
        // get observations data
        // this is a map of keyframes and the index of the point in that keyframe
        //int nobs = mpt->Observations();
        std::map<KeyFrame*,size_t> obs = mpt->GetObservations();
        
        //temporary vector while we look for bad keyframe references and remove
        //those points
        vector<size_t> view_feat_pair;
        vector<float>  view_feat_xy;
        
        // for each observation
        for(std::map<KeyFrame*,size_t>::iterator it = obs.begin(); it != obs.end(); ++it)
        {
            //need the view id: i.e. the number the keyframe is in the map. We find this
            //info above in the setup section
            KeyFrame* currKF = it->first;
            size_t viewID = currKF->mnViewId;
            
            //check to make sure we found a match, just skip the keyframe if no match found because
            //the keyframe must not have made it into the final map for some reason
            if(viewID == 999999)
            {
                cout << "Didn't find a keyframe match for " << currKF->mnId << endl;
                for(unsigned int i = 0; i < keyFrames.size(); i++) { cout << keyFrames[i]->mnId << " "; }
                cout << endl;
                continue;
            }
            
            //we get the index of the point in the keyframe for free
            size_t featID = it->second;
            
            //save view ID and feature ID pair
            view_feat_pair.push_back(viewID);
            view_feat_pair.push_back(featID);
            
            //get map points' 2D Keypoint as XY
            view_feat_xy.push_back( currKF->mvKeys.at(featID).pt.x );
            view_feat_xy.push_back( currKF->mvKeys.at(featID).pt.y );
        }
        
        // number of observations after remove bad keyframes, if this is less than 2,
        // skip the feature point because we need at least two frames to estimate depth
        // and MVE will be confused if only one is given.
        unsigned int nobs = static_cast<int>(view_feat_pair.size() / 2);
        if (nobs < 2) 
        { 
            nvm_f << "0" << endl;
            continue; 
        }
        
        // <number of measurements>
        // write number of observations that have clean references to keyframes
        nvm_f << nobs;
        
        // <List of Measurements>
        // write view ID and feature ID
        for(unsigned int i=0; i < view_feat_pair.size(); i++)
        {
            // <Image index> <Feature Index> <xy>
            nvm_f << " " << view_feat_pair.at(i) << " " << view_feat_pair.at(i+1) << " " << view_feat_xy.at(i) << " " << view_feat_xy.at(i+1);
            
            //increment an extra time because of featID
            i++;
        }
        
        //write end of observation list
        nvm_f << endl;
        /*--- End Ref Views ---*/
    }
    
    // close file
    nvm_f.close();
    cout << "    done." << endl;
    /*-------- End Write Points --------*/
}

} //namespace ORB_SLAM
