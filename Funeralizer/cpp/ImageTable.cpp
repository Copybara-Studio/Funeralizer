#include "ImageTable.h"

ImageTable::ImageTable(int width, int height)
{
	this->width = width;
	this->height = height;
	this->table = std::vector<std::vector<int>>(width, std::vector<int>(height, 0));
}
