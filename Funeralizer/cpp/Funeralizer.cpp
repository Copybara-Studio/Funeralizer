#include "pch.h"
#include "Funeralizer.h"
#include <cstdint>

constexpr float WEIGHT_RED = 0.299;
constexpr float WEIGHT_GREEN = 0.587;
constexpr float WEIGHT_BLUE = 0.114;

/**
 * @brief Function for calculating the average of the RGB values of a pixel.
 * @param r - red value
 * @param g - green value
 * @param b - blue value
 * @return Pixel average value for the given RGB values.
 */
uint8_t pixel_average(uint8_t r, uint8_t g, uint8_t b)
{
	return static_cast<uint8_t>(r * WEIGHT_RED + g * WEIGHT_GREEN +
		b * WEIGHT_BLUE);
}

/**
 * @brief Function for making an image greyscale.
 * * @param rgb_values - pointer to the RGB values of the image
 * * @param image_size - size of the image
 */
void funeralize_cpp(int* rgb_values, int image_size)
{
	for (int i = 0; i < image_size; i += 3)
	{
		rgb_values[i] = rgb_values[i + 1] = rgb_values[i + 2] =
			pixel_average(rgb_values[i], rgb_values[i + 1], rgb_values[i + 2]);
	}
}
