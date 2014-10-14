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
    public class FlxBarState : BaseExampleState
    {
        FlxBar bar;
        FlxBar bar2;
        FlxSprite robot;

        override public void create()
        {
            base.create();

            FlxG.hideHud();
            FlxG.resetHud();

            robot = new FlxSprite(60, 60);
            robot.loadGraphic("surt/race_or_die", true, false, 64, 64);
            robot.addAnimation("static", new int[] { 0 }, 12, true);
            robot.play("static");
            robot.angle = 0;
            robot.width = 32;
            robot.height = 32;
            robot.setOffset(12, 12);
            robot.health = 100;
            robot.alpha = 0.1f;

            add(robot);

            bar = new FlxBar(30, 30, FlxBar.FILL_LEFT_TO_RIGHT, 100, 10, robot, "rad", 0, 100, true);
            add(bar);

            bar2 = new FlxBar(60, 90, FlxBar.FILL_LEFT_TO_RIGHT, 20, 2, null, "health", 0, 24, false);
            bar2.loadCustomEmptyGraphic("initials/healthBar");
            bar2.emptyBar.offset.X = 2;
            bar2.emptyBar.offset.Y = 3;
            add(bar2);



        }

        override public void update()
        {

            if (FlxG.mouse.justPressed())
            {
                robot.hurt(2);

                bar2.setValue(robot.health);
                
            }


            //if (FlxG.keys.justPressed(Keys.W))
            //{
            //    bar2.emptyBar.offset.Y -= 1;
            //}
            //if (FlxG.keys.justPressed(Keys.S))
            //{
            //    bar2.emptyBar.offset.Y += 1;
            //}
            //if (FlxG.keys.justPressed(Keys.A))
            //{
            //    bar2.emptyBar.offset.X -= 1;
            //}
            //if (FlxG.keys.justPressed(Keys.D))
            //{
            //    bar2.emptyBar.offset.X += 1;
            //}
            //Console.WriteLine("Offset {0} {1}", bar2.emptyBar.offset.X, bar2.emptyBar.offset.Y);

            base.update();
        }


    }
}
