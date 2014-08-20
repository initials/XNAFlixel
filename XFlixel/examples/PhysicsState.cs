//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using org.flixel;

//using System.Linq;
//using System.Xml.Linq;

//using FarseerPhysics.Common;
//using FarseerPhysics.Dynamics;
//using FarseerPhysics.Dynamics.Joints;
//using FarseerPhysics.Factories;
//using FarseerPhysics.Dynamics.Contacts;
//using FarseerPhysics;

//namespace org.flixel
//{
//    public class PhysicsState : BaseExampleState
//    {

//        private FlxSprite b1;
//        private FlxSprite b2;
//        private FlxSprite g1;

//        private Body _body1;
//        private Body _body2;

//        private World _world;
//        private Body _ground;


//        override public void create()
//        {
//            base.create();

//            //FlxG.hideHud();
//            FlxG.resetHud();

//            b1 = new FlxSprite(20, 20);
//            b1.loadGraphic("initials/crate_80x60", true, false, 80, 60);
//            add(b1);

//            b2 = new FlxSprite(20, 20);
//            b2.loadGraphic("initials/crate_80x60", true, false, 80, 60);
//            add(b2);


//            _world = new World(new Vector2(0, 98.0f));

//            //_body1 = new Body(_world, new Vector2(40,40), 0, null);
//            _body1 = BodyFactory.CreateBody(_world, new Vector2(95,-40), 0, null);
//            FixtureFactory.AttachRectangle(80,60, 1f, new Vector2(0f, 0f), _body1);
//            _body1.BodyType = BodyType.Dynamic;
//            _body1.Mass = 21f;
//            //_body1.AngularVelocity = 110.0f;
//            _body1.Awake = true;
//            _body1.OnCollision += MyOnCollision;

//            _body2 = BodyFactory.CreateBody(_world, new Vector2(40, 110), 0, null);
//            FixtureFactory.AttachRectangle(80, 60, 1f, new Vector2(0f, 0f), _body2);
//            _body2.BodyType = BodyType.Dynamic;
//            _body2.Mass = 21f;
//            //_body2.AngularVelocity = -110.0f;
//            _body2.Awake = true;
//            _body2.OnCollision += MyOnCollision;
            


//            _ground = BodyFactory.CreateRectangle(_world, 1000, 20, 1.0f, new Vector2(0,200),null);
//            _ground.BodyType = BodyType.Static;
            
//            g1 = new FlxSprite(0, 240);
//            g1.loadGraphic("diagnostic/testpalette", true, false, 1000, 20);
//            add(g1);



//        }

//        public void MyOnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2)
//        {

//        }
//        public bool MyOnCollision(Fixture f1, Fixture f2, Contact contact)
//        {
//            return true;
//        }



//        override public void update()
//        {

//            FlxG.setHudText(1, _body1.Position.Y.ToString() + "   " + _body1.Rotation.ToString() );





            
//            _world.Step(Math.Min((float)FlxG.elapsedAsGameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

//            //_world.ContactManager.OnBroadphaseCollision += MyOnBroadphaseCollision;


//            if (FlxG.keys.A)
//            {
//                _body1.ApplyLinearImpulse(new Vector2(-1000, 0));
//                _body2.ApplyLinearImpulse(new Vector2(1000, 0));
//            }
//            if (FlxG.keys.D)
//            {
//                _body1.ApplyLinearImpulse(new Vector2(1000, 0));
//                _body2.ApplyLinearImpulse(new Vector2(-1000, 0));
//            }
//            if (FlxG.keys.S)
//            {
//                _body1.ApplyLinearImpulse(new Vector2(0,-1000));
//                _body2.ApplyLinearImpulse(new Vector2(0,1000));
//            }
//            if (FlxG.keys.W)
//            {
//                _body1.ApplyLinearImpulse(new Vector2(0, 1000));
//                _body2.ApplyLinearImpulse(new Vector2(0, -1000));
//            }



//            base.update();

//            b1.x = _body1.Position.X + 30;
//            b1.y = _body1.Position.Y + 20;
//            //_angle * (Math.PI / 180);
//            b1.angle = _body1.Rotation ; 

//            b2.x = _body2.Position.X + 30;
//            b2.y = _body2.Position.Y + 20;
//            b2.angle = _body2.Rotation;
//        }


//    }
//}
