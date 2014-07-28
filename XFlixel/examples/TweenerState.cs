using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using org.flixel;

using System.Linq;
using System.Xml.Linq;

// Use the XNATweener namespace.
using XNATweener;

namespace org.flixel
{
    public class TweenerState : FlxState
    {

        private Tweener tween;
        private FlxSprite car;

        override public void create()
        {
            base.create();

            tween = new Tweener(20, 300, TimeSpan.FromSeconds(3.0f), Quadratic.EaseOut);
            tween.Loop = true;

            car = new FlxSprite(20, 20);
            car.loadGraphic(FlxG.Content.Load<Texture2D>("surt/race_or_die"), true, false, 64, 64);
            car.addAnimation("Static", new int[] { 8 }, 0, true);
            car.play("Static");
            add(car);


        }

        override public void update()
        {
            tween.Update(FlxG.elapsedAsGameTime);
            car.x = tween.Position;



            base.update();
        }


    }
}
