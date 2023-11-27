#include "pch.h"
#include "Funeralizer.h"
#include <cstdint>

constexpr float WEIGHT_RED   = 0.299;
constexpr float WEIGHT_GREEN = 0.587;
constexpr float WEIGHT_BLUE  = 0.114;

/**
 * @brief This function takes in values of a single pixel and produces an average of them, which results in a pixel of gray color
 * 
 * @param r the red component of the pixel, ranging from 0 to 255
 * @param g the green component of the pixel, ranging from 0 to 255
 * @param b the blue component of the pixel, ranging from 0 to 255
 * @return uint8_t the average value of all the three components, which when used for r, g and b produces a grayscale pixel
 */
uint8_t pixel_average(uint8_t r, uint8_t g, uint8_t b)
{
    return static_cast<uint8_t>(r * WEIGHT_RED + g * WEIGHT_GREEN +
                                b * WEIGHT_BLUE);
}

void funeralizeCpp(int *rgb_values, int rgb_alues_length)
{
    for (int i = 0; i < rgb_values_length; i += 3)
    {
        rgb_values[i] = rgb_values[i + 1] = rgb_values[i + 2] =
            pixel_average(rgb_values[i], rgb_values[i + 1], rgb_values[i + 2]);
    }
}
