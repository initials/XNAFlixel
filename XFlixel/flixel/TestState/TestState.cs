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
    public class TestState : FlxState
    {

        override public void create()
        {
            base.create();

            FlxG.mouse.show(FlxG.Content.Load<Texture2D>("flixel/cursor"));


            FlxText t = new FlxText(20, 20, 100);
            t.text = "Welcome To Flixel";
            add(t);


        }

        override public void update()
        {




            base.update();
        }


    }
}
