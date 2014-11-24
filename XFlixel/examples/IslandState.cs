using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

namespace org.flixel.examples
{
    public class IslandState : BaseExampleState
    {
        protected Texture2D ImgDirt;

        private const float FOLLOW_LERP = 3.0f;
        private const int BULLETS_PER_ACTOR = 100;
        //private FlxSprite spaceShip;

        //private FlxTilemap tiles;

        private FlxGroup tileGrp;
        private FlxSprite m;

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
                    if (Convert.ToInt32(caveLevel[y, i])!=0)
                    {
                        FlxSprite x = new FlxSprite(i * 8, y * 8);
                        //x.createGraphic(8, 8, colors[Convert.ToInt32(caveLevel[y, i])]);
                        x.loadGraphic("flixel/autotilesIsland", false, false, 8, 8);
                        //x.color = colors[Convert.ToInt32(caveLevel[y, i])];

                        x.frame = Convert.ToInt32(caveLevel[y, i]);
                        //x.scale = 2;
                        x.angularDrag = 250;
                        x.setOffset(4, 4);
                        tileGrp.add(x);
                    }
                    //Console.Write(toPrint);
                }

                //Console.WriteLine();
            }

            //string newMap = caveExt.convertMultiArrayStringToString(caveLevel);


            add(tileGrp);

            m = new FlxSprite(0, 0);
            m.loadGraphic("flixel/cursor");
            add(m);
        }

        override public void update()
        {
            m.x = FlxG.mouse.x;
            m.y = FlxG.mouse.y;


            FlxU.overlap(tileGrp, m, scaleUp);

            base.update();


            for (int i = 0; i < 4; i++)
            {
                int f = (int)FlxU.random(1, tileGrp.members.Count - 1);
                if (((FlxSprite)(tileGrp.members[f])).frame > 14)
                    ((FlxSprite)(tileGrp.members[f])).scale = 0.1f;
                

            }

            foreach (FlxSprite item in tileGrp.members)
            {
                if (item.scale < 1)
                {
                    item.scale += 0.02f;
                    item.color = Color.Red;

                }
                else if (item.scale > 1)
                {
                    item.color = Color.White;

                    item.scale = 1;
                }
            }


        }


        protected bool scaleUp(object Sender, FlxSpriteCollisionEvent e)
        {
            //((FlxObject)(e.Object1)).overlapped(e.Object2);
            //((FlxObject)(e.Object2)).overlapped(e.Object1);

            if ( ((FlxSprite)(e.Object1)).color == Color.Aqua)
                ((FlxSprite)(e.Object1)).scale = 1.2f;
            
            //((FlxSprite)(e.Object2)).scale += 2;

            return true;
        }


    }
}
