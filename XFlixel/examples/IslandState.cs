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
    public class IslandState : BaseExampleState
    {
        protected Texture2D ImgDirt;

        private const float FOLLOW_LERP = 3.0f;
        private const int BULLETS_PER_ACTOR = 100;
        private FlxSprite spaceShip;

        private FlxTilemap tiles;

        private FlxGroup tileGrp;

        override public void create()
        {
            FlxG.resetHud();
            FlxG.hideHud();

            FlxG.backColor = Color.DarkTurquoise;

            base.create();



            FlxCaveGeneratorExt caveExt = new FlxCaveGeneratorExt(80,120, 0.49f, 1);
            string[,] caveLevel = caveExt.generateCaveLevel();

            //Optional step to print cave to the console.
            //caveExt.printCave(caveLevel);

            Color[] colors = new Color[] { 
                Color.Aqua, 
                Color.ForestGreen, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow,
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Green, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, 
                Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow, Color.Yellow };


            tileGrp = new FlxGroup();

            for (int i = 0; i < caveLevel.GetLength(1); i++)
            {
                for (int y = 0; y < caveLevel.GetLength(0); y++)
                {
                    //string toPrint = tiles[y, i];

                    FlxSprite x = new FlxSprite(y* 8, i * 8);
                    x.createGraphic(6, 6, colors[Convert.ToInt32(caveLevel[y, i])]);
                    tileGrp.add(x);
                    //Console.Write(toPrint);
                }

                //Console.WriteLine();
            }

            //string newMap = caveExt.convertMultiArrayStringToString(caveLevel);


            add(tileGrp);

        }


        public void makeCave(float Scroll, Color Col)
        {
            // make a new cave of tiles 50x40;
            FlxCaveGenerator cav = new FlxCaveGenerator(50, 40, 0.48f, 5);

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
            FlxCaveGeneratorExt caveExt = new FlxCaveGeneratorExt(50, 40, 0.49f, 5);
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

            FlxU.overlap(tileGrp, FlxG.mouse.cursor, scaleUp);

            base.update();


            for (int i = 0; i < 20; i++)
            {
                int f = (int)FlxU.random(1, tileGrp.members.Count- 1);

                //((FlxSprite)(tileGrp.members[f])).y += 8;
                //((FlxSprite)(tileGrp.members[f-1])).y -= 8;

                if (((FlxSprite)(tileGrp.members[f])).color == Color.Green)
                {
                    ((FlxSprite)(tileGrp.members[f])).color = Color.Brown;
                }
                //if (((FlxSprite)(tileGrp.members[f])).color == Color.Brown)
                //{
                //    ((FlxSprite)(tileGrp.members[f-1])).color = Color.Brown;
                //    ((FlxSprite)(tileGrp.members[f-2])).color = Color.Brown;
                //    ((FlxSprite)(tileGrp.members[f+1])).color = Color.Brown;
                //    ((FlxSprite)(tileGrp.members[f+2])).color = Color.Brown;
                //}
            }




        }


        protected bool scaleUp(object Sender, FlxSpriteCollisionEvent e)
        {
            //((FlxObject)(e.Object1)).overlapped(e.Object2);
            //((FlxObject)(e.Object2)).overlapped(e.Object1);

            ((FlxSprite)(e.Object1)).x += 2;
            ((FlxSprite)(e.Object2)).y += 2;

            return true;
        }


    }
}
