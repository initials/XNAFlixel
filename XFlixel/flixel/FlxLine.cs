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
            Vector2 pos = Vector2.Zero;
            Vector2 vc = Vector2.Zero;

            pos = getScreenXY() + origin;
            pos += (new Vector2(_flashRect.Width - width, _flashRect.Height - height)
                * (origin / new Vector2(width, height)));

            spriteBatch.DrawLine(startPos + pos, endPos + pos, color, lineWidth);
            //base.render(spriteBatch);
        }


    }
}
