using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace IMPORT_PLATFORM
{
    public class HalfSpinner : PatrolingEnemy
    {
        public HalfSpinner(ContentManager Content, Vector2 location)
        {
            this.texture = EnemiesManager.EnemiesSheet;

            animations.Add(
                "Idle",
                new AnimationStrip(
                    texture,
                    Content.Load<Rectangle[]>(@"SheetData/Enemies/HalfSpinner/HalfSpinnerIdle"),
                    "Idle"));
            animations["Idle"].LoopAnimation = true;
            animations["Idle"].FrameLength = 0.1f;

            this.worldLocation = location;
            this.maxSbd = new Vector2(0, 100);
            this.acc = 0.0f;
            this.dec = 0.0f;
            this.hasGravity = true;
            this.checkCollisions = true;
            this.limitSpeed = true;
            this.InitializePhisics(maxSbd, acc, dec, hasGravity, checkCollisions);

            this.drawDepth = 0.9f;
            this.enabled = true;
            this.scoreValue = 10;
            this.PlayAnimation("Idle");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
