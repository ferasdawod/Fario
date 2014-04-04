using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace IMPORT_PLATFORM
{
    public class Spider : PatrolingEnemy
    {
        #region Declerations

        #endregion

        public Spider(ContentManager content, Vector2 worldLocation)
        {
            this.texture = EnemiesManager.EnemiesSheet;

            animations.Add(
                "Walk",
                new AnimationStrip(
                    texture,
                    content.Load<Rectangle[]>(@"SheetData\Enemies\Spider\SpiderWalk"),
                    "Walk"));
            animations["Walk"].LoopAnimation = true;
            animations["Walk"].FrameLength = 0.1f;

            animations.Add(
                "Hurt",
                new AnimationStrip(
                    texture,
                    content.Load<Rectangle[]>(@"SheetData\Enemies\Spider\SpiderHurt"),
                    "Hurt"));
            animations["Hurt"].LoopAnimation = true;
            animations["Hurt"].NextAnimation = "Walk";
            animations["Hurt"].FrameLength = 0.25f;

            animations.Add(
                "Dead",
                new AnimationStrip(
                    texture,
                    content.Load<Rectangle>(@"SheetData\Enemies\Spider\SpiderDead"),
                    "Dead"));
            animations["Dead"].LoopAnimation = true;

            this.worldLocation = worldLocation;
            this.maxSbd = new Vector2(0.5f * 60.0f, 100.0f * 60.0f);
            this.acc = 50.0f * 60.0f;
            this.dec = 15.0f * 60.0f;
            this.hasGravity = true;
            this.checkCollisions = true;
            this.limitSpeed = true;
            this.InitializePhisics(maxSbd, acc, dec, hasGravity, checkCollisions);

            this.drawDepth = 0.9f;
            this.enabled = true;
            this.scoreValue = 100;
            this.PlayAnimation("Walk");
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
