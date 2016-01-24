/*
 * Add these to Visual Studio to quickly create new FlxSprites
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace org.flixel.examples
{
    class CarSprite : FlxSprite
    {
        public CarSprite(float xPos, float yPos)
            : base(xPos, yPos)
        {
            loadGraphic(FlxG.Content.Load<Texture2D>("flixel/surt/race_or_die"), true, false, 128, 128);
            scale = 3;
        }
        
        /// <summary>
        /// The Update Cycle. Called once every cycle.
        /// </summary>
        override public void update()
        {
            base.update();
        }

        
    }
}
