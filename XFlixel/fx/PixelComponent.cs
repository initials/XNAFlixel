#region File Description
//-----------------------------------------------------------------------------
// PixelComponent.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using org.flixel;
#endregion

namespace PixelPostprocess
{
    public class PixelComponent : DrawableGameComponent
    {
        #region Fields

        //SpriteBatch spriteBatch;

        Effect CompositeEffect;
        Effect NtscEffect;
        Effect ScreenEffect;
        Effect CrtEffect;

        RenderTarget2D sceneRenderTarget;
        RenderTarget2D renderTarget1;
        RenderTarget2D renderTarget2;

        public bool usePresets = true;


        // Optionally displays one of the intermediate buffers used
        // by the Pixel postprocess, so you can see exactly what is
        // being drawn into each rendertarget.
        public enum IntermediateBuffer
        {
            PrePixel,
            BlurredHorizontally,
            BlurredBothWays,
            FinalResult,
        }

        public IntermediateBuffer ShowBuffer
        {
            get { return showBuffer; }
            set { showBuffer = value; }
        }

        IntermediateBuffer showBuffer = IntermediateBuffer.FinalResult;


        #endregion

        #region Initialization


        public PixelComponent(Game game)
            : base(game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            Visible = false;
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            //spriteBatch = new SpriteBatch(GraphicsDevice);

#if WINDOWS

            CompositeEffect = Game.Content.Load<Effect>("flixel/fx/pixel/composite");
            NtscEffect = Game.Content.Load<Effect>("flixel/fx/pixel/ntsc");
            ScreenEffect = Game.Content.Load<Effect>("flixel/fx/pixel/screen");
            CrtEffect = Game.Content.Load<Effect>("flixel/fx/pixel/crtDistortion");


#endif

            // Look up the resolution and format of our main backbuffer.
            PresentationParameters pp = GraphicsDevice.PresentationParameters;


            int width = pp.BackBufferWidth / FlxG.zoom; //unsure of where this 4 comes from.
            int height = pp.BackBufferHeight / FlxG.zoom;

            SurfaceFormat format = pp.BackBufferFormat;

            // Create a texture for rendering the main scene, prior to applying Pixel.
            sceneRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
                                                   format, pp.DepthStencilFormat, pp.MultiSampleCount,
                                                   RenderTargetUsage.DiscardContents);

            // Create two rendertargets for the Pixel processing. These are half the
            // size of the backbuffer, in order to minimize fillrate costs. Reducing
            // the resolution in this way doesn't hurt quality, because we are going
            // to be blurring the Pixel images in any case.
            width /= 2;
            height /= 2;

            renderTarget1 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(GraphicsDevice, width, height, false, format, DepthFormat.None);
        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            sceneRenderTarget.Dispose();
            renderTarget1.Dispose();
            renderTarget2.Dispose();
        }


        #endregion

        #region Draw


        /// <summary>
        /// This should be called at the very start of the scene rendering. The Pixel
        /// component uses it to redirect drawing into its custom rendertarget, so it
        /// can capture the scene image in preparation for applying the Pixel filter.
        /// </summary>
        public void BeginDraw()
        {
            if (Visible)
            {
                GraphicsDevice.SetRenderTarget(sceneRenderTarget);
            }
        }


        /// <summary>
        /// This is where it all happens. Grabs a scene that has already been rendered,
        /// and uses postprocess magic to add a glowing Pixel effect over the top of it.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            //// Pass 1: draw the scene into rendertarget 1, using a
            //// shader that extracts only the brightest parts of the image.

            ////if (usePresets)
            ////    PixelExtractEffect.Parameters["PixelThreshold"].SetValue(
            ////    Settings.PixelThreshold);
            ////else
            ////    PixelExtractEffect.Parameters["PixelThreshold"].SetValue(PixelThreshold);

            //DrawFullscreenQuad(sceneRenderTarget, renderTarget1,
            //                   NtscEffect,
            //                   IntermediateBuffer.PrePixel);

            //// Pass 2: draw from rendertarget 1 into rendertarget 2,
            //// using a shader to apply a horizontal gaussian blur filter.
            ////SetBlurEffectParameters(1.0f / (float)renderTarget1.Width, 0);

            //DrawFullscreenQuad(renderTarget1, renderTarget2,
            //                   NtscEffect,
            //                   IntermediateBuffer.BlurredHorizontally);

            //// Pass 3: draw from rendertarget 2 back into rendertarget 1,
            //// using a shader to apply a vertical gaussian blur filter.
            ////SetBlurEffectParameters(0, 1.0f / (float)renderTarget1.Height);

            //DrawFullscreenQuad(renderTarget2, renderTarget1,
            //                   NtscEffect,
            //                   IntermediateBuffer.BlurredBothWays);

            //// Pass 4: draw both rendertarget 1 and the original scene
            //// image back into the main backbuffer, using a shader that
            //// combines them to produce the final Pixeled result.
            //GraphicsDevice.SetRenderTarget(null);

