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


#include<iostream>
#include<algorithm>
#include<fstream>
#include<chrono>
#include<string>
#include<limits>
#include<algorithm>
#include<cmath>
#include<sys/types.h>
#include<sys/stat.h>
#include<unistd.h>
#include<dirent.h>

#include<opencv2/core/core.hpp>

#include<System.h>

using namespace std;

void LoadImages(const string &strFile, vector<string> &vstrImageFilenames,
                vector<double> &vTimestamps);

bool loaded_from_video = false;

int main(int argc, char **argv)
{
    if(argc != 4)
    {
        cerr << endl << "Usage: ./mono_jmd path_to_vocabulary path_to_settings path_to_sequence_or_videofile" << endl;
        return 1;
    }   

    // Retrieve paths to images
    vector<string> vstrImageFilenames;
    vector<double> vTimestamps;
    LoadImages(argv[3], vstrImageFilenames, vTimestamps);

    int nImages = vstrImageFilenames.size();

    // Create SLAM system. It initializes all system threads and gets ready to process frames.
    ORB_SLAM2::System SLAM(argv[1],argv[2],ORB_SLAM2::System::MONOCULAR,true);

    // Vector for tracking time statistics
    vector<float> vTimesTrack;
    vTimesTrack.resize(nImages);

    cout << endl << "-------" << endl;
    cout << "Start processing sequence ..." << endl;
    cout << "Images in the sequence: " << nImages << endl << endl;

    // Main loop
    cv::Mat im;
    for(int ni=0; ni<nImages; ni++)
    {
        // Read image from file
        std::string frame_img_path = std::string(argv[3]) + vstrImageFilenames[ni];
        if(loaded_from_video)
        {
            im = cv::imread(vstrImageFilenames[ni],CV_LOAD_IMAGE_UNCHANGED);
        }
        else
        {
            im = cv::imread(string(argv[3])+"/"+vstrImageFilenames[ni],CV_LOAD_IMAGE_UNCHANGED);
        }
        double tframe = vTimestamps[ni];

        if(im.empty())
        {
            cerr << endl << "Failed to load image at: "
                 << string(argv[3]) << "/" << vstrImageFilenames[ni] << endl;
            return 1;
        }

#ifdef COMPILEDWITHC11
        std::chrono::steady_clock::time_point t1 = std::chrono::steady_clock::now();
#else
        std::chrono::monotonic_clock::time_point t1 = std::chrono::monotonic_clock::now();
#endif

        // Pass the image to the SLAM system
        SLAM.TrackMonocular(im,tframe,frame_img_path);

#ifdef COMPILEDWITHC11
        std::chrono::steady_clock::time_point t2 = std::chrono::steady_clock::now();
#else
        std::chrono::monotonic_clock::time_point t2 = std::chrono::monotonic_clock::now();
#endif

        double ttrack= std::chrono::duration_cast<std::chrono::duration<double> >(t2 - t1).count();

        vTimesTrack[ni]=ttrack;

        // Wait to load the next frame
        double T=0;
        if(ni<nImages-1)
            T = vTimestamps[ni+1]-tframe;
        else if(ni>0)
            T = tframe-vTimestamps[ni-1];

        if(ttrack<T)
            usleep((T-ttrack)*1e6);
    }
    cv::waitKey(0);

    // Stop all threads
    SLAM.Shutdown();

    // Tracking time statistics
    sort(vTimesTrack.begin(),vTimesTrack.end());
    float totaltime = 0;
    for(int ni=0; ni<nImages; ni++)
    {
        totaltime+=vTimesTrack[ni];
    }
    cout << "-------" << endl << endl;
    cout << "median tracking time: " << vTimesTrack[nImages/2] << endl;
    cout << "mean tracking time: " << totaltime/nImages << endl;

    // Save camera trajectory
    SLAM.SaveKeyFrameTrajectoryTUM("KeyFrameTrajectory.txt");
    
    // Save map
    SLAM.SaveMap_PLY_JMD("ReconstructPoints.ply");
    
    // Saves the map as an NVM, can be a MVE scene using SLAM.SaveScene2MVE(); but that function
    // is unreliable at this point so use SLAM.SaveScene2NVM(); instead
    SLAM.SaveScene2NVM();

    return 0;
}

