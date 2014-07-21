using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace org.flixel
{

    /// <summary>
    /// This is the EXTENDED Cave Generator.
    /// This uses strings for each block, to allow for more combinations.
    /// 
    /// This class uses the cellular automata algorithm
    /// to generate very nice caves.
    /// 
    /// (Coded by Eddie Lee, October 16, 2010)
    /// (Ported to C# by Shane Brouwer)
    /// </summary>
    public class FlxCaveGeneratorExt
    {
        static public int _numTilesCols = 50;
        static public int _numTilesRows = 50;

        private int[] _data;

        /// <summary>
        /// How many times do you want to "smooth" the cave.
        /// The higher number the smoother.
        /// </summary>
        public int numSmoothingIterations = 5;

        /// <summary>
        /// During initial state, how percent of matrix are walls?
        /// The closer the value is to 1.0, more wall-e the area is
        /// Values 0-1.
        /// </summary>
        public float initWallRatio = 0.5f;

        public int generateMethod = 0;

        /// <summary>
        /// Method for generating first set of random cave.
        /// </summary>
        public const int RANDOM = 0;

        /// <summary>
        /// Method for generating first set of random cave.
        /// </summary>
        public const int GROW_FROM = 1;

        /// <summary>
        /// No auto-tiling.
        /// </summary>
        public const int OFF = 0;

        /// <summary>
        /// Platformer-friendly auto-tiling.
        /// </summary>
        public const int AUTO = 1;

        /// <summary>
        /// Top-down auto-tiling.
        /// </summary>
        public const int ALT = 2;

        /// <summary>
        /// Random pick from tilesheet
        /// </summary>
        public const int RANDOMIZED = 3;

        /// <summary>
        /// Uses a string to choose tiles.
        /// </summary>
        public const int STRING = 4;


        /// <summary>
        /// Set this flag to use one of the 16-tile binary auto-tile algorithms (OFF, AUTO, or ALT).
        /// </summary>
        public int auto;

        //Extra decoration tiles.

        /// <summary>
        /// Top decoration.
        /// </summary>
        private const int TOP_DECORATION_MIN = 29;
        private const int TOP_DECORATION_MAX = 40;

        private const string RIGHT_DECORATION = "25";
        private const string LEFT_DECORATION = "26";
        private const string UNDER_DECORATION = "28";

        private const string TOP_RIGHT_DECORATION = "21";
        private const string TOP_LEFT_DECORATION = "22";
        private const string UNDER_RIGHT_DECORATION = "23";
        private const string UNDER_LEFT_DECORATION = "24";


        public FlxCaveGeneratorExt(int nCols, int nRows)
        {

            _numTilesCols = nCols;
            _numTilesRows = nRows;
            auto = AUTO;

        }

        /// <summary>
        /// Generate a matrix of zeroes.
        /// </summary>
        /// <param name="rows">Number of rows for the matrix</param>
        /// <param name="cols">Number of cols for the matrix</param>
        /// <returns>Spits out a matrix that is cols x rows, zero initiated</returns>
        public string[,] genInitMatrix(int rows, int cols)
        {
            // Build array of 1s
            string[,] mat = new string[rows, cols];

            for (int _y = 0; _y < rows; _y++)
            {
                for (int _x = 0; _x < cols; _x++)
                {
                    mat[_y, _x] = "0";
                }
            }

            return mat;
        }

        /// <summary>
        /// Returns an auto tiled cave.
        /// </summary>
        /// <returns>Returns a matrix of a cave!</returns>
        public string[,] generateCaveLevel()
        {
            return this.generateCaveLevel(false);
        }

        /// <summary>
        /// Returns an auto tiled cave.
        /// </summary>
        /// <returns>Returns a matrix of a cave!</returns>
        public string[,] generateCaveLevel(bool WithDecorations)
        {
            // Initialize random array

            string[,] mat = new string[_numTilesRows, _numTilesCols];

            mat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            int floor = _numTilesRows - 2;

            if (generateMethod==RANDOM)
            {
                for (int _y = 0; _y < _numTilesRows; _y++)
                {
                    for (int _x = 0; _x < _numTilesCols; _x++)
                    {
                        //Throw in a random assortment of ones and zeroes.
                        if (FlxU.random() < initWallRatio)
                        {
                            mat[_y, _x] = "1";
                        }
                        else
                        {
                            mat[_y, _x] = "0";
                        }
                    }
                }
            }
            else if (generateMethod == GROW_FROM)
            {
                for (int _y = 0; _y < _numTilesRows; _y++)
                {
                    for (int _x = 0; _x < _numTilesCols; _x++)
                    {
                        //Throw in a random assortment of ones and zeroes.
                        if (FlxU.random() < initWallRatio)
                        {
                            mat[_y, _x] = "1";
                        }
                        else
                        {
                            mat[_y, _x] = "0";
                        }
                    }
                }
            }

            // Secondary buffer
            string[,] mat2 = genInitMatrix(_numTilesRows, _numTilesCols);

            // Run automata

            for (int i = 0; i <= numSmoothingIterations; i++)
            {
                runCelluarAutomata(mat, mat2);

                string[,] temp = new string[mat.GetLength(0), mat.GetLength(1)];
                mat = mat2;
                mat2 = temp;

            }

            //auto tile

            _data = convertMultiArrayStringToIntArray(mat);

            //int total = 0;
            int ii = 0;
            int totalTiles = _data.Length;

            while (ii < totalTiles)
                autoTile(ii++);

            //reconvert int[] to string[,]

            mat = convertIntArrayToMultiArray(_data);

            //add decorations.
            if (WithDecorations)
                mat = addDecorations(mat);

            return mat;
        }



        /// <summary>
        /// autoTile puts the correct tiles in place.
        /// </summary>
        /// <param name="Index"></param>
        public void autoTile(int Index)
        {
            int totalTiles = _data.Length;

            if (_data[Index] == 0) return;
            _data[Index] = 0;
            if ((Index - _numTilesCols < 0) || (_data[Index - _numTilesCols] > 0)) 		//UP
                _data[Index] += 1;
            if ((Index % _numTilesCols >= _numTilesCols - 1) || (_data[Index + 1] > 0)) 		//RIGHT
                _data[Index] += 2;
            if ((Index + _numTilesCols >= totalTiles) || (_data[Index + _numTilesCols] > 0)) //DOWN
                _data[Index] += 4;
            if ((Index % _numTilesCols <= 0) || (_data[Index - 1] > 0)) 					//LEFT
                _data[Index] += 8;

            // (auto == ALT
            if ((auto==ALT) && (_data[Index] == 15))	//The alternate algo checks for interior corners
            {
                if ((Index % _numTilesCols > 0) && (Index + _numTilesCols < totalTiles) && (_data[Index + _numTilesCols - 1] <= 0))
                    _data[Index] = 1;		//BOTTOM LEFT OPEN
                if ((Index % _numTilesCols > 0) && (Index - _numTilesCols >= 0) && (_data[Index - _numTilesCols - 1] <= 0))
                    _data[Index] = 2;		//TOP LEFT OPEN
                if ((Index % _numTilesCols < _numTilesCols - 1) && (Index - _numTilesCols >= 0) && (_data[Index - _numTilesCols + 1] <= 0))
                    _data[Index] = 4;		//TOP RIGHT OPEN
                if ((Index % _numTilesCols < _numTilesCols - 1) && (Index + _numTilesCols < totalTiles) && (_data[Index + _numTilesCols + 1] <= 0))
                    _data[Index] = 8; 		//BOTTOM RIGHT OPEN
            }

            //_data[Index] += 1;

        }


        /// <summary>
        /// Generates a cave level
        /// </summary>
        /// <param name="solidRowsBeforeSmooth">int[] array = fill in these rows as solid before the smooth.</param>
        /// <param name="solidColumnsBeforeSmooth">int[] array = fill in these rows as solid before the smooth.</param>
        /// <param name="solidRowsAfterSmooth">int[] array = fill in these rows as solid before the smooth.</param>
        /// <param name="solidColumnsAfterSmooth">int[] array = fill in these rows as solid before the smooth.</param>
        /// <param name="emptyRowsBeforeSmooth">int[] array = fill in these rows as empty before the smooth.</param>
        /// <param name="emptyColumnsBeforeSmooth">int[] array = fill in these rows as empty before the smooth.</param>
        /// <param name="emptyRowsAfterSmooth">int[] array = fill in these rows as empty before the smooth.</param>
        /// <param name="emptyColumnsAfterSmooth">int[] array = fill in these rows as empty before the smooth.</param>
        /// <returns></returns>
        public string[,] generateCaveLevel(int[] solidRowsBeforeSmooth,
            int[] solidColumnsBeforeSmooth,
            int[] solidRowsAfterSmooth,
            int[] solidColumnsAfterSmooth,
            int[] emptyRowsBeforeSmooth,
            int[] emptyColumnsBeforeSmooth,
            int[] emptyRowsAfterSmooth,
            int[] emptyColumnsAfterSmooth)
        {
            //Console.WriteLine("x/y {0}, {1}", _numTilesCols, _numTilesRows);

            // Initialize random array

            string[,] mat = new string[_numTilesRows, _numTilesCols];

            mat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            for (int _y = 0; _y < _numTilesRows; _y++)
            {
                for (int _x = 0; _x < _numTilesCols; _x++)
                {
                    //Throw in a random assortment of ones and zeroes.
                    if (FlxU.random() < initWallRatio)
                    {
                        mat[_y, _x] = "1";
                    }
                    else
                    {
                        mat[_y, _x] = "0";
                    }
                }
            }
            if (emptyRowsBeforeSmooth != null)
            {
                foreach (int _yEmpty in emptyRowsBeforeSmooth)
                {
                    for (int _i = 0; _i < _numTilesCols; ++_i)
                    {
                        if (_yEmpty < _numTilesRows)
                            mat[_yEmpty, _i] = "0";
                    }
                }
            }
            if (emptyColumnsBeforeSmooth != null)
            {
                foreach (int _xEmpty in emptyColumnsBeforeSmooth)
                {
                    for (int _i = 0; _i < _numTilesRows; ++_i)
                    {
                        if (_xEmpty < _numTilesCols)
                            mat[_i, _xEmpty] = "0";
                    }
                }
            }

            if (solidRowsBeforeSmooth != null)
            {
                foreach (int _ySolid in solidRowsBeforeSmooth)
                {
                    for (int _i = 0; _i < _numTilesCols; ++_i)
                    {
                        if (_ySolid < _numTilesRows)
                            mat[_ySolid, _i] = "1";
                    }
                }
            }
            if (solidColumnsBeforeSmooth != null)
            {
                foreach (int _xSolid in solidColumnsBeforeSmooth)
                {
                    for (int _i = 0; _i < _numTilesRows; ++_i)
                    {
                        if (_xSolid < _numTilesCols)
                            mat[_i, _xSolid] = "1";
                    }
                }
            }

            // Secondary buffer
            string[,] mat2 = genInitMatrix(_numTilesRows, _numTilesCols);

            // Run automata
            for (int i = 0; i <= numSmoothingIterations; i++)
            {

                runCelluarAutomata(mat, mat2);

                string[,] temp = new string[mat.GetLength(0), mat.GetLength(1)];
                mat = mat2;
                mat2 = temp;

            }

            if (emptyRowsAfterSmooth != null)
            {
                foreach (int _yEmpty in emptyRowsAfterSmooth)
                {
                    for (int _i = 0; _i < _numTilesCols; ++_i)
                    {
                        if (_yEmpty < _numTilesRows)
                            mat[_yEmpty, _i] = "0";
                    }
                }
            }
            if (emptyColumnsAfterSmooth != null)
            {
                foreach (int _xEmpty in emptyColumnsAfterSmooth)
                {
                    for (int _i = 0; _i < _numTilesRows; ++_i)
                    {
                        if (_xEmpty < _numTilesCols)
                            mat[_i, _xEmpty] = "0";
                    }
                }
            }
            if (solidRowsAfterSmooth != null)
            {
                foreach (int _ySolid in solidRowsAfterSmooth)
                {
                    for (int _i = 0; _i < _numTilesCols; ++_i)
                    {
                        if (_ySolid < _numTilesRows)
                            mat[_ySolid, _i] = "1";
                    }
                }
            }
            if (solidColumnsAfterSmooth != null)
            {
                foreach (int _xSolid in solidColumnsAfterSmooth)
                {
                    for (int _i = 0; _i < _numTilesRows; ++_i)
                    {
                        if (_xSolid < _numTilesCols)
                            mat[_i, _xSolid] = "1";
                    }
                }
            }

            return mat;
        }





        /// <summary>
        /// Runs the cellular automata algo.
        /// </summary>
        /// <param name="inMat">Matrix going in</param>
        /// <param name="outMat">Matrix to edit</param>
        public void runCelluarAutomata(string[,] inMat, string[,] outMat)
        {
            int numRows = inMat.GetLength(0);
            int numCols = inMat.GetLength(1);

            for (int _y = 0; _y < numRows; _y++)
            {
                for (int _x = 0; _x < numCols; _x++)
                {
                    int numWalls = countNumWallsNeighbors(inMat, _x, _y, 1);

                    if (numWalls >= 5)
                    {
                        outMat[_y, _x] = "1";
                    }
                    else
                    {
                        outMat[_y, _x] = "0";
                    }
                }
            }

        }

        public string[,] addDecorations(string[,] mat)
        {
            return this.addDecorations(mat, 1.0f);
        }

        /// <summary>
        /// Add Decorations such as top, right, etc decorations.
        /// </summary>
        /// <param name="mat">In Matrix</param>
        /// <param name="Chance">Chance of adding.</param>
        /// <returns>a string[,] matrix cave.</returns>
        public string[,] addDecorations(string[,] mat, float Chance)
        {
            for (int x = 0; x < mat.GetLength(1); x++)
            {
                for (int y = 0; y < mat.GetLength(0); y++)
                {
                    if (FlxU.random() <= Chance)
                    {
                        string value = mat[y, x];

                        if (value == "8" && coordIsInMatrix(mat, x - 1, y))
                        {
                            mat[y, x - 1] = ((int)FlxU.random(TOP_DECORATION_MIN, TOP_DECORATION_MAX)).ToString();
                        }
                        if (value == "14" && coordIsInMatrix(mat, x + 1, y))
                        {
                            mat[y, x + 1] = UNDER_DECORATION;
                        }
                        if (value == "13" && coordIsInMatrix(mat, x + 1, y))
                        {
                            mat[y, x + 1] = TOP_RIGHT_DECORATION;
                        }
                        if (value == "10" && coordIsInMatrix(mat, x + 1, y))
                        {
                            mat[y, x + 1] = UNDER_LEFT_DECORATION;
                        }
                        if (value == "7" && coordIsInMatrix(mat, x - 1, y))
                        {
                            mat[y, x - 1] = TOP_LEFT_DECORATION;
                        }
                        if (value == "4" && coordIsInMatrix(mat, x - 1, y))
                        {
                            mat[y, x - 1] = UNDER_RIGHT_DECORATION;
                        }
                    }
                }
            }
            return mat;
        }

        /// <summary>
        /// Check if a co-ordinate is a valid part of the matrix. Handy to avoid crashes.
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool coordIsInMatrix(string[,] mat, int x, int y)
        {
            if (x >= 0 && y >= 0 && x < mat.GetLength(1) && y < mat.GetLength(0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Counts number of walls around neighbours
        /// </summary>
        /// <param name="mat">Matrix</param>
        /// <param name="xPos">Position in X</param>
        /// <param name="yPos">Position in Y</param>
        /// <param name="dist">Distance to count between</param>
        /// <returns>Integer of the number of walls surrounding.</returns>
        public int countNumWallsNeighbors(string[,] mat, int xPos, int yPos, int dist)
        {
            int count = 0;

            //var numbers = Enumerable.Range(-dist , dist + 1);

            for (int _y = -dist; _y <= dist; ++_y)
            {
                for (int _x = -dist; _x <= dist; ++_x)
                {

                    // Boundary
                    if (xPos + _x < 0 || xPos + _x > _numTilesCols - 1 || yPos + _y < 0 || yPos + _y > _numTilesRows - 1)
                    {
                        continue;
                    }

                    // Neighbor is non-wall
                    if (mat[yPos + _y, xPos + _x] != "0")
                        count += 1;

                }
            }

            return count;
        }

        /// <summary>
        /// Returns a string that is comma separated for use with FlxTilemap
        /// </summary>
        /// <param name="multiArray"></param>
        /// <returns></returns>
        public string convertMultiArrayToString(int[,] multiArray)
        {
            string newMap = "";

            for (int i = 0; i < multiArray.GetLength(0); i++)
            {
                for (int j = 0; j < multiArray.GetLength(1); j++)
                {
                    string s = multiArray[i, j].ToString();

                    newMap += s;
                    if (j != multiArray.GetLength(1) - 1)
                    {
                        newMap += ",";
                    }

                }
                newMap += "\n";
            }
            return newMap;

        }

        /// <summary>
        /// Returns a string that is comma separated for use with FlxTilemap
        /// </summary>
        /// <param name="multiArray"></param>
        /// <returns></returns>
        public string convertMultiArrayStringToString(string[,] multiArray)
        {
            string newMap = "";

            for (int i = 0; i < multiArray.GetLength(0); i++)
            {
                for (int j = 0; j < multiArray.GetLength(1); j++)
                {
                    string s = multiArray[i, j].ToString();

                    newMap += s;
                    if (j != multiArray.GetLength(1) - 1)
                    {
                        newMap += ",";
                    }

                }
                newMap += "\n";
            }
            return newMap;

        }


        /// <summary>
        /// Converts a Multi Dimensional Array of Strings to an Integer Array.
        /// </summary>
        /// <param name="multiArray">Multi Dimensional Array</param>
        /// <returns>Integer Array</returns>
        public int[] convertMultiArrayStringToIntArray(string[,] multiArray)
        {
            int total = multiArray.GetLength(0) * multiArray.GetLength(1);

            int[] newMap = new int[total];

            int count = 0;
            for (int i = 0; i < multiArray.GetLength(0); i++)
            {
                for (int j = 0; j < multiArray.GetLength(1); j++)
                {
                    //string s = multiArray[i, j].ToString();
                    int number;
                    bool s = Int32.TryParse(multiArray[i, j], out number);
                    if (s)
                    {
                        newMap[count] = number;
                    }
                    else
                    {
                        newMap[count] = 0;

                    }
                    count++;
                }
            }
            return newMap;
        }

        /// <summary>
        /// Convert Integer Array to Multi Dimensional String Array.
        /// </summary>
        /// <param name="intArray">In Integer Array</param>
        /// <returns>String[,]</returns>
        public string[,] convertIntArrayToMultiArray(int[] intArray)
        {
            string[,] mat = new string[_numTilesRows, _numTilesCols];
            int count = 0;
            for (int _y = 0; _y < _numTilesRows; _y++)
            {
                for (int _x = 0; _x < _numTilesCols; _x++)
                {
                    mat[_y, _x] = _data[count].ToString();
                    count++;
                }
            }
            return mat;
        }

        /// <summary>
        /// Convert a string (comma separated) to a Multi Dimensional Array.
        /// </summary>
        /// <param name="InString">A comma separated string with \n at the end of each line.</param>
        /// <returns>string[,]</returns>
        public string[,] convertStringToMultiArray(string InString)
        {
            string[] cols;
            string[] rows = InString.Split('\n');
            string[] cols1 = rows[0].Split(',');

            //Console.WriteLine("This cave is: " + rows.Length + "  " +  cols1.Length);

            string[,] mat = new string[cols1.Length, rows.Length];

            int count = 0;
            for (int _y = 0; _y < rows.Length; _y++)
            {
                cols = rows[_y].Split(',');

                //Console.WriteLine(cols.ToString() );

                for (int _x = 0; _x < cols.Length; _x++)
                {
                    mat[_x, _y] = cols[_x].ToString();
                    count++;
                }
            }
            
            return mat;
        }


        //addStrings(baseMap, newMap, 32, 4);

        /// <summary>
        /// Adds two strings together. The AddString will overwrite anything in the base.
        /// </summary>
        /// <param name="BaseString">The original string.</param>
        /// <param name="AddString">The base string</param>
        /// <param name="XPos">X position (use if the Addstring is smaller.)</param>
        /// <param name="YPos">X position (use if the Addstring is smaller.)</param>
        /// <param name="Width">Width of the AddString (to make it easy to break apart)</param>
        /// <param name="Height">Height of the AddString (to make it easy to break apart)</param>
        /// <returns>A new string with the AddString in.</returns>
        public string addStrings(string BaseString, string AddString, int XPos, int YPos, int Width, int Height)
        {
            //Console.WriteLine("1");
            string[,] NewBaseString = this.convertStringToMultiArray(BaseString);

            //Console.WriteLine("2:\n" + AddString);
            string[,] NewAddString = this.convertStringToMultiArray(AddString);
            //printCave(NewAddString);


            //this.printCave(NewBaseString);

            int total = NewBaseString.GetLength(0) * NewBaseString.GetLength(1);

            string newMap = "";

            for (int i = 0; i < NewBaseString.GetLength(0); i++)
            {
                for (int j = 0; j < NewBaseString.GetLength(1); j++)
                {
                    if (j >= XPos && j < XPos + Width && i >= YPos && i < YPos + Height)
                    {
                        //newMap += "1";
                        //Console.WriteLine("{0}, {1}, {2}, {3} == {4}, {5}", XPos, YPos, j, i, j - XPos, i - YPos);
                        newMap += NewAddString[i - YPos, j - XPos].ToString();
                    }
                    else
                    {
                        newMap += NewBaseString[j, i].ToString();
                    }
                    if (j != NewBaseString.GetLength(1) - 1)
                    {
                        newMap += ",";
                    }
                }

                newMap += "\n";

            }
            return newMap;

        }

        /// <summary>
        /// Prints a cave multi dimensional array to the console.
        /// </summary>
        /// <param name="tiles">A string[,] of tiles. </param>
        public void printCave(string[,] tiles)
        {
            for (int i = 0; i < tiles.GetLength(1); i++)
            {
                for (int y = 0; y < tiles.GetLength(0); y++)
                {
                    string toPrint = tiles[y, i];

                    if (toPrint.Length == 1)
                    {
                        toPrint += " ";

                    }
                    Console.Write(toPrint);
                }

                Console.WriteLine();
            }
        }

        //end

    }

}

