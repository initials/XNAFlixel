using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

// Use the XNATweener namespace.
using XNATweener;

namespace org.flixel
{
    public class TweenerState : FlxState
    {

        private Tweener redCarTween;
        private Tweener yellowCarTween;
        private Tweener greenCarTween;
        private Tweener blueCarTween;
        private Tweener purpleCarTween;
        private Tweener lightGreenCarTween;

        private FlxSprite redCar;
        private FlxSprite yellowCar;
        private FlxSprite greenCar;
        private FlxSprite blueCar;
        private FlxSprite purpleCar;
        private FlxSprite lightGreenCar;


        private FlxGroup carsGroup;

        override public void create()
        {
            base.create();

            FlxG.hideHud();




            carsGroup = new FlxGroup();
            add(carsGroup);

            redCar = new FlxSprite(20, 0);
            redCar.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            redCar.addAnimation("Static", new int[] { 6 }, 0, true);
            redCar.play("Static");
            redCar.angle = 180;
            carsGroup.add(redCar);

            redCarTween = new Tweener(20, 300, TimeSpan.FromSeconds(3.0f), Quadratic.EaseOut);
            redCarTween.Loop = true;


            yellowCar = new FlxSprite(20, 50);
            yellowCar.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            yellowCar.addAnimation("Static", new int[] { 7 }, 0, true);
            yellowCar.play("Static");
            yellowCar.angle = 180;
            carsGroup.add(yellowCar);

            yellowCarTween = new Tweener(20, 300, TimeSpan.FromSeconds(3.0f), Quadratic.EaseIn);
            yellowCarTween.Loop = true;

            greenCar = new FlxSprite(20, 100);
            greenCar.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            greenCar.addAnimation("Static", new int[] { 8 }, 0, true);
            greenCar.play("Static");
            greenCar.angle = 180;
            carsGroup.add(greenCar);

            greenCarTween = new Tweener(20, 300, TimeSpan.FromSeconds(3.0f), Quadratic.EaseInOut);
            greenCarTween.Loop = true;


            blueCar = new FlxSprite(20, 150);
            blueCar.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            blueCar.addAnimation("Static", new int[] { 9 }, 0, true);
            blueCar.play("Static");
            blueCar.angle = 180;
            carsGroup.add(blueCar);

            blueCarTween = new Tweener(20, 300, TimeSpan.FromSeconds(3.0f), XNATweener.Circular.EaseInOut);
            blueCarTween.Loop = true;


            purpleCar = new FlxSprite(20, 200);
            purpleCar.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            purpleCar.addAnimation("Static", new int[] { 10 }, 0, true);
            purpleCar.play("Static");
            purpleCar.angle = 180;
            carsGroup.add(purpleCar);

            purpleCarTween = new Tweener(20, 300, TimeSpan.FromSeconds(3.0f), XNATweener.Sinusoidal.EaseInOut);
            purpleCarTween.Loop = true;


            lightGreenCar = new FlxSprite(20, 250);
            lightGreenCar.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            lightGreenCar.addAnimation("Static", new int[] { 11 }, 0, true);
            lightGreenCar.play("Static");
            lightGreenCar.angle = 180;
            carsGroup.add(lightGreenCar);

            lightGreenCarTween = new Tweener(20, 300, TimeSpan.FromSeconds(3.0f), XNATweener.Quintic.EaseInOut);
            lightGreenCarTween.Loop = true;





        }

        override public void update()
        {
            redCarTween.Update(FlxG.elapsedAsGameTime);
            redCar.x = redCarTween.Position;

            yellowCarTween.Update(FlxG.elapsedAsGameTime);
            yellowCar.x = yellowCarTween.Position;

            greenCarTween.Update(FlxG.elapsedAsGameTime);
            greenCar.x = greenCarTween.Position;

            purpleCarTween.Update(FlxG.elapsedAsGameTime);
            purpleCar.x = purpleCarTween.Position;

            blueCarTween.Update(FlxG.elapsedAsGameTime);
            blueCar.x = blueCarTween.Position;

            lightGreenCarTween.Update(FlxG.elapsedAsGameTime);
            lightGreenCar.x = lightGreenCarTween.Position;



            base.update();
        }


    }
}
