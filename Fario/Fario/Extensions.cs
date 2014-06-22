using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IMPORT_PLATFORM
{
    public enum TextLocation { Center = 0, TopMid = 1, BotMid = 2, LeftMid = 3, RightMid = 4, BotLeft = 5, BotRight = 6, TopRight = 7, TopLeft = 8 }

    public static class Extensions
    {

        /// <summary>
        /// Calculates the signed depth of intersection between two rectangles.
        /// </summary>
        /// <returns>
        /// The amount of overlap between two intersecting rectangles. These
        /// depth values can be negative depending on which wides the rectangles
        /// intersect. This allows callers to determine the correct direction
        /// to push objects in order to resolve collisions.
        /// If the rectangles are not intersecting, Vector2.Zero is returned.
        /// </returns>
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        /// <summary>
        /// Gets the position of the center of the bottom edge of the rectangle.
        /// </summary>
        public static Vector2 GetBottomCenter(this Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width / 2.0f, rect.Bottom);
        }

        /// <summary>
        /// Use To Allign Stuff On The Screen Whitout Having To Worry About Screen Size
        /// </summary>
        /// <param name="size">The Size Of The Object You Are Drawing As a Vector2</param>
        /// <param name="whereToAllign">The Area Of The Screen You Wish To Put The Object In</param>
        /// <param name="area">The Size Of The Area You Are Drawing In</param>
        /// <returns>Returns A Vector2 Which Can Be Used To Draw The Object In The Desired Location On The Screen</returns>
        public static Vector2 AllignThing(this Vector2 size, TextLocation whereToAllign, Rectangle area)
        {
            Vector2 loc = Vector2.Zero;

            switch (whereToAllign)
            {
                case TextLocation.Center:
                    loc = new Vector2(
                        (area.Width / 2) - (size.X / 2),
                        (area.Height / 2) - (size.Y / 2));
                    break;
                case TextLocation.TopMid:
                    loc = new Vector2(
                        (area.Width / 2) - (size.X / 2),
                        0);
                    break;
                case TextLocation.BotMid:
                    loc = new Vector2(
                        (area.Width / 2) - (size.X / 2),
                        area.Height - size.Y);
                    break;
                case TextLocation.LeftMid:
                    loc = new Vector2(
                        0,
                        (area.Height / 2) - (size.Y / 2));
                    break;
                case TextLocation.RightMid:
                    loc = new Vector2(
                        area.Width - size.X,
                        (area.Height / 2) - (size.Y / 2));
                    break;
                case TextLocation.BotLeft:
                    loc = new Vector2(
                        0,
                        area.Height - size.Y);
                    break;
                case TextLocation.BotRight:
                    loc = new Vector2(
                        area.Width - size.X,
                        area.Height - size.Y);
                    break;
                case TextLocation.TopRight:
                    loc = new Vector2(
                        area.Width - size.X,
                        0);
                    break;
                case TextLocation.TopLeft:
                    loc = Vector2.Zero;
                    break;
                default:
                    break;
            }

            return loc;
        }

    }
}
