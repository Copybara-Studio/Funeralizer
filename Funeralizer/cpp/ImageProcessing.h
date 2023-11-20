#pragma once

namespace fs = std::filesystem;

/**
 * \brief Class for image processing
 */
class ImageProcessing
{
public:	
	/**
	 * \brief Constructor
	 */
	ImageProcessing();

	/**
	 * \brief Load image method
	 * \param imagePath - path to image
	 * \return ImageTable object
	 */
	ImageTable* loadImage(fs::path imagePath);

	/**
	 * \brief Calculates brightness of pixel
	 * \param red - red value
	 * \param green - green value
	 * \param blue - blue value
	 * \return brightness value
	 */
	int brightness(int red, int green, int blue);

	/**
	 * \brief Modifies image from colored to grayscale
	 * \param imageTable - image to modify
	 * \param numThreads - number of threads
	 * \return modified image
	 */
	ImageTable modifyImage(ImageTable& imageTable, int numThreads);

	/**
	 * \brief Method for saving image
	 * \param imageTable - image to save
	 * \param outputPath - path to save
	 */
	void saveImage(ImageTable& imageTable, fs::path outputPath);
};