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

            FlxSprite avatar = new FlxSprite(0, FlxG.height-64);
            avatar.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
            avatar.addAnimation("Static", new int[] { 2 }, 0, true);
            avatar.play("Static");
            avatar.setScrollFactors(0, 0);

            add(avatar);

            p = new FlxPath(null);
            addPathPointWithMarker(40.0f, 40.0f, 0);
            addPathPointWithMarker(522.67800098f, 342.106739267f, 1);
            addPathPointWithMarker(582.8467536f, 603.25142048f, 2);
            addPathPointWithMarker(531.13384868f, 263.27163512f, 3);
            addPathPointWithMarker(1447.02681801f, 449.15789055f, 4);
            addPathPointWithMarker(1201.74908191f, 528.33347167f, 5);
            addPathPointWithMarker(147.07640576f, 672.46462447f, 6);
            addPathPointWithMarker(199.62346884f, 584.58110087f, 7);
            addPathPointWithMarker(108.19931517f, 355.52333218f, 8);
            addPathPointWithMarker(698.584671041f, 174.70156601f, 9);

            for (int i = 0; i < 9; i++)
            {
                FlxLine line = new FlxLine(0, 0, new Vector2(p.nodes[i].X, p.nodes[i].Y), new Vector2(p.nodes[i + 1].X, p.nodes[i + 1].Y), Color.White, 2);
                add(line);
            }

            car = new FlxSprite(40, 40);
            car.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
            //car.addAnimation("Static", new int[] { 8 }, 0, true);
            //car.play("Static");
            car.frame = 8;
            car.setDrags(5, 5);
            add(car);

            car.path = p;
            car.pathCornering = 5.0f;
            car.pathAngleOffset = 180;
            //car.pathSpeed = 500.0f;
            car.followPath(p, 250.0f , FlxSprite.PATH_LOOP_FORWARD, true);

            FlxG.follow(car, 5.0f);
            FlxG.followBounds(0, 0, 100 * 16, 100 * 16);


            for (int i = 0; i < 7; i++)
            {

                p = new FlxPath(null);
                p.add(40.0f, 40.0f);
                p.add(522.67800098f + FlxU.random(1, 100), 342.106739267f + FlxU.random(1, 100));
                p.add(582.8467536f, 603.25142048f + FlxU.random(1, 1200));
                p.add(531.13384868f, 263.27163512f + FlxU.random(1, 1200));
                p.add(1447.02681801f, 449.15789055f + FlxU.random(1, 1200));
                p.add(1201.74908191f, 528.33347167f + FlxU.random(1, 1200));
                p.add(147.07640576f, 672.46462447f + FlxU.random(1, 1200));
                p.add(199.62346884f, 584.58110087f + FlxU.random(1, 1200));
                p.add(108.19931517f, 355.52333218f + FlxU.random(1, 1200));
                p.add(698.584671041f, 174.70156601f + FlxU.random(1, 2100));


                car = new FlxSprite(40, 40);
                car.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
                //car.addAnimation("Static", new int[] { 8 }, 0, true);
                //car.play("Static");
                car.frame = 6+i;
                car.setDrags(5, 5);
                add(car);

                car.path = p;
                car.pathCornering = 5.0f;
                car.pathAngleOffset = 180;
                //car.pathSpeed = 500.0f;
                car.followPath(p, 50.0f + (i*30), FlxSprite.PATH_LOOP_FORWARD, true);


            }
            





            

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

            FlxG.setHudText(1, "Car Speed: " + car.pathSpeed.ToString() + "\nCar Cornering: " + car.pathCornering.ToString());

            if (FlxG.keys.DOWN)
            {
                car.pathSpeed -= 10;
            }
            if (FlxG.keys.UP)
            {
                car.pathSpeed += 10;
            }
            if (FlxG.keys.LEFT)
            {
                car.pathCornering -= 0.1f;
            }
            if (FlxG.keys.RIGHT)
            {
                car.pathCornering += 0.1f;
            }




            if (car.pathSpeed > 1300)
            {
                FlxG.quake.start(0.05f, 0.5f);
            }


            base.update();
        }


    }
}
