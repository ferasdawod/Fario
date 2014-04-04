using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Tile_Engine;

using Platformer.Debugging;

namespace IMPORT_PLATFORM
{
    public class Player : MovingGameObject
    {
        #region Decleration

        private bool dead = false;
        private int score = 0;
        private Texture2D texture;

        private int livesRemaining;
        private int deathsRemaining;
        private int fullHealth = 3;
        private bool canBeDameged = true;
        private bool canTakeDamage = true;
        private float damegeDelay;
        private float timeSinceDamage = 0;

        private float jumpSpeed = 635.0f;
        private Vector2 hurtSpeed = new Vector2(0, -500);

        private bool hasInput;

        private Vector2 lastPosition;

        private FarioMain mainGame;

        #region Input Declerations

        private Keys jumpKey0 = Keys.Space;
        private Keys jumpKey1 = Keys.W;
        private Keys jumpKey2 = Keys.Up;

        private Keys leftKey0 = Keys.Left;
        private Keys leftKey1 = Keys.A;

        private Keys rightKey0 = Keys.Right;
        private Keys rightKey1 = Keys.D;

        private Keys downKey0 = Keys.S;
        private Keys downKey1 = Keys.Down;

        private Keys finishKey = Keys.Enter;

        #endregion

        #endregion

        #region Properties

        public bool HasInput
        {
            get { return hasInput; }
            set { hasInput = value; }
        }

        public bool CanBeDamaged
        {
            get { return canBeDameged; }
            set { canBeDameged = value; }
        }

        public bool CanTakeDamage
        {
            get { return canTakeDamage; }
            set { canTakeDamage = value; }
        }

        public int LivesRemaining
        {
            get { return livesRemaining; }
            set { livesRemaining = value; }
        }

