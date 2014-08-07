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
    public class CaveState : BaseExampleState
    {
        protected Texture2D ImgDirt;

        private const float FOLLOW_LERP = 3.0f;
        private const int BULLETS_PER_ACTOR = 100;
        private FlxSprite spaceShip;

        private FlxTilemap tiles;

        override public void create()
        {
            FlxG.resetHud();
            FlxG.showHud();

            FlxG.backColor = Color.LightGray;

            base.create();

            string levelData = FlxU.randomString(10);
            FlxG.log("levelData: " + levelData);

            makeCave(0.1f, Color.Black);
            makeCave(0.5f, new Color(0.98f, 1.0f, 0.95f));
            makeCave2(1.0f, Color.Green);

            spaceShip = new FlxSprite(60, 60 );
            spaceShip.loadGraphic(FlxG.Content.Load<Texture2D>("surt/spaceship_32x32"), true, false, 32,32);
            spaceShip.addAnimation("Static", new int[] { 0 }, 36, true);
            spaceShip.addAnimation("Transform", new int[] { 0, 1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39},36,false);
            spaceShip.play("Static");
            
            spaceShip.setDrags(1100, 1100);
            add(spaceShip);

            FlxG.follow(spaceShip, 10.0f);
            FlxG.followBounds(0, 0, 50*16, 40*16);

        }


        public void makeCave(float Scroll, Color Col)
        {
            // make a new cave of tiles 50x40;
            FlxCaveGenerator cav = new FlxCaveGenerator(50, 40, 0.48f, 5);

            //Create a matrix based on these parameters.
            int[,] matr = cav.generateCaveLevel(3, 0, 2, 0, 1, 1,1,1);

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
            FlxCaveGeneratorExt caveExt = new FlxCaveGeneratorExt(50,40,0.49f, 5);
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
            tiles.boundingBoxOverride = true;
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

            int tile = tiles.getTile((int)FlxG.mouse.x / 16, (int)FlxG.mouse.y / 16);
            FlxG.setHudTextScale(1, 3);
            FlxG.setHudText(1, "The tile index you are hovering over is: " + tile.ToString() );




            base.update();

            //Put the collide after base.update() to avoid flickering.
            FlxU.collide(spaceShip, tiles);

            int velValue = 500;
            if (FlxG.keys.A)
            {
                spaceShip.velocity.X = velValue * -1;
            }
            else if (FlxG.keys.D)
            {
                spaceShip.velocity.X = velValue;
            }
            else if (FlxG.keys.W)
            {
                spaceShip.velocity.Y = velValue * -1;
            }
            else if (FlxG.keys.S)
            {
                spaceShip.velocity.Y = velValue;
            }
            if (FlxG.keys.Z)
            {
                spaceShip.angle -= 5;
            }
            else if (FlxG.keys.X)
            {
                spaceShip.angle += 5;
            }
            else if (FlxG.keys.SPACE)
            {
                spaceShip.thrust = 500;
            }
            else if (FlxG.keys.L)
            {
                spaceShip.play("Transform");
                //FlxG.bloom.Visible = true;
            }
            else
            {
                spaceShip.thrust = 0;
            }


        }




    }
}
