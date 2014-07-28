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
    public class DebugMenuState : FlxState
    {

        override public void create()
        {
            FlxG.backColor = Color.DarkBlue;




            base.create();


            FlxG.resetHud();


            string textInfo = "";
            textInfo = "Choose:\n";
            textInfo += "1. Camera Test State\n";
            textInfo += "2. Cave Tiles Test State\n";


            FlxG.setHudText(1, textInfo);
            FlxG.setHudTextPosition(1, 20, 20);
            FlxG.setHudTextScale(1, 3);
            FlxG.setHudGamepadButton(FlxHud.TYPE_KEYBOARD, FlxHud.Keyboard_1, FlxG.width - 40, 30);

            FlxG.showHud();
            
        }

        override public void update()
        {

            if (FlxG.keys.ONE)
            {
                FlxG.state = new CameraTestState();
            }
            if (FlxG.keys.TWO)
            {
                FlxG.state = new CaveState();
            }


            base.update();
        }


    }
}
