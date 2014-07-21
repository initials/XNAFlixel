using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    /// <summary>
    /// Just a helper structure for the FlxSprite animation system
    /// </summary>
	
    public class FlxAnim
    {
        /// <summary>
        /// Name of the animation as a string.
        /// </summary>
		public string name;

        /// <summary>
        /// Frame rate.
        /// </summary>
		public float delay;

        /// <summary>
        /// Frame number sequence.
        /// Usage: new int[] {0,1,2,3,4,5,6,7,8,9 }
        /// </summary>
		public int[] frames;

        /// <summary>
        /// Whether to loop or not.
        /// </summary>
		public bool looped;

        /// <summary>
        /// FlxAnim stores an animation.
        /// </summary>
        /// <param name="Name">What this animation should be called (e.g. "run")</param>
        /// <param name="Frames">An array of numbers indicating what frames to play in what order new int [] {1, 2, 3, 0}</param>
        /// <param name="FrameRate">The speed in frames per second that the animation should play at (e.g. 40 fps)</param>
        /// <param name="Looped">Whether or not the animation is looped or just plays once</param>
		public FlxAnim(string Name, int[] Frames, int FrameRate, bool Looped)
		{
			name = Name;
			delay = 1.0f / (float)FrameRate;
			frames = Frames;
			looped = Looped;
		}
        /// <summary>
        /// FlxAnim stores an animation.
        /// </summary>
        /// <param name="Name">What this animation should be called (e.g. "run")</param>
        /// <param name="Frames">An array of numbers indicating what frames to play in what order new int [] {1, 2, 3, 0}</param>
        /// <param name="FrameRate">The speed in frames per second that the animation should play at (e.g. 40 fps)</param>
        public FlxAnim(string Name, int[] Frames, int FrameRate)
        {
            name = Name;
            delay = 1.0f / (float)FrameRate;
            frames = Frames;
            looped = true;
        }
        /// <summary>
        /// Constructor overloads
        /// </summary>
        /// <param name="Name">What this animation should be called (e.g. "run")</param>
        /// <param name="Frames">An array of numbers indicating what frames to play in what order new int [] {1, 2, 3, 0}</param>
        public FlxAnim(string Name, int[] Frames)
        {
            name = Name;
            delay = 0f;
            frames = Frames;
            looped = true;
        }

    }

}
