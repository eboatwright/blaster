using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Projectile : GameObject {

        private const float MOVE_SPEED = 6;
        private const int WIDTH = 5, HEIGHT = 2;

        private Texture2D projectileImg;

        public bool isPlayer, facingRight;

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

        public override void Initialize() {}

        public override void LoadContent() {
            projectileImg = Main.content.Load<Texture2D>(isPlayer ? "playerProjectile" : "enemyProjectile");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            position.X += MOVE_SPEED * (facingRight ? 1f : -1f);
            animator.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(projectileImg, position, new Rectangle(animator.animationFrame * WIDTH, 0, WIDTH, HEIGHT), Color.White, 0f, Vector2.Zero, 1f, (facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally), 0f);
        }
    }
}
