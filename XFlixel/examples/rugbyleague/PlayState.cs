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
        FlxTileblock playingField;

        FlxSprite ball;

        FlxGroup team1;
        FlxGroup team2;



        override public void create()
        {
            base.create();

            FlxG.mouse.hide();
            FlxG.hideHud();

            playingField = new FlxTileblock(0, 0, 122*16, 68*16);
            playingField.auto = FlxTileblock.RANDOM;
            playingField.loadTiles(FlxG.Content.Load<Texture2D>("examples/sports_ground"), 16, 16, 0);
            add(playingField);

            ball = new FlxSprite(122 * 8, 68 * 8);
            add(ball);

            FlxG.follow(ball, 10);
            FlxG.followBounds(0, 0, 122 * 16, 68 * 16);


            team1 = new FlxGroup();
            team2 = new FlxGroup();

            // Create two teams of 7 robots;
            for (int i = 0; i < 14; i++)
            {
                Player player = new Player(61 * 8 + (i * 64), (int)ball.y - 50, i + 1);
                player.color = Color.Blue;
                team1.add(player);

                if (i == 6) player.isSelected = true;
            }
            for (int i = 0; i < 14; i++)
            {
                Player player = new Player(61 * 8 + (i * 64), (int)ball.y + 50, i + 1);
                player.color = Color.Red;
                team2.add(player);


                if (i == 6) player.isSelected = true;
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

            base.update();
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
