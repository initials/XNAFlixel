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
        private FlxSprite logo;

        private FlxTilemap tiles;

        override public void create()
        {
            FlxG.hideHud();

            FlxG.backColor = new Color(0xFF, 0xC6, 0x5E);

            base.create();

            string levelData = FlxU.randomString(10);
            FlxG.log("levelData: " + levelData);


            makeCave(0.1f, Color.Red);
            makeCave(0.5f, Color.Blue);
            makeCave2(1.0f, Color.Green);

            logo = new FlxSprite(20, 20 );
            logo.loadGraphic(FlxG.Content.Load<Texture2D>("surt/spaceship_32x32"), true, false, 32,32);
            logo.addAnimation("Static", new int[] { 0 }, 36, true);
            logo.addAnimation("Transform", new int[] { 0, 1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39},36,false);
            logo.play("Static");
            
            logo.setDrags(1100, 1100);
            add(logo);

            FlxG.follow(logo, 10.0f);
            FlxG.followBounds(0, 0, 50*16, 40*16);


        }


        public void makeCave(float Scroll, Color Col)
        {
            // make a new cave of tiles 50x40;
            FlxCaveGenerator cav = new FlxCaveGenerator(50, 40, 0.48f, 5);

            //Create a matrix based on these parameters.
            int[,] matr = cav.generateCaveLevel(3, 0, 2, 0, 1, 0, 1, 0);

            //convert the array to a comma separated string
            string newMap = cav.convertMultiArrayToString(matr);

            //Create a tilemap and assign the cave map.
            tiles = new FlxTilemap();
            tiles.auto = FlxTilemap.AUTO;
            tiles.loadMap(newMap, FlxG.Content.Load<Texture2D>("flixel/autotiles_16x16"), 16, 16);
            tiles.setScrollFactors(Scroll, Scroll);
            tiles.color = Col;
            add(tiles);


        }


        public void makeCave2(float Scroll, Color Col)
        {
            FlxCaveGeneratorExt caveExt = new FlxCaveGeneratorExt(50,40,0.5f, 5);
            string[,] caveLevel = caveExt.generateCaveLevel();

            //Optional step to print cave to the console.
            caveExt.printCave(caveLevel);

            string newMap = caveExt.convertMultiArrayStringToString(caveLevel);

                    

            //Create a tilemap and assign the cave map.
            tiles = new FlxTilemap();
            tiles.auto = FlxTilemap.STRING;
            tiles.loadMap(newMap, FlxG.Content.Load<Texture2D>("flixel/autotiles_16x16"), 16, 16);
            tiles.setScrollFactors(Scroll, Scroll);
            tiles.color = Col;
            add(tiles);


        }

        override public void update()
        {

            int velValue = 500;
            if (FlxG.keys.A)
            {
                logo.velocity.X = velValue * -1;
            }
            else if (FlxG.keys.D)
            {
                logo.velocity.X = velValue;
            }
            else if (FlxG.keys.W)
            {
                logo.velocity.Y = velValue * -1;
            }
            else if (FlxG.keys.S)
            {
                logo.velocity.Y = velValue;
            }
            if (FlxG.keys.Z)
            {
                logo.angle -= 5;
            }
            else if (FlxG.keys.X)
            {
                logo.angle += 5;
            }
            else if (FlxG.keys.SPACE)
            {
                logo.thrust = 500;
            }
            else if (FlxG.keys.L)
            {
                logo.play("Transform");
            }
            else
            {
                logo.thrust = 0;

            }



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
