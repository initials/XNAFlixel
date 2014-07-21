using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace org.flixel
{
    class FlxRamp : FlxSprite
    {

        public int direction;

        public const int LOW_SIDE_LEFT = 1;
        public const int LOW_SIDE_RIGHT = 2;

        public FlxRamp(int xPos, int yPos, int Direction)
            : base(xPos, yPos)
        {
            direction = Direction;

            @fixed = true;
            solid = true;

            //width += 2;
            //height += 2;

            //setOffset(1, 1);

        }

        override public void update()
        {


            base.update();

        }


    }
}
