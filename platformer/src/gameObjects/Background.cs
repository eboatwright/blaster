using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Background : GameObject {

        public const float parallaxAmount = 3.72f;
        public const int WIDTH = 240;

        public float offset = 0f;

        private Texture2D backgroundImg;

        public Background(Scene scene, Vector2 position) : base(scene) {
            this.position = position;
        }

        public override void Initialize() {}

        public override void LoadContent() {
            backgroundImg = Main.content.Load<Texture2D>("background");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if (Main.camera == null) return;
            Vector2 parallaxScroll = Main.camera.smoothedScroll / parallaxAmount;
            if (parallaxScroll.X + offset < WIDTH)
                offset += WIDTH;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(backgroundImg, position - (Main.camera.smoothedScroll / parallaxAmount) + new Vector2(offset, 0f), Color.White);
        }
    }
}
