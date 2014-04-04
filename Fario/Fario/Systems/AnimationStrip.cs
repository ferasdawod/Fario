using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IMPORT_PLATFORM
{
    public class AnimationStrip
    {
        #region Declerations

        private Texture2D texture;
        private int numberOfFrames;

        private bool multiFrame;
        private Rectangle[] frames;
        private Rectangle frame;

        private float frameTimer = 0f;
        private float frameDelay = 0.08f;

        private int currentFrame = 0;

        private bool loopAnimation = true;
        private bool finishedPlaying = false;

        private string name;
        private string nextAnimation;

        #endregion

        #region Properties

        public int FrameWidth
        {
            get 
            {
                if (multiFrame)
                {
                    return frames[currentFrame].Width;
                }
                else
                {
                    return frame.Width;
                }
            }
        }

        public int FrameHeight
        {
            get
            {
                if (multiFrame)
                {
                    return frames[currentFrame].Height;
                }
                else
                {
                    return frame.Height;
                }
            }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string NextAnimation
        {
            get { return nextAnimation; }
            set { nextAnimation = value; }
        }

        public bool LoopAnimation
        {
            get { return loopAnimation; }
            set { loopAnimation = value; }
        }

        public bool FinishedPlaying
        {
            get { return finishedPlaying; }
            set { finishedPlaying = value; }
        }

        public int FrameCount
        {
            get { return numberOfFrames; }
            set { numberOfFrames = value; }
        }

        public float FrameLength
        {
            get { return frameDelay; }
            set { frameDelay = value; }
        }

        public Rectangle FrameRectangle
        {
            get
            {
                if (multiFrame)
                {
                    return frames[currentFrame];
                }
                else
                {
                    return frame;
                }
            }
        }

        #endregion

        #region Constructor

        public AnimationStrip(
            Texture2D texture, Rectangle[] frames, string name)
        {
            this.texture = texture;
            this.name = name;
            this.frames = frames;
            this.numberOfFrames = frames.Count();
            this.multiFrame = true;
        }

        public AnimationStrip(
            Texture2D texture, Rectangle frame, string name)
        {
            this.texture = texture;
            this.name = name;
            this.frame = frame;
            this.numberOfFrames = 1;
            this.multiFrame = false;
        }

        #endregion

        #region Methods

        public void Play()
        {
            currentFrame = 0;
            finishedPlaying = false;
        }

        public void Update(GameTime gameTime)
        {
            if (multiFrame)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameTimer += elapsed;
                if (frameTimer >= frameDelay)
                {
                    currentFrame++;
                    if (currentFrame >= FrameCount)
                    {
                        if (loopAnimation)
                        {
                            currentFrame = 0;
                        }
                        else
                        {
                            currentFrame = FrameCount - 1;
                            finishedPlaying = true;
                        }
                    }
                    frameTimer = 0f;
                }
            }
        }

        #endregion

    }
}
