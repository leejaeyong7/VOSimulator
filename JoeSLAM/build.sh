echo "Configuring and building Thirdparty/DBoW2 ..."
cd Thirdparty/DBoW2
mkdir build
cd build
cmake .. -DCMAKE_BUILD_TYPE=Release
make -j


echo "Configuring and building Thirdparty/g2o ..."
cd ../../g2o
mkdir build
cd build
cmake .. -DCMAKE_BUILD_TYPE=Release
make -j


echo "Configuring and building Thirdparty/AprilTags ..."
cd ../../AprilTagsC_3_18_2015
mkdir build
cd build
cmake .. -DCMAKE_BUILD_TYPE=Release
make -j


echo "Uncompress vocabulary ..."
cd ../../../
cd Vocabulary
tar -xf ORBvoc.txt.tar.gz


echo "Configuring and building ORB_SLAM2 ..."
cd ..
mkdir build
cd build
cmake .. -DCMAKE_BUILD_TYPE=Release
make -j
