using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Player : GameObject {

        public const int SPRITE_WIDTH = 12, SPRITE_HEIGHT = 13;
        public const float MOVE_SPEED = 1f, FRICTION = 0.7f, GRAVITY = 0.4f, JUMP_HEIGHT = -7.2f;

        private Vector2 velocity;

        private Texture2D playerImg;

        private Camera camera;

        public Player(Scene scene) : base(scene) {}

        public override void Initialize() {
            tags.Add("Player");
        }

        public override void LoadContent(ContentManager content) {
            playerImg = content.Load<Texture2D>("player");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) velocity.X -= MOVE_SPEED * deltaTime;
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) velocity.X += MOVE_SPEED * deltaTime;

            velocity.X *= FRICTION;
            position.X += velocity.X;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (camera == null) {
                camera = (Camera)scene.FindGameObjectWithTag("Camera");
                return;
            }
            spriteBatch.Draw(playerImg, position - camera.scroll, Color.White);
        }

        public Rectangle GetHitbox() {
            return new Rectangle((int)position.X, (int)position.Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        }

        public bool RectanglesOverlap(Rectangle a, Rectangle b) {
            return a.Left < b.Right &&
                    a.Right > b.Left &&
                    a.Top < b.Bottom &&
                    a.Bottom > b.Top;
        }
    }
}
