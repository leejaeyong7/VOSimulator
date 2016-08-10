/*
	Read JPGs and do AprilTag Detection
    Joe DeGol, UIUC
 */

#include <stdio.h>
#include <stdint.h>
#include <inttypes.h>
#include <ctype.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/stat.h>

#include "apriltag.h"
#include "image_u8.h"
#include "tag36h11.h"
#include "tag36h10.h"
#include "tag36artoolkit.h"
#include "tag25h9.h"
#include "tag25h7.h"
#include "tag16h5.h"

#include "zarray.h"
#include "getopt.h"

//lcm
#ifdef LCM_FOUND
#include <lcm/lcm.h>
#include "JMD/JMD_LCM_Image_Type.h"
#endif

//opencv
#ifdef OPENCV_FOUND
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/core/utility.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#endif

apriltag_detector_t *td;
FILE *fp;

static void my_handler(const lcm_recv_buf_t *rbuf, const char * channel, const JMD_LCM_Image_Type * msg, void * user)
{
    //printf("Received message on channel \"%s\":\n", channel);
    //printf("  timestamp   = %"PRId64"\n", msg->Timestamp);
    //printf("  Height = %d,  Width = %d,  Stride = %d,  Channels = %d,  PixelSize = %d\n", msg->Height, msg->Width, msg->Stride, msg->Channels, msg->PixelSize); 
	//int32_t DataSize;
    //byte Data[DataSize];
    
    
    /*----- ArilTag Detect -----*/
    // image
    image_u8_t *im = image_u8_create_buf(msg->Width,msg->Height,msg->Channels,msg->Data);
    
    //opencv mat
    cv::Mat image(msg->Height, msg->Width, CV_MAKETYPE(0,msg->Channels), msg->Data, msg->Width * msg->Channels);
    if (msg->Channels == 1) { cvtColor(image, image, CV_GRAY2RGB); }
    
    //detections
    zarray_t *detections = apriltag_detector_detect(td, im);
    
    for (int i = 0; i < zarray_size(detections); i++) {
        apriltag_detection_t *det;
        zarray_get(detections, i, &det);

        //if (!quiet)
        //{
            printf("detection %3d: id (%2dx%2d)-%-4d, hamming %d, goodness %8.3f, margin %8.3f\n",i, det->family->d*det->family->d, det->family->h, det->id, det->hamming, det->goodness, det->decision_margin);
        //}
        cv::line( image, cv::Point2f( det->p[0][0], det->p[0][1] ), cv::Point2f( det->p[1][0], det->p[1][1]), cv::Scalar( 0, 255, 0), 4 );
        cv::line( image, cv::Point2f( det->p[1][0], det->p[1][1] ), cv::Point2f( det->p[2][0], det->p[2][1]), cv::Scalar( 0, 255, 0), 4 );
        cv::line( image, cv::Point2f( det->p[2][0], det->p[2][1] ), cv::Point2f( det->p[3][0], det->p[3][1]), cv::Scalar( 0, 255, 0), 4 );
        cv::line( image, cv::Point2f( det->p[3][0], det->p[3][1] ), cv::Point2f( det->p[0][0], det->p[0][1]), cv::Scalar( 0, 255, 0), 4 );

        //hamm_hist[det->hamming]++;
    }

    //if (!quiet) 
    //{
        //timeprofile_display(td->tp);
        //printf("nedges: %d, nsegments: %d, nquads: %d\n", td->nedges, td->nsegments, td->nquads);
    //}
    //if (!quiet) { printf("Hamming histogram: "); }
    //for (int i = 0; i < hamm_hist_max; i++) { printf("%5d", hamm_hist[i]); }
    //if (quiet) { printf("%12.3f", timeprofile_total_utime(td->tp) / 1.0E3); }
    //printf("\n");
    
    //display
    cv::namedWindow( "Display window" );
    cv::imshow( "Display window", image );
    cv::waitKey(1);
    
    //delete
    apriltag_detections_destroy(detections);
    image_u8_destroy(im);
    /*--- End AprilTag Detect ---*/
}
   
