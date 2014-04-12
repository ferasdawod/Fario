using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tile_Engine;

namespace IMPORT_PLATFORM
{
    public class MovingGameObject : GameObject
    {
        protected Vector2 speed;
        protected Vector2 maxSpeed;

        protected float accelerationX;
        protected float deaccelerationX;

        protected Vector2 Gravity = new Vector2(0, 925.0f);

        protected bool useGravity;
        protected bool hasCollisions;

        protected bool limitSpeed;
        protected bool beingMoved;

        protected bool jumping = false;

        protected float preBottom;

        protected bool isPlayer = false;

        #region Properties

        public bool HasGravity
        {
            get { return useGravity; }
            set { useGravity = value; }
        }

        public bool HasCollisions
        {
            get { return hasCollisions; }
            set { hasCollisions = value; }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public bool BeingMoved
        {
            get { return beingMoved; }
            set { beingMoved = value; }
        }

        public Vector2 MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        public float AcclerationX
        {
            get { return accelerationX; }
            set { accelerationX = value; }
        }

        public float DeAccelerationX
        {
            get { return deaccelerationX; }
            set { deaccelerationX = value; }
        }

        #endregion

        #region Initialiazing Phisycs

        protected virtual void InitializePhisics(Vector2 maxSpeed, float accX, float decX, bool useGravity, bool checkForCollisions)
        {
            this.maxSpeed = maxSpeed;
            this.accelerationX = accX;
            this.deaccelerationX = decX;
            this.useGravity = useGravity;
            this.hasCollisions = checkForCollisions;
            this.beingMoved = false;
        }

        #endregion

        #region Updating and Handleing Collisions

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 prevLocation = worldLocation;

            if (useGravity) { speed.Y += Gravity.Y * elapsed; }

            Vector2 moveAmount;
            moveAmount.X = speed.X * elapsed;
            moveAmount.Y = speed.Y * elapsed;

            if (isPlayer)
            {
                moveAmount = horizontalCollisionTest(moveAmount);
                moveAmount = verticalCollisionTest(moveAmount);
            }
            if (moveAmount == Vector2.Zero)
            {
                worldLocation = prevLocation;
            }
            else
            {
                worldLocation.X += moveAmount.X;
                worldLocation.Y += moveAmount.Y;
            }

            HandleCollisions();

            if (limitSpeed)
            {
                if (speed.X > maxSpeed.X) speed.X = maxSpeed.X;
                if (speed.X < -maxSpeed.X) speed.X = -maxSpeed.X;
                if (speed.Y > maxSpeed.Y) speed.Y = maxSpeed.Y;
                if (speed.Y < -maxSpeed.Y) speed.Y = -maxSpeed.Y;
            }

            if (onGround && (speed.Y > 100))
            {
                onGround = false;
                jumping = true;
            }

            if (!beingMoved)
            {
                if (speed.X < 0) speed.X += deaccelerationX * elapsed;
                if (speed.X > 0) speed.X -= deaccelerationX * elapsed;

                if (speed.X > 0 && speed.X < (deaccelerationX * elapsed)) speed.X = 0;
                if (speed.X < 0 && speed.X > (-deaccelerationX * elapsed)) speed.X = 0;
            }

            if (hasCollisions)
            {
                if (worldLocation.X <= 0)
                {
                    worldLocation.X = 0;
                    speed.X = 0;
                }
                if (worldLocation.X >= (Camera.WorldRectangle.Width - FrameWidth))
                {
                    worldLocation.X = Camera.WorldRectangle.Width - FrameWidth;
                    speed.X = 0;
                }
            }

            base.Update(gameTime);
        }

        private void HandleCollisions()
        {
            Rectangle bounds = CollisionRectangle;
            int left = (int)((bounds.Left / TileMap.TileWidth)) * TileMap.TileWidth;
            int right = (int)((bounds.Right / TileMap.TileWidth)) * TileMap.TileWidth;

            int top = (int)((bounds.Top / TileMap.TileHeight)) * TileMap.TileHeight;
            int bottom = (int)((bounds.Bottom / TileMap.TileHeight)) * TileMap.TileHeight;

            for (int i = left; i <= right; i += TileMap.TileWidth)
            {
                for (int j = top; j <= bottom; j += TileMap.TileHeight)
                {
                    Rectangle tile = TileMap.CellWorldRectangle(i / TileMap.TileWidth, j / TileMap.TileHeight);

                    if (!TileMap.CellIsPassable(TileMap.GetCellByPixel(new Vector2(i, j))))
                    {
                        Rectangle tileRect = new Rectangle(i, j, TileMap.TileWidth, TileMap.TileHeight);
                        Vector2 depth = Extensions.GetIntersectionDepth(bounds, tileRect);

                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            if (absDepthY <= absDepthX)
                            {
                                if (preBottom <= tileRect.Top)
                                {
                                    onGround = true;
                                    jumping = false;
                                }
                                if (onGround)
                                {
                                    worldLocation = new Vector2(worldLocation.X, worldLocation.Y + depth.Y + 1);
                                    bounds = CollisionRectangle;
                                    speed.Y = 0;
                                }
                            }
                            else
                            {
                                worldLocation = new Vector2(worldLocation.X + depth.X, worldLocation.Y);
                                bounds = CollisionRectangle;
                                speed.X = 0;
                            }
                        }
                    }
                }
            }
            preBottom = bounds.Bottom;
        }

        private Vector2 horizontalCollisionTest(Vector2 moveAmount)
        {
            if (moveAmount.X == 0)
                return moveAmount;

            Rectangle afterMoveRect = CollisionRectangle;
            afterMoveRect.Offset((int)moveAmount.X, -5);
            Vector2 corner1, corner2;

            if (moveAmount.X < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left,
                                      afterMoveRect.Top + 1);
                corner2 = new Vector2(afterMoveRect.Left,
                                      afterMoveRect.Bottom - 1);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Right,
                                      afterMoveRect.Top + 1);
                corner2 = new Vector2(afterMoveRect.Right,
                                      afterMoveRect.Bottom - 1);
            }

            Vector2 mapCell1 = TileMap.GetCellByPixel(corner1);
            Vector2 mapCell2 = TileMap.GetCellByPixel(corner2);

            if (!TileMap.CellIsPassable(mapCell1) ||
                !TileMap.CellIsPassable(mapCell2))
            {
                moveAmount.X = 0;
                speed.X = 0;
            }

            return moveAmount;
        }

        private Vector2 verticalCollisionTest(Vector2 moveAmount)
        {
            if (moveAmount.Y == 0)
                return moveAmount;

            Rectangle afterMoveRect = CollisionRectangle;
            afterMoveRect.Offset((int)moveAmount.X, (int)moveAmount.Y + 1);
            Vector2 corner1, corner2;

            if (moveAmount.Y < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left + 1,
                                      afterMoveRect.Top);
                corner2 = new Vector2(afterMoveRect.Right - 1,
                                      afterMoveRect.Top);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Left + 1,
                                      afterMoveRect.Bottom);
                corner2 = new Vector2(afterMoveRect.Right - 1,
                                      afterMoveRect.Bottom);
            }

            Vector2 mapCell1 = TileMap.GetCellByPixel(corner1);
            Vector2 mapCell2 = TileMap.GetCellByPixel(corner2);

            if (!TileMap.CellIsPassable(mapCell1) ||
                !TileMap.CellIsPassable(mapCell2))
            {
                if (moveAmount.Y > 0)
                {
                    onGround = true;
                }
                moveAmount.Y = 0;
                speed.Y = 0;
            }

            return moveAmount;
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
