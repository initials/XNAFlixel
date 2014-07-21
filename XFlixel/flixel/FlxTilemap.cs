using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /// <summary>
    /// Block point struct
    /// </summary>
    public struct BlockPoint
    {
        /// <summary>
        /// X Position
        /// </summary>
        public int x;
        /// <summary>
        /// Y Position
        /// </summary>
        public int y;
        /// <summary>
        /// Data
        /// </summary>
        public int data;
        /// <summary>
        /// Blockpoint
        /// </summary>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y Position</param>
        /// <param name="Data">Data</param>
        public BlockPoint(int X, int Y, int Data)
        {
            x = X;
            y = Y;
            data = Data;
        }
    }

    /// <summary>
    /// This is a traditional tilemap display and collision class.
    /// It takes a string of comma-separated numbers and then associates
    /// those values with tiles from the sheet you pass in.
    /// It also includes some handy static parsers that can convert
    /// arrays or PNG files into strings that can be successfully loaded.
    /// 
    /// </summary>
    public class FlxTilemap : FlxObject
    {
        static public Texture2D ImgAuto;
        static public Texture2D ImgAutoAlt;

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
        public const int RANDOM = 3;

        /// <summary>
        /// Uses a string to choose tiles.
        /// </summary>
        public const int STRING = 4;

        /// <summary>
        /// !! Deprecated !! Now use CollideMin and CollideMax What tile index will you start colliding with (default: 1). 
        /// </summary>
        public int collideIndex;

        /// <summary>
        /// The minimum collidable tile. <value>1</value>
        /// </summary>
        public int collideMin;

        /// <summary>
        /// The last collidable tile <value>999999</value>
        /// </summary>
        public int collideMax;

        /// <summary>
        /// The first index of your tile sheet (default: 0) If you want to change it, do so before calling loadMap().
        /// </summary>
        public int startingIndex;

        /// <summary>
        /// What tile index will you start drawing with (default: 1)  NOTE: should always be >= startingIndex.
        /// If you want to change it, do so before calling loadMap().
        /// </summary>
        public int drawIndex;

        /// <summary>
        /// Set this flag to use one of the 16-tile binary auto-tile algorithms (OFF, AUTO, or ALT).
        /// </summary>
        public int auto;

        /// <summary>
        /// Set this flag to true to force the tilemap buffer to refresh on the next render frame.
        /// </summary>
        public bool refresh;

        /// <summary>
        /// Use this when using FlxTilemap.RANDOM to set the limit of tiles in the image.
        /// </summary>
        public int randomLimit;


        /// <summary>
        /// Read-only variable, do NOT recommend changing after the map is loaded!
        /// </summary>
        public int widthInTiles;
        /// <summary>
        /// Read-only variable, do NOT recommend changing after the map is loaded!
        /// </summary>
        public int heightInTiles;
        /// <summary>
        /// Read-only variable, do NOT recommend changing after the map is loaded!
        /// </summary>
        public int totalTiles;
        /// <summary>
        /// Rendering helper.
        /// </summary>
        protected Rectangle _flashRect;
        /// <summary>
        /// Rendering helper.
        /// </summary>
        protected Rectangle _flashRect2;

        /// <summary>
        /// Stores the number of extra filler tiles in your tile sheet.
        /// </summary>
        protected int _extraMiddleTiles;
        /// <summary>
        /// Integer array of the data building the map.
        /// The value corresponds to the texture's frame number.
        /// 0 = empty block
        /// 1 = block alone
        /// ...
        /// 16 is a solid block in the middle.
        /// </summary>
        protected int[] _data;
        protected List<Rectangle> _rects;
        protected int _tileWidth;
        protected int _tileHeight;
        protected FlxObject _block;
        //protected var _callbacks:Array;
        protected int _screenRows;
        protected int _screenCols;

        /// <summary>
        /// Renders a debug box
        /// </summary>
        protected bool _boundsVisible;

        protected Texture2D _tileBitmap;

        /// <summary>
        /// Used to offset numbers, for instance Tiled uses a non-zero based index system.
        /// </summary>
        public int indexOffset;

        /// <summary>
        /// If the number is below this value, it will be set to 0.
        /// </summary>
        public int stringTileMin;

        /// <summary>
        /// If the number is above this value, it will be set to 0.
        /// </summary>
        public int stringTileMax;

        /// <summary>
        /// color will tint the entire tilemap
        /// </summary>
        public Color color;

        /// <summary>
        /// Rainbow will make the whole map go rainbow.
        /// </summary>
        public bool rainbow = false;

        /// <summary>
        /// The tilemap constructor just initializes some basic variables.
        /// </summary>
        public FlxTilemap()
        {
            color = Color.White;

            if (ImgAuto == null || ImgAutoAlt == null)
            {
                ImgAuto = FlxG.Content.Load<Texture2D>("flixel/autotiles");
                ImgAutoAlt = FlxG.Content.Load<Texture2D>("flixel/autotiles_alt");
            }

            auto = OFF;
            collideIndex = 1;
            collideMin = 1;
            collideMax = int.MaxValue;

            stringTileMax = int.MaxValue;

            startingIndex = 0;
            drawIndex = 1;
            widthInTiles = 0;
            heightInTiles = 0;
            totalTiles = 0;
            _flashRect2 = new Rectangle();
            _flashRect = _flashRect2;
            _data = null;
            _tileWidth = 0;
            _tileHeight = 0;
            _rects = null;
            _block = new FlxObject();
            _block.width = _block.height = 0;
            _block.@fixed = true;
            //_callbacks = new Array();
            @fixed = true;
            moves = false;
            indexOffset = 0;
        }


        /// <summary>
        /// Load the tilemap with string data and a tile graphic.
        /// </summary>
        /// <param name="MapData">A string of comma and line-return delineated indices indicating what order the tiles should go in. <para>If you want to use 0,1,0,1... use FlxTilemap.OFF, FlxTilemap.AUTO or FlxTilemap.ALT</para><para>If you are using tile ids such as 0,23,12,4,1... use FlxTilemap.STRING</para></param>
        /// <param name="TileGraphic">All the tiles you want to use, arranged in a strip corresponding to the numbers in MapData.</param>
        /// <returns>A pointer this instance of FlxTilemap, for chaining as usual :)</returns>
        public FlxTilemap loadMap(string MapData, Texture2D TileGraphic)
        {
            return loadMap(MapData, TileGraphic, 0, 0);
        }

        /// <summary>
        /// Load a TMX Map (Tiled file)
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="Element"></param>
        /// <param name="Name"></param>
        /// <param name="Type"></param>
        /// <param name="TileGraphic"></param>
        /// <param name="TileWidth"></param>
        /// <param name="TileHeight"></param>
        /// <returns></returns>
        public FlxTilemap loadTMXMap(string Filename, string Element, string Name, int Type, Texture2D TileGraphic, int TileWidth, int TileHeight)
        {
            List<Dictionary<string, string>> bgString = FlxXMLReader.readNodesFromTmxFile(Filename,
                Element, 
                Name, 
                Type);


            return loadMap(bgString[0]["csvData"], TileGraphic, TileWidth, TileHeight);
        }

        /// <summary>
        /// Load the tilemap with string data and a tile graphic.
        /// </summary>
        /// <param name="MapData">A string of comma and line-return delineated indices indicating what order the tiles should go in. <para>If you want to use 0,1,0,1... use FlxTilemap.OFF, FlxTilemap.AUTO or FlxTilemap.ALT</para><para>If you are using tile ids such as 0,23,12,4,1... use FlxTilemap.STRING</para></param>
        /// <param name="TileGraphic">All the tiles you want to use, arranged in a strip corresponding to the numbers in MapData.</param>
        /// <param name="TileWidth">The width of your tiles (e.g. 8) - defaults to height of the tile graphic if unspecified.</param>
        /// <param name="TileHeight">The height of your tiles (e.g. 8) - defaults to width if unspecified.</param>
        /// <returns>A pointer this instance of FlxTilemap, for chaining as usual :)</returns>
        public FlxTilemap loadMap(string MapData, Texture2D TileGraphic, int TileWidth, int TileHeight)
        {
            refresh = true;

            _tileBitmap = TileGraphic;

            //Figure out the map dimensions based on the data string
            string[] cols;
            string[] rows = MapData.Split('\n');

            //int xxx = 0;
            //foreach (var item in rows)
            //{
            //    Console.WriteLine(xxx + " " + item);
            //    xxx++;
            //}

            heightInTiles = rows.Length;
            int r = 0;
            int c;

            cols = rows[r].Split(',');
            _data = new int[rows.Length * cols.Length];

            //Console.WriteLine(rows.Length + " " + cols.Length);

            //foreach (var item in _data)
            //    Console.Write(item.ToString() + ",");
            //Console.WriteLine("\n");


            while (r < heightInTiles)
            {
                cols = rows[r++].Split(',');
                if (cols.Length <= 1)
                {
                    heightInTiles = heightInTiles - 1;
                    continue;
                }
                if (widthInTiles == 0)
                    widthInTiles = cols.Length;
                c = 0;
                
                //Console.WriteLine(widthInTiles + " " + heightInTiles);

                while (c < widthInTiles)
                {
                    //int ff = ((r - 1) * widthInTiles) + c;
                    //Console.WriteLine(r + " " + c + " " + ff);
                    _data[((r - 1) * widthInTiles) + c] = int.Parse(cols[c++]); //.push(uint(cols[c++]));
                    
                }
            }

            //now that height and width have been determined, find how many extra 
            //"filler tiles" are at the end of your map.

            int standardLength = TileHeight * TileWidth;
            int graphicWidth = _tileBitmap.Width;
            _extraMiddleTiles = (graphicWidth - standardLength) / TileWidth;

            //Pre-process the map data if it's auto-tiled
            int i;
            totalTiles = widthInTiles * heightInTiles;
            if (auto == AUTO || auto == ALT)
            {
                collideMin = collideIndex = startingIndex = drawIndex = 1;
                i = 0;
                while (i < totalTiles)
                    autoTile(i++);
            }
            if (auto == RANDOM)
            {
                collideMin = collideIndex = startingIndex = drawIndex = 1;
                i = 0;
                while (i < totalTiles)
                    randomTile(i++);
            }

            if (auto == STRING)
            {
                collideMin = collideIndex = startingIndex = drawIndex = 1;
                i = 0;
                while (i < totalTiles)
                    stringTile(i++);
            }

            //Figure out the size of the tiles
            _tileWidth = TileWidth;
            if (_tileWidth == 0)
                _tileWidth = TileGraphic.Height;
            _tileHeight = TileHeight;
            if (_tileHeight == 0)
                _tileHeight = _tileWidth;
            _block.width = _tileWidth;
            _block.height = _tileHeight;

            //Then go through and create the actual map
            width = widthInTiles * _tileWidth;
            height = heightInTiles * _tileHeight;
            _rects = new List<Rectangle>();
            i = 0;
            while (i < totalTiles)
            {
                _rects.Add(Rectangle.Empty);
                updateTile(i++);
            }

            //Pre-set some helper variables for later
            _screenRows = (int)FlxU.ceil((float)FlxG.height / (float)_tileHeight) + 1;
            if (_screenRows > heightInTiles)
                _screenRows = heightInTiles;
            _screenCols = (int)FlxU.ceil((float)FlxG.width / (float)_tileWidth) + 1;
            if (_screenCols > widthInTiles)
                _screenCols = widthInTiles;

            generateBoundingTiles();
            refreshHulls();

            _flashRect.X = 0;
            _flashRect.Y = 0;
            _flashRect.Width = (int)(FlxU.ceil((float)FlxG.width / (float)_tileWidth) + 1) * _tileWidth; ;
            _flashRect.Height = (int)(FlxU.ceil((float)FlxG.height / (float)_tileHeight) + 1) * _tileHeight;

            //foreach (var item in _data)
            //    Console.Write(item.ToString() + ",");
            //Console.WriteLine("\n");

            return this;
        }

        /// <summary>
        /// Generates a bounding box version of the tiles, flixel should call this automatically when necessary.
        /// Warning! No code in here.
        /// </summary>
        protected void generateBoundingTiles()
        {
        }

        public override void update()
        {

            base.update();
        }

        /// <summary>
        /// Draws the tilemap.
        /// </summary>
        /// <param name="spriteBatch"> SpriteBatch </param>
        public override void render(SpriteBatch spriteBatch)
        {
            //NOTE: While this will only draw the tiles that are actually on screen, it will ALWAYS draw one screen's worth of tiles
            Vector2 _p = getScreenXY();
            int bX = (int)Math.Floor(-_p.X / _tileWidth);
            int bY = (int)Math.Floor(-_p.Y / _tileHeight);
            int eX = (int)Math.Floor((-_p.X + FlxG.width - 1) / _tileWidth);
            int eY = (int)Math.Floor((-_p.Y + FlxG.height - 1) / _tileHeight);

            if (bX < 0)
                bX = 0;
            if (bY < 0)
                bY = 0;
            if (eX >= widthInTiles)
                eX = widthInTiles - 1;
            if (eY >= heightInTiles)
                eY = heightInTiles - 1;

            int ri = bY * widthInTiles + bX;
            int cri;

            for (int iy = bY; iy <= eY; iy++)
            {
                cri = ri;
                for (int ix = bX; ix <= eX; ix++)
                {
                    if (_rects[cri] != Rectangle.Empty)
                    {
                        // Render the color too;

                        if (rainbow)
                        {
                            color = new Color(FlxU.random(0.15,1), FlxU.random(0.15,1), FlxU.random(0.15,1));
                        }

                        spriteBatch.Draw(_tileBitmap,
                            new Rectangle((ix * _tileWidth) + (int)Math.Floor(FlxG.scroll.X * scrollFactor.X), (iy * _tileHeight) + (int)Math.Floor(FlxG.scroll.Y * scrollFactor.Y), _tileWidth, _tileHeight),
                            _rects[iy * widthInTiles + ix],
                            color);
                        
                        if (FlxG.showBounds && boundingBoxOverride==true)
                        {
                            spriteBatch.Draw(FlxG.XnaSheet,
                            new Rectangle((ix * _tileWidth) + (int)Math.Floor(FlxG.scroll.X * scrollFactor.X), (iy * _tileHeight) + (int)Math.Floor(FlxG.scroll.Y * scrollFactor.Y), _tileWidth, _tileHeight),
                            _rects[iy * widthInTiles + ix],
                            getBoundingColor());
                        }

                    }
                    cri++;
                }
                ri += widthInTiles;
            }

        }

        /// <summary>
        /// Checks for overlaps between the provided object and any tiles above the collision index.
        /// </summary>
        /// <param name="Core">The <code>FlxObject</code> you want to check against.</param>
        /// <returns>True if overlap occurs, otherwise False</returns>
        override public bool overlaps(FlxObject Core)
        {
            int d;

            int dd;
            List<BlockPoint> blocks = new List<BlockPoint>();

            //First make a list of all the blocks we'll use for collision
            int ix = (int)FlxU.floor((Core.x - x) / _tileWidth);
            int iy = (int)FlxU.floor((Core.y - y) / _tileHeight);
            int iw = (int)FlxU.ceil((float)Core.width / (float)_tileWidth) + 1;
            int ih = (int)FlxU.ceil((float)Core.height / (float)_tileHeight) + 1;
            int r = 0;
            int c;
            while (r < ih)
            {
                if (r >= heightInTiles) break;
                d = (iy + r) * widthInTiles + ix;
                c = 0;
                while (c < iw)
                {
                    if (c >= widthInTiles) break;
                    dd = _data[d + c];
                    if (dd >= collideMin && dd>=collideMax)
                        blocks.Add(new BlockPoint((int)(x + (ix + c) * _tileWidth), (int)(y + (iy + r) * _tileHeight), dd));
                    c++;
                }
                r++;
            }

            //Then check for overlaps
            int bl = blocks.Count;
            int i = 0;
            while (i < bl)
            {
                _block.x = blocks[i].x;
                _block.y = blocks[i++].y;
                if (_block.overlaps(Core))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Checks to see if a point in 2D space overlaps a solid tile.
        /// </summary>
        /// <param name="X">The X coordinate of the point.</param>
        /// <param name="Y">The Y coordinate of the point.</param>
        /// <returns>Whether or not the point overlaps this object.</returns>
        override public bool overlapsPoint(float X, float Y)
        {
            return overlapsPoint(X, Y, false);
        }

        /// <summary>
        /// Checks to see if a point in 2D space overlaps a solid tile.
        /// </summary>
        /// <param name="X">The X coordinate of the point.</param>
        /// <param name="Y">The Y coordinate of the point.</param>
        /// <param name="PerPixel"></param>
        /// <returns>Whether or not the point overlaps this object.</returns>
        override public bool overlapsPoint(float X, float Y, bool PerPixel)
        {
            return getTile((int)((X - x) / _tileWidth), (int)((Y - y) / _tileHeight)) >= this.collideIndex;
        }


        /// <summary>
        /// Called by <code>FlxObject.updateMotion()</code> and some constructors to
        /// rebuild the basic collision data for this object.
        /// </summary>
        override public void refreshHulls()
        {
            colHullX.x = 0;
            colHullX.y = 0;
            colHullX.width = _tileWidth;
            colHullX.height = _tileHeight;
            colHullY.x = 0;
            colHullY.y = 0;
            colHullY.width = _tileWidth;
            colHullY.height = _tileHeight;
        }

        /// <summary>
        /// <code>FlxU.collide()</code> (and thus <code>FlxObject.collide()</code>) call
        /// this function each time two objects are compared to see if they collide.
        /// It doesn't necessarily mean these objects WILL collide, however.
        /// </summary>
        /// <param name="Object">The <code>FlxObject</code> you're about to run into.</param>
        override public void preCollide(FlxObject Object)
        {
            //Collision fix, in case updateMotion() is called
            colHullX.x = 0;
            colHullX.y = 0;
            colHullY.x = 0;
            colHullY.y = 0;

            int r;
            int c;
            int rs;
            int ix = (int)FlxU.floor((Object.x - x) / _tileWidth);
            int iy = (int)FlxU.floor((Object.y - y) / _tileHeight);
            int iw = ix + (int)FlxU.ceil((float)Object.width / (float)_tileWidth) + 1;
            int ih = iy + (int)FlxU.ceil((float)Object.height / (float)_tileHeight) + 1;
            if (ix < 0)
                ix = 0;
            if (iy < 0)
                iy = 0;
            if (iw > widthInTiles)
                iw = widthInTiles;
            if (ih > heightInTiles)
                ih = heightInTiles;
            rs = iy * widthInTiles;
            r = iy;
            colOffsets.Clear();
            while (r < ih)
            {
                c = ix;
                while (c < iw)
                {
                    if (_data[rs + c] >= collideMin && _data[rs + c] <= collideMax)
                    {
                        colOffsets.Add(new Vector2(x + c * _tileWidth, y + r * _tileHeight));
                    }
                    c++;
                }
                rs += widthInTiles;
                r++;
            }
        }


        /// <summary>
        /// Check the value of a particular tile.
        /// </summary>
        /// <param name="X">The X coordinate of the tile (in tiles, not pixels).</param>
        /// <param name="Y">The Y coordinate of the tile (in tiles, not pixels).</param>
        /// <returns>A uint containing the value of the tile at this spot in the array.</returns>
        public int getTile(int X, int Y)
        {
            return getTileByIndex(Y * widthInTiles + X);
        }

        /// <summary>
        /// Get the value of a tile in the tilemap by index.
        /// </summary>
        /// <param name="Index">The slot in the data array (Y * widthInTiles + X) where this tile is stored.</param>
        /// <returns>A uint containing the value of the tile at this spot in the array.</returns>
        public int getTileByIndex(int Index)
        {
            if (Index < 0 || Index >= _data.Length) return -1;
            return _data[Index];
        }


        /// <summary>
        /// Change the data and graphic of a tile in the tilemap.
        /// </summary>
        /// <param name="X">The X coordinate of the tile (in tiles, not pixels).</param>
        /// <param name="Y">The Y coordinate of the tile (in tiles, not pixels).</param>
        /// <param name="Tile">The new integer data you wish to inject.</param>
        /// <returns>Whether or not the tile was actually changed.</returns>

        public bool setTile(int X, int Y, int Tile)
        {
            if ((X >= widthInTiles) || (Y >= heightInTiles))
                return false;
            return setTileByIndex(Y * widthInTiles + X, Tile, true);
        }
        /// <summary>
        /// Change the data and graphic of a tile in the tilemap.
        /// </summary>
        /// <param name="X">The X coordinate of the tile (in tiles, not pixels).</param>
        /// <param name="Y">The Y coordinate of the tile (in tiles, not pixels).</param>
        /// <param name="Tile">The new integer data you wish to inject.</param>
        /// <param name="UpdateGraphics">Whether the graphical representation of this tile should change.</param>
        /// <returns>Whether or not the tile was actually changed.</returns>
        public bool setTile(int X, int Y, int Tile, bool UpdateGraphics)
        {
            if ((X >= widthInTiles) || (Y >= heightInTiles))
                return false;
            return setTileByIndex(Y * widthInTiles + X, Tile, UpdateGraphics);
        }

        /// <summary>
        /// Change the data and graphic of a tile in the tilemap.
        /// </summary>
        /// <param name="Index">The slot in the data array (Y * widthInTiles + X) where this tile is stored.</param>
        /// <param name="Tile">The new integer data you wish to inject.</param>
        /// <param name="UpdateGraphics">Whether the graphical representation of this tile should change.</param>
        /// <returns>Whether or not the tile was actually changed.</returns>
        public bool setTileByIndex(int Index, int Tile, bool UpdateGraphics)
        {
            if (Index >= _data.Length)
                return false;

            bool ok = true;
            _data[Index] = Tile;

            if (!UpdateGraphics)
                return ok;

            refresh = true;

            if (auto == OFF || auto == RANDOM || auto==STRING)
            {
                updateTile(Index);
                return ok;
            }

            //If this map is autotiled and it changes, locally update the arrangement
            if (auto == AUTO || auto == ALT)
            {
                int i;
                int r = (int)(Index / widthInTiles) - 1;
                int rl = r + 3;
                int c = Index % widthInTiles - 1;
                int cl = c + 3;
                while (r < rl)
                {
                    c = cl - 3;
                    while (c < cl)
                    {
                        if ((r >= 0) && (r < heightInTiles) && (c >= 0) && (c < widthInTiles))
                        {
                            i = r * widthInTiles + c;
                            autoTile(i);
                            updateTile(i);
                        }
                        c++;
                    }
                    r++;
                }
            }

            



            return ok;
        }

        /// <summary>
        /// Bind a function Callback(Core:FlxCore,X:uint,Y:uint,Tile:uint) to a range of tiles.
        /// Temporarily deprecated.
        /// </summary>
        /// <param name="Tile">The tile to trigger the callback.</param>
        /// <param name="Callback">The function to trigger.  Parameters should be <code>(Core:FlxCore,X:uint,Y:uint,Tile:uint)</code>.</param>
        /// <param name="Range">If you want this callback to work for a bunch of different tiles, input the range here.  Default value is 1.</param>
        public void setCallback(int Tile, int Callback, int Range)
        {
            FlxG.log("WARNING: FlxTilemap.setCallback()\nhas been temporarily deprecated.");
            //if(Range <= 0) return;
            //for(var i:uint = Tile; i < Tile+Range; i++)
            //	_callbacks[i] = Callback;
        }
        /// <summary>
        /// Follow an object
        /// </summary>
        public void follow()
        {
            follow(0);
        }
        /// <summary>
        /// Call this function to lock the automatic camera to the map's edges.
        /// </summary>
        /// <param name="Border">Adjusts the camera follow boundary by whatever number of tiles you specify here.  Handy for blocking off deadends that are offscreen, etc.  Use a negative number to add padding instead of hiding the edges.</param>
        public void follow(int Border)
        {
            FlxG.followBounds((int)x + Border * _tileWidth, (int)y + Border * _tileHeight, (int)width - Border * _tileWidth, (int)height - Border * _tileHeight);
        }

        /// <summary>
        /// Shoots a ray from the start point to the end point.
        /// If/when it passes through a tile, it stores and returns that point.
        /// </summary>
        /// <param name="StartX">The X component of the ray's start.</param>
        /// <param name="StartY">The Y component of the ray's start.</param>
        /// <param name="EndX">The X component of the ray's end.</param>
        /// <param name="EndY">The Y component of the ray's end.</param>
        /// <param name="Result">A <code>Point</code> object containing the first wall impact.</param>
        /// <param name="Resolution">Defaults to 1, meaning check every tile or so.  Higher means more checks!</param>
        /// <returns>Whether or not there was a collision between the ray and a colliding tile.</returns>
        public bool ray(int StartX, int StartY, int EndX, int EndY, Vector2 Result, int Resolution)
        {
            int step = _tileWidth;
            if (_tileHeight < _tileWidth)
                step = _tileHeight;
            step /= Resolution;
            int dx = EndX - StartX;
            //if (dx == 0) return false;
            int dy = EndY - StartY;
            //if (dy == 0) return false;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            int steps = (int)FlxU.ceil(distance / step);
            int stepX = dx / steps;
            int stepY = dy / steps;
            int curX = StartX - stepX;
            int curY = StartY - stepY;
            int tx;
            int ty;
            int i = 0;
            

            while (i < steps)
            {
                curX += stepX;
                curY += stepY;

                if ((curX < 0) || (curX > width) || (curY < 0) || (curY > height))
                {
                    i++;
                    continue;
                }

                tx = curX / _tileWidth;
                ty = curY / _tileHeight;
                if (_data[ty * widthInTiles + tx] >= collideMin && _data[ty * widthInTiles + tx] <= collideMax)
                {
                    //Some basic helper stuff
                    tx *= _tileWidth;
                    ty *= _tileHeight;
                    int rx = 0;
                    int ry = 0;
                    int q;
                    int lx = curX - stepX;
                    int ly = curY - stepY;

                    //Figure out if it crosses the X boundary
                    q = tx;
                    if (dx < 0)
                        q += _tileWidth;
                    rx = q;
                    ry = ly + stepY * ((q - lx) / stepX);
                    if ((ry > ty) && (ry < ty + _tileHeight))
                    {
                        Console.WriteLine("X Result: {0} {1}", rx, ry);
                        Result.X = rx;
                        Result.Y = ry;
                        return true;
                    }

                    //Else, figure out if it crosses the Y boundary
                    q = ty;
                    if (dy < 0)
                        q += _tileHeight;
                    rx = lx + stepX * ((q - ly) / stepY);
                    ry = q;
                    if ((rx > tx) && (rx < tx + _tileWidth))
                    {
                        Console.WriteLine("Y Result: {0} {1}", rx, ry);
                        Result.X = rx;
                        Result.Y = ry;
                        return true;
                    }
                    return false;
                }
                i++;
            }
            return false;
        }

        /// <summary>
        /// Converts a one-dimensional array of tile data to a comma-separated string.
        /// </summary>
        /// <param name="Data">An array full of integer tile references.</param>
        /// <param name="Width">The number of tiles in each row.</param>
        /// <returns>A comma-separated string containing the level data in a <code>FlxTilemap</code>-friendly format.</returns>
        static public string arrayToCSV(int[] Data, int Width)
        {
            int r = 0;
            int c;
            string csv = "";
            int Height = Data.Length / Width;
            while (r < Height)
            {
                c = 0;
                while (c < Width)
                {
                    if (c == 0)
                    {
                        if (r == 0)
                            csv += Data[0];
                        else
                            csv += "\n" + Data[r * Width];
                    }
                    else
                        csv += ", " + Data[r * Width + c];
                    c++;
                }
                r++;
            }
            return csv;
        }

        /// <summary>
        ///  Converts a <code>BitmapData</code> object to a comma-separated string.
        ///  Black pixels are flagged as 'solid' by default,
        /// non-black pixels are set as non-colliding.
        /// Black pixels must be PURE BLACK.
        /// </summary>
        /// <param name="bitmapData">A Texture2D, preferably black and white.</param>
        /// <returns>A comma-separated string containing the level data in a <code>FlxTilemap</code>-friendly format.</returns>
        static public string bitmapToCSV(Texture2D bitmapData)
        {
            return bitmapToCSV(bitmapData, false);
        }
        /// <summary>
        ///  Converts a <code>BitmapData</code> object to a comma-separated string.
        ///  Black pixels are flagged as 'solid' by default,
        /// non-black pixels are set as non-colliding.
        /// Black pixels must be PURE BLACK.
        /// </summary>
        /// <param name="bitmapData">A Texture2D, preferably black and white.</param>
        /// <param name="Invert">Load white pixels as solid instead.</param>
        /// <returns>A comma-separated string containing the level data in a <code>FlxTilemap</code>-friendly format.</returns>
        static public string bitmapToCSV(Texture2D bitmapData, bool Invert)
        {
            //Walk image and export pixel values
            int r = 0;
            int c;
            uint p;
            string csv = "";
            int w = bitmapData.Width;
            int h = bitmapData.Height;
            uint[] _bitData = new uint[1];

            bitmapData.GetData<uint>(_bitData);

            while (r < h)
            {
                c = 0;
                while (c < w)
                {
                    //Decide if this pixel/tile is solid (1) or not (0)
                    p = _bitData[(r * w) + c];
                    if ((Invert && (p > 0)) || (!Invert && (p == 0)))
                        p = 1;
                    else
                        p = 0;

                    //Write the result to the string
                    if (c == 0)
                    {
                        if (r == 0)
                            csv += p;
                        else
                            csv += "\n" + p;
                    }
                    else
                        csv += ", " + p;
                    c++;
                }
                r++;
            }
            return csv;
        }

        /// <summary>
        /// An internal function used by the binary auto-tilers.
        /// </summary>
        /// <param name="Index">The index of the tile you want to analyze.</param>
        protected void autoTile(int Index)
        {
            if (_data[Index] == 0) return;
            _data[Index] = 0;
            if ((Index - widthInTiles < 0) || (_data[Index - widthInTiles] > 0)) 		//UP
                _data[Index] += 1;
            if ((Index % widthInTiles >= widthInTiles - 1) || (_data[Index + 1] > 0)) 		//RIGHT
                _data[Index] += 2;
            if ((Index + widthInTiles >= totalTiles) || (_data[Index + widthInTiles] > 0)) //DOWN
                _data[Index] += 4;
            if ((Index % widthInTiles <= 0) || (_data[Index - 1] > 0)) 					//LEFT
                _data[Index] += 8;
            if ((auto == ALT) && (_data[Index] == 15))	//The alternate algo checks for interior corners
            {
                if ((Index % widthInTiles > 0) && (Index + widthInTiles < totalTiles) && (_data[Index + widthInTiles - 1] <= 0))
                    _data[Index] = 1;		//BOTTOM LEFT OPEN
                if ((Index % widthInTiles > 0) && (Index - widthInTiles >= 0) && (_data[Index - widthInTiles - 1] <= 0))
                    _data[Index] = 2;		//TOP LEFT OPEN
                if ((Index % widthInTiles < widthInTiles - 1) && (Index - widthInTiles >= 0) && (_data[Index - widthInTiles + 1] <= 0))
                    _data[Index] = 4;		//TOP RIGHT OPEN
                if ((Index % widthInTiles < widthInTiles - 1) && (Index + widthInTiles < totalTiles) && (_data[Index + widthInTiles + 1] <= 0))
                    _data[Index] = 8; 		//BOTTOM RIGHT OPEN
            }
            _data[Index] += 1;

            if (_extraMiddleTiles >= 1 && _data[Index] == 16)
            {
                _data[Index] += (int)(FlxU.random() * (_extraMiddleTiles+1));
            }
        }

        /// <summary>
        /// An internal function used by the binary auto-tilers.
        /// </summary>
        /// <param name="Index">The index of the tile you want to analyze.</param>
        protected void randomTile(int Index)
        {
            if (_data[Index] == 0) return;
            _data[Index] = (int)(FlxU.random() * randomLimit);
        }

        /// <summary>
        /// An internal function used by the binary auto-tilers.
        /// </summary>
        /// <param name="Index">The index of the tile you want to analyze.</param>
        protected void stringTile(int Index)
        {
            if (_data[Index] <= 0) 
            {
                _data[Index] = 0;
                return;
            }
            if (_data[Index] > stringTileMax)
            {
                _data[Index] = 0;
                return;
            }
            if (_data[Index] <= stringTileMin)
            {
                _data[Index] = 0;
                return;
            }
            _data[Index] += 1;
            _data[Index] += indexOffset;
        }

        /// <summary>
        /// Internal function used in setTileByIndex() and the constructor to update the map.
        /// </summary>
        /// <param name="Index">The index of the tile you want to update.</param>
        protected void updateTile(int Index)
        {
            if (_data[Index] < drawIndex)
            {
                _rects[Index] = Rectangle.Empty;
                return;
            }
            int rx = (_data[Index] - startingIndex) * _tileWidth;
            int ry = 0;
            if (rx >= _tileBitmap.Width)
            {
                ry = (int)(rx / _tileBitmap.Width) * _tileHeight;
                rx %= _tileBitmap.Width;
            }
            _rects[Index] = (new Rectangle(rx, ry, _tileWidth, _tileHeight));
        }

    }
}