// Invoke:
//
// tagtest [options] input.pnm

int main(int argc, char *argv[])
{
    getopt_t *getopt = getopt_create();

    getopt_add_bool(getopt, 'h', "help", 0, "Show this help");
    getopt_add_bool(getopt, 'd', "debug", 0, "Enable debugging output (slow)");
    getopt_add_bool(getopt, 'q', "quiet", 0, "Reduce output");
    getopt_add_string(getopt, 'f', "family", "tag36h11", "Tag family to use");
    getopt_add_string(getopt, 'L', "LCM", "","Use Provided LCM Channel");
    getopt_add_string(getopt, 'o', "output","-1","Output to folder");
    getopt_add_int(getopt, 'g', "timing","0","Outputs timing info to AprilTags_Debug.txt");
    getopt_add_int(getopt, '\0', "border", "1", "Set tag family border size");
    getopt_add_int(getopt, 'i', "iters", "1", "Repeat processing on input set this many times");
    getopt_add_int(getopt, 't', "threads", "4", "Use this many CPU threads");
    getopt_add_double(getopt, 'x', "decimate", "1.0", "Decimate input image by this factor");
    getopt_add_double(getopt, 'b', "blur", "0.0", "Apply low-pass blur to input");
    getopt_add_bool(getopt, '0', "refine-edges", 1, "Spend more time trying to align edges of tags");
    getopt_add_bool(getopt, '1', "refine-decode", 0, "Spend more time trying to decode tags");
    getopt_add_bool(getopt, '2', "refine-pose", 0, "Spend more time trying to precisely localize tags");

    if (!getopt_parse(getopt, argc, argv, 1) || getopt_get_bool(getopt, "help")) {
        printf("Usage: %s [options] <input files>\n", argv[0]);
        getopt_do_usage(getopt);
        exit(0);
    }

    const zarray_t *inputs = getopt_get_extra_args(getopt);

    apriltag_family_t *tf = NULL;
    const char *famname = getopt_get_string(getopt, "family");
    if (!strcmp(famname, "tag36h11"))
        tf = tag36h11_create();
    else if (!strcmp(famname, "tag36h10"))
        tf = tag36h10_create();
    else if (!strcmp(famname, "tag36artoolkit"))
        tf = tag36artoolkit_create();
    else if (!strcmp(famname, "tag25h9"))
        tf = tag25h9_create();
    else if (!strcmp(famname, "tag25h7"))
        tf = tag25h7_create();
    else if (!strcmp(famname, "tag16h5"))
        tf = tag16h5_create();
    else {
        printf("Unrecognized tag family name. Use e.g. \"tag36h11\".\n");
        exit(-1);
    }

    tf->black_border = getopt_get_int(getopt, "border");

    //apriltag_detector_t *td = apriltag_detector_create();
    td = apriltag_detector_create();
    apriltag_detector_add_family(td, tf);
    td->quad_decimate = getopt_get_double(getopt, "decimate");
    td->quad_sigma = getopt_get_double(getopt, "blur");
    td->nthreads = getopt_get_int(getopt, "threads");
    td->debug = getopt_get_bool(getopt, "debug");
    td->timing = getopt_get_bool(getopt,"timing");
    td->refine_edges = getopt_get_bool(getopt, "refine-edges");
    td->refine_decode = getopt_get_bool(getopt, "refine-decode");
    td->refine_pose = getopt_get_bool(getopt, "refine-pose");

    int quiet = getopt_get_bool(getopt, "quiet");

    int maxiters = getopt_get_int(getopt, "iters");
    const char *lcm_channel = getopt_get_string(getopt,"LCM");
    const char *out_folder  = getopt_get_string(getopt,"output");
    const int hamm_hist_max = 10;

    //open file for timing
    fp = fopen("AprilTag_Debug.txt","w+");

    /*--------------- Run From LCM ---------------*/
    if (lcm_channel != NULL && lcm_channel[0]) 
    {
        printf("LCM Channel Given: %s\n",lcm_channel);
        
        //create
        lcm_t * lcm = lcm_create(NULL);
        if(!lcm) { return 1; }
   
        //subscribe
        JMD_LCM_Image_Type_subscribe(lcm, lcm_channel, &my_handler, NULL);
   
        //handle
        while(1) { lcm_handle(lcm); }
        
        //destroy
        lcm_destroy(lcm);
    }
    /*------------- End Run From LCM -------------*/
    
    /*--------------- Run from Input PNGs or JPGs ---------------*/
    else if ( strcmp(out_folder,"-1") != 0)
    {
        //make directory
        struct stat st = {0};
        if (stat(out_folder, &st) == -1) {
            mkdir(out_folder, 0777);
        }
        
        //iterate -- not sure why this is here
        for (int iter = 0; iter < maxiters; iter++) 
        {
    
            if (maxiters > 1) { printf("iter %d / %d\n", iter + 1, maxiters); }
                
            //for each image
            for (int input = 0; input < zarray_size(inputs); input++) {
    
                int hamm_hist[hamm_hist_max];
                memset(hamm_hist, 0, sizeof(hamm_hist));
    
                char *path;
                zarray_get(inputs, input, &path);
                if (!quiet)
                    printf("loading %s\n", path);
    
                //opencv mat
                cv::Mat image = cv::imread(path,0);
                image_u8_t *im = image_u8_create_buf(image.cols,image.rows,1,image.data);
                if (im == NULL) {
                    printf("couldn't find %s\n", path);
                    continue;
                }
                cvtColor(image, image, CV_GRAY2RGB);
                
                //detection
                double t = (double)cv::getTickCount();
                zarray_t *detections = apriltag_detector_detect(td, im);
                t = ((double)cv::getTickCount() - t)/cv::getTickFrequency();
                fprintf(fp,"%f\n",t);
    
                for (int i = 0; i < zarray_size(detections); i++) {
                    apriltag_detection_t *det;
                    zarray_get(detections, i, &det);
    
                    //if (!quiet)
                        printf("detection %3d: id (%2dx%2d)-%-4d, hamming %d, goodness %8.3f, margin %8.3f\n", i, det->family->d*det->family->d, det->family->h, det->id, det->hamming, det->goodness, det->decision_margin);
    
                    cv::line( image, cv::Point2f( det->p[0][0], det->p[0][1] ), cv::Point2f( det->p[1][0], det->p[1][1]), cv::Scalar( 0, 255, 0), 4 );
                    cv::line( image, cv::Point2f( det->p[1][0], det->p[1][1] ), cv::Point2f( det->p[2][0], det->p[2][1]), cv::Scalar( 0, 255, 0), 4 );
                    cv::line( image, cv::Point2f( det->p[2][0], det->p[2][1] ), cv::Point2f( det->p[3][0], det->p[3][1]), cv::Scalar( 0, 255, 0), 4 );
                    cv::line( image, cv::Point2f( det->p[3][0], det->p[3][1] ), cv::Point2f( det->p[0][0], det->p[0][1]), cv::Scalar( 0, 255, 0), 4 );
                    
                    hamm_hist[det->hamming]++;
                }
    
                apriltag_detections_destroy(detections);
    
                if (!quiet) {
                    timeprofile_display(td->tp);
                    printf("nedges: %d, nsegments: %d, nquads: %d\n", td->nedges, td->nsegments, td->nquads);
                }
    
                if (!quiet)
                {
                    printf("Hamming histogram: ");
                    for (int i = 0; i < hamm_hist_max; i++) { printf("%5d", hamm_hist[i]); }
                }
    
                if (quiet) {
                    printf("%12.3f", timeprofile_total_utime(td->tp) / 1.0E3);
                }
    
                printf("\n");
                
                //show image
                cv::namedWindow( "Display window" );
                cv::imshow( "Display window", image );
                cv::waitKey(1);
                
                //write image
                std::string outfile = std::string( basename(path) );
                std::string outpath = out_folder + std::string("//Tag_") + outfile;
                cv::imwrite(outpath,image);
                if (!quiet)
                    printf("saving %s\n", outpath.c_str());
                
                image_u8_destroy(im);
            }
        }
    }
    /*------------- End Run from Input PNGs or JPGs -------------*/
    
    /*--------------- Run from Input Files ---------------*/
    else
    {
        //make directory
        struct stat st = {0};
        if (stat(out_folder, &st) == -1) {
            mkdir(out_folder, 0777);
        }
        
        //iterate -- not sure why this is here
        for (int iter = 0; iter < maxiters; iter++) 
        {
    
            if (maxiters > 1) { printf("iter %d / %d\n", iter + 1, maxiters); }
                
            //for each image
            for (int input = 0; input < zarray_size(inputs); input++) {
    
                int hamm_hist[hamm_hist_max];
                memset(hamm_hist, 0, sizeof(hamm_hist));
    
                char *path;
                zarray_get(inputs, input, &path);
                if (!quiet)
                    printf("loading %s\n", path);
                
                //read image
                cv::Mat image = cv::imread(path,0);
                image_u8_t *im = image_u8_create_buf(image.cols,image.rows,1,image.data);
                //image_u8_t *im = image_u8_create_from_pnm(path);
                if (im == NULL) {
                    printf("couldn't find %s\n", path);
                    continue;
                }
                cvtColor(image, image, CV_GRAY2RGB);
                
                //detection
                double t = (double)cv::getTickCount();
                zarray_t *detections = apriltag_detector_detect(td, im);
                t = ((double)cv::getTickCount() - t)/cv::getTickFrequency();
                fprintf(fp,"%f\n",t);
    
                for (int i = 0; i < zarray_size(detections); i++) {
                    apriltag_detection_t *det;
                    zarray_get(detections, i, &det);
    
                    //if (!quiet)
                        printf("detection %3d: id (%2dx%2d)-%-4d, hamming %d, goodness %8.3f, margin %8.3f\n", i, det->family->d*det->family->d, det->family->h, det->id, det->hamming, det->goodness, det->decision_margin);
    
                    cv::line( image, cv::Point2f( det->p[0][0], det->p[0][1] ), cv::Point2f( det->p[1][0], det->p[1][1]), cv::Scalar( 0, 255, 0), 4 );
                    cv::line( image, cv::Point2f( det->p[1][0], det->p[1][1] ), cv::Point2f( det->p[2][0], det->p[2][1]), cv::Scalar( 0, 255, 0), 4 );
                    cv::line( image, cv::Point2f( det->p[2][0], det->p[2][1] ), cv::Point2f( det->p[3][0], det->p[3][1]), cv::Scalar( 0, 255, 0), 4 );
                    cv::line( image, cv::Point2f( det->p[3][0], det->p[3][1] ), cv::Point2f( det->p[0][0], det->p[0][1]), cv::Scalar( 0, 255, 0), 4 );
                    
                    hamm_hist[det->hamming]++;
                }
    
                apriltag_detections_destroy(detections);
    
                if (!quiet) {
                    timeprofile_display(td->tp);
                    printf("nedges: %d, nsegments: %d, nquads: %d\n", td->nedges, td->nsegments, td->nquads);
                }
    
                if (!quiet)
                {
                    printf("Hamming histogram: ");
                    for (int i = 0; i < hamm_hist_max; i++) { printf("%5d", hamm_hist[i]); }
                }
    
                if (quiet) {
                    printf("%12.3f", timeprofile_total_utime(td->tp) / 1.0E3);
                }
    
                printf("\n");
                
                //show image
                cv::namedWindow( "Display window" );
                cv::imshow( "Display window", image );
                cv::waitKey(1);
                
                //write image
                std::string outpath = out_folder + std::string("/Tag_") + path;
                cv::imwrite(outpath,image);
                
                image_u8_destroy(im);
            }
        }
    }
    /*------------- End Run From Input Files -------------*/

    //fclose
    fclose(fp);
    
    // don't deallocate contents of inputs; those are the argv
    apriltag_detector_destroy(td);

    tag36h11_destroy(tf);
    return 0;
}
