#! /bin/sh

source_dir='Art'
export_dir='Report/Images/Art/'

for image in $(find $source_dir -name '*.ase' ); do

	image_path=$(dirname $image)

	mkdir -p $export_dir/${image_path#*/}
	~/Setup/aseprite/build/bin/aseprite -b $image --scale 32 --save-as $export_dir/${image_path#*/}/$(basename $image .ase).png
done
