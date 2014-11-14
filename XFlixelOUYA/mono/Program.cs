#region File Description
//-----------------------------------------------------------------------------
// Flixel for XNA.
// Original repo : https://github.com/StAidan/X-flixel
// Extended and edited repo : https://github.com/initials/XNAMode
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.flixel;
using Microsoft.Xna.Framework;

namespace Loader_SuperLemonadeFactory
{
	/// <summary>
	/// Flixel enters here.
	/// <code>FlxFactory</code> refers to it as the "masterclass".
	/// </summary>
	public class FlixelEntryPoint2 : FlxGame
	{
		public FlixelEntryPoint2(Game game)
			: base(game)
		{

			Console.WriteLine("PROGRAM -- ");

			int w = 1920;
			int h = 1080;

			initGame(w, h, new org.flixel.TestState(), new Color(15, 15, 15), true, new Color(5, 5, 5));

			FlxG.debug = false;
			FlxG.level = 1;




		}
	}
}
