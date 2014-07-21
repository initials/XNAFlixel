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
    /// <summary>
    /// FlxMovingPlatform is just so you don't forget to set to solid and @fixed
    /// </summary>
    class FlxMovingPlatform : FlxSprite
    {

        public FlxMovingPlatform(int xPos, int yPos)
            : base(xPos, yPos)
        {

            solid = true;
            @fixed = true;

        }

        override public void update()
        {


            base.update();

        }


    }
}
