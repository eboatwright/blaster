using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Camera : GameObject {

        public Vector2 scroll;
        public Vector2 mainScroll;
        public Vector2 offset = new Vector2(1792, 1200);
        public static float intensity;
        public float shakeTime = 0.2f;

        public Camera(Scene scene) : base(scene) {
            scroll = mainScroll = Vector2.Zero;
            intensity = 0f;
        }

        public override void Initialize() {
            AddTags(new string[]{ "Camera" });
        }

        public override void LoadContent() {}

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            scroll = mainScroll + new Vector2((float)Main.random.NextDouble() * (intensity * 2) - intensity, (float)Main.random.NextDouble() * (intensity * 2) - intensity);

            if (intensity > 0f)
                intensity -= shakeTime * deltaTime;
            else intensity = 0f;
        }

        public override void Draw(SpriteBatch spriteBatch) {}

        public static void Shake(float intensity) {
            Camera.intensity += intensity;
        }
    }
}
