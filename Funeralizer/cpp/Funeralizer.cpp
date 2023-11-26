#include "pch.h"
#include "Funeralizer.h"
#include <cstdint>

constexpr float WEIGHT_RED   = 0.299;
constexpr float WEIGHT_GREEN = 0.587;
constexpr float WEIGHT_BLUE  = 0.114;

uint8_t pixel_average(uint8_t r, uint8_t g, uint8_t b)
{
    return static_cast<uint8_t>(r * WEIGHT_RED + g * WEIGHT_GREEN +
                                b * WEIGHT_BLUE);
}

void funeralize_cpp(int *rgb_values, int image_size)
{
    for (int i = 0; i < image_size; i += 3)
    {
        rgb_values[i] = rgb_values[i + 1] = rgb_values[i + 2] =
            pixel_average(rgb_values[i], rgb_values[i + 1], rgb_values[i + 2]);
    }
}
