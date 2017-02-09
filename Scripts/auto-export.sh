#! /bin/sh

source_folder=Art

chsum1=""

while [[ true ]]
do
	chsum2=`find $source_folder -type f -exec md5 {} \;`
	if [[ $chsum1 != $chsum2 ]] ; then           
		./Scripts/export-art.sh
		chsum1=$chsum2
	fi
	sleep 2
done

