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
    class FlxPlatformActor : FlxSprite
    {

        public string animationPrefix = "";

        /// <summary>
        /// A set of Actions to determine what animation to play.
        /// </summary>
        public enum Actions
        {
            idle = 1,
            walk = 2,
            run = 3,
            hurt = 4,
            death = 5,
            talk = 6,
            dash = 7,
        }

        /// <summary>
        /// The current action.
        /// </summary>
        public Actions action = Actions.idle;


        /// <summary>
        /// <para>What is currently controlling the Actor</para>
        /// <para>None = No ability to control.</para>
        /// <para>Player = controlling with the keyboard or mouse or gamepad</para>
        /// <para>File = playback from a file.</para>
        /// </summary>
        public enum Controls
        {
            none = 0,
            player = 1,
            file = 2,
        }

        /// <summary>
        /// The current controlling action.
        /// </summary>
        public Controls control = Controls.none;

        public string controlFile = "";

        /// <summary>
        /// Player index controls which controller is controlling this. 1,2,3 or 4.
        /// </summary>
        private PlayerIndex playerIndex;

        private int playerIndexAsInt;

        //Player Physics

        public int runSpeed;

        // JUMP PHYSICS

        /// <summary>
        /// holds the amount of time character has been jumping.
        /// </summary>
        private float _jump = 0.0f;

        /// <summary>
        /// How high the character will jump.
        /// </summary>
        private float _jumpPower = -180.0f;

        private float _jumpInitialPower = -140.0f;

        private float _jumpMaxTime = 0.25f;

        private float _jumpInitialTime = 0.065f;

        /// <summary>
        /// set this for double jumps etc.
        /// </summary>
        public int numberOfJumps = 1;

        private int _jumpCounter = 0;

        /// <summary>
        /// How many frames have passed since the character left the ground.
        /// </summary>
        public float framesSinceLeftGround;

        private List<bool[]> _history = new List<bool[]>();
        public int frameCount;

        protected float _runningMax;

        public bool reverseControls;

        public PlayerIndex ControllingPlayer
        {
               get 
               { 
                  return playerIndex; 
               }
               set 
               {
                   playerIndex = value;

                   if (value == PlayerIndex.One) playerIndexAsInt = 1;
                   else if (value == PlayerIndex.Two) playerIndexAsInt = 2;
                   else if (value == PlayerIndex.Three) playerIndexAsInt = 3;
                   else if (value == PlayerIndex.Four) playerIndexAsInt = 4;

               }
        }

        

        public FlxPlatformActor(int xPos, int yPos)
            : base(xPos, yPos)
        {
            playerIndexAsInt = 1;
            if (playerIndex == PlayerIndex.One) playerIndexAsInt = 1;
            else if (playerIndex == PlayerIndex.Two) playerIndexAsInt = 2;
            else if (playerIndex == PlayerIndex.Three) playerIndexAsInt = 3;
            else if (playerIndex == PlayerIndex.Four) playerIndexAsInt = 4;
            frameCount = 0;
            reverseControls = false;

        }

        override public void update()
        {
            PlayerIndex pi;

            updateFramesSinceLeftFloor();

            if (FlxG.debug)
            {
                if (FlxGlobal.cheatString.StartsWith("slowdown") )
                {
                    runSpeed = 22;
                }
                if (FlxGlobal.cheatString=="control"+GetType().ToString().Split('.')[1] )
                {
                    control = Controls.player;
                    FlxG.follow(this, 11.0f);

                }
                if (FlxGlobal.cheatString == "controlFile" + GetType().ToString().Split('.')[1])
                {
                    
                    control = Controls.file;
                    FlxG.follow(this, 11.0f);
                    startPlayingBack();
                }
            }

            if (control == Controls.player)
            {
                if (FlxG.keys.A || FlxG.keys.LEFT) leftPressed();
                if (FlxG.gamepads.isButtonDown(Buttons.DPadLeft, ControllingPlayer, out pi)) leftPressed();
                if (FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickLeft, ControllingPlayer, out pi)) leftPressed();

                if (FlxG.keys.D || FlxG.keys.RIGHT) rightPressed();
                if (FlxG.gamepads.isButtonDown(Buttons.DPadRight, ControllingPlayer, out pi)) rightPressed();
                if (FlxG.gamepads.isButtonDown(Buttons.LeftThumbstickRight, ControllingPlayer, out pi)) rightPressed();

                if (FlxG.keys.B || FlxG.gamepads.isButtonDown(Buttons.RightTrigger))
                {
                    maxVelocity.X = _runningMax * 2;
                }
                else
                {
                    maxVelocity.X = _runningMax;
                }

                if (FlxG.keys.W || FlxG.keys.SPACE || FlxG.keys.X || FlxG.gamepads.isButtonDown(Buttons.A, ControllingPlayer, out pi)) jump();
                else
                {
                    if (_jumpCounter < numberOfJumps)
                    {
                        _jump = 0.0f;
                    }
                    else
                    {
                        _jump = -1;
                    }
                }
                
                if (FlxG.keys.N || FlxG.gamepads.isButtonDown(Buttons.LeftTrigger)) action = Actions.talk;
                else action = Actions.idle;


            }
            else if (control == Controls.file)
            {

                if (frameCount > _history.Count - 2)
                {
                    frameCount=0;
                    return;
                }

                frameCount++;

                int left = 3;
                int right = 1;

                if (reverseControls)
                {
                    left = 1;
                    right = 3;
                }

                if (_history[frameCount][left]) leftPressed();
                if (_history[frameCount][right]) rightPressed();

                if (_history[frameCount][4]) jump();
                
                if (_history[frameCount][10]) action = Actions.talk;
                else action = Actions.idle;


             
            }


            updateAnims();

            

            base.update();
        }

        public void startPlayingBack()
        {
            startPlayingBack(controlFile);
        }

        public void startPlayingBack(string Filename)
        {
            _history = new List<bool[]>();

			string x = FlxU.loadFromDevice(Filename);

            string[] y = x.Split('\n');

            int line = 0;

            foreach (var item in y)
            {
                string[] item1 = item.Split(',');

                line++;
                if (item1.Length == 14)
                {
                    try
                    {
                        _history.Add(new bool[] { bool.Parse(item1[0]), 
                            bool.Parse(item1[1]), 
                            bool.Parse(item1[2]), 
                            bool.Parse(item1[3]), 
                            bool.Parse(item1[4]), 
                            bool.Parse(item1[5]), 
                            bool.Parse(item1[6]), 
                            bool.Parse(item1[7]),
                            bool.Parse(item1[8]), 
                            bool.Parse(item1[9]), 
                            bool.Parse(item1[10]), 
                            bool.Parse(item1[11]),
                            bool.Parse(item1[12]),
                            bool.Parse(item1[13])});
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("History Not Added " + item1.Length + " -- " + item1[1]);
                    }
                }
            }
            //_rec = Recording.Playback;

            control = Controls.file;

            frameCount = 0;
        }


        private void updateFramesSinceLeftFloor()
        {
            if (onFloor)
            {
                framesSinceLeftGround = 0;
            }
            else
            {
                framesSinceLeftGround++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="JumpPower"></param>
        /// <param name="JumpInitialPower"></param>
        /// <param name="JumpMaxTime"></param>
        /// <param name="JumpInitialTime"></param>
        protected void setJumpValues(float JumpPower, float JumpInitialPower, float JumpMaxTime, float JumpInitialTime)
        {
            _jumpPower = JumpPower;
            _jumpInitialPower = JumpInitialPower;
            _jumpMaxTime = JumpMaxTime;
            _jumpInitialTime = JumpInitialTime;
        }

        public void jump()
        {
            //Console.WriteLine("Jump() " + _jump + " Frames since left floor " + framesSinceLeftGround + " {0} ", _jumpInitialPower);
            if (_jump == 0)
            {
                _jumpCounter++;
            }
            if (_jump >= 0 || framesSinceLeftGround < 10 )
            {
                
                if (framesSinceLeftGround < 10)
                {
                    _jump = 0.0f;
                    framesSinceLeftGround = 10000;
                }

                _jump += FlxG.elapsed;
                if (_jump > _jumpMaxTime) _jump = -1;
            }
            else
            {
                _jump = -1;
            }
            if (_jump > 0)
            {
                if (_jump < _jumpInitialTime)
                    velocity.Y = _jumpInitialPower;
                else
                    velocity.Y = _jumpPower;
            }

        }


        private void leftPressed()
        {
            facing = Flx2DFacing.Left;
            velocity.X -= runSpeed;
        }
        private void rightPressed()
        {
            facing = Flx2DFacing.Right;
            velocity.X += runSpeed;
        }

        /// <summary>
        /// Updates to the correct animation
        /// </summary>
        private void updateAnims()
        {
            if (dead)
            {
                play(animationPrefix + "death");
            }
            else if (velocity.Y != 0)
            {
                play(animationPrefix + "jump");
            }
            else if (velocity.X == 0)
            {
                if (action == Actions.talk) play(animationPrefix + "talk");
                else play(animationPrefix + "idle");
            }
            else if (velocity.X > 1)
            {
                play(animationPrefix + "run");
            }
            else if (velocity.X < 1)
            {
                play(animationPrefix + "run");
            }


        }

        public override void hitBottom(FlxObject Contact, float Velocity)
        {
            _jumpCounter = 0;
            _jump = 0.0f;
            base.hitBottom(Contact, Velocity);
        }

        public override void hitSide(FlxObject Contact, float Velocity)
        {
            reverseControls = !reverseControls;

            base.hitSide(Contact, Velocity);
        }

    }
}
