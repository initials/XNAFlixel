using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics;

namespace org.flixel
{
    public class FarSprite : FlxSprite
    {

        private float ratio = 30;

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
		
        public FarSprite(int xPos, int yPos, World _w)
            : base(xPos, yPos)
        {
            _world = _w;

        }

        override public void update()
        {

            x = _body.Position.X + (width/2);
            y = _body.Position.Y + (height/2);

            angle = (float)(_body.Rotation * (180 / Math.PI));


            base.update();

        }


    }
}
