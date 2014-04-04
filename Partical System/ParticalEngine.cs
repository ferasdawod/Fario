using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Partical_System
{
    public class ParticalEngine
    {
        #region Declerations

        private Random _rnd;
        private Vector2 _emmiterLocation;
        private List<Partical> _particales;
        private List<Texture2D> _textures;

        public enum ParticalEffect { None, OneTime, Contenuis };
        ParticalEffect _effect = ParticalEffect.None;

        int particalsCount;
        float baseTTL;
        float baseVelocityX;
        float baseVelocityY;

        float baseAngularVelocity;

        float baseSize;

        #endregion

        #region Properties

        public int ParticalsCount
        {
            get { return particalsCount; }
            set { particalsCount = value; }
        }

        public float BaseTTL
        {
            get { return baseTTL; }
            set { baseTTL = value; }
        }

        public float BaseVelocityX
        {
            get { return baseVelocityX; }
            set { baseVelocityX = value; }
        }

        public float BaseVelocityY
        {
            get { return baseVelocityY; }
            set { baseVelocityY = value; }
        }

        public float BaseAngularVelocity
        {
            get { return baseAngularVelocity; }
            set { baseAngularVelocity = value; }
        }

        public float BaseSize
        {
            get { return baseSize; }
            set { baseSize = value; }
        }

        public Vector2 EmitterLocation
        {
            get { return _emmiterLocation; }
            set { _emmiterLocation = value; }
        }

        public ParticalEffect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        #endregion

        #region Constructor

        public ParticalEngine(
            List<Texture2D> textures
            )
        {
            _emmiterLocation = Vector2.Zero;
            this._textures = textures;
            this._particales = new List<Partical>();
            _rnd = new Random();
            this._effect = ParticalEffect.None;
            this.particalsCount = 30;
            this.baseVelocityX = 10f;
            this.baseVelocityY = 10f;
            this.baseTTL = 1f;
            this.baseAngularVelocity = 0.1f;
            this.baseSize = 0.8f;
        }

        public ParticalEngine(List<Texture2D> textures, int particalsCount, float baseTTL, float baseSize)
        {
            _emmiterLocation = Vector2.Zero;
            this._textures = textures;
            this._particales = new List<Partical>();
            _rnd = new Random();
            this._effect = ParticalEffect.None;
            this.particalsCount = particalsCount;
            this.baseVelocityX = 10f;
            this.baseVelocityY = 10f;
            this.baseTTL = baseTTL;
            this.baseAngularVelocity = 1f;
            this.baseSize = baseSize;
        }

        #endregion

        #region Helper Methods

        private Partical GenerateNewPartical(Color cColor)
        {
            Texture2D texture = _textures[_rnd.Next(_textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                baseVelocityX * (float)(_rnd.NextDouble() * 2 - 1) * 4,
                baseVelocityY * (float)(_rnd.NextDouble() * 2 - 1) * 4);
            float angle = 0;
            float angularVelocity = baseAngularVelocity * (float)(_rnd.NextDouble() * 2 - 1) * 5;
            Color color;
            if (cColor == null)
            {
                color = new Color(
                    (float)_rnd.NextDouble(),
                    (float)_rnd.NextDouble(),
                    (float)_rnd.NextDouble());
            }
            else
            {
                //color = cColor;
                color = new Color(
                    (float)_rnd.NextDouble(),
                    (float)_rnd.NextDouble(),
                    (float)_rnd.NextDouble());
            }
            float size = baseSize * (float)_rnd.NextDouble();
            float ttl = baseTTL + (float)_rnd.NextDouble();

            return new Partical(texture, position, velocity, angle,
                angularVelocity, color, size, ttl);
        }

        public void GenerateEffect(Vector2 location, Color cColor)
        {
            _emmiterLocation = location;
            for (int i = 0; i <= particalsCount; i++)
            {
                _particales.Add(GenerateNewPartical(cColor));
            }
        }

        public void EmitParticals(Vector2 location)
        {
        }

        #endregion

        #region Update And Draw

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _particales.Count; i++)
            {
                _particales[i].Update(gameTime);
                if (_particales[i].TTL <= 0)
                {
                    _particales.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _particales.Count; i++)
            {
                _particales[i].Draw(spriteBatch);
            }
        }

        #endregion
    }
}
