using System;
using System.Collections.Generic;


namespace org.flixel
{
    /// <summary>
    /// FlxMonitor is a simple class that aggregates and averages data.
    /// Flixel uses this to display the framerate and profiling data
    /// in the developer console.  It's nice for keeping track of
    /// things that might be changing too fast from frame to frame.
    /// </summary>
    public class FlxMonitor
    {
		/// <summary>
        /// Stores the requested size of the monitor array.
		/// </summary>
		protected int _size;
		/// <summary>
        /// Keeps track of where we are in the array.
		/// </summary>
		protected int _itr;
		/// <summary>
        /// An array to hold all the data we are averaging.
		/// </summary>
		protected List<float> _data;

        /// <summary>
        /// Creates the monitor array and sets the size.
        /// </summary>
        /// <param name="Size">The desired size - more entries means a longer window of averaging.</param>
        /// <param name="Default">The default value of the entries in the array (0 by default).</param>
		public FlxMonitor(int Size, float Default)
		{
			_size = Size;
			if(_size <= 0)
				_size = 1;
			_itr = 0;
			_data = new List<float>(_size);
			int i = 0;
			while(i < _size)
				_data[i++] = Default;
		}
		
        /// <summary>
        /// Adds an entry to the array of data.
        /// </summary>
        /// <param name="Data">The value you want to track and average.</param>
		public void add(float Data)
		{
            if (_itr < _data.Count)
            {
                _data[_itr++] = Data;
            }
			if(_itr >= _size)
				_itr = 0;
		}
		
        /// <summary>
        /// Averages the value of all the numbers in the monitor window.
        /// </summary>
        /// <returns>The average value of all the numbers in the monitor window.</returns>
		public float average()
		{
			float sum = 0;
			int i = 0;
			while(i < _size)
				sum += _data[i++];
			return sum/_size;
		}
    }
}
