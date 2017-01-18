using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using XNATweener;

namespace org.flixel
{
    /// <summary>
    /// @benbaird X-flixel only. Moves all of the flixel logo screen stuff to a FlxState.
    /// 
    /// @initials_games customised for initials releases.
    /// </summary>
    public class FlxSplash : FlxState
    {
        //logo stuff
        private List<FlxLogoPixel> _f;
        private static Color _fc = Color.Yellow;
        private float _logoTimer = 0;
        private Texture2D _poweredBy;
        private SoundEffect _fSound;
        private static FlxState _nextScreen;

        private Texture2D _initialsLogo;
        private string SndTag = "";

        private FlxSprite _logo;

        private Tweener _logoTweener;

        private FlxText debugMode;
        private string cheatStorage = "";

        public FlxSplash()
            : base()
        {
        }

        public override void create()
        {
            base.create();

            FlxSprite bg = new FlxSprite(0, 0);
            bg.createGraphic(FlxG.width, FlxG.height, FlxG.splashBGColor);
            add(bg);

            _f = null;
            _poweredBy = FlxG.Content.Load<Texture2D>("flixel/poweredby");
            _fSound = FlxG.Content.Load<SoundEffect>(FlxG.splashAudioWaveFlixel);

            _initialsLogo = FlxG.Content.Load<Texture2D>(FlxG.splashLogo);

            _logo = new FlxSprite();
            _logo.loadGraphic(_initialsLogo, false, false, 216,24);
            _logo.x = FlxG.width / 2 - 216 / 2;
            _logo.y = FlxG.height / 2 - 24;
            add(_logo);

            _logoTweener = new Tweener(-150, FlxG.height / 2 - 24, TimeSpan.FromSeconds(0.9f), Bounce.EaseOut);

            SndTag = FlxG.splashAudioWave;
            //FlxG.play(SndTag,1.0f);

            FlxG.transition.startFadeIn(0.1f);

            debugMode = new FlxText(10, 90, 200, "DEBUG MODE!");
            debugMode.visible = false;
            add(debugMode);

        }

        public static void setSplashInfo(Color flixelColor, FlxState nextScreen)
        {
            _fc = flixelColor;
            _nextScreen = nextScreen;
        }

        public override void update()
        {
            if (FlxG.keys.justPressed(Keys.B)) { cheatStorage+="B";}
            if (FlxG.keys.justPressed(Keys.U)) { cheatStorage+="U";}
            if (FlxG.keys.justPressed(Keys.G)) { cheatStorage+="G";}
            if (FlxG.keys.justPressed(Keys.S)) { cheatStorage += "S"; }

            if (cheatStorage=="BUGGS")
            {
                debugMode.visible = true;
                FlxG.debug = true;
            }

            _logoTweener.Update(FlxG.elapsedAsGameTime);
            _logo.y = _logoTweener.Position;
            if (_logoTimer > 1.15f)
            {
                //FlxG.bloom.Visible = true;
                //FlxG.bloom.bloomIntensity += 1.5f;
                //FlxG.bloom.baseIntensity += 1.0f;
                //FlxG.bloom.blurAmount += 1.1f;
            }
            if (_f == null && _logoTimer > 2.5f)
            {

                //_logo.visible = false;

                _logoTweener.Reverse();
                _logoTweener.Start();

                //FlxG.flash.start(FlxG.backColor, 1.0f, null, false);

                _f = new List<FlxLogoPixel>();
                int scale = 10;
                float pwrscale;

                int pixelsize = (FlxG.height / scale);
                int top = (FlxG.height / 2) - (pixelsize * 2);
                int left = (FlxG.width / 2) - pixelsize;

                pwrscale = ((float)pixelsize / 24f);

                //Add logo pixels
                add(new FlxLogoPixel(left + pixelsize, top, pixelsize, 0, _fc));
                add(new FlxLogoPixel(left, top + pixelsize, pixelsize, 1, _fc));
                add(new FlxLogoPixel(left, top + (pixelsize * 2), pixelsize, 2, _fc));
                add(new FlxLogoPixel(left + pixelsize, top + (pixelsize * 2), pixelsize, 3, _fc));
                add(new FlxLogoPixel(left, top + (pixelsize * 3), pixelsize, 4, _fc));

                FlxSprite pwr = new FlxSprite((FlxG.width - (int)((float)_poweredBy.Width * pwrscale)) / 2, top + (pixelsize * 4) + 16, _poweredBy);
                pwr.loadGraphic(_poweredBy, false, false, 64);

                //pwr.color = _fc;
                //pwr.scale = pwrscale;
                add(pwr);

                _fSound.Play(FlxG.volume, 0f, 0f);
            }

            _logoTimer += FlxG.elapsed;
            
            base.update();
            if (FlxG.keys.ONE) FlxG.level = 1;
            if (FlxG.keys.TWO) FlxG.level = 2;
            if (FlxG.keys.THREE) FlxG.level = 3;

            if (FlxG.keys.FOUR) FlxG.level = 4;
            if (FlxG.keys.FIVE) FlxG.level = 5;
            if (FlxG.keys.SIX) FlxG.level = 6;

            if (FlxG.keys.SEVEN) FlxG.level = 7;
            if (FlxG.keys.EIGHT) FlxG.level = 8;
            if (FlxG.keys.NINE) FlxG.level = 9;

            if (_logoTimer > 5.5f || FlxG.keys.SPACE || FlxG.keys.ENTER || FlxG.gamepads.isButtonDown(Buttons.A))
            {
                FlxG.destroySounds(true);

                //FlxG.bloom.Visible = false;

                FlxG.state = _nextScreen;
            }
			if (FlxG.keys.F1 || (FlxG.gamepads.isButtonDown(Buttons.RightStick) && FlxG.gamepads.isButtonDown(Buttons.LeftStick)))
            {
                //FlxG.bloom.Visible = false;
                FlxG.destroySounds(true);
				#if !__ANDROID__
				FlxG.state = new org.flixel.examples.TestState();
				#endif

            }
        }
    }
}
