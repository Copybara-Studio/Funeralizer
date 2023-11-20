#include <iostream>
#include <filesystem>
#include <string>

#include "ImageTable.h"
#include "ImageProcessing.h"

namespace fs = std::filesystem;

void main(int argc, char* argv[])
{
	// getting parameters -> image path, output path, number of threads
	fs::path imagePath = argv[1];
	fs::path outputPath = argv[2];
	int numThreads = std::stoi(argv[3]);


	std::cout << "Hello World!" << std::endl;
}

