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
    class FlxScrollingSprite : FlxSprite
    {
        public Vector2 scrollSpeed;
        private Vector2 scrollProgress;

        public FlxScrollingSprite(int xPos, int yPos)
            : base(xPos, yPos)
        {

        }

        override public void update()
        {
            //if (FlxG.keys.LEFT)
            //    _flashRect.X++;
            //if (FlxG.keys.RIGHT)
            //    _flashRect.X--;
            //if (FlxG.keys.UP)
            //    _flashRect.Y++;
            //if (FlxG.keys.DOWN)
            //    _flashRect.Y--;

            scrollProgress += scrollSpeed;

            _flashRect.X = (int)scrollProgress.X;
            _flashRect.Y = (int)scrollProgress.Y;



            base.update();

        }


    }
}
