#-----------------------------------------------------#
# AprilTags Level CMakeLists.txt
# creates executables for april tags files
#
# Written by Joseph DeGol
#-----------------------------------------------------#





#-----------------------------------------------------#
#-----------------------------------------------------#
#--------------------- Functions ---------------------#
#-----------------------------------------------------#
#-----------------------------------------------------#

#-----------------------------------------------------#
#-----------------------------------------------------#
#------------------- End Functions -------------------#
#-----------------------------------------------------#
#-----------------------------------------------------#





#-----------------------------------------------------#
#-----------------------------------------------------#
#----------------------- Build -----------------------#
#-----------------------------------------------------#
#-----------------------------------------------------#

#----- AprilTags -----#

# project
cmake_minimum_required(VERSION 2.8)
project(AprilTags)

# setup
SET(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -std=gnu99 -Wall -Wno-unused-parameter -Wno-unused-function -pthread -I. -Icommon -O4")
SET(CMAKE_CXX_FLAGS "${CMAKE_C_FLAGS} -std=gnu99 -Wall -Wno-unused-parameter -Wno-unused-function -pthread -I. -Icommon -O4")
set(LIBRARY_OUTPUT_PATH ${PROJECT_SOURCE_DIR}/lib)
include_directories(common)

# include libraries
find_package(OpenCV REQUIRED)
set(Dependencies ${OpenCV_LIBS})

# add executable
add_library(AprilTags SHARED apriltag.c apriltag_quad_thresh.c tag16h5.c tag25h7.c tag25h9.c tag36h10.c tag36h11.c tag36artoolkit.c g2d.c common/zarray.c common/zhash.c common/zmaxheap.c common/unionfind.c common/matd.c common/image_u8.c common/pnm.c common/image_f32.c common/image_u32.c common/workerpool.c common/time_util.c common/svd22.c common/homography.c common/string_util.c common/getopt.c)
#add_executable(Demo_AprilTagsC apriltag_demo_jmd.cpp apriltag.c apriltag_quad_thresh.c tag16h5.c tag25h7.c tag25h9.c tag36h10.c tag36h11.c tag36artoolkit.c g2d.c common/zarray.c common/zhash.c common/zmaxheap.c common/unionfind.c common/matd.c common/image_u8.c common/pnm.c common/image_f32.c common/image_u32.c common/workerpool.c common/time_util.c common/svd22.c common/homography.c common/string_util.c common/getopt.c)
target_link_libraries(AprilTags pthread m ${Dependencies})

#--- End AprilTags ---#

#-----------------------------------------------------#
#-----------------------------------------------------#
#--------------------- End Build ---------------------#
#-----------------------------------------------------#
#-----------------------------------------------------#
