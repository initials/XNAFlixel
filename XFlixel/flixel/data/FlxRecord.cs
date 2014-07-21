using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

namespace org.flixel
{


    /// <summary>
    /// The main "game object" class, handles basic physics and animation.
    /// </summary>
    public class FlxRecord : FlxObject
    {

        private List<bool[]> _history = new List<bool[]>();
        /// <summary>
        /// State of recording, whether playback or recording etc
        /// </summary>
        private Recording _rec = Recording.None;

        private FlxSprite openSprite;
        private FlxSprite pauseSprite;
        private FlxSprite playSprite;
        private FlxSprite recordSprite;
        private FlxSprite restartSprite;
        //private FlxSprite stepSprite;
        private FlxSprite stopSprite;

        private FlxGroup vcrGroup;

        private FlxText infoText;



        public string filename;
        public PlayerIndex? controller;

        /// <summary>
        /// Recording style.
        /// </summary>
        public enum Recording
        {
            None = 0,
            RecordingController = 1,
            RecordingPosition = 2,
            Playback = 3,
            PlaybackReverse = 4
        }

        public enum ButtonMap
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3,
            A = 4,
            B = 5,
            X = 6,
            Y = 7,
            LeftShoulder = 8,
            LeftTrigger = 9,
            RightShoulder = 10,
            RightTrigger = 11,
            LeftMouse = 12,
            RightMouse = 13
        }


        public FlxRecord()
        {
            vcrGroup = new FlxGroup();
            
            
            // Customizable things
            filename = "File";
            controller = PlayerIndex.One;


            //int xPos = 150;
            int yPos = 30;
            

            openSprite = new FlxSprite(130, yPos);
            openSprite.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/vcr/open"));
            openSprite.setScrollFactors(0, 0);
            openSprite.debugName = "open";
            vcrGroup.add(openSprite);


            pauseSprite = new FlxSprite(230, yPos);
            pauseSprite.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/vcr/pause"));
            pauseSprite.setScrollFactors(0, 0);
            pauseSprite.debugName = "pause";
            vcrGroup.add(pauseSprite);

            playSprite = new FlxSprite(330, yPos);
            playSprite.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/vcr/play"));
            playSprite.setScrollFactors(0, 0);
            playSprite.debugName = "play";
            vcrGroup.add(playSprite);

            recordSprite = new FlxSprite(430, yPos);
            recordSprite.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/vcr/record_on"));
            recordSprite.setScrollFactors(0, 0);
            recordSprite.debugName = "record";
            vcrGroup.add(recordSprite);

            restartSprite = new FlxSprite(530, yPos);
            restartSprite.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/vcr/restart"));
            restartSprite.setScrollFactors(0, 0);
            restartSprite.debugName = "restart";
            vcrGroup.add(restartSprite);

            stopSprite = new FlxSprite(630, yPos);
            stopSprite.loadGraphic(FlxG.Content.Load<Texture2D>("flixel/vcr/stop"));
            stopSprite.setScrollFactors(0, 0);
            stopSprite.debugName = "stop";
            vcrGroup.add(stopSprite);

            infoText = new FlxText(360, yPos, FlxG.width);


        }

        public override void render(SpriteBatch spriteBatch)
        {
            openSprite.render(spriteBatch);
            pauseSprite.render(spriteBatch);
            playSprite.render(spriteBatch);
            recordSprite.render(spriteBatch);
            restartSprite.render(spriteBatch);
            stopSprite.render(spriteBatch);
            infoText.render(spriteBatch);

            base.render(spriteBatch);
        }

        virtual public bool customOverlapsPoint(float X, float Y, FlxObject _point)
        {
            if ((X <= _point.x) || (X >= _point.x + _point.width ) || (Y <= _point.y) || (Y >= _point.y + _point.height))
                return false;
            return true;
        }

        
        public override void update()
        {

            foreach (FlxSprite item in vcrGroup.members)
            {
                if (customOverlapsPoint(FlxG.mouse.screenX, FlxG.mouse.screenY, item))
                {
                    if (FlxG.mouse.justPressed())
                    {
                        item.alpha = 1.0f;

                        vcrAction(item.debugName);
                        //FlxG.log(FlxG.mouse.screenX + " " + FlxG.mouse.screenY + " " + pauseSprite.x + " " + pauseSprite.y);
                    }
                    else
                    {
                        item.alpha = 0.75f;
                    }
                }
                else
                {
                    item.alpha = 0.25f;
                }


            }

            PlayerIndex pi;

            if (_rec == Recording.RecordingController)
            {

                _history.Add(new bool[] { 
                    (FlxG.keys.W || FlxG.gamepads.isButtonDown(Buttons.DPadUp, controller, out pi) || FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickUp)), //0
                    (FlxG.keys.D || FlxG.gamepads.isButtonDown(Buttons.DPadRight, controller, out pi) || FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickRight)), //1
                    (FlxG.keys.S || FlxG.gamepads.isButtonDown(Buttons.DPadDown, controller, out pi) || FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickDown)), //2
                    (FlxG.keys.A || FlxG.gamepads.isButtonDown(Buttons.DPadLeft, controller, out pi) || FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickLeft)), //3
                    (FlxG.keys.SPACE ||FlxG.keys.M || FlxG.gamepads.isButtonDown(Buttons.A, controller, out pi)), //4
                    (FlxG.keys.N || FlxG.gamepads.isButtonDown(Buttons.B, controller, out pi)), //5
                    (FlxG.keys.J || FlxG.gamepads.isNewButtonPress(Buttons.X, controller, out pi)), //6
                    (FlxG.keys.K || FlxG.gamepads.isButtonDown(Buttons.Y, controller, out pi)), //7
                    (FlxG.keys.H || FlxG.gamepads.isButtonDown(Buttons.LeftShoulder, controller, out pi)), //8
                    (FlxG.keys.U || FlxG.gamepads.isButtonDown(Buttons.LeftTrigger, controller, out pi)), //9
                    (FlxG.keys.L || FlxG.gamepads.isButtonDown(Buttons.RightShoulder, controller, out pi)), //10
                    (FlxG.keys.I || FlxG.gamepads.isButtonDown(Buttons.RightTrigger, controller, out pi)), //11
                    (FlxG.mouse.pressedLeftButton()), //12
                    (FlxG.mouse.pressedRightButton()), //13

                });


            }

            base.update();

        }

        public void vcrAction(string Action)
        {
            Console.WriteLine(Action);

            if (Action == "stop") 
            {
                saveRecording();

                //Flush history.
                _history = null;
                _history = new List<bool[]>();
                
            }

            else if (Action == "record")
            {
                infoText.text = "Recording: " + filename + ".txt";
                _rec = Recording.RecordingController;
            }

        }

        public void pause()
        {

        }

        public void openRecording()
        {

        }

        public void saveRecording()
        {
            string _historyString = "";
            foreach (var item in _history)
            {
                _historyString += item[0].ToString() + "," + item[1].ToString() + "," + item[2].ToString() + "," + item[3].ToString() + "," +
                    item[4].ToString() + "," + item[5].ToString() + "," + item[6].ToString() + "," + item[7].ToString() + "," +
                    item[8].ToString() + "," + item[9].ToString() + "," + item[10].ToString() + "," + item[11].ToString() + "," + 
                    item[12].ToString() + "," + item[13].ToString() + "\n";
            }
            
            FlxU.saveToDevice(_historyString, (filename + "_" + DateTime.Now.Ticks.ToString() + ".txt"));

            infoText.text = "Saved file to device: " + filename + "_" + DateTime.Now.Ticks.ToString() + ".txt";

            _rec = Recording.None;

        }
    }
}
