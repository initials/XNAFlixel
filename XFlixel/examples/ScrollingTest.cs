using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

namespace org.flixel.examples
{
    public class ScrollingTest : FlxState
    {
        private FlxScrollingSprite scroller;

        override public void create()
        {
            base.create();

            FlxG.hideHud();

            scroller = new FlxScrollingSprite(20, 20);
            scroller.loadGraphic("flixel/initials/crate_80x60", true, false,80,60);
            scroller.scrollSpeed = new Vector2(0.5f, 0.1f);
            add(scroller);
            scroller = new FlxScrollingSprite(120, 20);
            scroller.loadGraphic("flixel/initials/crate_80x60", true, false, 80, 60);
            scroller.scrollSpeed = new Vector2(10.5f, -10.1f);
            add(scroller);




        }

        override public void update()
        {




            base.update();
        }


    }
}
