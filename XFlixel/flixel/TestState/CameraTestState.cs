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
    public class CameraTestState : FlxState
    {

        override public void create()
        {
            FlxG.backColor = Color.DarkGreen;

            base.create();

            //FlxCamera cam1 = new FlxCamera(0, 0, FlxG.width/2, FlxG.height, 1);
            //cam1.color = Color.Blue;
            //FlxG.cameras.Add(cam1);

            //FlxCamera cam2 = new FlxCamera(FlxG.width, 0, FlxG.width / 2, FlxG.height, 1);
            //cam2.color = Color.GreenYellow;
            //FlxG.cameras.Add(cam2);

            FlxCamera cam1 = new FlxCamera(0, 0, 100, 100, 1);
            cam1.color = Color.Blue;
            FlxG.cameras.Add(cam1);

            FlxCamera cam2 = new FlxCamera(100, 0, 100, 100, 1);
            cam2.color = Color.GreenYellow;
            FlxG.cameras.Add(cam2);


            FlxSprite s = new FlxSprite(0,0, FlxG.Content.Load<Texture2D>("initials/initialsLogo"));
            add(s);

        }

        override public void update()
        {




            base.update();
        }


    }
}
