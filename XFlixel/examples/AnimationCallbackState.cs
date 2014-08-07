using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

namespace org.flixel
{
    public class AnimationCallbackState : BaseExampleState
    {
        FlxSprite spaceShip;

        FlxGroup stars;
        FlxSprite star;

        override public void create()
        {
            base.create();

            FlxG.hideHud();

            stars = new FlxGroup();


            // Make a starfield to fly through.
            for (int i = 0; i < 100; i++)
            {
                star = new FlxSprite(FlxU.random(0,FlxG.width), FlxU.random(0,FlxG.height));
                star.createGraphic(1, 1, Color.White);
                stars.velocity.Y = FlxU.random(20, 100);
                stars.velocity.X = 0;
                stars.add(star);
            }

            add(stars);

            spaceShip = new FlxSprite(FlxG.width/2, FlxG.height/2);
            spaceShip.loadGraphic(FlxG.Content.Load<Texture2D>("surt/spaceship_32x32"), true, false, 32, 32);
            
            //Add some animations to our Spaceship
            spaceShip.addAnimation("static", new int[] { 0 }, 36, true);

            spaceShip.addAnimation("transform1", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 }, 12, false);
            spaceShip.addAnimation("transform2", new int[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39 }, 12, false);
            spaceShip.addAnimation("transform3", new int[] { 40,41,42 }, 12, false);
            

            spaceShip.addAnimation("transform", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39,40,41,42 }, 12, false);
            
            spaceShip.play("static");


            //Add an animation callback - This will call Pulse on every frame.
            spaceShip.addAnimationCallback(pulse);

            
            spaceShip.scale = 1;
            spaceShip.setDrags(1100, 1100);
            add(spaceShip);




        }

        override public void update()
        {

            if (FlxG.keys.justReleased(Keys.Space))
            {
                spaceShip.play("transform");

            }
            if (FlxG.keys.justReleased(Keys.D1))
            {
                spaceShip.play("transform1");

            }
            if (FlxG.keys.justReleased(Keys.D2))
            {
                spaceShip.play("transform2");

            }
            if (FlxG.keys.justReleased(Keys.D3))
            {
                spaceShip.play("transform3");

            }

            foreach (FlxSprite s in stars.members)
            {
                if (s.y > FlxG.height) s.y = -10;
            }

            base.update();
        }

        public void pulse(string Name, uint Frame, int FrameIndex)
        {
            // Flash on Frame 5
            if (Name == "transform" && Frame == 2)
            {
                FlxG.flash.start(Color.White);
                spaceShip.scale = 2;
            }
            else if (Name == "transform" && Frame == 10)
            {
                spaceShip.scale = 3;
            }
            else if (Name == "transform" && Frame == 15)
            {
                spaceShip.scale = 4;
            }
            else if (Name == "transform" && Frame == 20)
            {
                spaceShip.scale = 5;
            }
            else
            {
                spaceShip.scale = 1;
            }
        }


    }
}
