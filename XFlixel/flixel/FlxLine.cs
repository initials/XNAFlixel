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
    public class FlxLine : FlxSprite
    {
        public Vector2 startPos;
        public Vector2 endPos;
        //public Color color;
        public float lineWidth;


        public FlxLine(int xPos, int yPos, Vector2 StartPos, Vector2 EndPos, Color Color, float LineWidth)
            : base(xPos, yPos)
        {
            startPos = StartPos;
            endPos = EndPos;
            color = Color;
            lineWidth = LineWidth;
        }

        override public void update()
        {


            base.update();

        }

        public override void render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(startPos, endPos, color, lineWidth);
            //base.render(spriteBatch);
        }


    }
}
