using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#if WINDOWS 
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics;

namespace org.flixel
{
    public class FarTileblock : FlxTileblock
    {
        public Body _body;

        //public var _fixDef:b2FixtureDef;
        //public var _bodyDef:b2BodyDef
        //public var _obj:b2Body;

        ////References			
        private World _world;

        ////Physics params default value
        //public var _friction:Number = 0.8;
        //public var _restitution:Number = 0.3;
        //public var _density:Number = 0.7;

        public FarTileblock(int X, int Y, int Width, int Height, World _w)
            : base( X,  Y,  Width,  Height)
        {
            _world = _w;

            @fixed = false;

            width = Width;
            height = Height;


            _body = BodyFactory.CreateBody(_world, new Vector2(x, y), 0, null);
            FixtureFactory.AttachRectangle(Width, Height, 1f, new Vector2(0f, 0f), _body);
            _body.BodyType = BodyType.Dynamic;
            _body.Mass = 500f;



        }

        override public void update()
        {

            x = _body.Position.X - (width / 2);
            y = _body.Position.Y - (height / 2);

            angle = (float)(_body.Rotation * (180 / Math.PI));

            base.update();

        }


    }
}
#endif