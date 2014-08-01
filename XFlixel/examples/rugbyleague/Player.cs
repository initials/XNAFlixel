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
        private Ball ball;

        public bool isSelected;
        public bool hasBall;
        

        public Player(int xPos, int yPos, int JerseyNumber, Ball ReferenceToBall)
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
            selectedPlayerIcon.alpha = 0.25f;
            selectedPlayerIcon.setOffset(9, 9);

            ball = ReferenceToBall;


            loadGraphic(FlxG.Content.Load<Texture2D>("examples/running"), true, false, 32, 32);

            addAnimation("idle", new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52 }, (int)FlxU.random(12,24), true);
            addAnimation("run", new int[] { 53, 54, 55, 56, 57, 58, 59, 60,61,62,63,64,65,66,67,68,69,70,71 }, 24, true);
            play("idle");

            setDrags(1350, 1350);

            angle = 90;

            isSelected = false;

            height = 16;
            width = 16;
            //offset.X = 12;
            //offset.Y = 12;

            setOffset(9,9);

        }

        override public void update()
        {
            float runSpeed = 200;

            if (isSelected)
            {
                if (FlxControl.LEFT)
                {
                    this.velocity.X = runSpeed * -1;
                }
                if (FlxControl.RIGHT)
                {
                    this.velocity.X = runSpeed;
                }
                if (FlxControl.UP)
                {
                    this.velocity.Y = runSpeed * -1;
                }
                if (FlxControl.DOWN)
                {
                    this.velocity.Y = runSpeed;
                }
            }

            if (velocity.X!=0) { 
                play("run");
                setAngleBasedOnVelocity();
                //flicker(555);
            }
            else if (velocity.Y != 0)
            {
                play("run");
                setAngleBasedOnVelocity();
            }
            else
            {
                play("idle");
                //flicker(0.001f);
            }

            selectedPlayerIcon.at(this);
            selectedPlayerIcon.x -= 8;
            selectedPlayerIcon.y -= 8;
            selectedPlayerIcon.angle += 2;
            selectedPlayerIcon.update();

            jerseyText.at(this); 
            jerseyText.x -=8;
            jerseyText.y +=24;
            jerseyText.update();



            base.update();

            if (hasBall)
            {
                ball.at(this);
            }

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


        public void passBall(float xVel, float yVel)
        {
            ball.x = x - 3;
            ball.y = y + height + 3;
            ball.timeSincePass = 0;
            ball.setVelocity(xVel, yVel);

        }

    }
}