            //EffectParameterCollection parameters = PixelCombineEffect.Parameters;
            //if (usePresets)
            //{
            //    parameters["PixelIntensity"].SetValue(Settings.PixelIntensity);
            //    parameters["BaseIntensity"].SetValue(Settings.BaseIntensity);
            //    parameters["PixelSaturation"].SetValue(Settings.PixelSaturation);
            //    parameters["BaseSaturation"].SetValue(Settings.BaseSaturation);
            //}
            //else
            //{
            //    parameters["PixelIntensity"].SetValue(PixelIntensity);
            //    parameters["BaseIntensity"].SetValue(baseIntensity);
            //    parameters["PixelSaturation"].SetValue(PixelSaturation);
            //    parameters["BaseSaturation"].SetValue(baseSaturation);
            //}
            //GraphicsDevice.Textures[1] = sceneRenderTarget;

            //Viewport viewport = GraphicsDevice.Viewport;

            //DrawFullscreenQuad(sceneRenderTarget,
            //                   viewport.Width, 
            //                   viewport.Height,
            //                   NtscEffect,
            //                   IntermediateBuffer.FinalResult);

            Viewport viewport = GraphicsDevice.Viewport;
            FlxG.spriteBatch.Begin(0, BlendState.Opaque, null, null, null, CrtEffect);
            FlxG.spriteBatch.Draw(sceneRenderTarget, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
            FlxG.spriteBatch.End();


        }


        /// <summary>
        /// Helper for drawing a texture into a rendertarget, using
        /// a custom shader to apply postprocessing effects.
        /// </summary>
        void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget,
                                Effect effect, IntermediateBuffer currentBuffer)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            DrawFullscreenQuad(texture,
                               renderTarget.Width, renderTarget.Height,
                               effect, currentBuffer);
        }


        /// <summary>
        /// Helper for drawing a texture into the current rendertarget,
        /// using a custom shader to apply postprocessing effects.
        /// </summary>
        void DrawFullscreenQuad(Texture2D texture, int width, int height,
                                Effect effect, IntermediateBuffer currentBuffer)
        {
            // If the user has selected one of the show intermediate buffer options,
            // we still draw the quad to make sure the image will end up on the screen,
            // but might need to skip applying the custom pixel shader.
            if (showBuffer < currentBuffer)
            {
                effect = null;
            }

            FlxG.spriteBatch.Begin(0, BlendState.Opaque, null, null, null, effect);
            FlxG.spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
            FlxG.spriteBatch.End();
        }


        /// <summary>
        /// Computes sample weightings and texture coordinate offsets
        /// for one pass of a separable gaussian blur filter.
        /// </summary>
        //void SetBlurEffectParameters(float dx, float dy)
        //{
        //    // Look up the sample weight and offset effect parameters.
        //    EffectParameter weightsParameter, offsetsParameter;

        //    //weightsParameter = gaussianBlurEffect.Parameters["SampleWeights"];
        //    //offsetsParameter = gaussianBlurEffect.Parameters["SampleOffsets"];

        //    // Look up how many samples our gaussian blur effect supports.
        //    //int sampleCount = weightsParameter.Elements.Count;

        //    // Create temporary arrays for computing our filter settings.
        //    float[] sampleWeights = new float[sampleCount];
        //    Vector2[] sampleOffsets = new Vector2[sampleCount];

        //    // The first sample always has a zero offset.
        //    sampleWeights[0] = ComputeGaussian(0);
        //    sampleOffsets[0] = new Vector2(0);

        //    // Maintain a sum of all the weighting values.
        //    float totalWeights = sampleWeights[0];

        //    // Add pairs of additional sample taps, positioned
        //    // along a line in both directions from the center.
        //    for (int i = 0; i < sampleCount / 2; i++)
        //    {
        //        // Store weights for the positive and negative taps.
        //        float weight = ComputeGaussian(i + 1);

        //        sampleWeights[i * 2 + 1] = weight;
        //        sampleWeights[i * 2 + 2] = weight;

        //        totalWeights += weight * 2;

        //        // To get the maximum amount of blurring from a limited number of
        //        // pixel shader samples, we take advantage of the bilinear filtering
        //        // hardware inside the texture fetch unit. If we position our texture
        //        // coordinates exactly halfway between two texels, the filtering unit
        //        // will average them for us, giving two samples for the price of one.
        //        // This allows us to step in units of two texels per sample, rather
        //        // than just one at a time. The 1.5 offset kicks things off by
        //        // positioning us nicely in between two texels.
        //        float sampleOffset = i * 2 + 1.5f;

        //        Vector2 delta = new Vector2(dx, dy) * sampleOffset;

        //        // Store texture coordinate offsets for the positive and negative taps.
        //        sampleOffsets[i * 2 + 1] = delta;
        //        sampleOffsets[i * 2 + 2] = -delta;
        //    }

        //    // Normalize the list of sample weightings, so they will always sum to one.
        //    for (int i = 0; i < sampleWeights.Length; i++)
        //    {
        //        sampleWeights[i] /= totalWeights;
        //    }

        //    // Tell the effect about our new filter settings.
        //    //weightsParameter.SetValue(sampleWeights);
        //    //offsetsParameter.SetValue(sampleOffsets);
        //}


        /// <summary>
        /// Evaluates a single point on the gaussian falloff curve.
        /// Used for setting up the blur filter weightings.
        /// </summary>
        //float ComputeGaussian(float n)
        //{
        //    //float theta = Settings.BlurAmount;

        //    return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
        //                   Math.Exp(-(n * n) / (2 * theta * theta)));
        //}


        #endregion
    }
}
