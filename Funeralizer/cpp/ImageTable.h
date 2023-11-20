#pragma once
#include <vector>

/**
 * \brief Represents a table of pixels.
 */
class ImageTable
{
private:
	int width; // width of the table
	int height; // height of the table
	std::vector<std::vector<int>> table; // the table itself

public:
	/**
	 * \brief ImageTable constructor.
	 * \param width - width of the table
	 * \param height - height of the table
	 */
	ImageTable(int width, int height);

	/**
	 * \brief Width getter.
	 * \return width
	 */
	int getWidth()
	{
		return width;
	}

	/**
	* \brief Height getter.
	* \return height
	*/
	int getHeight()
	{
		return height;
	}

	/**
	 * \brief Gets the value of the pixel at the given coordinates.
	 * \param x - x coordinate
	 * \param y - y coordinate
	 * \return - value of the pixel at the given coordinates
	 */
	int get(int x, int y)
	{
		return table[x][y];
	}

	/**
	 * \brief Sets the value of the pixel at the given coordinates.
	 * \param x - x coordinate
	 * \param y - y coordinate
	 * \param value - value to set
	 */
	void set(int x, int y, int value)
	{
		table[x][y] = value;
	}
};
