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
    class Team : FlxGroup
    {
        private float kickPower;
        private float passAngle;

        public Team()
            : base()
        {
            kickPower = 0.0f;
            passAngle = 0.0f;

        }

        public void setPlayerModeTo(string Mode)
        {
            for (int i = 0; i < this.members.Count; i++)
            {
                ((Player)this.members[i]).mode = Mode;
            }
        }

        public void setAllToUnselected()
        {
            for (int i = 0; i < this.members.Count; i++)
            {
                ((Player)this.members[i]).isSelected = false;
            }
        }

        public void selectNextPlayerToLeft()
        {
            // Sort members by x position.

            members.Sort((x, y) => x.x.CompareTo(y.x));

            for (int i = 0; i < this.members.Count; i++)
            {
                if (((Player)this.members[i]).isSelected == true)
                {
                    //Console.WriteLine(" selected player is {0}", i);

                    ((Player)this.members[i]).isSelected = false;
                    if (i == this.members.Count - 1)
                    {
                        ((Player)this.members[0]).isSelected = true;
                        //FlxG.follow(this.members[0], Registry.FOLLOW_LERP);
                    }
                    else
                    {
                        ((Player)this.members[i + 1]).isSelected = true;
                        //FlxG.follow(this.members[i + 1], Registry.FOLLOW_LERP);
                    }

                    return;
                }
            }
        }

        public void selectNextPlayerToRight()
        {
            members.Sort((x, y) => x.x.CompareTo(y.x));

            for (int i = 0; i < this.members.Count; i++)
            {
                if (((Player)this.members[i]).isSelected == true)
                {
                    ((Player)this.members[i]).isSelected = false;
                    if (i == 0)
                    {
                        ((Player)this.members[this.members.Count - 1]).isSelected = true;
                        //FlxG.follow(this.members[this.members.Count - 1], Registry.FOLLOW_LERP);
                    }
                    else
                    {
                        ((Player)this.members[i - 1]).isSelected = true;
                        //FlxG.follow(this.members[i - 1], Registry.FOLLOW_LERP);
                    }
                    return;
                }
            }
        }

        public bool teamHasBall()
        {
            for (int i = 0; i < this.members.Count; i++)
            {
                if (((Player)this.members[i]).hasBall == true)
                {
                    return true;
                }
            }
            return false;
        }

        public void setSelectedPlayerToPlayerWithBall()
        {
            for (int i = 0; i < this.members.Count; i++)
            {
                if (((Player)this.members[i]).hasBall == true)
                {
                    ((Player)this.members[i]).isSelected = true;
                }
                else
                {
                    ((Player)this.members[i]).isSelected = false;
                }
            }
        }

        public void passBall(int Direction)
        {
            members.Sort((x, y) => x.x.CompareTo(y.x));

            for (int i = 0; i < this.members.Count; i++)
            {
                if (((Player)this.members[i]).hasBall == true)
                {
                    Console.WriteLine("Pass ball");
                    ((Player)this.members[i]).hasBall = false;
                    //((Player)this.members[i]).isSelected = false;
                    ((Player)this.members[i]).passBall(250 * Direction, -200);

                }

            }
            
        }

        public void kickBall(int Direction, int Power)
        {
            members.Sort((x, y) => x.x.CompareTo(y.x));

            for (int i = 0; i < this.members.Count; i++)
            {
                if (((Player)this.members[i]).hasBall == true)
                {
                    Console.WriteLine("Pass ball");
                    ((Player)this.members[i]).hasBall = false;
                    //((Player)this.members[i]).isSelected = false;
                    ((Player)this.members[i]).passBall(FlxU.random(-20,20), Power);

                }

            }

        }

        override public void update()
        {

            if (FlxG.keys.justPressed(Keys.OemPeriod))
            {
                if (teamHasBall())
                {
                    passBall(1);
                    selectNextPlayerToLeft();
                }
                else
                {
                    selectNextPlayerToLeft();
                }
            }
            if (FlxG.keys.justPressed(Keys.OemComma))
            {
                if (teamHasBall())
                {
                    passBall(-1);
                    selectNextPlayerToRight();
                }
                else
                {
                    selectNextPlayerToRight();
                }
            }

            if (FlxG.keys.justPressed(Keys.Space))
            {

            }
            if (FlxG.keys.SPACE)
            {
                kickPower += FlxG.elapsed;
            }
            if (FlxG.keys.justReleased(Keys.Space))
            {
                kickBall(1, (int)kickPower * 200);

                kickPower = 0;
            }

            if (teamHasBall())
            {
                setSelectedPlayerToPlayerWithBall();
            }

            base.update();

        }


    }
}
