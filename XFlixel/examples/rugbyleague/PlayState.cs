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
    public class PlayState : BaseExampleState
    {

        //Define a playing field
        FlxTilemap playingField;

        Ball ball;

        Team team1;
        Team team2;



        override public void create()
        {
            base.create();

            FlxG.mouse.hide();
            FlxG.hideHud();

            Dictionary<string, string> levelInformation = new Dictionary<string, string>();
            levelInformation = FlxXMLReader.readAttributesFromOelFile("OgmoEditor2/SportsGround.oel", "level/SportsField");

            //0, 0, 122*16, 68*16
            playingField = new FlxTilemap();
            playingField.auto = FlxTileblock.STRING;
            playingField.loadMap(levelInformation["SportsField"], FlxG.Content.Load<Texture2D>("examples/sports_ground"), 16,16);
            add(playingField);


            ball = new Ball(78 * 8, 132 * 8);
            add(ball);
            FlxG.follow(ball, Registry.FOLLOW_LERP);
            FlxG.followBounds(0, 0, 78 * 16, 132 * 16);



            team1 = new Team();
            team2 = new Team();

            // Create two teams of 7 robots;
            for (int i = 0; i < 13; i++)
            {
                //Player player = new Player(90 + (i * 80), (int)ball.y - 50, i + 1);
                Player player = new Player(
                    (int)Registry.StartPositions_KickOffAttack[i].X, 
                    (int)Registry.StartPositions_KickOffAttack[i].Y, 
                    i + 1, ball);

                player.color = Color.Blue;
                team1.add(player);

                if (i == 6)
                {
                    player.isSelected = true;

                }
            }
            for (int i = 0; i < 13; i++)
            {
                //Player player = new Player(90 + (i * 80), (int)ball.y + 50, i + 1);

                Player player = new Player(
                (int)Registry.StartPositions_KickOffDefense[i].X,
                (int)Registry.StartPositions_KickOffDefense[i].Y,
                i + 1, ball);

                player.color = Color.Red;
                team2.add(player);


                //if (i == 6) player.isSelected = true;
            }

            add(team1);
            add(team2);
        }

        override public void update()
        {
            if (FlxG.keys.justPressed(Keys.B))
            {
                FlxG.showBounds = !FlxG.showBounds;
            }

            //if (FlxG.keys.justPressed(Keys.OemComma))
            //{
            //    for (int i = 0; i < team1.members.Count; i++)
            //    {
            //        if (((Player)team1.members[i]).isSelected == true)
            //        {
            //            ((Player)team1.members[i]).isSelected = false;
            //            ((Player)team1.members[i+1]).isSelected = true;
            //        }
            //    }
            //}
            //if (FlxG.keys.justPressed(Keys.OemPeriod))
            //{
            //    for (int i = 0; i < team1.members.Count; i++)
            //    {
            //        if (((Player)team1.members[i]).isSelected == true)
            //        {
            //            ((Player)team1.members[i]).isSelected = false;
            //            ((Player)team1.members[i - 1]).isSelected = true;
            //        }
            //    }
            //}

            FlxU.overlap(team1, team2, overlapped);

            FlxU.overlap(team1, ball, overlappedBall);
            FlxU.overlap(team2, ball, overlappedBall);


            base.update();
        }

        protected bool overlappedBall(object Sender, FlxSpriteCollisionEvent e)
        {
            //Console.WriteLine("Overlapped Ball " + e.Object2.ToString());

            //you can fire functions on each object.
            ((FlxObject)(e.Object1)).overlapped(e.Object2);
            ((FlxObject)(e.Object2)).overlapped(e.Object1);



            if (((Ball)(e.Object2)).timeSincePass > 0.25f)
            {
                ((Player)(e.Object1)).hasBall = true;
                ((Player)(e.Object1)).isSelected = true;
            }

            return true;
        }

        protected bool overlapped(object Sender, FlxSpriteCollisionEvent e)
        {
            //you can fire functions on each object.
            //((FlxObject)(e.Object1)).overlapped(e.Object2);
            //((FlxObject)(e.Object2)).overlapped(e.Object1);




            return true;
        }


    }
}
