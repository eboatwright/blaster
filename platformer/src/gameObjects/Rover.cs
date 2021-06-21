using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Rover : GameObject, IDamageable {


        enum ANIMATION_STATES {
            IDLE = 0,
            WALK = 1,
            SHOOT = 2
        }


        public const int SPRITE_WIDTH = 18, SPRITE_HEIGHT = 16;

        private Texture2D roverImg;

        private Animator animator;
        private bool flipSprite;


        public int Health { get; set; }

        public void Damage() {
            Health--;
            if (Health <= 0)
                Destroy();
        }


        public Rover(Scene scene) : base(scene) {
            position = new Vector2(0, 64);
        }

        public override void Initialize() {
            animator = new Animator(new Animation[]{
                new Animation(new int[] { 0 }, 1f),
                new Animation(new int[] { 1, 0 }, 10f),
                new Animation(new int[] { 2 }, 8f)
            });
            animator.ChangeAnimation((int)ANIMATION_STATES.WALK, false);
            AddTags(new string[]{ "Rover" });
            Health = 3;
        }

        public override void LoadContent() {
            roverImg = Main.content.Load<Texture2D>("enemyRover");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            animator.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(roverImg, position - Main.camera.scroll, new Rectangle(animator.animationFrame * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT), Color.White, 0f, Vector2.Zero, 1f, (flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
        }
    }
}
