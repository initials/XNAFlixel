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
    class Player : FlxSprite
    {
        private int jerseyNumber;

        private FlxText jerseyText;

        

        public Player(int xPos, int yPos, int JerseyNumber)
            : base(xPos, yPos)
        {

            jerseyNumber = JerseyNumber;

            jerseyText = new FlxText(0, 0, 50);
            jerseyText.text = jerseyNumber.ToString();
            jerseyText.setFormat(null, 1, Color.White, FlxJustification.Center, Color.Black);

            //loadGraphic(FlxG.Content.Load<Texture2D>("Lemonade/"), true, false, 50, 80);

            //addAnimation("animation", new int[] { 72, 73, 74, 75, 76, 77 }, 12, true);

            //play("animation");
        }

        override public void update()
        {

            jerseyText.update();
            jerseyText.x = x;
            jerseyText.y = y;

            base.update();

        }

        public override void render(SpriteBatch spriteBatch)
        {

            jerseyText.render(spriteBatch);

            base.render(spriteBatch);
        }

    }
}
