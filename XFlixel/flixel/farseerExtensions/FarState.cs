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
    public class FarState : FlxState
    {
        public World _world;

        override public void create()
        {
            base.create();

            this._world = new World(new Vector2(0, 98.0f));

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
            this._world.Step(Math.Min((float)FlxG.elapsedAsGameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            base.update();
        }


    }
}
