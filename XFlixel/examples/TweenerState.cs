using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

// Important to use the XNATweener namespace.
using XNATweener;

namespace org.flixel.examples
{
    public class TweenerState : BaseExampleState
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


        private FlxText redCarText;
        private FlxText yellowCarText;
        private FlxText greenCarText;
        private FlxText blueCarText;
        private FlxText purpleCarText;
        private FlxText lightGreenCarText;

        float timeToMove;
        int driveDistance;

        Vector2 car1Pos;
        Vector2 car2Pos;
        Vector2 car3Pos;

        Vector2 car4Pos;
        Vector2 car5Pos;
        Vector2 car6Pos;

        private FlxGroup carsGroup;

        override public void create()
        {
            base.create();

            FlxG.resetHud();

            String hudText = "Press \n";
            hudText += "Q. Back \n";
            hudText += "W. Bounce \n";
            hudText += "E. Circular \n";
            hudText += "R. Cubic \n";
            hudText += "A. Elastic \n";
            hudText += "S. Exponential \n";
            hudText += "D. Linear \n";
            hudText += "F. Quadratic \n";
            hudText += "Z. Quartic \n";
            hudText += "X. Quintic \n";
            hudText += "C. Sinusoidal";

            FlxG.setHudText(2, hudText);

            FlxG.mouse.hide();

            //Define the time it takes to move the car across the screen.
            timeToMove = 2.5f;

            //Define the distance the car will move.
            driveDistance = 350;

            //Define the starting positions of the cars.
            car1Pos = new Vector2(20, 0);
            car2Pos = new Vector2(20, 50);
            car3Pos = new Vector2(20, 100);
            car4Pos = new Vector2(20, 150);
            car5Pos = new Vector2(20, 200);
            car6Pos = new Vector2(20, 250);

            redCarText = new FlxText(car1Pos.X, car1Pos.Y+40, FlxG.width);
            redCarText.text = "Back.EaseInOut";
            redCarText.setFormat(null, 1, Color.Red, FlxJustification.Left, Color.Black);
            add(redCarText);

            yellowCarText = new FlxText(car2Pos.X, car2Pos.Y + 40, FlxG.width);
            yellowCarText.text = "Bounce.EaseInOut";
            yellowCarText.setFormat(null, 1, Color.Yellow, FlxJustification.Left, Color.Black);
            add(yellowCarText);

            greenCarText = new FlxText(car3Pos.X, car3Pos.Y + 40, FlxG.width);
            greenCarText.text = "Circular.EaseInOut";
            greenCarText.setFormat(null, 1, Color.Green, FlxJustification.Left, Color.Black);
            add(greenCarText);

            blueCarText = new FlxText(car4Pos.X, car4Pos.Y + 40, FlxG.width);
            blueCarText.text = "Cubic.EaseInOut";
            blueCarText.setFormat(null, 1, Color.Blue, FlxJustification.Left, Color.Black);
            add(blueCarText);

            purpleCarText = new FlxText(car5Pos.X, car5Pos.Y + 40, FlxG.width);
            purpleCarText.text = "Elastic.EaseInOut - Loop";
            purpleCarText.setFormat(null, 1, Color.Purple, FlxJustification.Left, Color.Black);
            add(purpleCarText);

            lightGreenCarText = new FlxText(car6Pos.X, car6Pos.Y + 40, FlxG.width);
            lightGreenCarText.text = "Exponential.EaseInOut - Ping Pong";
            lightGreenCarText.setFormat(null, 1, Color.LightGreen, FlxJustification.Left, Color.Black);
            add(lightGreenCarText);



            carsGroup = new FlxGroup();
            add(carsGroup);

            redCar = new FlxSprite(car1Pos.X, car1Pos.Y);
            redCar.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
            redCar.addAnimation("Static", new int[] { 6 }, 0, true);
            redCar.play("Static");
            redCar.angle = 180;
            carsGroup.add(redCar);

            redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Back.EaseInOut);
            redCarTween.Loop = true;
            redCarTween.Start();

            yellowCar = new FlxSprite(car2Pos.X, car2Pos.Y);
            yellowCar.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
            yellowCar.addAnimation("Static", new int[] { 7 }, 0, true);
            yellowCar.play("Static");
            yellowCar.angle = 180;
            carsGroup.add(yellowCar);

            yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Bounce.EaseInOut);
            yellowCarTween.Loop = true;

            greenCar = new FlxSprite(car3Pos.X, car3Pos.Y);
            greenCar.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
            greenCar.addAnimation("Static", new int[] { 8 }, 0, true);
            greenCar.play("Static");
            greenCar.angle = 180;
            carsGroup.add(greenCar);

            greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), XNATweener.Circular.EaseInOut);
            greenCarTween.Loop = true;


            blueCar = new FlxSprite(car4Pos.X, car4Pos.Y);
            blueCar.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
            blueCar.addAnimation("Static", new int[] { 9 }, 0, true);
            blueCar.play("Static");
            blueCar.angle = 180;
            carsGroup.add(blueCar);

            blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), XNATweener.Cubic.EaseInOut);
            blueCarTween.Loop = true;


            purpleCar = new FlxSprite(car5Pos.X, car5Pos.Y);
            purpleCar.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
            purpleCar.addAnimation("Static", new int[] { 10 }, 0, true);
            purpleCar.play("Static");
            purpleCar.angle = 180;
            carsGroup.add(purpleCar);

            purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), XNATweener.Elastic.EaseInOut);
            purpleCarTween.Loop = true;


            lightGreenCar = new FlxSprite(car6Pos.X, car6Pos.Y);
            lightGreenCar.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 64, 64);
            lightGreenCar.addAnimation("Static", new int[] { 11 }, 0, true);
            lightGreenCar.play("Static");
            lightGreenCar.angle = 180;
            carsGroup.add(lightGreenCar);

            lightGreenCarTween = new Tweener(car6Pos.X , driveDistance, TimeSpan.FromSeconds(timeToMove), XNATweener.Exponential.EaseInOut);
            lightGreenCarTween.PingPong = true;

            Console.WriteLine(lightGreenCar.GetType().AssemblyQualifiedName);


            String myClass = "org.flixel.examples.CarSprite";

            var type = Type.GetType(myClass);
            Console.WriteLine(type);
            var myObject = (FlxSprite)Activator.CreateInstance(type,800, 400);
            //Console.WriteLine(myObject.GetType().AssemblyQualifiedName);
            //myObject.x = 300;
            //myObject.y = 300;
            add(myObject);


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


            if (FlxG.keys.justPressed(Keys.Q))
            {
                this.changeTweenFunction("Back");
            }
            if (FlxG.keys.justPressed(Keys.W))
            {
                this.changeTweenFunction("Bounce");
            }
            if (FlxG.keys.justPressed(Keys.E))
            {
                this.changeTweenFunction("Circular");
            }
            if (FlxG.keys.justPressed(Keys.R))
            {
                this.changeTweenFunction("Cubic");
            }
            if (FlxG.keys.justPressed(Keys.A))
            {
                this.changeTweenFunction("Elastic");
            }
            if (FlxG.keys.justPressed(Keys.S))
            {
                this.changeTweenFunction("Exponential");
            }
            if (FlxG.keys.justPressed(Keys.D))
            {
                this.changeTweenFunction("Linear");
            }
            if (FlxG.keys.justPressed(Keys.F))
            {
                this.changeTweenFunction("Quadratic");
            }
            if (FlxG.keys.justPressed(Keys.Z))
            {
                this.changeTweenFunction("Quartic");
            }
            if (FlxG.keys.justPressed(Keys.X))
            {
                this.changeTweenFunction("Quintic");
            }
            if (FlxG.keys.justPressed(Keys.C))
            {
                this.changeTweenFunction("Sinusoidal");
            }



            base.update();
        }

        public void changeTweenFunction(String TweenFunction)
        {
            if (TweenFunction == "Back")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Back.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Back.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Back.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Back.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Back.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Back.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Back.EaseIn";
                yellowCarText.text = "Back.EaseInOut";
                greenCarText.text = "Back.EaseOut";
                blueCarText.text = "Back.EaseIn";
                purpleCarText.text = "Back.EaseInOut - Loop";
                lightGreenCarText.text = "Back.EaseOut - Ping Pong";

            }
            if (TweenFunction == "Bounce")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Bounce.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Bounce.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Bounce.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Bounce.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Bounce.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Bounce.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Bounce.EaseIn";
                yellowCarText.text = "Bounce.EaseInOut";
                greenCarText.text = "Bounce.EaseOut";
                blueCarText.text = "Bounce.EaseIn";
                purpleCarText.text = "Bounce.EaseInOut - Loop";
                lightGreenCarText.text = "Bounce.EaseOut - Ping Pong";

            }
            if (TweenFunction == "Circular")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Circular.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Circular.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Circular.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Circular.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Circular.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Circular.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Circular.EaseIn";
                yellowCarText.text = "Circular.EaseInOut";
                greenCarText.text = "Circular.EaseOut";
                blueCarText.text = "Circular.EaseIn";
                purpleCarText.text = "Circular.EaseInOut - Loop";
                lightGreenCarText.text = "Circular.EaseOut - Ping Pong";

            }
            if (TweenFunction == "Cubic")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Cubic.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Cubic.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Cubic.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Cubic.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Cubic.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Cubic.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Cubic.EaseIn";
                yellowCarText.text = "Cubic.EaseInOut";
                greenCarText.text = "Cubic.EaseOut";
                blueCarText.text = "Cubic.EaseIn";
                purpleCarText.text = "Cubic.EaseInOut - Loop";
                lightGreenCarText.text = "Cubic.EaseOut - Ping Pong";

            }
            if (TweenFunction == "Elastic")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Elastic.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Elastic.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Elastic.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Elastic.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Elastic.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Elastic.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Elastic.EaseIn";
                yellowCarText.text = "Elastic.EaseInOut";
                greenCarText.text = "Elastic.EaseOut";
                blueCarText.text = "Elastic.EaseIn";
                purpleCarText.text = "Elastic.EaseInOut - Loop";
                lightGreenCarText.text = "Elastic.EaseOut - Ping Pong";

            }
            if (TweenFunction == "Exponential")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Exponential.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Exponential.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Exponential.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Exponential.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Exponential.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Exponential.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Exponential.EaseIn";
                yellowCarText.text = "Exponential.EaseInOut";
                greenCarText.text = "Exponential.EaseOut";
                blueCarText.text = "Exponential.EaseIn";
                purpleCarText.text = "Exponential.EaseInOut - Loop";
                lightGreenCarText.text = "Exponential.EaseOut - Ping Pong";
            }
            if (TweenFunction == "Linear")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Linear.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Linear.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Linear.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Linear.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Linear.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Linear.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Linear.EaseIn";
                yellowCarText.text = "Linear.EaseInOut";
                greenCarText.text = "Linear.EaseOut";
                blueCarText.text = "Linear.EaseIn";
                purpleCarText.text = "Linear.EaseInOut - Loop";
                lightGreenCarText.text = "Linear.EaseOut - Ping Pong";
            }
            if (TweenFunction == "Quadratic")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quadratic.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quadratic.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quadratic.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quadratic.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quadratic.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quadratic.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Quadratic.EaseIn";
                yellowCarText.text = "Quadratic.EaseInOut";
                greenCarText.text = "Quadratic.EaseOut";
                blueCarText.text = "Quadratic.EaseIn";
                purpleCarText.text = "Quadratic.EaseInOut - Loop";
                lightGreenCarText.text = "Quadratic.EaseOut - Ping Pong";
            }
            if (TweenFunction == "Quartic")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quartic.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quartic.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quartic.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quartic.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quartic.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quartic.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Quartic.EaseIn";
                yellowCarText.text = "Quartic.EaseInOut";
                greenCarText.text = "Quartic.EaseOut";
                blueCarText.text = "Quartic.EaseIn";
                purpleCarText.text = "Quartic.EaseInOut - Loop";
                lightGreenCarText.text = "Quartic.EaseOut - Ping Pong";
            }
            if (TweenFunction == "Quintic")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quintic.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quintic.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quintic.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quintic.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quintic.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Quintic.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Quintic.EaseIn";
                yellowCarText.text = "Quintic.EaseInOut";
                greenCarText.text = "Quintic.EaseOut";
                blueCarText.text = "Quintic.EaseIn";
                purpleCarText.text = "Quintic.EaseInOut - Loop";
                lightGreenCarText.text = "Quintic.EaseOut - Ping Pong";
            }
            if (TweenFunction == "Sinusoidal")
            {
                redCarTween = new Tweener(car1Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Sinusoidal.EaseIn);
                yellowCarTween = new Tweener(car2Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Sinusoidal.EaseInOut);
                greenCarTween = new Tweener(car3Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Sinusoidal.EaseOut);
                blueCarTween = new Tweener(car4Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Sinusoidal.EaseIn);
                purpleCarTween = new Tweener(car5Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Sinusoidal.EaseInOut);
                purpleCarTween.Loop = true;

                lightGreenCarTween = new Tweener(car6Pos.X, driveDistance, TimeSpan.FromSeconds(timeToMove), Sinusoidal.EaseOut);
                lightGreenCarTween.PingPong = true;

                redCarText.text = "Sinusoidal.EaseIn";
                yellowCarText.text = "Sinusoidal.EaseInOut";
                greenCarText.text = "Sinusoidal.EaseOut";
                blueCarText.text = "Sinusoidal.EaseIn";
                purpleCarText.text = "Sinusoidal.EaseInOut - Loop";
                lightGreenCarText.text = "Sinusoidal.EaseOut - Ping Pong";
            }

        }

    }
}
