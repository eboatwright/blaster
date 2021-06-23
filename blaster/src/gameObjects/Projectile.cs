using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Projectile : GameObject {

        private const float MOVE_SPEED = 3.5f;
        private const int WIDTH = 5, HEIGHT = 2;
        private float xVelocity;

        private Texture2D projectileImg;

        public bool isPlayer, facingRight;

        private Player player;
        private Boss boss;

        private Animator animator = new Animator(new Animation[]{
            new Animation(new int[]{ 0, 1 }, 5f),
        });

        public Projectile(Scene scene, bool isPlayer, bool facingRight, Vector2 position) : base(scene) {
            this.isPlayer = isPlayer;
            this.facingRight = facingRight;
            this.position = position;
            Initialize();
            LoadContent();
        }

        public override void Initialize() {
            xVelocity = MOVE_SPEED * (facingRight ? 1f : -1f);
            player = (Player)scene.FindGameObjectWithTag("Player");
        }

        public override void LoadContent() {
            projectileImg = Main.content.Load<Texture2D>(isPlayer ? "playerProjectile" : "enemyProjectile");
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
            

            if(isPlayer) {
                foreach (Rover rover in scene.FindGameObjectsWithTag("Rover")) {
                    Rect roverRect = new Rect(rover.position, Rover.SPRITE_WIDTH, Rover.SPRITE_HEIGHT);
                    if (projectileRect.Overlaps(roverRect)) {
                        rover.Damage();
                        Destroy();
                    }
                }
                if (boss != null) {
                    Rect bossRect = new Rect(boss.position, Boss.SPRITE_WIDTH, Boss.SPRITE_HEIGHT);
                    if(projectileRect.Overlaps(bossRect)) {
                        boss.Damage();
                        Destroy();
                    }
                } else
                    boss = (Boss)scene.FindGameObjectWithTag("Boss");
            } else if(player != null) {
                Rect playerRect = new Rect(player.position, Player.SPRITE_WIDTH, Player.SPRITE_HEIGHT);
                if(projectileRect.Overlaps(playerRect)) {
                    player.Damage();
                    Destroy();
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