        public int DeathsRemaining
        {
            get { return deathsRemaining; }
            set { deathsRemaining = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        #endregion

        #region Constructor

        public Player(FarioMain parentGame, int playerNumber, NumbersHelper helper)
        {
            this.mainGame = parentGame;
            Texture = parentGame.Content.Load<Texture2D>(@"Textures\Player");
            livesRemaining = fullHealth;
            deathsRemaining = fullHealth;

            this.xStartOffset = helper.PlayersXStartOffset[playerNumber];
            this.yStartOffset = helper.PlayersYStartOffset[playerNumber];
            this.xEndOffset = helper.PlayersXEndOffset[playerNumber];
            this.yEndOffset = helper.PlayersYEndOffset[playerNumber];

            #region Animations
            animations.Add(
                "Stand",
                new AnimationStrip(
                    texture,
                    parentGame.Content.Load<Rectangle[]>(@"SheetData\Player\Alien" + playerNumber + "/Stand"),
                    "Stand"));
            animations["Stand"].LoopAnimation = true;

            animations.Add(
                "Walk",
                new AnimationStrip(
                    texture,
                    parentGame.Content.Load<Rectangle[]>(@"SheetData\Player\Alien" + playerNumber + "/Walk"),
                    "Walk"));
            animations["Walk"].LoopAnimation = true;
            animations["Walk"].FrameLength = 0.15f;

            //Jumping Animation
            animations.Add(
                "Jump",
                new AnimationStrip(
                    texture,
                    parentGame.Content.Load<Rectangle[]>(@"SheetData\Player\Alien" + playerNumber + "/Jump"),
                    "Jump"));
            animations["Jump"].LoopAnimation = true;

            //Getting Hurt Animation
            animations.Add(
                "Hurt",
                new AnimationStrip(
                    texture,
                    parentGame.Content.Load<Rectangle[]>(@"SheetData\Player\Alien" + playerNumber + "/Hurt"),
                    "Hurt"));
            animations["Hurt"].LoopAnimation = true;
            #endregion

            drawDepth = 0.9f;
            enabled = true;
            this.damegeDelay = helper.PlayerDamageDelay;
            PlayAnimation("Stand");
            this.isPlayer = true;

            Vector2 max = new Vector2(300.0f, 3000.0f);
            float acc = 10f * 60.0f;
            float dec = 15f * 60.0f;
            bool useGravity = true;
            bool checkCollisions = true;

            this.InitializePhisics(max, acc, dec, useGravity, checkCollisions);

            this.limitSpeed = true;
            this.hasInput = true;
        }

        #endregion

        #region Update And Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Dead)
            {
                base.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.beingMoved = false;

            if (canBeDameged)
            {
                if (!canTakeDamage)
                {
                    timeSinceDamage += (float)gameTime.ElapsedGameTime.Milliseconds;
                    if (timeSinceDamage >= damegeDelay)
                    {
                        canTakeDamage = true;
                        timeSinceDamage = 0;
                    }
                }
            }
            if (hasInput)
            {
                string newAnimation = "Stand";

                if (Input.KeyDown(leftKey0) ||
                    Input.KeyDown(leftKey1))
                {
                    speed.X -= accelerationX * elapsed;
                    beingMoved = true;
                    flipped = true;
                    newAnimation = "Walk";
                }
                if (Input.KeyDown(rightKey0) ||
                    Input.KeyDown(rightKey1))
                {
                    speed.X += accelerationX * elapsed;
                    beingMoved = true;
                    flipped = false;
                    newAnimation = "Walk";
                }
                if ((Input.KeyPressed(jumpKey0) ||
                    Input.KeyPressed(jumpKey1) ||
                    Input.KeyPressed(jumpKey2)))
                {
                    Jump();
                    newAnimation = "Jump";
                }
                if ((Input.KeyDown(downKey0) ||
                    Input.KeyDown(downKey1)))
                {
                    speed.Y += Gravity.Y * 3 * elapsed;
                }
                HandelAnimations(newAnimation);

            }

            base.Update(gameTime);

            if (hasInput)
            {
                repositionCamera();
            }
            lastPosition = worldLocation;
        }

        private void HandelAnimations(string newAnimation)
        {
            if (currentAnimation == "Hurt")
                newAnimation = "Hurt";
            if (currentAnimation == "Jump")
                newAnimation = "Jump";

            if ((currentAnimation == "Stand" || currentAnimation == "Walk"))
            {
                if (!OnGround)
                {
                   newAnimation = "Jump";
                }
            }
            if (currentAnimation == "Jump" || (currentAnimation == "Hurt"))
            {
                if (onGround)
                {
                    newAnimation = "Stand";
                }
            }

            if (newAnimation != currentAnimation)
            {
                PlayAnimation(newAnimation);
            }
        }

        #endregion

        public void Jump()
        {
            if (OnGround)
            {
                speed.Y = -jumpSpeed;
                onGround = false;
                jumping = true;
                MusicManager.Instance.PlayEffect(SFXType.Jump);
            }
        }

        private void repositionCamera()
        {
            /*int screenLocX = (int)Camera.WorldToScreen(WorldLocation).X;
            if (screenLocX > 500)
            {
                Camera.Move(new Vector2(screenLocX - 500, 0));
            }
            if (screenLocX < 200)
            {
                Camera.Move(new Vector2(screenLocX - 200, 0));
            }
            int screenLocY = (int)Camera.WorldToScreen(WorldLocation).Y;
            if (screenLocY > 300)
            {
                Camera.Move(new Vector2(0, screenLocY - 300));
            }
            if (screenLocY < 100)
            {
                Camera.Move(new Vector2(0, screenLocY - 100));
            }*/
            float y;
            if (currentAnimation == "Walk")
            {
                y = Camera.Position.Y;
            }
            else
            {
                y = worldLocation.Y - (mainGame.Window.ClientBounds.Height / 2);
            }
            Camera.Position = new Vector2(
                worldLocation.X - (mainGame.Window.ClientBounds.Width / 2),
                y);
        }

        public void Reset()
        {
            this.livesRemaining = fullHealth;
            this.deathsRemaining = fullHealth;
            this.score = 0;
            this.dead = false;
            this.enabled = true;
        }

        #region Taking Damage And Getting Killed

        public void GetHurt(int scoreValue)
        {
            if (CanBeDamaged)
            {
                if (canTakeDamage)
                {
                    PlayAnimation("Hurt");
                    speed.X = hurtSpeed.X;
                    speed.Y = hurtSpeed.Y;
                    score -= scoreValue;
                    livesRemaining--;
                    if (livesRemaining <= 0)
                    {
                        LoseLife();
                    }
                    canTakeDamage = false;
                    onGround = false;
                }
            }
        }

        public void LoseLife()
        {
            if (canTakeDamage)
            {
                this.deathsRemaining--;
                livesRemaining = 3;
                canTakeDamage = false;
                if (deathsRemaining <= 0)
                {
                    MusicManager.Instance.StopLoop();
                    MusicManager.Instance.PlayEffect(SFXType.GameOver);
                    mainGame.SaveHighScore();
                    Kill();
                    mainGame.SetState(GameState.GameOver);
                }
                else
                {
                    mainGame.InGameScreen.InGameState = InGameState.Died;
                    speed = Vector2.Zero;
                    PlayAnimation("Stand");
                    score = mainGame.InGameScreen.LevelMANAGER.StartingScore;
                }
            }
        }

        public void Kill()
        {
            this.dead = true;
        }

        #endregion
    }
}
