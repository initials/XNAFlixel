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

        public Team()
            : base()
        {
            
        }

        public void sortMembersByX()
        {

        }

        override public void update()
        {
            if (FlxG.keys.justPressed(Keys.OemPeriod))
            {
                //members.Sort(0,
                for (int i = 0; i < this.members.Count; i++)
                {
                    if (((Player)this.members[i]).isSelected == true)
                    {
                        //Console.WriteLine(" selected player is {0}", i);

                        ((Player)this.members[i]).isSelected = false;
                        if (i == this.members.Count - 1)
                            ((Player)this.members[0]).isSelected = true;
                        else
                            ((Player)this.members[i + 1]).isSelected = true;
                        return;
                    }
                }
            }
            if (FlxG.keys.justPressed(Keys.OemComma))
            {
                for (int i = 0; i < this.members.Count; i++)
                {
                    if (((Player)this.members[i]).isSelected == true)
                    {
                        ((Player)this.members[i]).isSelected = false;
                        if (i == 0)
                            ((Player)this.members[this.members.Count - 1]).isSelected = true;
                        else
                            ((Player)this.members[i - 1]).isSelected = true;
                        return;
                    }
                }
            }

            base.update();

        }


    }
}
