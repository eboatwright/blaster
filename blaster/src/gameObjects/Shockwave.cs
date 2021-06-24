using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Shockwave : GameObject {

        public const int WIDTH = 32, HEIGHT = 32;

        public float timer = 16f;
        private Texture2D shockwaveImg;

        private Animator animator;
        private Boss boss;
        private Player player;

        public Shockwave(Scene scene) : base(scene) {
            Initialize();
            LoadContent();
        }

        public override void Initialize() {
            Main.content.Load<SoundEffect>("boss/shockwave").Play();
            animator = new Animator(new Animation[]{
                new Animation(new int[]{ 0, 1, 2 }, 4.6f),
            });
    }

        public override void LoadContent() {
            shockwaveImg = Main.content.Load<Texture2D>("boss/shockwaveImg");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            boss = (Boss)scene.FindGameObjectWithTag("Boss");
            if (boss == null) {
                Destroy();
                return;
            }
            position = scene.FindGameObjectWithTag("Boss").position;

            player = (Player)scene.FindGameObjectWithTag("Player");
            if(player != null) {
                Rect playerRect = new Rect(player.position, Player.SPRITE_WIDTH, Player.SPRITE_HEIGHT);
                Rect shockwaveRect = new Rect(position, WIDTH, HEIGHT);
                if(shockwaveRect.Overlaps(playerRect)) {
                    if(player.position.X < position.X)
                        player.velocity = new Vector2(-13f, -6f);
                    else
                        player.velocity = new Vector2(13f, -6f);
                }
            }

            timer -= deltaTime;
            if(timer <= 0f)
                Destroy();
            animator.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(shockwaveImg, position - Main.camera.scroll, new Rectangle(animator.animationFrame * WIDTH, 0, WIDTH, HEIGHT), Color.White);
        }
    }
}
