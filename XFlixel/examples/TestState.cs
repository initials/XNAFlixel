﻿using System;
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
    /// This is a test state.
    /// </summary>
    public class TestState : FlxState
    {
        override public void create()
        {
            base.create();

            FlxG.mouse.show(FlxG.Content.Load<Texture2D>("flixel/cursor"));

            //FlxText t = new FlxText(20, 20, 100);
            //t.text = "Welcome To XNA Flixel\nTwitter: @initials_games\nPress Q for more options.";
            //t.alignment = FlxJustification.Left;
            //add(t);

            FlxG.resetHud();

            string textInfo = "";
            textInfo = "Choose:\n";
            textInfo += "1. Cheat State\n";
            textInfo += "2. Cave State\n";
            textInfo += "3. Race Or Die\n";
            textInfo += "4. Tweens\n";
            textInfo += "5. Robot Football\n";
            textInfo += "6. Rugby League\n";
            textInfo += "7. \n";
            textInfo += "8. \n";
            textInfo += "9. \n";
            textInfo += "10. \n";


            textInfo += "Q. Garbage Tests \n";
            FlxG.setHudText(1, textInfo);
            FlxG.setHudTextPosition(1, 20, 20);
            FlxG.setHudTextScale(1, 2);
            FlxG.setHudGamepadButton(FlxHud.TYPE_KEYBOARD, FlxHud.Keyboard_1, FlxG.width - 40, 30);

            FlxG.showHud();




        }

        override public void update()
        {
            if (FlxG.keys.justPressed(Keys.Q))
            {
                FlxG.state = new DebugMenuState();
            }
            
            
            
            if (FlxG.keys.ONE)
            {
                FlxG.state = new CheatState();
            }
            if (FlxG.keys.TWO)
            {
                FlxG.state = new CaveState();
            }
            if (FlxG.keys.THREE)
            {
                FlxG.state = new RaceOrDieState();
            }
            if (FlxG.keys.FOUR)
            {
                FlxG.state = new TweenerState();
            }
            if (FlxG.keys.FIVE)
            {
                FlxG.state = new RobotFootballState();
            }
            if (FlxG.keys.SIX)
            {
                FlxG.state = new PlayState();
            }
            base.update();
        }


    }
}
