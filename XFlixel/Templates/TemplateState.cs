///*
// * Add these to Visual Studio to quickly create new FlxStates
// */

//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using org.flixel;
//using System.Linq;
//using System.Xml.Linq;
//namespace $rootnamespace$
//{
//    public class $safeitemname$ : FlxState
//    {
//        override public void create()
//        {
//            base.create();
//            //FlxG.mouse.show("Mode/cursor");

//        }
//        override public void update()
//        {
//            base.update();
//        }

//        protected bool overlapped(object Sender, FlxSpriteCollisionEvent e)
//        {
//            ((FlxObject)(e.Object1)).overlapped(e.Object2);
//            ((FlxObject)(e.Object2)).overlapped(e.Object1);
//            return true;
//        }

//    }
//}
