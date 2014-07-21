using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace org.flixel
{
    /// <summary>
    /// This is a simple path data container.  Basically a list of points that
    /// a <code>FlxObject</code> can follow.  Also has code for drawing debug visuals.
    /// <code>FlxTilemap.findPath()</code> returns a path object, but you can
    /// also just make your own, using the <code>add()</code> functions below
    /// or by creating your own array of points.
    /// </summary>
    public class FlxPath
    {
        /// <summary>
        /// The list of <code>FlxPoint</code>s that make up the path data.
        /// </summary>
        public List<Vector2> nodes;

        /// <summary>
        /// Specify a debug display color for the path.  Default is white.
        /// </summary>
        public Color debugColor;

        /// <summary>
        ///  Specify a debug display scroll factor for the path.  Default is (1,1).
        ///  NOTE: does not affect world movement!  Object scroll factors take care of that.
        /// </summary>
        public Vector2 debugScrollFactor;

        /// <summary>
        /// Setting this to true will prevent the object from appearing
        /// when the visual debug mode in the debugger overlay is toggled on.
        /// @default false
        /// </summary>
        public bool ignoreDrawDebug;

        /// <summary>
        /// Internal helper for keeping new variable instantiations under control.
        /// </summary>
        protected Vector2 _point;

        /// <summary>
        /// Instantiate a new path object.
        /// </summary>
        /// <param name="Nodes">Optional, can specify all the points for the path up front if you want.</param>
        public FlxPath(List<Vector2> Nodes = null)
        {
            // Port progress - complete, needs testing. - draw debug stuff is left incomplete.

            if (Nodes==null)
                nodes = new List<Vector2>();
            else
                nodes = Nodes;

            _point = new Vector2();

            debugScrollFactor = new Vector2(1.0f, 1.0f);

            debugColor = new Color(0xff, 0x00, 0x00);

            ignoreDrawDebug = false;

            //  var debugPathDisplay:DebugPathDisplay = manager;
            //  if(debugPathDisplay != null)
            //      debugPathDisplay.add(this);


        }

        /// <summary>
        /// Clean up memory.
        /// </summary>
        public void destroy()
        {
            // Port progress - complete, needs testing.

            //  var debugPathDisplay:DebugPathDisplay = manager;
            //  if(debugPathDisplay != null)
            //      debugPathDisplay.remove(this);

            debugScrollFactor = Vector2.Zero;
            _point = Vector2.Zero;
            nodes = null;
        }

        /// <summary>
        /// Simple helper to turn a string from an ogmo level into a uint.
        /// </summary>
        /// <param name="PathType"></param>
        /// <returns></returns>
        public static uint convertStringValueForPathType(string PathType)
        {
            if (PathType == "FORWARD")
            {
                return FlxObject.PATH_FORWARD;
            }
            else if (PathType == "BACKWARD")
            {
                return FlxObject.PATH_BACKWARD;
            }
            else if (PathType == "LOOP_FORWARD")
            {
                return FlxObject.PATH_LOOP_FORWARD;
            }
            else if (PathType == "LOOP_BACKWARD")
            {
                return FlxObject.PATH_LOOP_BACKWARD;
            }
            else if (PathType == "YOYO")
            {
                return FlxObject.PATH_YOYO;
            }
            else if (PathType == "HORIZONTAL_ONLY")
            {
                return FlxObject.PATH_HORIZONTAL_ONLY;
            }
            else if (PathType == "VERTICAL_ONLY")
            {
                return FlxObject.PATH_VERTICAL_ONLY;
            }



            return FlxObject.PATH_FORWARD;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PointsX"></param>
        /// <param name="PointsY"></param>
        public void addPointsUsingStrings(string PointsX, string PointsY)
        {
            string[] splX = PointsX.Split(',');
            string[] splY = PointsY.Split(',');

            if (splX.Length != splY.Length)
            {
                FlxG.log("Length of both paths does not match");

                return;
            }


            for (int i = 0; i < splX.Length; i++)
            {
                //Console.WriteLine(splX[i]);

                if (splX[i] != "" || splY[i] != "")
                {
                    add(float.Parse(splX[i]), float.Parse(splY[i]));
                }
            }
        }

        /// <summary>
        /// Add a new node to the end of the path at the specified location.
        /// </summary>
        /// <param name="X">X position of the new path point in world coordinates.</param>
        /// <param name="Y">Y position of the new path point in world coordinates.</param>
        public void add(float X, float Y)
        {
            // Port progress - complete, needs testing.

            nodes.Add(new Vector2(X, Y));
        }

        /// <summary>
        /// Add a new node to the end of the path at the specified location.
        /// </summary>
        /// <param name="X">X position of the new path point in world coordinates.</param>
        /// <param name="Y">Y position of the new path point in world coordinates.</param>
        /// <param name="Index">Where within the list of path nodes to insert this new point.</param>
        public void addAt(float X, float Y, int Index)
        {
            // Port progress - complete, needs testing.

            if (Index > nodes.Count)
                Index = nodes.Count;
            nodes.Insert(Index, new Vector2(X, Y));
        }

        /// <summary>
        /// Sometimes its easier or faster to just pass a point object instead of separate X and Y coordinates.
        /// This also gives you the option of not creating a new node but actually adding that specific
        /// <code>Vector2</code> object to the path.  This allows you to do neat things, like dynamic paths.
        /// </summary>
        /// <param name="Node">The point in world coordinates you want to add to the path.</param>
        /// <param name="AsReference">Whether to add the point as a reference, or to create a new point with the specified values.</param>
        public void addPoint(Vector2 Node, bool AsReference)
        {
            // Port progress - complete, needs testing.

            if (AsReference)
                nodes.Add(Node);
            else
                nodes.Add(new Vector2(Node.X, Node.Y));
        }

        /// <summary>
        /// Sometimes its easier or faster to just pass a point object instead of separate X and Y coordinates.
        /// This also gives you the option of not creating a new node but actually adding that specific
        /// <code>Vector2</code> object to the path.  This allows you to do neat things, like dynamic paths.
        /// </summary>
        /// <param name="Node">The point in world coordinates you want to add to the path.</param>
        /// <param name="Index">Where within the list of path nodes to insert this new point.</param>
        /// <param name="AsReference">Whether to add the point as a reference, or to create a new point with the specified values.</param>
        public void addPointAt(Vector2 Node, int Index, bool AsReference)
        {
            // Port progress - complete, needs testing.

            if (Index > nodes.Count)
                Index = nodes.Count;
            if (AsReference)
                nodes.Insert(Index, Node);
            else
                nodes.Insert(Index, new Vector2(Node.X, Node.Y));
        }

        /// <summary>
        /// Remove a node from the path.
        /// NOTE: only works with points added by reference or with references from <code>nodes</code> itself!
        /// </summary>
        /// <param name="Node">The point object you want to remove from the path.</param>
        /// <returns>The node that was excised.  Returns null if the node was not found.</returns>
        public Vector2 remove(Vector2 Node)
        {
            // Port progress - incomplete. Needs testing.

            int index = nodes.IndexOf(Node);

            /*
            int index = nodes.indexOf(Node);
            if(index >= 0)
                    return nodes.splice(index,1)[0];
            else
                    return null;
            */

            return Vector2.Zero;
        }

        /// <summary>
        /// Remove a node from the path using the specified position in the list of path nodes.
        /// </summary>
        /// <param name="Index">Where within the list of path nodes you want to remove a node.</param>
        /// <returns>The node that was excised.  Returns null if there were no nodes in the path.</returns>
        public Vector2 removeAt(uint Index)
        {
            // Port progress - incomplete.

            /*
            if (nodes.length <= 0)
                return null;
            if (Index >= nodes.length)
                Index = nodes.length - 1;
            return nodes.splice(Index, 1)[0];
             */

            return Vector2.Zero;
        }

        /// <summary>
        /// Get the first node in the list.
        /// </summary>
        /// <returns>The first node in the path.</returns>
        public Vector2 head()
        {
            // Port progress - complete, needs testing.

            if (nodes.Count > 0)
                return nodes[0];
            return Vector2.Zero;
        }

        /// <summary>
        /// Get the last node in the list.
        /// </summary>
        /// <returns>The last node in the path.</returns>
        public Vector2 tail()
        {
            // Port progress - complete, needs testing.
            
            if (nodes.Count > 0)
                return nodes[nodes.Count - 1];
            return Vector2.Zero;
        }

        /// <summary>
        /// While this doesn't override <code>FlxBasic.drawDebug()</code>, the behavior is very similar.
        /// Based on this path data, it draws a simple lines-and-boxes representation of the path
        /// if the visual debug mode was toggled in the debugger overlay.  You can use <code>debugColor</code>
        /// and <code>debugScrollFactor</code> to control the path's appearance.
        /// 
        /// </summary>
        public void drawDebug()
        {
            // Port progress - incomplete.
        }


    }
}
//package org.flixel
//{
//        import flash.display.Graphics;
        
//        import org.flixel.plugin.DebugPathDisplay;
        
//        /**
//         * This is a simple path data container.  Basically a list of points that
//         * a <code>FlxObject</code> can follow.  Also has code for drawing debug visuals.
//         * <code>FlxTilemap.findPath()</code> returns a path object, but you can
//         * also just make your own, using the <code>add()</code> functions below
//         * or by creating your own array of points.
//         * 
//         * @author        Adam Atomic
//         */
//        public class FlxPath
//        {
//                /**
//                 * The list of <code>FlxPoint</code>s that make up the path data.
//                 */
//                public var nodes:Array;
//                /**
//                 * Specify a debug display color for the path.  Default is white.
//                 */
//                public var debugColor:uint;
//                /**
//                 * Specify a debug display scroll factor for the path.  Default is (1,1).
//                 * NOTE: does not affect world movement!  Object scroll factors take care of that.
//                 */
//                public var debugScrollFactor:FlxPoint;
//                /**
//                 * Setting this to true will prevent the object from appearing
//                 * when the visual debug mode in the debugger overlay is toggled on.
//                 * @default false
//                 */
//                public var ignoreDrawDebug:Boolean;

//                /**
//                 * Internal helper for keeping new variable instantiations under control.
//                 */
//                protected var _point:FlxPoint;
                
//                /**
//                 * Instantiate a new path object.
//                 * 
//                 * @param        Nodes        Optional, can specify all the points for the path up front if you want.
//                 */
//                public function FlxPath(Nodes:Array=null)
//                {
//                        if(Nodes == null)
//                                nodes = new Array();
//                        else
//                                nodes = Nodes;
//                        _point = new FlxPoint();
//                        debugScrollFactor = new FlxPoint(1.0,1.0);
//                        debugColor = 0xffffff;
//                        ignoreDrawDebug = false;
                        
//                        var debugPathDisplay:DebugPathDisplay = manager;
//                        if(debugPathDisplay != null)
//                                debugPathDisplay.add(this);
//                }
                
//                /**
//                 * Clean up memory.
//                 */
//                public function destroy():void
//                {
//                        var debugPathDisplay:DebugPathDisplay = manager;
//                        if(debugPathDisplay != null)
//                                debugPathDisplay.remove(this);
                        
//                        debugScrollFactor = null;
//                        _point = null;
//                        nodes = null;
//                }
                
//                /**
//                 * Add a new node to the end of the path at the specified location.
//                 * 
//                 * @param        X        X position of the new path point in world coordinates.
//                 * @param        Y        Y position of the new path point in world coordinates.
//                 */
//                public function add(X:Number,Y:Number):void
//                {
//                        nodes.push(new FlxPoint(X,Y));
//                }
                
//                /**
//                 * Add a new node to the path at the specified location and index within the path.
//                 * 
//                 * @param        X                X position of the new path point in world coordinates.
//                 * @param        Y                Y position of the new path point in world coordinates.
//                 * @param        Index        Where within the list of path nodes to insert this new point.
//                 */
//                public function addAt(X:Number, Y:Number, Index:uint):void
//                {
//                        if(Index > nodes.length)
//                                Index = nodes.length;
//                        nodes.splice(Index,0,new FlxPoint(X,Y));
//                }
                
//                /**
//                 * Sometimes its easier or faster to just pass a point object instead of separate X and Y coordinates.
//                 * This also gives you the option of not creating a new node but actually adding that specific
//                 * <code>FlxPoint</code> object to the path.  This allows you to do neat things, like dynamic paths.
//                 * 
//                 * @param        Node                        The point in world coordinates you want to add to the path.
//                 * @param        AsReference                Whether to add the point as a reference, or to create a new point with the specified values.
//                 */
//                public function addPoint(Node:FlxPoint,AsReference:Boolean=false):void
//                {
//                        if(AsReference)
//                                nodes.push(Node);
//                        else
//                                nodes.push(new FlxPoint(Node.x,Node.y));
//                }
                
//                /**
//                 * Sometimes its easier or faster to just pass a point object instead of separate X and Y coordinates.
//                 * This also gives you the option of not creating a new node but actually adding that specific
//                 * <code>FlxPoint</code> object to the path.  This allows you to do neat things, like dynamic paths.
//                 * 
//                 * @param        Node                        The point in world coordinates you want to add to the path.
//                 * @param        Index                        Where within the list of path nodes to insert this new point.
//                 * @param        AsReference                Whether to add the point as a reference, or to create a new point with the specified values.
//                 */
//                public function addPointAt(Node:FlxPoint,Index:uint,AsReference:Boolean=false):void
//                {
//                        if(Index > nodes.length)
//                                Index = nodes.length;
//                        if(AsReference)
//                                nodes.splice(Index,0,Node);
//                        else
//                                nodes.splice(Index,0,new FlxPoint(Node.x,Node.y));
//                }
                
//                /**
//                 * Remove a node from the path.
//                 * NOTE: only works with points added by reference or with references from <code>nodes</code> itself!
//                 * 
//                 * @param        Node        The point object you want to remove from the path.
//                 * 
//                 * @return        The node that was excised.  Returns null if the node was not found.
//                 */
//                public function remove(Node:FlxPoint):FlxPoint
//                {
//                        var index:int = nodes.indexOf(Node);
//                        if(index >= 0)
//                                return nodes.splice(index,1)[0];
//                        else
//                                return null;
//                }
                
//                /**
//                 * Remove a node from the path using the specified position in the list of path nodes.
//                 * 
//                 * @param        Index        Where within the list of path nodes you want to remove a node.
//                 * 
//                 * @return        The node that was excised.  Returns null if there were no nodes in the path.
//                 */
//                public function removeAt(Index:uint):FlxPoint
//                {
//                        if(nodes.length <= 0)
//                                return null;
//                        if(Index >= nodes.length)
//                                Index = nodes.length-1;
//                        return nodes.splice(Index,1)[0];
//                }
                
//                /**
//                 * Get the first node in the list.
//                 * 
//                 * @return        The first node in the path.
//                 */
//                public function head():FlxPoint
//                {
//                        if(nodes.length > 0)
//                                return nodes[0];
//                        return null;
//                }
                
//                /**
//                 * Get the last node in the list.
//                 * 
//                 * @return        The last node in the path.
//                 */
//                public function tail():FlxPoint
//                {
//                        if(nodes.length > 0)
//                                return nodes[nodes.length-1];
//                        return null;
//                }
                
//                /**
//                 * While this doesn't override <code>FlxBasic.drawDebug()</code>, the behavior is very similar.
//                 * Based on this path data, it draws a simple lines-and-boxes representation of the path
//                 * if the visual debug mode was toggled in the debugger overlay.  You can use <code>debugColor</code>
//                 * and <code>debugScrollFactor</code> to control the path's appearance.
//                 * 
//                 * @param        Camera                The camera object the path will draw to.
//                 */
//                public function drawDebug(Camera:FlxCamera=null):void
//                {
//                        if(nodes.length <= 0)
//                                return;
//                        if(Camera == null)
//                                Camera = FlxG.camera;
                        
//                        //Set up our global flash graphics object to draw out the path
//                        var gfx:Graphics = FlxG.flashGfx;
//                        gfx.clear();
                        
//                        //Then fill up the object with node and path graphics
//                        var node:FlxPoint;
//                        var nextNode:FlxPoint;
//                        var i:uint = 0;
//                        var l:uint = nodes.length;
//                        while(i < l)
//                        {
//                                //get a reference to the current node
//                                node = nodes[i] as FlxPoint;
                                
//                                //find the screen position of the node on this camera
//                                _point.x = node.x - int(Camera.scroll.x*debugScrollFactor.x); //copied from getScreenXY()
//                                _point.y = node.y - int(Camera.scroll.y*debugScrollFactor.y);
//                                _point.x = int(_point.x + ((_point.x > 0)?0.0000001:-0.0000001));
//                                _point.y = int(_point.y + ((_point.y > 0)?0.0000001:-0.0000001));
                                
//                                //decide what color this node should be
//                                var nodeSize:uint = 2;
//                                if((i == 0) || (i == l-1))
//                                        nodeSize *= 2;
//                                var nodeColor:uint = debugColor;
//                                if(l > 1)
//                                {
//                                        if(i == 0)
//                                                nodeColor = FlxG.GREEN;
//                                        else if(i == l-1)
//                                                nodeColor = FlxG.RED;
//                                }
                                
//                                //draw a box for the node
//                                gfx.beginFill(nodeColor,0.5);
//                                gfx.lineStyle();
//                                gfx.drawRect(_point.x-nodeSize*0.5,_point.y-nodeSize*0.5,nodeSize,nodeSize);
//                                gfx.endFill();

//                                //then find the next node in the path
//                                var linealpha:Number = 0.3;
//                                if(i < l-1)
//                                        nextNode = nodes[i+1];
//                                else
//                                {
//                                        nextNode = nodes[0];
//                                        linealpha = 0.15;
//                                }
                                
//                                //then draw a line to the next node
//                                gfx.moveTo(_point.x,_point.y);
//                                gfx.lineStyle(1,debugColor,linealpha);
//                                _point.x = nextNode.x - int(Camera.scroll.x*debugScrollFactor.x); //copied from getScreenXY()
//                                _point.y = nextNode.y - int(Camera.scroll.y*debugScrollFactor.y);
//                                _point.x = int(_point.x + ((_point.x > 0)?0.0000001:-0.0000001));
//                                _point.y = int(_point.y + ((_point.y > 0)?0.0000001:-0.0000001));
//                                gfx.lineTo(_point.x,_point.y);

//                                i++;
//                        }
                        
//                        //then stamp the path down onto the game buffer
//                        Camera.buffer.draw(FlxG.flashGfxSprite);
//                }
                
//                static public function get manager():DebugPathDisplay
//                {
//                        return FlxG.getPlugin(DebugPathDisplay) as DebugPathDisplay;
//                }
//        }
//}