void LoadImages(const string &strFile, vector<string> &vstrImageFilenames, vector<double> &vTimestamps)
{
    //variables
    DIR *dir;
    struct dirent *ent;
    int ct = 0;
    double timestamp = 1000000000.000000;
    typedef std::numeric_limits< double > dbl;
    cout.precision(dbl::max_digits10);
    
    // try to open directory
    if ((dir = opendir (strFile.c_str())) != NULL) 
    {
        // print all the files and directories within directory
        while ((ent = readdir (dir)) != NULL) 
        {
            //skip dot paths
            if( strcmp(ent->d_name,".") == 0 || strcmp(ent->d_name,"..") == 0 ) { continue; }
            
            // make string and double
            string fullname(ent->d_name);
            size_t lastindex = fullname.find_last_of("."); 
            string rawname = fullname.substr(0, lastindex); 
            timestamp += 0.033333;
            string filepath = strFile + '/' + fullname; 
            
            // save
            vTimestamps.push_back( timestamp );
            vstrImageFilenames.push_back( filepath );
        }
        
        // close dir
        closedir (dir);
        
        // sort
        sort( vstrImageFilenames.begin(), vstrImageFilenames.end() );
        
        // output
        for(unsigned int i = 0; i < vstrImageFilenames.size(); i++)
        {
            // output
            cout << "Image List: image " << ct << " : " << vstrImageFilenames.at(i) << endl;
            cout << "            timestamp " << fixed << vTimestamps.at(i) << endl;
            ct++;
        }
    } 
    else 
    {
        //try to open as video
        cv::VideoCapture cap(strFile);
        if(cap.isOpened())
        {
            // variables
            cv::Mat frame, sized_frame;
            double fps = cap.get(CV_CAP_PROP_FPS);
            int total_frames = cap.get(CV_CAP_PROP_FRAME_COUNT);
            int frame_part = floor(total_frames / 10);
            double hz = 1.0 / fps;
            double scale = 0.5;
            double timestamp = 0;
            int ct = 1, ct10 = 10;
            long file_timestamp = 0;
            vector<int> params;
            
            //imwrite properties
            params.push_back(CV_IMWRITE_JPEG_QUALITY);
            params.push_back(100);
            
            //input directory
            size_t lastdotindex = strFile.find_last_of("."); 
            string input_file_name = strFile.substr(0, lastdotindex);
            size_t lastslashindex = input_file_name.find_last_of("/");
            string input_dir_path = input_file_name.substr(0,lastslashindex);
            
            // output
            struct stat st = {0};
            string output_dir(input_file_name);
            cout << "Video Opened" << endl;
            cout << "    Frames per second: " << fps << endl;
            cout << "    Total Frames:      " << total_frames << endl;
            if(stat(output_dir.c_str(),&st ) == -1)
            {
                cout << "    Output Directory:  " << output_dir << endl;
                mkdir(output_dir.c_str(),0777);
            }
            cout << "    Reading Frames:    |" << flush;
            
            //grab frame
            while(cap.read(frame))
            {
                // make filename string and double timestamp 
                timestamp += hz;
                file_timestamp += (100*hz);
                char buffer [30];
                sprintf(buffer,"%012ld.jpg",file_timestamp);
                string filepath = output_dir + "/" + string(buffer); 
                
                //resize image
                cv::resize(frame,sized_frame, cv::Size(), scale, scale);
                
                //write image
                cv::imwrite(filepath,sized_frame,params);
                
                // save image location and timestamp
                vTimestamps.push_back( timestamp );
                vstrImageFilenames.push_back( filepath );
                
                if( ct % frame_part == 0 ) 
                { 
                    cout << "-- " << ct10 << " --|" << flush;
                    ct10 += 10;
                }
                ct++;
            }
            
            //release
            cap.release();
            
            // sort
            sort( vstrImageFilenames.begin(), vstrImageFilenames.end() );
            
            // output
            for(unsigned int i = 0; i < vstrImageFilenames.size(); i++)
            {
                // output
                cout << "Image List: image " << ct << " : " << vstrImageFilenames.at(i) << endl;
                cout << "            timestamp " << fixed << vTimestamps.at(i) << endl;
                ct++;
            }
            
            //set flag
            loaded_from_video = true;
        }
        else
        {
            // could not open directory
            cout << "Could not open directory or video file: " << strFile << endl;
            exit(0);
        }
    }
}