using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

namespace org.flixel
{
    public class CaveState : FlxState
    {
        protected Texture2D ImgDirt;

        private const float FOLLOW_LERP = 3.0f;
        private const int BULLETS_PER_ACTOR = 100;

        private FlxTilemap tiles;





        override public void create()
        {
            FlxG.hideHud();

            FlxG.backColor = new Color(0xFF, 0xC6, 0x5E);

            base.create();

            string levelData = FlxU.randomString(10);
            FlxG.log("levelData: " + levelData);


            makeCave(0.1f, Color.White);
            makeCave(0.5f, Color.Turquoise);
            makeCave(1.0f, Color.Thistle);

            FlxSprite s = new FlxSprite(0, 0, FlxG.Content.Load<Texture2D>("initials/initialsLogo"));
            add(s);
            s.velocity.X = 150;
            s.velocity.Y = 50;

            FlxG.follow(s, 10.0f);
            FlxG.followBounds(0, 0, 2000, 2000);


        }

        public void makeCave(float Scroll, Color Col)
        {
            FlxCaveGenerator cav = new FlxCaveGenerator(50, 40);
            cav.initWallRatio = 0.48f;
            cav.numSmoothingIterations = 5;
            cav.genInitMatrix(50, 40);

            int[,] matr = cav.generateCaveLevel(3, 0, 2, 0, 1, 0, 1, 0);

            string newMap = cav.convertMultiArrayToString(matr);

            Console.WriteLine(newMap);

            tiles = new FlxTilemap();
            tiles.auto = FlxTilemap.AUTO;
            tiles.loadMap(newMap, FlxG.Content.Load<Texture2D>("flixel/autotiles_16x16"), 16, 16);
            tiles.setScrollFactors(Scroll, Scroll);
            tiles.color = Col;
            add(tiles);


        }

        override public void update()
        {

            

            //Toggle the bounding box visibility
            if (FlxG.keys.justPressed(Microsoft.Xna.Framework.Input.Keys.B))
                FlxG.showBounds = !FlxG.showBounds;


            if (FlxG.mouse.pressedRightButton())
            {
                tiles.setTile((int)FlxG.mouse.x / 16, (int)FlxG.mouse.y / 16, 0, true);
            }
            if (FlxG.mouse.pressedLeftButton())
            {
                tiles.setTile((int)FlxG.mouse.x / 16, (int)FlxG.mouse.y / 16, 1, true);
            }




            base.update();
        }




    }
}
