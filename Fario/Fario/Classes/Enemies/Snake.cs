using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IMPORT_PLATFORM
{
    public class Snake : PatrolingEnemy
    {

        public Snake(ContentManager content, Vector2 worldLocation)
        {
            this.texture = EnemiesManager.EnemiesSheet;

            animations.Add(
                "Walk",
                new AnimationStrip(
                    texture,
                    content.Load<Rectangle[]>(@"SheetData\Enemies\Snake\SnakeWalk"),
                    "Walk"));
            animations["Walk"].LoopAnimation = true;
            animations["Walk"].FrameLength = 0.1f;

            animations.Add(
                "Hurt",
                new AnimationStrip(
                    texture,
                    content.Load<Rectangle[]>(@"SheetData\Enemies\Snake\SnakeHurt"),
                    "Hurt"));
            animations["Hurt"].LoopAnimation = false;
            animations["Hurt"].NextAnimation = "Walk";
            animations["Hurt"].FrameLength = 1.0f;

            animations.Add(
                "Dead",
                new AnimationStrip(
                    texture,
                    content.Load<Rectangle>(@"SheetData\Enemies\Snake\SnakeDead"),
                    "Dead"));
            animations["Dead"].LoopAnimation = true;

            this.worldLocation = worldLocation;
            this.maxSbd = new Vector2(2f * 60.0f, 100.0f * 60.0f);
            this.acc = 50.0f * 60.0f;
            this.dec = 0;// 15.0f * 60.0f;
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
