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
    public class Physics2State : BaseExampleState
    {

        private FlxSprite b1;
        private FlxSprite b2;
        private FlxSprite g1;

        private Body _body1;
        private Body _body2;

        private World _world;
        private Body _ground;


        override public void create()
        {
            base.create();

            FlxG.hideHud();
            //FlxG.resetHud();
            
            _world = new World(new Vector2(0, 98.0f));

            for (int i = 0; i < 40; i++)
            {
                Vector2 v = new Vector2((int)FlxU.random(0, 400), (int)FlxU.random(0, 200));
                FarSprite f = new FarSprite((int)v.X, (int)v.Y, _world);
                f.loadGraphic("initials/crate_80x60", true, false, (int)FlxU.random(10, 80), (int)FlxU.random(10, 60));

                f._body.Mass = 1000f;
                f._body.AngularVelocity = 200f;

                add(f);

            }

            _ground = BodyFactory.CreateRectangle(_world, 2000, 20, 1.0f, new Vector2(0,350), null);
            _ground.BodyType = BodyType.Static;

            g1 = new FlxSprite(0, 340);
            g1.loadGraphic("diagnostic/testpalette", true, false, 1000, 20);
            add(g1);




        }

        public void MyOnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2)
        {

        }
        public bool MyOnCollision(Fixture f1, Fixture f2, Contact contact)
        {
            return true;
        }



        override public void update()
        {




            _world.Step(Math.Min((float)FlxG.elapsedAsGameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));





            base.update();


        }


    }
}
