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
    /// The Base Example State exists to add common functionality to all example states.
    /// </summary>
    public class BaseExampleState : FlxState
    {

        override public void create()
        {
            base.create();



        }

        override public void update()
        {


            if (FlxG.keys.justPressed(Keys.Escape))
            {
                FlxG.state = new TestState();
            }

            base.update();
        }


    }
}
