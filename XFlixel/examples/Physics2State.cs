using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;
#if WINDOWS 
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics;

namespace org.flixel.examples
{
    public class Physics2State : BaseExampleState
    {

        //private FlxSprite b1;
        private FlxSprite b2;
        private FlxSprite g1;

        //private Body _body1;
        private Body _body2;

        private World _world;
        private Body _ground;


        override public void create()
        {
            base.create();

            FlxG.hideHud();
            //FlxG.resetHud();
            
            _world = new World(new Vector2(0, 98.0f));

            for (int i = 0; i < 10; i++)
            {
                Vector2 v = new Vector2((int)FlxU.random(0, 400), (int)FlxU.random(0, 200));
                FarSprite f = new FarSprite((int)v.X, (int)v.Y, _world);
                f.loadGraphic("flixel/initials/crate_80x60", true, false, (int)FlxU.random(10, 20), (int)FlxU.random(10, 20));
                f._body.Mass = 500f;
                //f._body.AngularVelocity = FlxU.random(-20,20);
                add(f);
            }

            for (int i = 0; i < 10; i++)
            {
                Vector2 v = new Vector2((int)FlxU.random(0, 400), (int)FlxU.random(0, 200));
                FarTileblock f = new FarTileblock((int)v.X, (int)v.Y, (int)FlxU.random(10, 40), (int)FlxU.random(10, 40), _world);
                f.loadTiles("flixel/initials/crate_80x60", 5, 5, 0);
                //f.loadGraphic("flixel/initials/crate_80x60", true, false, (int)FlxU.random(10, 20), (int)FlxU.random(10, 20));
                f._body.Mass = 500f;
                f._body.AngularVelocity = FlxU.random(-20,20);
                f.moves = true;
                add(f);
            }

            _ground = BodyFactory.CreateRectangle(_world, 2000, 20, 1.0f, new Vector2(0,350), null);
            _ground.BodyType = BodyType.Static;

            g1 = new FlxSprite(0, 340);
            g1.loadGraphic("diagnostic/testpalette", true, false, 1000, 20);
            add(g1);




        }

        //public void MyOnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2)
        //{

        //}
        //public bool MyOnCollision(Fixture f1, Fixture f2, Contact contact)
        //{
        //    return true;
        //}



        override public void update()
        {




            _world.Step(Math.Min((float)FlxG.elapsedAsGameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));





            base.update();


        }


    }
}
#endif 