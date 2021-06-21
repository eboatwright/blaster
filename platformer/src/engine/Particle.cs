using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Particle : GameObject {

        public const int WIDTH = 6, HEIGHT = 6;

        public Texture2D particleImg;

        public Vector2 velocity;
        public float gravity, friction;
        public float startLife, life;
        public ParticleData data;
        public ParticleData[] datas;

        public Particle(Scene scene, Vector2 position, Vector2 velocity, float gravity, float friction, float startLife, ParticleData[] datas) : base(scene) {
            this.position = position;
            this.velocity = velocity;
            this.gravity = gravity;
            this.friction = friction;
            this.startLife = this.life = startLife;
            this.datas = datas;
            this.data = datas[0];
            Initialize();
            LoadContent();
        }

        public override void Initialize() {}

        public override void LoadContent() {
            particleImg = Main.content.Load<Texture2D>("circleParticles");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            velocity.Y += gravity;
            velocity *= friction;
            position += velocity;

            life -= deltaTime;

            for (int i = 0; i < datas.Length; i++)
                if (life / startLife < datas[i].lifePercentage)
                    data = datas[i];

            if (life <= 0f)
                Destroy();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(particleImg, position - Main.camera.scroll, new Rectangle(data.spriteIndex * WIDTH, 0, WIDTH, HEIGHT), data.color);
        }
    }
}
