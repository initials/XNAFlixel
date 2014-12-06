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
    public class FlxScrollingSprite : FlxSprite
    {
        public Vector2 scrollSpeed;
        public Vector2 scrollMultiplier;
        private Vector2 scrollProgress;

        public bool allowScrolling = true;

        public FlxScrollingSprite(int xPos, int yPos)
            : base(xPos, yPos)
        {
            scrollMultiplier = new Vector2(1,1);
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

            //Console.WriteLine(is );


            if (allowScrolling)
            {

                scrollProgress += scrollSpeed * scrollMultiplier;

                _flashRect.X = (int)scrollProgress.X;
                _flashRect.Y = (int)scrollProgress.Y;
            }


            base.update();

        }


    }
}
