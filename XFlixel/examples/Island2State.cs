using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics;


namespace org.flixel
{
    public class Island2State : BaseExampleState
    {
        protected Texture2D ImgDirt;

        private const float FOLLOW_LERP = 3.0f;
        private const int BULLETS_PER_ACTOR = 100;
        private FlxSprite spaceShip;

        private FlxTilemap tiles;

        private FlxGroup tileGrp;
        private FlxSprite m;


        private FlxSprite b1;
        private FlxSprite b2;
        private FlxSprite g1;

        private Body _body1;
        private Body _body2;

        private World _world;
        private Body _ground;



        override public void create()
        {
            FlxG.resetHud();
            FlxG.hideHud();

            FlxG.backColor = Color.DarkTurquoise;

            base.create();

            _world = new World(new Vector2(0, 0));

            FlxCaveGeneratorExt caveExt = new FlxCaveGeneratorExt(20, 20, 0.49f, 1);
            string[,] caveLevel = caveExt.generateCaveLevel();

            //Optional step to print cave to the console.
            //caveExt.printCave(caveLevel);

            tileGrp = new FlxGroup();

            for (int i = 0; i < caveLevel.GetLength(1); i++)
            {
                for (int y = 0; y < caveLevel.GetLength(0); y++)
                {
                    //string toPrint = tiles[y, i];
                    if (Convert.ToInt32(caveLevel[y, i]) != 0)
                    {
                        FarSprite x = new FarSprite(100+(i * 10), 10+(y * 10), _world );
                        //x.createGraphic(8, 8, colors[Convert.ToInt32(caveLevel[y, i])]);
                        x.loadGraphic("flixel/autotilesIsland", false, false, 8, 8);
                        //x.color = colors[Convert.ToInt32(caveLevel[y, i])];

                        x._body.BodyType = BodyType.Dynamic;

                            //_body.BodyType = BodyType.Dynamic;

                        x.frame = Convert.ToInt32(caveLevel[y, i]);
                        //x.scale = 2;
                        //x.angularDrag = 250;
                        //
                        //x.setOffset(4, 4);

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


            _ground = BodyFactory.CreateRectangle(_world, 2000, 20, 1.0f, new Vector2(0, 350), null);
            _ground.BodyType = BodyType.Static;

            //g1 = new FlxSprite(0, 340);
            //g1.loadGraphic("diagnostic/testpalette", true, false, 1000, 20);
            //add(g1);


        }

        override public void update()
        {
            _world.Step(Math.Min((float)FlxG.elapsedAsGameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));


            m.x = FlxG.mouse.x;
            m.y = FlxG.mouse.y;


            FlxU.overlap(tileGrp, m, scaleUp);

            base.update();

            if (FlxG.keys.justPressed(Keys.C))
            {
                foreach (FarSprite item in tileGrp.members)
                {
                    item._body.BodyType = BodyType.Dynamic;
                    item._body.ApplyLinearImpulse(new Vector2(FlxU.random(-13120, 13120), FlxU.random(-13120, 13120)));


                }
            }

            if (FlxG.keys.justPressed(Keys.D))
            {
                FlxG.quake.start(0.003f, 0.2f);
                for (int i = 0; i < 7; i++)
                {
                    int f = (int)FlxU.random(1, tileGrp.members.Count - 1);

                    ((FarSprite)(tileGrp.members[f]))._body.BodyType = BodyType.Dynamic;
                    ((FarSprite)(tileGrp.members[f]))._body.ApplyLinearImpulse(new Vector2(FlxU.random(-13120, 13120), FlxU.random(-13120, 13120)));


                }
            }
            if (FlxG.keys.justPressed(Keys.F))
            {
                _world.Gravity = new Vector2(0, 98);
            }


        }


        protected bool scaleUp(object Sender, FlxSpriteCollisionEvent e)
        {
            //((FlxObject)(e.Object1)).overlapped(e.Object2);
            //((FlxObject)(e.Object2)).overlapped(e.Object1);

            if (((FlxSprite)(e.Object1)).color == Color.Aqua)
                ((FlxSprite)(e.Object1)).scale = 1.2f;

            //((FlxSprite)(e.Object2)).scale += 2;

            return true;
        }


    }
}
