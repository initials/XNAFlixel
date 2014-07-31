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

        FlxGroup team1;
        FlxGroup team2;



        override public void create()
        {
            base.create();

            FlxG.mouse.hide();
            FlxG.hideHud();

            playingField = new FlxTileblock(0, 0, 122*16, 68*16);
            playingField.auto = FlxTileblock.RANDOM;
            playingField.loadTiles(FlxG.Content.Load<Texture2D>("initials/sports_ground"), 16, 16, 0);
            add(playingField);

            team1 = new FlxGroup();
            team2 = new FlxGroup();

            // Create two teams of 7 robots;
            for (int i = 0; i < 14; i++)
            {
                Player player = new Player(20 + (i * 90), 10, i+1);
                team1.add(player);
            }
            for (int i = 0; i < 14; i++)
            {
                Player player = new Player(20 + (i * 90), 200, i + 1);
                team2.add(player);
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
