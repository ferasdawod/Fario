using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IMPORT_PLATFORM
{
    public static class Input
    {
        #region Declerations

        private static KeyboardState currentState;
        private static KeyboardState prevState;

        private static bool result;

        #endregion

        /// <summary>
        /// Updates The Current Input State
        /// </summary>
        public static void Update()
        {
            prevState = currentState;

            currentState = Keyboard.GetState();
        }

        #region Helper Methods

        /// <summary>
        /// Returns True If The Key Provided Was Down In The Current Frame
        /// </summary>
        /// <param name="key">The Key To Check</param>
        /// <returns>True If Im Feras</returns>
        public static bool KeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns True If The Key Provided Was Pressed And Released
        /// </summary>
        /// <param name="key">The Key To Check</param>
        /// <returns>False Dude</returns>
        public static bool KeyPressed(Keys key)
        {
            result = (prevState.IsKeyUp(key) && currentState.IsKeyDown(key));
            if (result)
            {
                if (key != Keys.Up && key != Keys.W && key != Keys.Space)
                {
                    MusicManager.Instance.PlayEffect(SFXType.ButtenPress);
                }
                return result;
            }
            else
            {
                return result;
            }
        }
        
        #endregion
    }
}
