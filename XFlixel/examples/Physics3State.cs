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
    public class Physics3State : FarState
    {

        private FarSprite puck;
        private FlxGroup team1;
        private FlxGroup team2;

        override public void create()
        {
            base.create();

            FlxG.resetHud();
            FlxG.hideHud();

            _world.Gravity.Y = 0;

            team1 = new FlxGroup();
            team2 = new FlxGroup();

            puck = new FarSprite(FlxG.width / 2, FlxG.height / 2, _world);
            puck.createGraphic(20, 20, Color.Green);
            puck.attachCircle(20, 20);
            add(puck);

            puck._body.ApplyLinearImpulse(new Vector2(FlxU.random(-15000, 15000), FlxU.random(-15000, 15000)));


            for (int i = 0; i < 6; i++)
            {
                FarSprite p1 = new FarSprite(i * 80, (int)FlxU.random(0,FlxG.height), _world);

                p1.createGraphic(4, 40, Color.Red);

                p1.attachRectangle(4, 40);

                team1.add(p1);

            }

            add(team1);
            add(team2);

        }

        override public void update()
        {

            if (FlxG.keys.ONE)
            {
                //((FarSprite)(team1.members[0]))._body.ApplyAngularImpulse(5550);
                foreach (FarSprite item in team1.members)
                {
                    item._body.ApplyAngularImpulse(5550);
                }

            }
            if (FlxG.keys.TWO)
            {
                //((FarSprite)(team1.members[0]))._body.ApplyAngularImpulse(5550);
                foreach (FarSprite item in team1.members)
                {
                    item._body.ApplyAngularImpulse(-5550);
                }

            }
            if (FlxG.keys.SPACE)
            {
                puck._body.Position = new Vector2(FlxG.width / 2, FlxG.height / 2);
                puck._body.ApplyLinearImpulse(new Vector2(FlxU.random(-15000, 15000), FlxU.random(-15000, 15000)));

            }


            base.update();
        }


    }
}
