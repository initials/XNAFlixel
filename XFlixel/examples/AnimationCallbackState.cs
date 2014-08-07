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
    /// <summary>
    /// Examples for :
    /// * FlxAnimationCallback
    /// * FlxSprite.generateFrameNumbersBetween(0,39)
    /// * FlxHud
    /// * FlxGroup for the star field
    /// * FlxEmitter
    /// </summary>
    public class AnimationCallbackState : BaseExampleState
    {
        FlxSprite spaceShip;

        FlxGroup stars;
        FlxSprite star;

        FlxEmitter jets;

        override public void create()
        {
            base.create();

            FlxG.setHudGamepadButton(FlxHud.TYPE_KEYBOARD, FlxHud.Keyboard_Arrow_Left, 10, 110);
            FlxG.setHudGamepadButton(FlxHud.TYPE_KEYBOARD_DIRECTION, FlxHud.Keyboard_Arrow_Right, 110, 110);



            FlxSprite bg = new FlxSprite(0,0);
            bg.createGraphic(FlxG.width,FlxG.width, new Color(0.05f, 0.05f,0.08f));
            bg.setScrollFactors(0, 0);
            add(bg);

            stars = new FlxGroup();


            // Make a starfield to fly through.
            for (int i = 0; i < 100; i++)
            {
                star = new FlxSprite(FlxU.random(0,FlxG.width), FlxU.random(0,FlxG.height));
                star.createGraphic(3, 3, Color.White);
                star.velocity.Y = FlxU.random(20, 100);
                star.velocity.X = 0;
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
            

            //spaceShip.addAnimation("transform", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39}, 24, false);
            //spaceShip.addAnimation("reverse", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39 }, 24, false);
            
            spaceShip.addAnimation("transform", spaceShip.generateFrameNumbersBetween(0,39) , 24, false);
            spaceShip.addAnimation("reverse", spaceShip.generateFrameNumbersBetween(39, 0), 24, false);

            spaceShip.play("static");

            //Add an animation callback - This will call Pulse on every frame.
            spaceShip.addAnimationCallback(pulse);
            
            spaceShip.scale = 3;
            spaceShip.setDrags(1100, 1100);
            add(spaceShip);

            jets = new FlxEmitter();
            
            jets.setSize(5, 50);
            jets.createSprites( FlxG.Content.Load<Texture2D>("diagnostic/testpalette"), 100, true,0.0f,0.0f );
            jets.setXSpeed(-110, 110);
            jets.setYSpeed(40, 80);
            
            add(jets);
            jets.at(spaceShip);

        }

        override public void update()
        {

            if (FlxG.keys.justReleased(Keys.Space))
            {
                spaceShip.play("transform", true);

            }
            if (FlxG.keys.justReleased(Keys.Left))
            {
                spaceShip.play("reverse", true);

            }
            if (FlxG.keys.justReleased(Keys.Right))
            {
                spaceShip.play("transform", true);

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

            string info = "Current animation: " + Name + " Frame: " + Frame +  " FrameIndex: " + FrameIndex;

            FlxG.setHudText(1, info);
            FlxG.setHudTextScale(1, 3);

            if (Name == "transform" && Frame == 39)
            {
                FlxG.flash.start(Color.White);
            }

            // Lets say you want to fire some jets on the fifth frame of any animation
            if (Frame == 5)
            {
                jets.start();
            }

            // Fire jets at the fifth frame of reverse.
            if (Name == "reverse" && FrameIndex == 5)
            {
                jets.start();
            }






        }


    }
}
