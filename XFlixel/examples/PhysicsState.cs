using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;
#if WINDOWS 
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics;

namespace org.flixel.examples
{
    public class PhysicsState : BaseExampleState
    {

        private FlxSprite g1;

        private World _world;
        private Body _ground;

        override public void create()
        {
            base.create();

            //FlxG.hideHud();
            FlxG.resetHud();

            _world = new World(new Vector2(0, 98.0f));

            _ground = BodyFactory.CreateRectangle(_world, 3000, 20, 1.0f, new Vector2(0, 700), null);
            _ground.BodyType = BodyType.Static;

            g1 = new FlxSprite(0, 700);
            g1.loadGraphic("flixel/diagnostic/testpalette", true, false, 1000, 20);
            add(g1);

            for (int i = 0; i < 40; i++)
            {
                FarSprite f = new FarSprite(Convert.ToInt32(FlxU.random(90, 800)), Convert.ToInt32(FlxU.random(-600, 0)), _world);
                f.loadGraphic("flixel/initials/crate_80x60", true, false, FlxU.randomInt(20, 40), FlxU.randomInt(40, 60));
                f._body.AngularVelocity = FlxU.randomInt(0, 5);
                add(f);

            }


            List<Dictionary<string, string>> completeSet = new List<Dictionary<string, string>>();

            string currentLevel = "l" + FlxG.level.ToString();

            XDocument xdoc = XDocument.Load("Content/flixel/ogmo/PhysicsLevel.oel");

            // Load level main stats.
            XElement xelement = XElement.Load("Content/flixel/ogmo/PhysicsLevel.oel");
            IEnumerable<XAttribute> attList =
            from at in xelement.Attributes()
            select at;

            foreach (XAttribute xAttr in attList)
            {
                //Console.WriteLine(xAttr.Name.ToString() + "  " + xAttr.Value.ToString());
                //levelAttrs.Add(xAttr.Name.ToString(), xAttr.Value.ToString());
            }

            foreach (XElement xEle in xdoc.Descendants("level").Descendants("NewLayer0").Elements())
            {
                Console.WriteLine(xEle.Name.ToString() + "  " + xEle.Attribute("x").ToString().Split('=')[1].Replace('\"', ' ') );


                FarSprite f = new FarSprite(
                    Convert.ToInt32(xEle.Attribute("x").ToString().Split('=')[1].Replace('\"', ' '))+500, 
                    Convert.ToInt32(xEle.Attribute("y").ToString().Split('=')[1].Replace('\"', ' '))+220,
                    _world);

                f.loadGraphic("flixel/initials/crate_80x60", true, false, 
                    Convert.ToInt32(xEle.Attribute("width").ToString().Split('=')[1].Replace('\"', ' ')),
                    Convert.ToInt32(xEle.Attribute("height").ToString().Split('=')[1].Replace('\"', ' ')));
                
                add(f);
                
            }

        }

        

        override public void update()
        {

            if (FlxG.mouse.justPressed())
            {
                FarSprite f = new FarSprite(FlxG.mouse.screenX, FlxG.mouse.screenY, _world);
                f.loadGraphic("flixel/initials/crate_80x60", true, false, FlxU.randomInt(20, 40), FlxU.randomInt(40, 60));
                //f._body.AngularVelocity = FlxU.randomInt(0, 5);
                //f._body.ApplyLinearImpulse(new Vector2(FlxG.mouse.screenX, FlxG.mouse.screenY), new Vector2(FlxG.mouse.screenX, FlxG.mouse.screenY));
                //f._body.ApplyLinearImpulse(new Vector2(FlxU.random(-15000, 15000), FlxU.random(-15000, 15000)));
                //f._body.LinearVelocity = new Vector2(100000000000000000, 1000000000000000) ;


                f._body.ApplyLinearImpulse(new Vector2(int.MaxValue, int.MaxValue * -1));
                f._body.Mass = 2100;
                add(f);

            }

            _world.Step(Math.Min((float)FlxG.elapsedAsGameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            base.update();
        }


    }
}
#endif