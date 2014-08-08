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
    public class AngleAndVelocityState : BaseExampleState
    {
        override public void create()
        {
            base.create();

            FlxSprite robot = new FlxSprite(0, 0);
            robot.loadGraphic("surt/race_or_die", true, false, 64, 64);
            robot.addAnimation("static", new int[] { 9 }, 0, true);
            robot.play("static");
            add(robot);

            robot.angle = 210;
            robot.setVelocityFromAngle(100);



        }

        override public void update()
        {


            base.update();
        }


    }
}
