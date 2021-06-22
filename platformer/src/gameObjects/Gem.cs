using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Gem : GameObject {

        public const int SPRITE_WIDTH = 5, SPRITE_HEIGHT = 4;

        private Player player;

        private Texture2D gemImg;
        private SoundEffect pickupSfx;

        public Gem(Scene scene, Vector2 position) : base(scene) {
            this.position = position;

            Initialize();
            LoadContent();
        }

        public override void Initialize() {
            player = (Player)scene.FindGameObjectWithTag("Player");
        }

        public override void LoadContent() {
            gemImg = Main.content.Load<Texture2D>("gem");
            pickupSfx = Main.content.Load<SoundEffect>("sfx/gemPickup");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            Rect gemRect = new Rect(position, SPRITE_WIDTH, SPRITE_HEIGHT);
            Rect playerRect = new Rect(player.position + new Vector2(4, 4), 8, 12);

            if(gemRect.Overlaps(playerRect)) {
                ScoreCounter.score += 50;
                pickupSfx.Play();

                for (int i = 0; i < 2; i++) {
                    Vector2 particlePosition = position + new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2) + new Vector2((float)Main.random.NextDouble() * 10f - 5f, (float)Main.random.NextDouble() * 10f - 5f);
                    Vector2 particleVelocity = new Vector2((float)Main.random.NextDouble() * 3 - 1.5f, (float)Main.random.NextDouble() * 3 - 2.5f);
                    scene.AddGameObject(new Particle(scene, particlePosition, particleVelocity, 0.05f, 0.9f, 16f, new ParticleData[] { new ParticleData(new Color(255, 255, 255), 1f, 1), new ParticleData(new Color(255, 255, 255), 0.5f, 2) }));
                }

                Destroy();
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(gemImg, position - Main.camera.scroll, Color.White);
        }
    }
}
