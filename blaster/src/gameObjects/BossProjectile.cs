using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class BossProjectile : GameObject {

        private const float MOVE_SPEED = 3.5f;
        private const int WIDTH = 23, HEIGHT = 10;
        private float xVelocity;

        private Texture2D projectileImg;

        private bool facingRight, hitPlayer;

        private Player player;
        private Animator animator;

        public BossProjectile(Scene scene, bool facingRight, Vector2 position) : base(scene) {
            this.facingRight = facingRight;
            this.position = position;
            Initialize();
            LoadContent();
        }

        public override void Initialize() {
            xVelocity = MOVE_SPEED * (facingRight ? 1f : -1f);
            player = (Player)scene.FindGameObjectWithTag("Player");
            animator = new Animator(new Animation[]{
                new Animation(new int[]{ 0, 1 }, 5f),
            });
        }

        public override void LoadContent() {
            projectileImg = Main.content.Load<Texture2D>("boss/bossProjectile");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            position.X += xVelocity;

            Rect projectileRect = new Rect((int)(position.X + xVelocity), (int)position.Y, WIDTH, HEIGHT);

            for (int y = 0; y < Map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x < Map.mapValues.GetUpperBound(1); x++)
                    if (Map.mapValues[y, x] > 0 && Map.mapValues[y, x] < 14) {
                        Rect tileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (projectileRect.Overlaps(tileRect))
                            Destroy();
                    }


            if(player != null) {
                Rect playerRect = new Rect(player.position, Player.SPRITE_WIDTH, Player.SPRITE_HEIGHT);
                if (projectileRect.Overlaps(playerRect) && !hitPlayer) {
                    hitPlayer = true;
                    player.Damage();
                }
            }

            animator.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(projectileImg, position - Main.camera.scroll, new Rectangle(animator.animationFrame * WIDTH, 0, WIDTH, HEIGHT), Color.White, 0f, Vector2.Zero, 1f, (facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0f);
        }
    }
}
