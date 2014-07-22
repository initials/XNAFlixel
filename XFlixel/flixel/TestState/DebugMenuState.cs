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

            string textInfo = "";
            textInfo = "Choose:\n";
            textInfo += "1. Camera Test State\n";
            
            FlxG.setHudText(1, textInfo);
            FlxG.setHudGamepadButton(FlxHud.TYPE_KEYBOARD, FlxButton.ControlPadA,10, 10);
            FlxG.showHud();

            
        }

        override public void update()
        {

            if (FlxG.keys.ONE)
            {
                FlxG.state = new CameraTestState();
                
            }
            
            base.update();
        }


    }
}
