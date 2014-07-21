using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace org.flixel
{

    /// <summary>
    /// This class uses the cellular automata algorithm
    /// to generate very nice caves.
    /// (Coded by Eddie Lee, October 16, 2010)
    /// (Ported to C# by Shane Brouwer)
    /// </summary>
    public class FlxCaveGenerator
    {
        static public int _numTilesCols = 50;
	    static public int _numTilesRows = 50;


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


        public FlxCaveGenerator(int nCols, int nRows)
        {

    		_numTilesCols = nCols;
	    	_numTilesRows = nRows;

        }

        /// <summary>
        /// Generate a matrix of zeroes.
        /// </summary>
        /// <param name="rows">Number of rows for the matrix</param>
        /// <param name="cols">Number of cols for the matrix</param>
        /// <returns>Spits out a matrix that is cols x rows, zero initiated</returns>
        public int[,] genInitMatrix(int rows, int cols)
        {
            // Build array of 1s
            int[,] mat = new int[rows,cols];

            for (int _y = 0; _y < rows; _y++)
            {
                for (int _x = 0; _x < cols; _x++)
                {
                    mat[_y, _x] = 0;
                }

            }

            return mat;
        }

        /// <summary>
        /// Generates a completely random set of 1s and 0s.
        /// </summary>
        /// <returns>Returns a matrix of a cave!</returns>
	    public int[,] generateCaveLevel()
        {
		    // Initialize random array
		
		    int[,] mat = new int[_numTilesRows,_numTilesCols];
 
		    mat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            int floor = _numTilesRows - 2;

            for (int _y = 0; _y < _numTilesRows; _y++) {
                for (int _x = 0; _x < _numTilesCols; _x++){
                    

                    //Throw in a random assortment of ones and zeroes.
                    if (FlxU.random() < initWallRatio)
                    {
                        mat[_y, _x] = 1;
                    }
                    else
                    {
                        mat[_y,_x] = 0;
                    }
                }
            }

		    // Secondary buffer
		    int[,] mat2 = genInitMatrix( _numTilesRows, _numTilesCols );
		
		    // Run automata

            for (int i = 0; i <= numSmoothingIterations; i++)
            {

                runCelluarAutomata(mat, mat2);

                int[,] temp = new int[mat.GetLength(0), mat.GetLength(1)];
                mat = mat2;
                mat2 = temp;

            }

            return mat;
        }

        /// <summary>
        /// Generates a cave level. <para> use a different method with int arrays for more control.</para>
        /// </summary>
        /// <param name="floor">levels of rows on the floor before smoothing</param>
        /// <param name="ceiling">levels of rows on the ceiling before smoothing</param>
        /// <param name="leftWall">left walls before smoothing</param>
        /// <param name="rightWall">right walls before smoothing</param>
        /// <param name="floorPermanent">floors after smoothing</param>
        /// <param name="ceilingPermanent">ceiling rows after smoothing</param>
        /// <param name="leftWallPermanent">left walls after smoothing</param>
        /// <param name="rightWallPermanent">right walls after smoothing</param>
        /// <returns></returns>
        public int[,] generateCaveLevel(int floor, int ceiling, int leftWall, int rightWall, int floorPermanent, int ceilingPermanent, int leftWallPermanent, int rightWallPermanent)
        {
            // Initialize random array

            int[,] mat = new int[_numTilesRows, _numTilesCols];

            mat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            for (int _y = 0; _y < _numTilesRows; _y++)
            {
                for (int _x = 0; _x < _numTilesCols; _x++)
                {
                    //Throw in a random assortment of ones and zeroes.
                    if (FlxU.random() < initWallRatio)
                    {
                        mat[_y, _x] = 1;
                    }
                    else
                    {
                        mat[_y, _x] = 0;
                    }

                    //If you need a floor or walls or ceiling, throw them in here.
                    //Note: These will be smoothed.

                    // Floor
                    if (_y >= _numTilesRows - floor)
                    {
                        mat[_y, _x] = 1;
                    }

                    // Ceiling
                    if (_y < ceiling)
                    {
                        mat[_y, _x] = 1;
                    }

                    // Left wall

                    if (_x < leftWall)
                    {
                        mat[_y, _x] = 1;
                    }

                    if (_x >= _numTilesCols - rightWall)
                    {
                        mat[_y, _x] = 1;
                    }
                }
            }

            // Secondary buffer
            int[,] mat2 = genInitMatrix(_numTilesRows, _numTilesCols);

            // Run automata

            for (int i = 0; i <= numSmoothingIterations; i++)
            {

                runCelluarAutomata(mat, mat2);

                int[,] temp = new int[mat.GetLength(0), mat.GetLength(1)];
                mat = mat2;
                mat2 = temp;

            }

            // This step puts some walls in after the smooth. Important only to run if needed

            if (floorPermanent != 0 || ceilingPermanent != 0 || leftWallPermanent != 0 || rightWallPermanent != 0)
            {
                for (int _y = 0; _y < _numTilesRows; _y++)
                {
                    for (int _x = 0; _x < _numTilesCols; _x++)
                    {
                        // Floor
                        if (_y >= _numTilesRows - floorPermanent)
                        {
                            mat[_y, _x] = 1;
                        }

                        // Ceiling
                        if (_y < ceilingPermanent)
                        {
                            mat[_y, _x] = 1;
                        }

                        // Left wall

                        if (_x < leftWallPermanent)
                        {
                            mat[_y, _x] = 1;
                        }

                        if (_x >= _numTilesCols - rightWallPermanent)
                        {
                            mat[_y, _x] = 1;
                        }
                    }
                }
            }

            return mat;
        }


        /// <summary>
        /// Generates a cave level.
        /// </summary>
        /// <param name="solidRowsBeforeSmooth">int[] array = fill in these rows as solid before the smooth.</param>
        /// <param name="solidColumnsBeforeSmooth">int[] array = fill in these rows as solid before the smooth.</param>
        /// <param name="solidRowsAfterSmooth">int[] array = fill in these rows as solid before the smooth.</param>
        /// <param name="solidColumnsAfterSmooth">int[] array = fill in these rows as solid before the smooth.</param>
        /// <returns></returns>
        public int[,] generateCaveLevel(int[] solidRowsBeforeSmooth, int[] solidColumnsBeforeSmooth, int[] solidRowsAfterSmooth, int[] solidColumnsAfterSmooth)
        {
            // Initialize random array

            int[,] mat = new int[_numTilesRows, _numTilesCols];

            mat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            for (int _y = 0; _y < _numTilesRows; _y++)
            {
                for (int _x = 0; _x < _numTilesCols; _x++)
                {
                    //Throw in a random assortment of ones and zeroes.
                    if (FlxU.random() < initWallRatio)
                    {
                        mat[_y, _x] = 1;
                    }
                    else
                    {
                        mat[_y, _x] = 0;
                    }

                    foreach (int _ySolid in solidRowsBeforeSmooth)
                    {
                        for (int _i = 0; _i <= _numTilesRows; ++_i)
                        {
                            mat[_ySolid, _i] = 1;
                        }   
                    }
                    foreach (int _xSolid in solidColumnsBeforeSmooth)
                    {
                        for (int _i = 0; _i <= _numTilesRows; ++_i)
                        {
                            mat[_i, _xSolid] = 1;
                        }
                    }

                }
            }

            // Secondary buffer
            int[,] mat2 = genInitMatrix(_numTilesRows, _numTilesCols);

            // Run automata

            for (int i = 0; i <= numSmoothingIterations; i++)
            {

                runCelluarAutomata(mat, mat2);

                int[,] temp = new int[mat.GetLength(0), mat.GetLength(1)];
                mat = mat2;
                mat2 = temp;

            }

            foreach (int _ySolid in solidRowsAfterSmooth)
            {
                for (int _i = 0; _i <= _numTilesRows; ++_i)
                {
                    mat[_ySolid, _i] = 1;
                }
            }
            foreach (int _xSolid in solidColumnsAfterSmooth)
            {
                for (int _i = 0; _i <= _numTilesRows; ++_i)
                {
                    mat[_i, _xSolid] = 1;
                }
            }
            
            return mat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inMat"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="fillWith">number to fill path with.</param>
        /// <returns></returns>
        public int[,] drawPath(int[,] inMat, int startX, int startY, int endX, int endY, int fillWith)
        {
            int currentX = startX;
            int currentY = startY;

            int height = endY - startY;
  
            while (currentX != endX || currentY != endY)
            {
                inMat[ currentY , currentX ] = fillWith;

                if (endX < currentX) currentX--;
                if (endX > currentX) currentX++;
                
                if (endY < currentY) currentY--;
                if (endY > currentY) currentY++;

            }

            int[,] mat2 = genInitMatrix(_numTilesRows, _numTilesCols);
            for (int i = 0; i <= numSmoothingIterations; i++)
            {
                runCelluarAutomata(inMat, mat2);
                int[,] temp = new int[inMat.GetLength(0), inMat.GetLength(1)];
                inMat = mat2;
                mat2 = temp;
            }

            return inMat;
        }

        public int[,] addChunks(int[,] inMat, int number, int minSize, int maxSize, int fillWith)
        {
            //int[,] mat = new int[_numTilesRows, _numTilesCols];

            for (int i=0; i<number;i++) 
            {
                int ysize = (int)FlxU.random(minSize, maxSize);
                int xsize = (int)FlxU.random(minSize, maxSize);

                int offsetx = (int)FlxU.random(0, _numTilesCols - xsize);
                int offsety = (int)FlxU.random(0, _numTilesRows - ysize);

                for (int _y = offsety; _y < ysize + offsety; _y++)
                {
                    for (int _x = offsetx; _x < xsize + offsetx; _x++)
                    {
                        if (offsetx < _numTilesCols && offsety < _numTilesRows)
                            inMat[_y , _x ] = fillWith;
                    }
                }
            }

            int[,] mat2 = genInitMatrix(_numTilesRows, _numTilesCols);

            // Run automata
            for (int i = 0; i <= numSmoothingIterations; i++)
            {

                runCelluarAutomata(inMat, mat2);

                int[,] temp = new int[inMat.GetLength(0), inMat.GetLength(1)];
                inMat = mat2;
                mat2 = temp;

            }


            return inMat;
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
        public int[,] generateCaveLevel(int[] solidRowsBeforeSmooth, 
            int[] solidColumnsBeforeSmooth, 
            int[] solidRowsAfterSmooth,
            int[] solidColumnsAfterSmooth,
            int[] emptyRowsBeforeSmooth,
            int[] emptyColumnsBeforeSmooth,
            int[] emptyRowsAfterSmooth,
            int[] emptyColumnsAfterSmooth)
        {
            Console.WriteLine("x/y {0}, {1}", _numTilesCols, _numTilesRows);

            // Initialize random array

            int[,] mat = new int[_numTilesRows, _numTilesCols];

            mat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            for (int _y = 0; _y < _numTilesRows; _y++)
            {
                for (int _x = 0; _x < _numTilesCols; _x++)
                {
                    //Throw in a random assortment of ones and zeroes.
                    if (FlxU.random() < initWallRatio)
                    {
                        mat[_y, _x] = 1;
                    }
                    else
                    {
                        mat[_y, _x] = 0;
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
                            mat[_yEmpty, _i] = 0;
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
                            mat[_i, _xEmpty] = 0;
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
                            mat[_ySolid, _i] = 1;
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
                            mat[_i, _xSolid] = 1;
                    }
                }
            }

            // Secondary buffer
            int[,] mat2 = genInitMatrix(_numTilesRows, _numTilesCols);

            // Run automata
            for (int i = 0; i <= numSmoothingIterations; i++)
            {

                runCelluarAutomata(mat, mat2);

                int[,] temp = new int[mat.GetLength(0), mat.GetLength(1)];
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
                            mat[_yEmpty, _i] = 0;
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
                            mat[_i, _xEmpty] = 0;
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
                            mat[_ySolid, _i] = 1;
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
                            mat[_i, _xSolid] = 1;
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
        public void runCelluarAutomata(int[,] inMat, int[,] outMat)
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
                        outMat[_y, _x] = 1;
                    }
                    else
                    {
                        outMat[_y, _x] = 0;
                    }
                }
            }

        }

        /// <summary>
        /// Counts number of walls around neighbours
        /// </summary>
        /// <param name="mat">Matrix</param>
        /// <param name="xPos">Position in X</param>
        /// <param name="yPos">Position in Y</param>
        /// <param name="dist">Distance to count between</param>
        /// <returns>Integer of the number of walls surrounding.</returns>
        public int countNumWallsNeighbors(int[,] mat, int xPos, int yPos, int dist)
        {
            int count = 0;

            //var numbers = Enumerable.Range(-dist , dist + 1);

            for ( int _y = -dist; _y <= dist; ++_y )
			{
				for ( int _x = -dist; _x <= dist; ++_x )
				{

                    // Boundary
                    if ( xPos + _x < 0 || xPos + _x > _numTilesCols - 1 || yPos + _y < 0 || yPos + _y > _numTilesRows - 1 )
                    {
                        continue;
                    }

                    // Neighbor is non-wall
                    if (mat[yPos + _y, xPos + _x] != 0)
                        count += 1;

                }
            }

            return count;
        }

        /// <summary>
        /// Finds a random solid piece in a Matrix
        /// </summary>
        /// <param name="inMat">Matrix to search</param>
        /// <returns>int[] array {rx, ry} of a solid</returns>
        public int[] findRandomSolid(int[,] inMat)
        {
            //Console.WriteLine("Find Random Solid");

            int numRows = inMat.GetLength(0);
            int numCols = inMat.GetLength(1);

            int n = 0;
            int rx = (int)(FlxU.random() * (numRows - 1));
            int ry = (int)(FlxU.random() * (numCols - 1));
            if (inMat[rx, ry] == 1)
            {
                return new int[] { rx, ry };
            }
            else
            {
                while (n != 1)
                {
                    rx ++;
                    ry ++;
                    if (rx > numRows - 1) rx = (int)(FlxU.random() * (numRows - 1));
                    if (ry > numRows - 1) ry = (int)(FlxU.random() * (numCols - 1));

                    if (inMat[rx, ry] == 1)
                    {
                        n = 1;
                    }
                }
            }

            return new int[] {rx, ry};
        }

        /// <summary>
        /// Finds a random empty spot
        /// </summary>
        /// <param name="inMat">Matrix to search</param>
        /// <returns>int[] array {rx, ry} of a empty spot</returns>
        public int[] findRandomEmpty(int[,] inMat)
        {
            //Console.WriteLine("Find Random Empty");

            int numRows = inMat.GetLength(0);
            int numCols = inMat.GetLength(1);

            int n = 0;
            int rx = 0;
            int ry = 0;

            while (n != 1)
            {
                rx = (int)(FlxU.random() * (numRows-1));
                ry = (int)(FlxU.random() * (numCols-1));

                if (inMat[rx, ry] == 0)
                {
                    n = 1;
                    //Console.WriteLine("X:" + rx + "Y:" + ry);
                }
            }

            return new int[] { rx, ry };
        }

        /// <summary>
        /// Looks for any tiles that are "ground tiles"
        /// ! = new tile.
        /// !00!
        /// 1!!1
        /// 1111
        /// 0000
        /// </summary>
        /// <param name="inMat">multi array to analyse</param>
        /// <param name="ratio">0-1 how much chance of returning a solid block</param>
        /// <returns>multi array that has decorations only</returns>
        public int[,] createDecorationsMap(int[,] inMat, float ratio)
        {
            Console.WriteLine("Create Decorations map");

            int numRows = inMat.GetLength(0);
            int numCols = inMat.GetLength(1);

            int[,] outMat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            for (int _y = 1; _y < numRows - 2; _y++)
            {
                for (int _x = 1; _x < numCols - 1; _x++)
                {
                    // test for flat surface with empty above 
                    if (inMat[_y, _x] == 0 && inMat[_y + 1, _x] == 1 && inMat[_y + 1, _x - 1] == 1 && inMat[_y + 1, _x + 1] == 1)
                    {
                        if (FlxU.random() < ratio) outMat[_y, _x] = 1;
                        else outMat[_y, _x] = 0;
                    }
                    else
                    {
                        outMat[_y, _x] = 0;
                    }
                }
            }
            return outMat;
        }



        /// <summary>
        /// Looks for ground tiles.
        /// </summary>
        /// <param name="inMat"></param>
        /// <param name="ratio">ratio</param>
        /// <returns></returns>
        public int[,] createHangingDecorationsMap(int[,] inMat, float ratio)
        {
            Console.WriteLine("hanging decorations");

            int numRows = inMat.GetLength(0);
            int numCols = inMat.GetLength(1);

            int[,] outMat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            for (int _y = 1; _y < numRows - 2; _y++)
            {
                for (int _x = 1; _x < numCols - 1; _x++)
                {
                    // test for flat surface with empty above 
                    //if (inMat[_y, _x] == 1)
                    //{
                    //    outMat[_y, _x] = 5;
                    //}
                  //if (inMat[_y, _x] == 0 && inMat[_y + 1, _x] == 1 && inMat[_y + 1, _x - 1] == 1 && inMat[_y + 1, _x + 1] == 1)
                    if (inMat[_y, _x] == 0 && inMat[_y - 1, _x] == 1) //&& inMat[_y + 1, _x - 1] == 1 && inMat[_y + 1, _x + 1] == 1
                    {
                        if (FlxU.random() < ratio) outMat[_y, _x] = 1;
                        else outMat[_y, _x] = 0;
                    }
                    
                    else
                    {
                        outMat[_y, _x] = 0;
                    }
                }
            }
            return outMat;
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
        /// Generates a set of ladders going vertically.
        /// </summary>
        /// <returns>Returns a matrix of a ladder level</returns>
        public int[,] generateLadderLevel(int NumberOfLadders, int MinLength, int MaxLength)
        {
            Console.WriteLine("generateLadderLevel");

            // Initialize random array

            int[,] mat = new int[_numTilesRows, _numTilesCols];

            mat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            for (int i = 0; i < NumberOfLadders; i++)
            {
                int length = (int)(FlxU.random(MinLength, MaxLength));
                int _x = (int)(FlxU.random(0, _numTilesRows));
                int _y = (int)(FlxU.random(0, _numTilesCols - length));

                for (int j = 0; j < length; j++)
                {
                    mat[_y + j, _x] = 1;
                }
            }
            return mat;
        }


        public int[,] generateBridgeLevel(int NumberOfLadders, int MinLength, int MaxLength)
        {
            Console.WriteLine("generateBridgeLevel");
            // Initialize random array

            int[,] mat = new int[_numTilesRows, _numTilesCols];

            mat = this.genInitMatrix(_numTilesRows, _numTilesCols);

            for (int i = 0; i < NumberOfLadders; i++)
            {
                int length = (int)(FlxU.random(MinLength, MaxLength));
                int _x = (int)(FlxU.random(0, _numTilesRows));
                int _y = (int)(FlxU.random(0, _numTilesCols - length));

                for (int j = 0; j < length; j++)
                {
                    mat[_y + j, _x] = 1;
                }
            }
            return mat;
        }



        //end

    }

}

