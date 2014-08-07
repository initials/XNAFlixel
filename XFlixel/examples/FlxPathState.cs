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
    public class FlxPathState : BaseExampleState
    {
        FlxSprite car;
        FlxPath p;

        FlxTilemap tiles;

        override public void create()
        {
            base.create();

            FlxG.resetHud();

            makeCave(1.0f, Color.White);

            car = new FlxSprite(40, 40);
            car.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            car.addAnimation("Static", new int[] { 8 }, 0, true);
            car.play("Static");

            car.setDrags(5, 5);
            add(car);

            p = new FlxPath(null);
            addPathPointWithMarker(40.0f, 40.0f, 0);
            addPathPointWithMarker(222.67800098f, 342.106739267f, 1);
            addPathPointWithMarker(398.8467536f, 103.25142048f, 2);
            addPathPointWithMarker(531.13384868f, 263.27163512f, 3);
            addPathPointWithMarker(447.02681801f, 449.15789055f, 4);
            addPathPointWithMarker(291.74908191f, 528.33347167f, 5);
            addPathPointWithMarker(147.07640576f, 672.46462447f, 6);
            addPathPointWithMarker(199.62346884f, 584.58110087f, 7);
            addPathPointWithMarker(108.19931517f, 355.52333218f, 8);
            addPathPointWithMarker(698.584671041f, 174.70156601f, 9);


            car.path = p;
            car.pathCornering = 5.0f;
            car.pathAngleOffset = 180;
            //car.pathSpeed = 500.0f;
            car.followPath(p, 20.0f, FlxSprite.PATH_LOOP_FORWARD, true);


            FlxG.follow(car, 5.0f);
            FlxG.followBounds(0, 0, 1500 * 16, 1400 * 16);


        }

        public void addPathPointWithMarker(float xPos, float yPos, int MarkerNumber)
        {
            p.add(xPos, yPos);

            FlxText t = new FlxText(xPos, yPos, 100);
            t.alignment = FlxJustification.Left;
            t.text = MarkerNumber.ToString();
            t.setScrollFactors(1, 1);
            add(t);
        }

        public void makeCave(float Scroll, Color Col)
        {
            // make a new cave of tiles 50x40;
            FlxCaveGenerator cav = new FlxCaveGenerator(100, 100, 0.48f, 1);

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


        override public void update()
        {

            FlxG.setHudText(1, "Car Speed: " + car.pathSpeed.ToString());

            if (FlxG.keys.DOWN)
            {
                car.pathSpeed -= 10;
            }
            if (FlxG.keys.UP)
            {
                car.pathSpeed += 10;
            }

            if (car.pathSpeed > 1300)
            {
                FlxG.quake.start(0.05f, 0.5f);
            }


            base.update();
        }


    }
}
