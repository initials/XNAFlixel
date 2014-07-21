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
    public class FlxMenuState : FlxState
    {

        public FlxGroup buttons;

        override public void create()
        {
            base.create();

            buttons = new FlxGroup();
            


        }

        public void addButtons()
        {
            add(buttons);

            ((FlxButton)(buttons.members[0])).on = true;
        }

        public void moveSelected(string direction)
        {
            int[] cur = getCurrentSelected();

            if (direction == "forward")
            {
                ((FlxButton)(buttons.members[cur[0]])).on = false;
                ((FlxButton)(buttons.members[cur[2]])).on = true;
            }
            else if (direction == "backward")
            {
                ((FlxButton)(buttons.members[cur[0]])).on = false;
                ((FlxButton)(buttons.members[cur[1]])).on = true;
            }
        }

        public void setAllButtonsToOff()
        {
            foreach (FlxButton b in buttons.members)
            {
                b.on = false;
            }
        }

        /// <summary>
        /// current, previous, next
        /// </summary>
        /// <returns></returns>
        public int[] getCurrentSelected()
        {
            int count = 0;
            foreach (var item in buttons.members)
            {
                if (((FlxButton)(buttons.members[count])).on == true)
                {

                    int[] three = new int[] { count, 0, 0 };

                    if (count == 0) three[1] = buttons.members.Count-1;
                    else
                    {
                        int nx = count - 1;
                        three[1] = nx;
                    }

                    if (count == buttons.members.Count - 1) three[2] = 0;
                    else
                    {
                        int nx = count + 1;
                        three[2] = nx;
                    }

                    return three;
                }
                count++;
            }
            return new int[] {-1, -1, -1};
        }

        override public void update()
        {
            //if (FlxG.mouse.pressed())
            //{
            //    setAllButtonsToOff();
            //}

            if (FlxG.keys.justPressed(Keys.Left))
            {
                moveSelected("backward");
            }

            if (FlxG.keys.justPressed(Keys.Right))
            {
                moveSelected("forward");
            }


            base.update();
        }


    }
}
