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
    public class RaceOrDieState : FlxState
    {
        protected Texture2D ImgDirt;

        private const float FOLLOW_LERP = 3.0f;
        private const int BULLETS_PER_ACTOR = 100;
        private FlxSprite logo;

        private FlxTilemap tiles;

        private int velValue;

        override public void create()
        {
            FlxG.hideHud();

            FlxG.backColor = Color.LightGray;

            base.create();

            string levelData = FlxU.randomString(10);
            FlxG.log("levelData: " + levelData);

            //makeCave(0.1f, Color.LightPink);
            //makeCave(0.5f, Color.LightBlue);
            makeCave2(1.0f, Color.Green);

            logo = new FlxSprite(360, 360);
            logo.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            logo.addAnimation("Static", new int[] { 8 }, 0, true);
            logo.play("Static");
            
            logo.setDrags(5, 5);
            add(logo);

            FlxG.follow(logo, 10.0f);
            FlxG.followBounds(0, 0, 150 * 16, 140 * 16);

            velValue = 0;

        }


        public void makeCave(float Scroll, Color Col)
        {
            // make a new cave of tiles 50x40;
            FlxCaveGenerator cav = new FlxCaveGenerator(150, 140, 0.5f, 5);

            //Create a matrix based on these parameters.
            int[,] matr = cav.generateCaveLevel(3, 0, 2, 0, 1, 1, 1, 1);

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
            FlxCaveGeneratorExt caveExt = new FlxCaveGeneratorExt(150, 140, 0.5f, 5);
            string[,] caveLevel = caveExt.generateCaveLevel();

            //Optional step to print cave to the console.
            //caveExt.printCave(caveLevel);

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

            //Put the collide after base.update() to avoid flickering.
            //FlxU.collide(logo, tiles);


            double radians = Math.PI / 180 * logo.angle;

            double velocity_x = Math.Cos((float)radians);
            double velocity_y = Math.Sin((float)radians);
            Console.WriteLine("degrees {0} radians {1} x {2} y {3}", velValue, radians, velocity_x, velocity_y);
            logo.velocity.X = velValue * (float)velocity_x * -1;
            logo.velocity.Y = velValue * (float)velocity_y * -1;



            if (FlxG.keys.A)
            {
                if (FlxG.keys.SPACE) logo.angle -= 5;
                else 
                    logo.angle -= 2;

                

            }
            if (FlxG.keys.D)
            {
                if (FlxG.keys.SPACE) logo.angle += 5;
                else
                    logo.angle += 2;

                
            }
            if (FlxG.keys.W)
            {
                velValue += 15;

            }
            else
            {
                velValue -= 20;
            }

            if (velValue < 0)
            {
                velValue = 0;
            }
            if (FlxG.keys.S)
            {
                velValue -= 10;
            }





        }




    }
}
