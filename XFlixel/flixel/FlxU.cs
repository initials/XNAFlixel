using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace org.flixel
{
    public class FlxU
    {
        //@benbaird Replacement for AS3's Math.random() method
        /// <summary>
        /// Replacement for AS3's Math.random() method
        /// </summary>
        private static Random _randglobal = new Random();
        [ThreadStatic]
        private static Random _rand;

        /// <summary>
        /// Helps to eliminate false collisions and/or rendering glitches caused by rounding errors
        /// Rounding error is actually 0.0000001f in AS3 flixel, but doesn't work correctly
        /// here because we don't use double-precision floats.
        /// </summary>
        static internal float roundingError = 0.0001f; // Rounding error is actually 0.0000001f in AS3 flixel, but doesn't work correctly
        // here because we don't use double-precision floats.
        /// <summary>
        /// The last quad tree you generated will be stored here for reference or whatever.
        /// </summary>
        static public FlxQuadTree quadTree;

        /// <summary>
        /// Opens a web page in a new tab or window.
        /// Empty!
        /// </summary>
        /// <param name="URL">The address of the web page.</param>
        static public void openURL(string URL)
        {
            //navigateToURL(new URLRequest(URL), "_blank");

            Process.Start(URL);

        }

        /// <summary>
        /// Absolute value.
        /// </summary>
        /// <param name="N">Value to find absolute value of.</param>
        /// <returns>The absolute value of N</returns>
        static public float abs(float N)
        {
            return (N > 0) ? N : -N;
        }

        /// <summary>
        /// Map a number to the nearest lowest real number.
        /// </summary>
        /// <param name="N">N</param>
        /// <returns>The floor of N</returns>
        static public float floor(float N)
        {
            float n = (int)N;
            return (N > 0) ? (n) : ((n != N) ? (n - 1) : (n));
        }

        /// <summary>
        /// Map a number to the nearest highest real number.
        /// </summary>
        /// <param name="N">N</param>
        /// <returns>The ceiling of N</returns>
        static public float ceil(float N)
        {
            float n = (int)N;
            return (N > 0) ? ((n != N) ? (n + 1) : (n)) : (n);
        }
        /// <summary>
        /// Min of two numbers
        /// </summary>
        /// <param name="N1">N1</param>
        /// <param name="N2">N2</param>
        /// <returns>The Min</returns>
        static public float min(float N1, float N2)
        {
            return (N1 <= N2) ? N1 : N2;
        }

        /// <summary>
        /// The Max of two numbers
        /// </summary>
        /// <param name="N1"></param>
        /// <param name="N2"></param>
        /// <returns>Maximum value</returns>
        static public float max(float N1, float N2)
        {
            return (N1 >= N2) ? N1 : N2;
        }

        /// <summary>
        /// Generates a random number.  NOTE: To create a series of predictable
        /// random numbers, add the random number you generate each time
        /// to the <code>Seed</code> value before calling <code>random()</code> again.
        /// </summary>
        /// <returns>A <code>Number</code> between 0 and 1.</returns>
        static public float random()
        {
            if (_rand == null)
            {
                int seed;
                lock (_randglobal) seed = _randglobal.Next();
                _rand = new Random(seed);
            }
            return (float)_rand.NextDouble();
        }

        static public float random(double min, double max)
        {
            if (_rand == null)
            {
                int seed;
                lock (_randglobal) seed = _randglobal.Next();
                _rand = new Random(seed);
            }

            return (float)(_rand.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Generates a random number.  NOTE: To create a series of predictable
        /// random numbers, add the random number you generate each time
        /// to the <code>Seed</code> value before calling <code>random()</code> again.
        /// </summary>
        /// <param name="Seed">A user-provided value used to calculate a predictable random number.</param>
        /// <returns>A <code>Number</code> between 0 and 1.</returns>
        static public float random(float Seed)
        {
            //Make sure the seed value is OK
            if (Seed == 0)
                Seed = float.MinValue;
            if (Seed >= 1)
            {
                if ((Seed % 1) == 0)
                    Seed /= (float)Math.PI;
                Seed %= 1;
            }
            else if (Seed < 0)
                Seed = (Seed % 1) + 1;

            //Then do an LCG thing and return a predictable random number
            return ((69621 * (int)(Seed * 0x7FFFFFFF)) % 0x7FFFFFFF) / 0x7FFFFFFF;
        }

        /// <summary>
        /// Useful for finding out how long it takes to execute specific blocks of code.
        /// </summary>
        /// <returns>A <code>uint</code> to be passed to <code>FlxU.endProfile()</code>.</returns>
        static public uint startProfile()
        {
            return FlxG.getTimer;
        }


        /// <summary>
        /// Useful for finding out how long it takes to execute specific blocks of code.
        /// 
        /// </summary>
        /// <param name="Start">A <code>uint</code> created by <code>FlxU.startProfile()</code>.</param>
        /// <param name="Name">Optional tag (for debug console display).  Default value is "Profiler".</param>
        /// <param name="Log">Whether or not to log this elapsed time in the debug console.</param>
        /// <returns>A <code>uint</code> to be passed to <code>FlxU.endProfile()</code>.</returns>
        static public uint endProfile(uint Start, string Name, bool Log)
        {
            uint t = FlxG.getTimer;
			if (Log) {
				FlxG.log (Name + ": " + ((t - Start) / 1000) + "s");
				Console.WriteLine (Name + ": " + ((t - Start) / 1000) + "s");
			}
            return t;
        }

        /// <summary>
        /// Rotates a point in 2D space around another point by the given angle.
        /// </summary>
        /// <param name="X">The X coordinate of the point you want to rotate.</param>
        /// <param name="Y">The Y coordinate of the point you want to rotate.</param>
        /// <param name="PivotX">The X coordinate of the point you want to rotate around.</param>
        /// <param name="PivotY">The Y coordinate of the point you want to rotate around.</param>
        /// <param name="Angle">Rotate the point by this many degrees.</param>
        /// <returns>A <code>FlxPoint</code> containing the coordinates of the rotated point.</returns>
        public static Vector2 rotatePoint(float X, float Y, float PivotX, float PivotY, float Angle)
        {
            float radians = -Angle / 180f * (float)Math.PI;
            float dx = X - PivotX;
            float dy = PivotY - Y;
            return new Vector2((float)(PivotX + Math.Cos(radians) * dx - Math.Sin(radians) * dy), (float)(PivotY - (Math.Sin(radians) * dx + Math.Cos(radians) * dy)));
        }

        /// <summary>
        /// Calculates the angle between a point and the origin (0,0).
        /// </summary>
        /// <param name="X">The X coordinate of the point.</param>
        /// <param name="Y">The Y coordinate of the point.</param>
        /// <returns>The angle in degrees.</returns>
        public static float getAngle(float X, float Y)
        {
            return (float)(Math.Atan2(Y, X) * 180f / Math.PI);
        }

        /// <summary>
        /// Calculates the angle between two points.  0 degrees points straight up.
        /// </summary>
        /// <param name="Point1">The X coordinate of the point.</param>
        /// <param name="Point2">The Y coordinate of the point.</param>
        /// <returns>The angle in degrees, between -180 and 180.</returns>
        public static float getAngle(Vector2 Point1, Vector2 Point2)
        {

            float x = Point2.X - Point1.X;
            float y = Point2.Y - Point1.Y;
            if((x == 0) && (y == 0))
                return 0;
            float c1 = 3.14159265f * 0.25f;
            float c2 = 3 * c1;
            float ay = (y < 0)?-y:y;
            float angle = 0;
            if (x >= 0)
                angle = c1 - c1 * ((x - ay) / (x + ay));
            else
                angle = c2 - c1 * ((x + ay) / (ay - x));
            angle = ((y < 0)?-angle:angle)*57.2957796f;
            if(angle > 90)
                angle = angle - 270;
            else
                angle += 90;
            return angle;


        }


        /// <summary>
        /// Generate a Flash <code>uint</code> color from RGBA components.
        /// </summary>
        /// <param name="Red">The red component, between 0 and 255.</param>
        /// <param name="Green">The green component, between 0 and 255.</param>
        /// <param name="Blue">The blue component, between 0 and 255.</param>
        /// <returns>The color as a <code>uint</code>.</returns>
        static public uint getColor(uint Red, uint Green, uint Blue)
        {
            return getColor(Red, Green, Blue, 1.0f);
        }
        /// <summary>
        /// Generate a Flash <code>uint</code> color from RGBA components.
        /// </summary>
        /// <param name="Red">The red component, between 0 and 255.</param>
        /// <param name="Green">The green component, between 0 and 255.</param>
        /// <param name="Blue">The blue component, between 0 and 255.</param>
        /// <param name="Alpha"></param>
        /// <returns>The color as a <code>uint</code>.</returns>
        static public uint getColor(uint Red, uint Green, uint Blue, float Alpha)
        {
            return ((uint)((Alpha > 1) ? Alpha : (Alpha * 255)) & 0xFF) << 24 | (Red & 0xFF) << 16 | (Green & 0xFF) << 8 | (Blue & 0xFF);
        }

        /// <summary>
        /// Generate a Flash <code>uint</code> color from HSB components.
        /// </summary>
        /// <param name="Hue">A number between 0 and 360, indicating position on a color strip or wheel.</param>
        /// <param name="Saturation">A number between 0 and 1, indicating how colorful or gray the color should be.  0 is gray, 1 is vibrant.</param>
        /// <param name="Brightness">A number between 0 and 1, indicating how bright the color should be.  0 is black, 1 is full bright.</param>
        /// <returns>The color as a <code>uint</code>.</returns>
        static public uint getColorHSB(float Hue, float Saturation, float Brightness)
        {
            return getColorHSB(Hue, Saturation, Brightness, 1.0f);
        }
        /// <summary>
        /// Generate a Flash <code>uint</code> color from HSB components.
        /// </summary>
        /// <param name="Hue">A number between 0 and 360, indicating position on a color strip or wheel.</param>
        /// <param name="Saturation">A number between 0 and 1, indicating how colorful or gray the color should be.  0 is gray, 1 is vibrant.</param>
        /// <param name="Brightness">A number between 0 and 1, indicating how bright the color should be.  0 is black, 1 is full bright.</param>
        /// <param name="Alpha">How opaque the color should be, either between 0 and 1 or 0 and 255.</param>
        /// <returns>The color as a <code>uint</code>.</returns>
        static public uint getColorHSB(float Hue, float Saturation, float Brightness, float Alpha)
        {
            float red;
            float green;
            float blue;
            if (Saturation == 0.0)
            {
                red = Brightness;
                green = Brightness;
                blue = Brightness;
            }
            else
            {
                if (Hue == 360)
                    Hue = 0;
                int slice = (int)Hue / 60;
                float hf = Hue / 60 - slice;
                float aa = Brightness * (1 - Saturation);
                float bb = Brightness * (1 - Saturation * hf);
                float cc = Brightness * (1 - Saturation * (1.0f - hf));
                switch (slice)
                {
                    case 0: red = Brightness; green = cc; blue = aa; break;
                    case 1: red = bb; green = Brightness; blue = aa; break;
                    case 2: red = aa; green = Brightness; blue = cc; break;
                    case 3: red = aa; green = bb; blue = Brightness; break;
                    case 4: red = cc; green = aa; blue = Brightness; break;
                    case 5: red = Brightness; green = aa; blue = bb; break;
                    default: red = 0; green = 0; blue = 0; break;
                }
            }

            return ((uint)((Alpha > 1) ? Alpha : (Alpha * 255)) & 0xFF) << 24 | (uint)(red * 255) << 16 | (uint)(green * 255) << 8 | (uint)(blue * 255);
        }

        /// <summary>
        /// Loads an array with the RGBA values of a Flash <code>uint</code> color.
        /// RGB values are stored 0-255.  Alpha is stored as a floating point number between 0 and 1. 
        /// </summary>
        /// <param name="Color">The color you want to break into components.</param>
        /// <param name="Results">An optional parameter, allows you to use an array that already exists in memory to store the result.</param>
        /// <returns>An <code>Array</code> object containing the Red, Green, Blue and Alpha values of the given color.</returns>
        static public List<object> getRGBA(uint Color, List<object> Results)
        {
            if (Results == null)
                Results = new List<object>(4);
            Results[0] = (Color >> 16) & 0xFF;
            Results[1] = (Color >> 8) & 0xFF;
            Results[2] = Color & 0xFF;
            Results[3] = (float)((Color >> 24) & 0xFF) / 255;
            return Results;
        }

        /// <summary>
        /// Loads an array with the HSB values of a Flash <code>uint</code> color.
        /// Hue is a value between 0 and 360.  Saturation, Brightness and Alpha
        /// are as floating point numbers between 0 and 1.
        /// </summary>
        /// <param name="Color">The color you want to break into components.</param>
        /// <param name="Results">An optional parameter, allows you to use an array that already exists in memory to store the result.</param>
        /// <returns>An <code>Array</code> object containing the Red, Green, Blue and Alpha values of the given color.</returns>
        static public List<object> getHSB(uint Color, List<object> Results)
        {
            if (Results == null)
                Results = new List<object>(4);

            float red = (float)((Color >> 16) & 0xFF) / 255;
            float green = (float)((Color >> 8) & 0xFF) / 255;
            float blue = (float)((Color) & 0xFF) / 255;

            float m = (red > green) ? red : green;
            float dmax = (m > blue) ? m : blue;
            m = (red > green) ? green : red;
            float dmin = (m > blue) ? blue : m;
            float range = dmax - dmin;

            Results[2] = dmax;
            Results[1] = 0;
            Results[0] = 0;

            if (dmax != 0)
                Results[1] = range / dmax;
            if ((float)Results[1] != 0f)
            {
                if (red == dmax)
                    Results[0] = (green - blue) / range;
                else if (green == dmax)
                    Results[0] = 2 + (blue - red) / range;
                else if (blue == dmax)
                    Results[0] = 4 + (red - green) / range;
                Results[0] = (int)Results[0] * 60;
                if ((int)Results[0] < 0)
                    Results[0] = (int)Results[0] + 360;
            }

            Results[3] = (float)((Color >> 24) & 0xFF) / 255;
            return Results;
        }

        /// <summary>
        /// Get the <code>String</code> name of any <code>Object</code>.
        /// </summary>
        /// <param name="Obj">The Object object in question.</param>
        /// <param name="Simple">Returns only the class name, not the package or packages.</param>
        /// <returns>The name of the Class as a String object.</returns>
        static public string getClassName(object Obj, bool Simple)
        {
            if (Simple)
            {
                return Obj.GetType().Name;
            }
            else
            {
                return Obj.GetType().FullName;
            }
        }

        /// <summary>
        /// Look up a <code>Class</code> object by its string name.
        /// Not working in C#
        /// </summary>
        /// <param name="Name">Name	The <code>String</code> name of the <code>Class</code> you are interested in.</param>
        /// <returns>A <code>Class</code> object.</returns>
        static public object getClass(string Name)
        {
            return null;
            //return getDefinitionByName(Name) as Class;
        }

        /// <summary>
        /// A tween-like function that takes a starting velocity
        /// and some other factors and returns an altered velocity.
        /// </summary>
        /// <param name="Velocity">Any component of velocity (e.g. 20).</param>
        /// <param name="Acceleration">Rate at which the velocity is changing.</param>
        /// <param name="Drag">Really kind of a deceleration, this is how much the velocity changes if Acceleration is not set.</param>
        /// <param name="Max">An absolute value cap for the velocity.</param>
        /// <returns>The altered Velocity value.</returns>
        public static float computeVelocity(float Velocity, float Acceleration, float Drag, float Max)
        {
            if (Acceleration != 0)
            {
                Velocity += Acceleration * FlxG.elapsed;
            }
            else if (Drag != 0)
            {
                float d = Drag * FlxG.elapsed;
                if (Velocity - d > 0)
                {
                    Velocity -= d;
                }
                else if (Velocity + d < 0)
                {
                    Velocity += d;
                }
                else
                {
                    Velocity = 0;
                }
            }

            if (Velocity != 0 && Max != 10000)
            {
                if (Velocity > Max)
                {
                    Velocity = Max;
                }
                else if (Velocity < -Max)
                {
                    Velocity = -Max;
                }
            }
            return Velocity;
        }

        /// <summary>
        /// Call this function to specify a more efficient boundary for your game world.
        /// This boundary is used by overlap() and collide(), so it
        /// can't hurt to have it be the right size!  Flixel will invent a size for you, but
        /// it's pretty huge - 256x the size of the screen, whatever that may be.
        /// Leave width and height empty if you want to just update the game world's position.
        /// </summary>
        /// <param name="X">The X-coordinate of the left side of the game world.</param>
        /// <param name="Y">The Y-coordinate of the top of the game world.</param>
        /// <param name="Width">Desired width of the game world.</param>
        /// <param name="Height">Desired height of the game world.</param>
        static public void setWorldBounds(float X, float Y, float Width, float Height)
        {
            setWorldBounds(X, Y, Width, Height, 3);
        }


        /// <summary>
        /// Call this function to specify a more efficient boundary for your game world.
        /// This boundary is used by <code>overlap()</code> and <code>collide()</code>, so it
        /// can't hurt to have it be the right size!  Flixel will invent a size for you, but
        /// it's pretty huge - 256x the size of the screen, whatever that may be.
        /// Leave width and height empty if you want to just update the game world's position.
        /// </summary>
        /// <param name="X">The X-coordinate of the left side of the game world.</param>
        /// <param name="Y">The Y-coordinate of the top of the game world.</param>
        /// <param name="Width">Desired width of the game world.</param>
        /// <param name="Height">Desired height of the game world.</param>
        /// <param name="Divisions">Pass a non-zero value to set <code>quadTreeDivisions</code>.  Default value is 3.</param>
        static public void setWorldBounds(float X, float Y, float Width, float Height, uint Divisions)
        {
            FlxQuadTree.bounds.x = X;
            FlxQuadTree.bounds.y = Y;
            if (Width > 0)
                FlxQuadTree.bounds.width = Width;
            if (Height > 0)
                FlxQuadTree.bounds.height = Height;
            if (Divisions > 0)
                FlxQuadTree.divisions = Divisions;
        }

        /// <summary>
        /// Call this function to see if one <code>FlxObject</code> overlaps another.
        /// Can be called with one object and one group, or two groups, or two objects,
        /// whatever floats your boat!  It will put everything into a quad tree and then
        /// check for overlaps.  For maximum performance try bundling a lot of objects
        /// together using a <code>FlxGroup</code> (even bundling groups together!)
        /// NOTE: does NOT take objects' scrollfactor into account.
        /// <para>Sending null to callback will kill both objects.</para>
        /// </summary>
        /// <param name="Object1">The first object or group you want to check.</param>
        /// <param name="Object2">The second object or group you want to check.  If it is the same as the first, flixel knows to just do a comparison within that group.</param>
        /// <param name="Callback">A function with two <code>FlxObject</code> parameters - e.g. <code>myOverlapFunction(Object1:FlxObject,Object2:FlxObject);</code>  If no function is provided, <code>FlxQuadTree</code> will call <code>kill()</code> on both objects.</param>
        /// <returns>Whether an overlap has been found.</returns>
        static public bool overlap(FlxObject Object1, FlxObject Object2, SpriteCollisionEvent Callback)
        {
            if ((Object1 == null) || !Object1.exists ||
                (Object2 == null) || !Object2.exists || 
                Object1.dead == true || Object2.dead == true)
                return false;
            quadTree = new FlxQuadTree(FlxQuadTree.bounds.x, FlxQuadTree.bounds.y, FlxQuadTree.bounds.width, FlxQuadTree.bounds.height, null);
            quadTree.add(Object1, FlxQuadTree.A_LIST);
            if (Object1 == Object2)
                return quadTree.overlap(false, Callback);
            quadTree.add(Object2, FlxQuadTree.B_LIST);
            //bool overlaps = quadTree.overlap(true, Callback);
            //if (overlaps)
            //{
            //    Console.WriteLine("overlapps in flxU");
            //    Object1.overlapped(Object2);
            //    Object2.overlapped(Object1);

            //}
            //return overlaps;
            return quadTree.overlap(true, Callback);

        }

        /// <summary>
        /// Call this function to see if one <code>FlxObject</code> collides with another.
        /// Can be called with one object and one group, or two groups, or two objects,
        /// whatever floats your boat!  It will put everything into a quad tree and then
        /// check for collisions.  For maximum performance try bundling a lot of objects
        /// together using a <code>FlxGroup</code> (even bundling groups together!)
        /// NOTE: does NOT take objects' scrollfactor into account.
        /// </summary>
        /// <param name="Object1">The first object or group you want to check.</param>
        /// <param name="Object2">The second object or group you want to check.  If it is the same as the first, flixel knows to just do a comparison within that group.</param>
        /// <returns>Whether you found a collide or not.</returns>
        static public bool collide(FlxObject Object1, FlxObject Object2)
        {
            if ((Object1 == null) || !Object1.exists ||
                (Object2 == null) || !Object2.exists)
                return false;
            quadTree = new FlxQuadTree(FlxQuadTree.bounds.x, FlxQuadTree.bounds.y, FlxQuadTree.bounds.width, FlxQuadTree.bounds.height, null);
            quadTree.add(Object1, FlxQuadTree.A_LIST);
            bool match = Object1 == Object2;
            if (!match)
                quadTree.add(Object2, FlxQuadTree.B_LIST);
            bool cx = quadTree.overlap(!match, solveXCollision);
            bool cy = quadTree.overlap(!match, solveYCollision);
            return cx || cy;
        }

        static public bool collideOnY(FlxObject Object1, FlxObject Object2)
        {
            if ((Object1 == null) || !Object1.exists ||
                (Object2 == null) || !Object2.exists)
                return false;
            quadTree = new FlxQuadTree(FlxQuadTree.bounds.x, FlxQuadTree.bounds.y, FlxQuadTree.bounds.width, FlxQuadTree.bounds.height, null);
            quadTree.add(Object1, FlxQuadTree.A_LIST);
            bool match = Object1 == Object2;
            if (!match)
                quadTree.add(Object2, FlxQuadTree.B_LIST);
            //bool cx = quadTree.overlap(!match, solveXCollision);
            bool cy = quadTree.overlap(!match, solveYCollision);
            return cy;
        }

        static public bool collideRamp(FlxObject Object1, FlxObject Object2)
        {
            if ((Object1 == null) || !Object1.exists ||
                (Object2 == null) || !Object2.exists)
                return false;
            quadTree = new FlxQuadTree(FlxQuadTree.bounds.x, FlxQuadTree.bounds.y, FlxQuadTree.bounds.width, FlxQuadTree.bounds.height, null);
            quadTree.add(Object1, FlxQuadTree.A_LIST);
            bool match = Object1 == Object2;
            if (!match)
                quadTree.add(Object2, FlxQuadTree.B_LIST);
            bool cx = quadTree.overlap(!match, solveXCollisionRamp);
            bool cy = quadTree.overlap(!match, solveYCollisionRamp);
            return cx || cy;
        }




        /// <summary>
        /// This quad tree callback function can be used externally as well.
        /// Takes two objects and separates them along their X axis (if possible/reasonable).
        /// </summary>
        /// <param name="sender">The first object or group you want to check.</param>
        /// <param name="e">The second object or group you want to check.</param>
        /// <returns></returns>
        static public bool solveXCollision(object sender, FlxSpriteCollisionEvent e)
        {
            //Avoid messed up collisions ahead of time
            float o1 = e.Object1.colVector.X;
            float o2 = e.Object2.colVector.X;
            if (o1 == o2)
                return false;

            //Give the objects a heads up that we're about to resolve some collisions
            e.Object1.preCollide(e.Object2);
            e.Object2.preCollide(e.Object1);

            //Basic resolution variables
            bool f1;
            bool f2;
            float overlap;
            bool hit = false;
            bool p1hn2;

            //Directional variables
            bool obj1Stopped = o1 == 0;
            bool obj1MoveNeg = o1 < 0;
            bool obj1MovePos = o1 > 0;
            bool obj2Stopped = o2 == 0;
            bool obj2MoveNeg = o2 < 0;
            bool obj2MovePos = o2 > 0;

            //Offset loop variables
            int i1;
            int i2;
            FlxRect obj1Hull = e.Object1.colHullX;
            FlxRect obj2Hull = e.Object2.colHullX;
            List<Vector2> co1 = e.Object1.colOffsets;
            List<Vector2> co2 = e.Object2.colOffsets;
            int l1 = co1.Count;
            int l2 = co2.Count;
            float ox1;
            float oy1;
            float ox2;
            float oy2;
            float r1;
            float r2;
            float sv1;
            float sv2;

            //Decide based on object's movement patterns if it was a right-side or left-side collision
            p1hn2 = ((obj1Stopped && obj2MoveNeg) || (obj1MovePos && obj2Stopped) || (obj1MovePos && obj2MoveNeg) || //the obvious cases
                    (obj1MoveNeg && obj2MoveNeg && (((o1 > 0) ? o1 : -o1) < ((o2 > 0) ? o2 : -o2))) || //both moving left, obj2 overtakes obj1
                    (obj1MovePos && obj2MovePos && (((o1 > 0) ? o1 : -o1) > ((o2 > 0) ? o2 : -o2)))); //both moving right, obj1 overtakes obj2

            //Check to see if these objects allow these collisions
            if (p1hn2 ? (!e.Object1.collideRight || !e.Object2.collideLeft) : (!e.Object1.collideLeft || !e.Object2.collideRight))
                return false;

            //this looks insane, but we're just looping through collision offsets on each object
            i1 = 0;
            while (i1 < l1)
            {
                ox1 = co1[i1].X;
                oy1 = co1[i1].Y;
                obj1Hull.x += ox1;
                obj1Hull.y += oy1;
                i2 = 0;
                while (i2 < l2)
                {
                    ox2 = co2[i2].X;
                    oy2 = co2[i2].Y;
                    obj2Hull.x += ox2;
                    obj2Hull.y += oy2;

                    //See if it's a actually a valid collision
                    if ((obj1Hull.x + obj1Hull.width < obj2Hull.x + roundingError) ||
                        (obj1Hull.x + roundingError > obj2Hull.x + obj2Hull.width) ||
                        (obj1Hull.y + obj1Hull.height < obj2Hull.y + roundingError) ||
                        (obj1Hull.y + roundingError > obj2Hull.y + obj2Hull.height))
                    {
                        obj2Hull.x = obj2Hull.x - ox2;
                        obj2Hull.y = obj2Hull.y - oy2;
                        i2++;
                        continue;
                    }

                    //Calculate the overlap between the objects
                    if (p1hn2)
                    {
                        if (obj1MoveNeg)
                            r1 = obj1Hull.x + e.Object1.colHullY.width;
                        else
                            r1 = obj1Hull.x + obj1Hull.width;
                        if (obj2MoveNeg)
                            r2 = obj2Hull.x;
                        else
                            r2 = obj2Hull.x + obj2Hull.width - e.Object2.colHullY.width;
                    }
                    else
                    {
                        if (obj2MoveNeg)
                            r1 = -obj2Hull.x - e.Object2.colHullY.width;
                        else
                            r1 = -obj2Hull.x - obj2Hull.width;
                        if (obj1MoveNeg)
                            r2 = -obj1Hull.x;
                        else
                            r2 = -obj1Hull.x - obj1Hull.width + e.Object1.colHullY.width;
                    }
                    overlap = r1 - r2;

                    //Slightly smarter version of checking if objects are 'fixed' in space or not
                    f1 = e.Object1.@fixed;
                    f2 = e.Object2.@fixed;
                    if (f1 && f2)
                    {
                        f1 &= (e.Object1.colVector.X == 0) && (o1 == 0);
                        f2 &= (e.Object2.colVector.X == 0) && (o2 == 0);
                    }

                    //Last chance to skip out on a bogus collision resolution
                    if ((overlap == 0) ||
                        ((!f1 && ((overlap > 0) ? overlap : -overlap) > obj1Hull.width * 0.8)) ||
                        ((!f2 && ((overlap > 0) ? overlap : -overlap) > obj2Hull.width * 0.8)))
                    {
                        obj2Hull.x = obj2Hull.x - ox2;
                        obj2Hull.y = obj2Hull.y - oy2;
                        i2++;
                        continue;
                    }
                    hit = true;

                    //Adjust the objects according to their flags and stuff
                    sv1 = e.Object2.velocity.X;
                    sv2 = e.Object1.velocity.X;
                    if (!f1 && f2)
                    {
                        if (e.Object1._group)
                            e.Object1.reset((e.Object1.x - overlap), e.Object1.y);
                        else
                            e.Object1.x = e.Object1.x - overlap;
                    }
                    else if (f1 && !f2)
                    {
                        if (e.Object2._group)
                            e.Object2.reset((e.Object2.x + overlap), e.Object2.y);
                        else
                            e.Object2.x += overlap;
                    }
                    else if (!f1 && !f2)
                    {
                        overlap /= 2;
                        if (e.Object1._group)
                            e.Object1.reset((e.Object1.x - overlap), e.Object1.y);
                        else
                            e.Object1.x = e.Object1.x - overlap;
                        if (e.Object2._group)
                            e.Object2.reset((e.Object2.x + overlap), e.Object2.y);
                        else
                            e.Object2.x += overlap;
                        sv1 *= 0.5f;
                        sv2 *= 0.5f;
                    }
                    if (p1hn2)
                    {
                        e.Object1.hitRight(e.Object2, sv1);
                        e.Object2.hitLeft(e.Object1, sv2);
                    }
                    else
                    {
                        e.Object1.hitLeft(e.Object2, sv1);
                        e.Object2.hitRight(e.Object1, sv2);
                    }

                    //Adjust collision hulls if necessary
                    if (!f1 && (overlap != 0))
                    {
                        if (p1hn2)
                            obj1Hull.width = obj1Hull.width - overlap;
                        else
                        {
                            obj1Hull.x = obj1Hull.x - overlap;
                            obj1Hull.width += overlap;
                        }
                        e.Object1.colHullY.x = e.Object1.colHullY.x - overlap;
                    }
                    if (!f2 && (overlap != 0))
                    {
                        if (p1hn2)
                        {
                            obj2Hull.x += overlap;
                            obj2Hull.width = obj2Hull.width - overlap;
                        }
                        else
                            obj2Hull.width += overlap;
                        e.Object2.colHullY.x += overlap;
                    }
                    obj2Hull.x = obj2Hull.x - ox2;
                    obj2Hull.y = obj2Hull.y - oy2;
                    i2++;
                }
                obj1Hull.x = obj1Hull.x - ox1;
                obj1Hull.y = obj1Hull.y - oy1;
                i1++;
            }

            return hit;
        }

        /// <summary>
        /// This quad tree callback function can be used externally as well.
        /// Takes two objects and separates them along their Y axis (if possible/reasonable).
        /// </summary>
        /// <param name="sender">The first object or group you want to check.</param>
        /// <param name="e">The second object or group you want to check.</param>
        /// <returns></returns>
        static public bool solveYCollision(object sender, FlxSpriteCollisionEvent e)
        {
            //Avoid messed up collisions ahead of time
            float o1 = e.Object1.colVector.Y;
            float o2 = e.Object2.colVector.Y;
            if (o1 == o2)
                return false;

            //Give the objects a heads up that we're about to resolve some collisions
            e.Object1.preCollide(e.Object2);
            e.Object2.preCollide(e.Object1);

            //Basic resolution variables
            bool f1;
            bool f2;
            float overlap;
            bool hit = false;
            bool p1hn2;

            //Directional variables
            bool obj1Stopped = o1 == 0;
            bool obj1MoveNeg = o1 < 0;
            bool obj1MovePos = o1 > 0;
            bool obj2Stopped = o2 == 0;
            bool obj2MoveNeg = o2 < 0;
            bool obj2MovePos = o2 > 0;

            //Offset loop variables
            int i1;
            int i2;
            FlxRect obj1Hull = e.Object1.colHullY;
            FlxRect obj2Hull = e.Object2.colHullY;
            List<Vector2> co1 = e.Object1.colOffsets;
            List<Vector2> co2 = e.Object2.colOffsets;
            int l1 = co1.Count;
            int l2 = co2.Count;
            float ox1;
            float oy1;
            float ox2;
            float oy2;
            float r1;
            float r2;
            float sv1;
            float sv2;

            //Decide based on object's movement patterns if it was a top or bottom collision
            p1hn2 = ((obj1Stopped && obj2MoveNeg) || (obj1MovePos && obj2Stopped) || (obj1MovePos && obj2MoveNeg) || //the obvious cases
                (obj1MoveNeg && obj2MoveNeg && (((o1 > 0) ? o1 : -o1) < ((o2 > 0) ? o2 : -o2))) || //both moving up, obj2 overtakes obj1
                (obj1MovePos && obj2MovePos && (((o1 > 0) ? o1 : -o1) > ((o2 > 0) ? o2 : -o2)))); //both moving down, obj1 overtakes obj2

            //Check to see if these objects allow these collisions
            if (p1hn2 ? (!e.Object1.collideBottom || !e.Object2.collideTop) : (!e.Object1.collideTop || !e.Object2.collideBottom))
                return false;

            //this looks insane, but we're just looping through collision offsets on each object
            i1 = 0;
            while (i1 < l1)
            {
                ox1 = co1[i1].X;
                oy1 = co1[i1].Y;
                obj1Hull.x += ox1;
                obj1Hull.y += oy1;
                i2 = 0;
                while (i2 < l2)
                {
                    ox2 = co2[i2].X;
                    oy2 = co2[i2].Y;
                    obj2Hull.x += ox2;
                    obj2Hull.y += oy2;

                    //See if it's a actually a valid collision
                    if ((obj1Hull.x + obj1Hull.width < obj2Hull.x + roundingError) ||
                        (obj1Hull.x + roundingError > obj2Hull.x + obj2Hull.width) ||
                        (obj1Hull.y + obj1Hull.height < obj2Hull.y + roundingError) ||
                        (obj1Hull.y + roundingError > obj2Hull.y + obj2Hull.height))
                    {
                        obj2Hull.x = obj2Hull.x - ox2;
                        obj2Hull.y = obj2Hull.y - oy2;
                        i2++;
                        continue;
                    }

                    //Calculate the overlap between the objects
                    if (p1hn2)
                    {
                        if (obj1MoveNeg)
                            r1 = obj1Hull.y + e.Object1.colHullX.height;
                        else
                            r1 = obj1Hull.y + obj1Hull.height;
                        if (obj2MoveNeg)
                            r2 = obj2Hull.y;
                        else
                            r2 = obj2Hull.y + obj2Hull.height - e.Object2.colHullX.height;
                    }
                    else
                    {
                        if (obj2MoveNeg)
                            r1 = -obj2Hull.y - e.Object2.colHullX.height;
                        else
                            r1 = -obj2Hull.y - obj2Hull.height;
                        if (obj1MoveNeg)
                            r2 = -obj1Hull.y;
                        else
                            r2 = -obj1Hull.y - obj1Hull.height + e.Object1.colHullX.height;
                    }
                    overlap = r1 - r2;
                    //if (overlap > -0.00008f && overlap < 0.00008f)
                    //{
                    //    i2++;
                    //    continue;
                    //}

                    //Slightly smarter version of checking if objects are 'fixed' in space or not
                    f1 = e.Object1.@fixed;
                    f2 = e.Object2.@fixed;
                    if (f1 && f2)
                    {
                        f1 &= (e.Object1.colVector.X == 0) && (o1 == 0);
                        f2 &= (e.Object2.colVector.X == 0) && (o2 == 0);
                    }

                    //Last chance to skip out on a bogus collision resolution
                    if ((overlap == 0) ||
                        ((!f1 && ((overlap > 0) ? overlap : -overlap) > obj1Hull.height * 0.8)) ||
                        ((!f2 && ((overlap > 0) ? overlap : -overlap) > obj2Hull.height * 0.8)))
                    {
                        obj2Hull.x = obj2Hull.x - ox2;
                        obj2Hull.y = obj2Hull.y - oy2;
                        i2++;
                        continue;
                    }
                    hit = true;

                    //Adjust the objects according to their flags and stuff
                    sv1 = e.Object2.velocity.Y;
                    sv2 = e.Object1.velocity.Y;
                    if (!f1 && f2)
                    {
                        if (e.Object1._group)
                            e.Object1.reset(e.Object1.x, (e.Object1.y - overlap));
                        else
                            e.Object1.y = e.Object1.y - overlap;
                    }
                    else if (f1 && !f2)
                    {
                        if (e.Object2._group)
                            e.Object2.reset(e.Object2.x, (e.Object2.y + overlap));
                        else
                            e.Object2.y += overlap;
                    }
                    else if (!f1 && !f2)
                    {
                        overlap /= 2;
                        if (e.Object1._group)
                            e.Object1.reset(e.Object1.x, (e.Object1.y - overlap));
                        else
                            e.Object1.y = e.Object1.y - overlap;
                        if (e.Object2._group)
                            e.Object2.reset(e.Object2.x, (e.Object2.y + overlap));
                        else
                            e.Object2.y += overlap;
                        sv1 *= 0.5f;
                        sv2 *= 0.5f;
                    }
                    if (p1hn2)
                    {
                        e.Object1.hitBottom(e.Object2, sv1);
                        e.Object2.hitTop(e.Object1, sv2);
                    }
                    else
                    {
                        e.Object1.hitTop(e.Object2, sv1);
                        e.Object2.hitBottom(e.Object1, sv2);
                    }

                    //Adjust collision hulls if necessary
                    if (!f1 && (overlap != 0))
                    {
                        if (p1hn2)
                        {
                            obj1Hull.y = obj1Hull.y - overlap;

                            //This code helps stuff ride horizontally moving platforms.
                            if (f2 && e.Object2.moves)
                            {
                                sv1 = e.Object2.colVector.X;
                                e.Object1.x += sv1;
                                obj1Hull.x += sv1;
                                e.Object1.colHullX.x += sv1;
                            }
                        }
                        else
                        {
                            obj1Hull.y = obj1Hull.y - overlap;
                            obj1Hull.height += overlap;
                        }
                    }
                    if (!f2 && (overlap != 0))
                    {
                        if (p1hn2)
                        {
                            obj2Hull.y += overlap;
                            obj2Hull.height = obj2Hull.height - overlap;
                        }
                        else
                        {
                            obj2Hull.height += overlap;

                            //This code helps stuff ride horizontally moving platforms.
                            if (f1 && e.Object1.moves)
                            {
                                sv2 = e.Object1.colVector.X;
                                e.Object2.x += sv2;
                                obj2Hull.x += sv2;
                                e.Object2.colHullX.x += sv2;
                            }
                        }
                    }
                    obj2Hull.x = obj2Hull.x - ox2;
                    obj2Hull.y = obj2Hull.y - oy2;
                    i2++;
                }
                obj1Hull.x = obj1Hull.x - ox1;
                obj1Hull.y = obj1Hull.y - oy1;
                i1++;
            }

            return hit;
        }




        /// <summary>
        /// This quad tree callback function can be used externally as well.
        /// Takes two objects and separates them along their X axis (if possible/reasonable).
        /// </summary>
        /// <param name="sender">The first object or group you want to check.</param>
        /// <param name="e">The second object or group you want to check.</param>
        /// <returns></returns>
        static public bool solveXCollisionRamp(object sender, FlxSpriteCollisionEvent e)
        {
            //e.Object1.colHullX.x += 0.25f;
            return false;
        }

        /// <summary>
        /// This quad tree callback function can be used externally as well.
        /// Takes two objects and separates them along their Y axis (if possible/reasonable).
        /// </summary>
        /// <param name="sender">The first object or group you want to check.</param>
        /// <param name="e">The second object or group you want to check.</param>
        /// <returns></returns>
        static public bool solveYCollisionRamp(object sender, FlxSpriteCollisionEvent e)
        {
            //Avoid messed up collisions ahead of time
            float o1 = e.Object1.colVector.Y;
            float o2 = e.Object2.colVector.Y;
            if (o1 == o2)
                return false;

            //Give the objects a heads up that we're about to resolve some collisions
            e.Object1.preCollide(e.Object2);
            e.Object2.preCollide(e.Object1);

            //Basic resolution variables
            bool f1;
            bool f2;
            float overlap;
            bool hit = false;
            bool p1hn2;

            //Directional variables
            bool obj1Stopped = o1 == 0;
            bool obj1MoveNeg = o1 < 0;
            bool obj1MovePos = o1 > 0;
            bool obj2Stopped = o2 == 0;
            bool obj2MoveNeg = o2 < 0;
            bool obj2MovePos = o2 > 0;

            //Offset loop variables
            int i1;
            int i2;
            FlxRect obj1Hull = e.Object1.colHullY;
            FlxRect obj2Hull = e.Object2.colHullY;
            List<Vector2> co1 = e.Object1.colOffsets;
            List<Vector2> co2 = e.Object2.colOffsets;
            int l1 = co1.Count;
            int l2 = co2.Count;
            float ox1;
            float oy1;
            float ox2;
            float oy2;
            float r1;
            float r2;
            float sv1;
            float sv2;

            //Decide based on object's movement patterns if it was a top or bottom collision
            p1hn2 = ((obj1Stopped && obj2MoveNeg) || (obj1MovePos && obj2Stopped) || (obj1MovePos && obj2MoveNeg) || //the obvious cases
                (obj1MoveNeg && obj2MoveNeg && (((o1 > 0) ? o1 : -o1) < ((o2 > 0) ? o2 : -o2))) || //both moving up, obj2 overtakes obj1
                (obj1MovePos && obj2MovePos && (((o1 > 0) ? o1 : -o1) > ((o2 > 0) ? o2 : -o2)))); //both moving down, obj1 overtakes obj2

            //Check to see if these objects allow these collisions
            if (p1hn2 ? (!e.Object1.collideBottom || !e.Object2.collideTop) : (!e.Object1.collideTop || !e.Object2.collideBottom))
                return false;

            //this looks insane, but we're just looping through collision offsets on each object
            i1 = 0;
            while (i1 < l1)
            {
                ox1 = co1[i1].X;
                oy1 = co1[i1].Y;
                obj1Hull.x += ox1;
                obj1Hull.y += oy1;


                if (((FlxRamp)(e.Object2)).direction == FlxRamp.LOW_SIDE_RIGHT)
                {
                    float rampOffsetGOOD = ((float)(obj1Hull.x % 20.0f));
                    float rampOffset = 20 - (obj1Hull.x - (obj2Hull.x + 20));
                    //Console.WriteLine("Ramp % 20 {0} {1}", obj1Hull.x, rampOffset);
                    Console.WriteLine("obj1 hull x {0} obj2 hull x {1} rampOffset {2} rampOffsetGOOD {3}", (obj1Hull.x ), (obj2Hull.x), rampOffset, rampOffsetGOOD);

                    
                    //if (rampOffset <= 0) rampOffset = 0;

                    obj2Hull.y += (rampOffset*1.0f);
                }
                else if (((FlxRamp)(e.Object2)).direction == FlxRamp.LOW_SIDE_LEFT)
                {
                    //float rampOffsetGOOD =  (20.00f - ((float)((obj1Hull.x + obj1Hull.width) % 20.0f))) ;

                    float rampOffset = 20 - ( (obj1Hull.x + obj1Hull.width) - obj2Hull.x);
                    if (rampOffset <= 0) rampOffset = 0;

                    //Console.WriteLine("obj1 hull x {0} obj2 hull x {1} rampOffset {2} rampOffsetGOOD {3}", (obj1Hull.x + obj1Hull.width), (obj2Hull.x), rampOffset, rampOffsetGOOD);



                    obj2Hull.y += (rampOffset * 1.0f);
                }


                i2 = 0;
                while (i2 < l2)
                {
                    ox2 = co2[i2].X;
                    oy2 = co2[i2].Y;
                    obj2Hull.x += ox2;
                    obj2Hull.y += oy2;

                    //See if it's a actually a valid collision
                    if ((obj1Hull.x + obj1Hull.width < obj2Hull.x + roundingError) ||
                        (obj1Hull.x + roundingError > obj2Hull.x + obj2Hull.width) ||
                        (obj1Hull.y + obj1Hull.height < obj2Hull.y + roundingError) ||
                        (obj1Hull.y + roundingError > obj2Hull.y + obj2Hull.height))
                    {
                        obj2Hull.x = obj2Hull.x - ox2;
                        obj2Hull.y = obj2Hull.y - oy2;
                        i2++;
                        continue;
                    }

                    //Calculate the overlap between the objects
                    if (p1hn2)
                    {
                        if (obj1MoveNeg)
                            r1 = obj1Hull.y + e.Object1.colHullX.height;
                        else
                            r1 = obj1Hull.y + obj1Hull.height;
                        if (obj2MoveNeg)
                            r2 = obj2Hull.y;
                        else
                            r2 = obj2Hull.y + obj2Hull.height - e.Object2.colHullX.height;
                    }
                    else
                    {
                        if (obj2MoveNeg)
                            r1 = -obj2Hull.y - e.Object2.colHullX.height;
                        else
                            r1 = -obj2Hull.y - obj2Hull.height;
                        if (obj1MoveNeg)
                            r2 = -obj1Hull.y;
                        else
                            r2 = -obj1Hull.y - obj1Hull.height + e.Object1.colHullX.height;
                    }
                    overlap = r1 - r2;

                    //Slightly smarter version of checking if objects are 'fixed' in space or not
                    f1 = e.Object1.@fixed;
                    f2 = e.Object2.@fixed;
                    if (f1 && f2)
                    {
                        f1 &= (e.Object1.colVector.X == 0) && (o1 == 0);
                        f2 &= (e.Object2.colVector.X == 0) && (o2 == 0);
                    }

                    //Last chance to skip out on a bogus collision resolution
                    if ((overlap == 0) ||
                        ((!f1 && ((overlap > 0) ? overlap : -overlap) > obj1Hull.height * 0.8)) ||
                        ((!f2 && ((overlap > 0) ? overlap : -overlap) > obj2Hull.height * 0.8)))
                    {
                        obj2Hull.x = obj2Hull.x - ox2;
                        obj2Hull.y = obj2Hull.y - oy2;
                        i2++;
                        continue;
                    }
                    hit = true;

                    //Adjust the objects according to their flags and stuff
                    sv1 = e.Object2.velocity.Y;
                    sv2 = e.Object1.velocity.Y;
                    if (!f1 && f2)
                    {
                        if (e.Object1._group)
                            e.Object1.reset(e.Object1.x, (e.Object1.y - overlap));
                        else
                            e.Object1.y = e.Object1.y - overlap;
                    }
                    else if (f1 && !f2)
                    {
                        if (e.Object2._group)
                            e.Object2.reset(e.Object2.x, (e.Object2.y + overlap));
                        else
                            e.Object2.y += overlap;
                    }
                    else if (!f1 && !f2)
                    {
                        overlap /= 2;
                        if (e.Object1._group)
                            e.Object1.reset(e.Object1.x, (e.Object1.y - overlap));
                        else
                            e.Object1.y = e.Object1.y - overlap;
                        if (e.Object2._group)
                            e.Object2.reset(e.Object2.x, (e.Object2.y + overlap));
                        else
                            e.Object2.y += overlap;
                        sv1 *= 0.5f;
                        sv2 *= 0.5f;
                    }
                    if (p1hn2)
                    {
                        e.Object1.hitBottom(e.Object2, sv1);
                        e.Object2.hitTop(e.Object1, sv2);
                    }
                    else
                    {
                        e.Object1.hitTop(e.Object2, sv1);
                        e.Object2.hitBottom(e.Object1, sv2);
                    }

                    //Adjust collision hulls if necessary
                    if (!f1 && (overlap != 0))
                    {
                        if (p1hn2)
                        {
                            obj1Hull.y = obj1Hull.y - overlap;

                            //This code helps stuff ride horizontally moving platforms.
                            if (f2 && e.Object2.moves)
                            {
                                sv1 = e.Object2.colVector.X;
                                e.Object1.x += sv1;
                                obj1Hull.x += sv1;
                                e.Object1.colHullX.x += sv1;
                            }
                        }
                        else
                        {
                            obj1Hull.y = obj1Hull.y - overlap;
                            obj1Hull.height += overlap;
                        }
                    }
                    if (!f2 && (overlap != 0))
                    {
                        if (p1hn2)
                        {
                            obj2Hull.y += overlap;
                            obj2Hull.height = obj2Hull.height - overlap;
                        }
                        else
                        {
                            obj2Hull.height += overlap;

                            //This code helps stuff ride horizontally moving platforms.
                            if (f1 && e.Object1.moves)
                            {
                                sv2 = e.Object1.colVector.X;
                                e.Object2.x += sv2;
                                obj2Hull.x += sv2;
                                e.Object2.colHullX.x += sv2;
                            }
                        }
                    }
                    obj2Hull.x = obj2Hull.x - ox2;
                    obj2Hull.y = obj2Hull.y - oy2;
                    i2++;
                }
                obj1Hull.x = obj1Hull.x - ox1;
                
                
                obj1Hull.y = obj1Hull.y - oy1;

                


                i1++;
            }

            return hit;
        }



        /// <summary>
        /// 
        /// </summary>
        private static Random randomTicks = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Generate a random string
        /// </summary>
        /// <param name="size">of this length</param>
        /// <returns></returns>
        static public string randomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * randomTicks.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Internal function to turn a texture into an array of colors
        /// </summary>
        /// <param name="texture">Texture to load</param>
        /// <returns>Returns a multi array of colors (Color[,])</returns>
        static public Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

        /// <summary>
        /// Get's a color from a texture.
        /// </summary>
        /// <param name="bitmapData">Texture to get the color from</param>
        /// <param name="xPos">X position in pixels</param>
        /// <param name="yPos">Y position in pixels</param>
        /// <returns>Returns a uint of the color.</returns>
        static public Color getColorFromBitmapAtPoint(Texture2D bitmapData, int xPos, int yPos)
        {
            Color[,] cols = TextureTo2DArray(bitmapData);

            Color colorAtPoint = cols[xPos, yPos];

            return colorAtPoint;
        }

        /// <summary>
        /// Takes a string that is formatted 0,1,1,0,0,0
        /// </summary>
        /// <param name="StringToSplit">The string to split with commas and \n splits</param>
        /// <returns>an integer array (int[])</returns>
        static public int[] convertStringToIntegerArray(string StringToSplit)
        {

            if (StringToSplit == "") return null;

            if (StringToSplit != null)
            {
                string[] words = StringToSplit.Split(',');

                int[] intArray = new int[words.Length];

                int i = 0;

                foreach (string word in words)
                {
                    intArray[i] = Convert.ToInt32(word);
                    i++;

                }

                return intArray;
            }

            return null;
        }

        /// <summary>
        /// Calculate the distance between two points.
        /// </summary>
        /// <param name="Point1">A <code>FlxPoint</code> object referring to the first location.</param>
        /// <param name="Point2">A <code>FlxPoint</code> object referring to the second location.</param>
        /// <returns>The distance between the two points as a floating point <code>Number</code> object.</returns>
        public static float getDistance(Vector2 Point1, Vector2 Point2)
        {
            float dx = Point1.X - Point2.X;
            float dy = Point1.Y - Point2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }


        /// <summary>
        /// Added to make saving files to disk easy.
        /// </summary>
        /// <param name="Lines"></param>
        /// <param name="Filename"></param>
        public static void saveToDevice(string Lines, string Filename)
        {

			#if __ANDROID__

			IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

			using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(Filename, FileMode.Create, isoStore))
			{
				using (StreamWriter writer = new StreamWriter(isoStream))
				{
					writer.WriteLine(Lines);
					//Console.WriteLine("You have written to the file.");
					writer.Close();
				}
			}


			#endif
			#if !__ANDROID__
			System.IO.StreamWriter file = new System.IO.StreamWriter(Filename);
			file.WriteLine(Lines);
			file.Close();
			#endif


            // Write the string to a file.



        }

		public static string loadFromDevice(string Filename)
		{
			return loadFromDevice (Filename, false);
		}

        /// <summary>
        /// Loads a text file from the Hard Drive
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
		public static string loadFromDevice(string Filename, bool FromIsolatedStorage)
		{
			//Console.WriteLine ("-- Load from Device -- ", Filename);

			string value1;

			#if __ANDROID__

			/*
			using (StreamReader sr = new StreamReader (Game.Activity.Assets.Open(Filename)))
			{
				value1 = sr.ReadToEnd();
			}
			*/

			if (FromIsolatedStorage) 
			{

				IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

				using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(Filename, FileMode.Open, isoStore))
				{
					using (StreamReader reader = new StreamReader(isoStream))
					{
						//Console.WriteLine("Reading contents:");
						//Console.WriteLine(reader.ReadToEnd());

						value1 = reader.ReadToEnd();

					}
				}
			}
			else {
				using (StreamReader sr = new StreamReader (Game.Activity.Assets.Open(Filename)))
				{
					value1 = sr.ReadToEnd();
				}
			}

			#endif
			#if !__ANDROID__
			value1 = File.ReadAllText(Filename);
			#endif

			return value1.Substring(0, value1.Length - 1);

        }
    }
}
