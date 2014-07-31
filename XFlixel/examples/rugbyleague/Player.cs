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
        private FlxSprite selectedPlayerIcon;

        public bool isSelected;
        

        public Player(int xPos, int yPos, int JerseyNumber)
            : base(xPos, yPos)
        {

            jerseyNumber = JerseyNumber;

            jerseyText = new FlxText(xPos, yPos, 50);
            jerseyText.text = jerseyNumber.ToString();
            jerseyText.setFormat(null, 1, Color.White, FlxJustification.Center, Color.Black);
            jerseyText.setScrollFactors(1, 1);

            selectedPlayerIcon = new FlxSprite(xPos, yPos);
            selectedPlayerIcon.loadGraphic(FlxG.Content.Load<Texture2D>("examples/selectedPlayerIcon"), true, false, 48, 48);
            selectedPlayerIcon.addAnimation("selected", new int[] { 0, 1 }, 12, true);
            selectedPlayerIcon.play("selected");



            loadGraphic(FlxG.Content.Load<Texture2D>("examples/running"), true, false, 32, 32);

            addAnimation("idle", new int[] { 0,1,2,3,4,5,6,7,8,9 }, 12, true);
            addAnimation("run", new int[] { 53, 54, 55, 56, 57, 58, 59, 60,61,62,63,64,65,66,67,68,69,70,71 }, 24, true);
            play("idle");

            setDrags(1350, 1350);

            isSelected = false;

        }

        override public void update()
        {
            if (isSelected)
            {
                if (FlxControl.LEFT)
                {
                    this.velocity.X = -550;
                }
                if (FlxControl.RIGHT)
                {
                    this.velocity.X = 550;
                }
                if (FlxControl.UP)
                {
                    this.velocity.Y = -550;
                }
                if (FlxControl.DOWN)
                {
                    this.velocity.Y = 550;
                }
            }





            if (isSelected == true) { 
                play("run");
                //flicker(555);
            }
            else {
                play("idle");
                //flicker(0.001f);
            }

            selectedPlayerIcon.at(this);
            selectedPlayerIcon.x -= 8;
            selectedPlayerIcon.y -= 8;
            selectedPlayerIcon.update();

            jerseyText.at(this); 
            jerseyText.x -=8;
            jerseyText.y +=24;
            jerseyText.update();
            base.update();

        }

        public override void render(SpriteBatch spriteBatch)
        {

            if (isSelected)
            {
                selectedPlayerIcon.render(spriteBatch);
            }

            base.render(spriteBatch);
            jerseyText.render(spriteBatch);
        }

    }
}
