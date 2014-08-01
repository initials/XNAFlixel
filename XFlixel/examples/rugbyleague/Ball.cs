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
    class Ball : FlxSprite
    {

        FlxSprite shadow;

        public float heightAboveGround;

        public float timeSincePass;

        public Ball(int xPos, int yPos)
            : base(xPos, yPos)
        {
            loadGraphic(FlxG.Content.Load<Texture2D>("examples/ball"), true, false, 8, 8);

            shadow = new FlxSprite(0, 0);
            shadow.loadGraphic(FlxG.Content.Load<Texture2D>("examples/ball"), true, false, 8, 8);
            shadow.color = Color.Black;
            shadow.alpha = 0.5f;

            heightAboveGround = 0;

            // physics
            setDrags(150, 150);

            timeSincePass = 0;

        }

        override public void update()
        {

            if (velocity.Y > 3)
            {
                heightAboveGround = velocity.Y / 20;
            }

            shadow.at(this);
            shadow.x -= heightAboveGround;
            shadow.y += heightAboveGround;
            shadow.update();

            timeSincePass += FlxG.elapsed;

            base.update();

        }

        public override void render(SpriteBatch spriteBatch)
        {
            shadow.render(spriteBatch);

            base.render(spriteBatch);
        }

        public override void overlapped(FlxObject obj)
        {

            base.overlapped(obj);
        }

    }
}
