using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Projectile : GameObject {

        private const float MOVE_SPEED = 4;
        private const int WIDTH = 5, HEIGHT = 2;
        private float xVelocity;

        private Texture2D projectileImg;

        public bool isPlayer, facingRight;

        private Map map;

        private Animator animator = new Animator(new Animation[]{
            new Animation(new int[]{ 0, 1 }, 8f),
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
        }

        public override void LoadContent() {
            projectileImg = Main.content.Load<Texture2D>(isPlayer ? "playerProjectile" : "enemyProjectile");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            position.X += xVelocity;

            if (map != null) {
                Rect ProjectileRect = new Rect((int)(position.X + xVelocity), (int)position.Y, WIDTH, HEIGHT);
                for (int y = 0; y < map.mapValues.GetUpperBound(0); y++)
                    for (int x = 0; x < map.mapValues.GetUpperBound(1); x++)
                        if (map.mapValues[y, x] > 0) {
                            Rect TileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                            if (ProjectileRect.Overlaps(TileRect))
                                Destroy();
                        }
            } else
                map = (Map)scene.FindGameObjectWithTag("Map");

            animator.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(projectileImg, position, new Rectangle(animator.animationFrame * WIDTH, 0, WIDTH, HEIGHT), Color.White, 0f, Vector2.Zero, 1f, (facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0f);
        }
    }
}
