#pragma once
/**
 * @brief This function changes the supplied array of integers representing pixels to a grayscale color range
 * 
 * @param rgb_values The pointer to an array of rgb values of an image
 * @param image_size The size of the array provided, its value should be divisible by 3
 */
extern "C" __declspec(dllexport) void funeralizeCpp(int *rgb_values, int rgb_values_length);