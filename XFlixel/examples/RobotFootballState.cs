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
    // This top line would usually be:
    //public class RobotFootballState : FlxState
    // We use a BaseExampleState to add common functionality 

    /*
     * This examples shows how to use FlxSprites, FlxGroups and FlxEmitters.
     */ 

    public class RobotFootballState : BaseExampleState
    {
        //Define a playing field
        FlxTileblock playingField;

        FlxGroup team1;
        FlxGroup team2;

        

        override public void create()
        {
            base.create();

            FlxG.mouse.hide();
            FlxG.hideHud();

            playingField = new FlxTileblock(0, 0, 640, 640);
            playingField.auto = FlxTileblock.RANDOM;
            playingField.loadTiles(FlxG.Content.Load<Texture2D>("initials/sports_ground"), 16, 16, 0);
            add(playingField);

            team1 = new FlxGroup();
            team2 = new FlxGroup();

            // Create two teams of 7 robots;
            for (int i = 0; i < 7; i++)
            {
                FlxSprite robot = new FlxSprite(20 + (i * 90), 10);
                robot.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
                robot.addAnimation("Static", new int[] { 7 }, 0, true);
                robot.play("Static");
                robot.angle = 270;
                robot.velocity.Y = FlxU.random(10, 100);
                //robot.width = 32;
                //robot.height = 32;
                //robot.offset.X = 16;
                //robot.offset.Y = 16;
                team1.add(robot);


            }
            for (int i = 0; i < 7; i++)
            {
                FlxSprite robot = new FlxSprite(20 + (i * 90), 200);
                robot.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
                robot.addAnimation("Static", new int[] { 9 }, 0, true);
                robot.play("Static");
                robot.angle = 90;
                robot.velocity.Y = FlxU.random(-10, -100);
                //robot.width = 32;
                //robot.height = 32;
                //robot.offset.X = 16;
                //robot.offset.Y = 16;
                team2.add(robot);

            }


            add(team1);
            add(team2);

            


            
        }

        override public void update()
        {
            if (FlxG.keys.justPressed(Keys.B))
            {
                FlxG.showBounds = true;
            }

            //Reset
            if (FlxG.keys.justPressed(Keys.Space))
            {
                int i = 0;
                foreach (FlxSprite robot in team1.members)
                {
                    robot.dead = false;
                    robot.exists = true;
                    robot.x = 20 + (i * 90);
                    robot.y = 10;
                    robot.velocity.Y = FlxU.random(10, 100);
                    i++;
                }
                i = 0;
                foreach (FlxSprite robot in team2.members)
                {
                    robot.dead = false;
                    robot.exists = true;

                    // or
                    // robot.reset(20, 20); 

                    robot.x = 20 + (i * 90);
                    robot.y = 200;
                    robot.velocity.Y = FlxU.random(-10, -100);
                    i++;
                }


            }



            FlxU.overlap(team1, team2, overlapped);

            base.update();
        }

        protected bool overlapped(object Sender, FlxSpriteCollisionEvent e)
        {
            //you can fire functions on each object.
            //((FlxObject)(e.Object1)).overlapped(e.Object2);
            //((FlxObject)(e.Object2)).overlapped(e.Object1);
            //if (e.Object1.dead == false && e.Object2.dead == false)
            //{
            //    if (Math.Abs(e.Object1.velocity.Y) > Math.Abs(e.Object2.velocity.Y))
            //    {
            //        e.Object2.kill();
            //    }
            //    else if (Math.Abs(e.Object2.velocity.Y) < Math.Abs(e.Object1.velocity.Y))
            //    {
            //        e.Object1.kill();
            //    }
            //}

            if (FlxU.random() < 0.5f)
            {
                e.Object2.kill();
            }
            else
            {
                e.Object1.kill();
            }



            return true;
        }


    }
}
