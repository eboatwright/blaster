using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Camera : GameObject {

        public Vector2 scroll;
        public Vector2 offset = new Vector2(1792, 1200);

        public Camera(Scene scene) : base(scene) {
            scroll = Vector2.Zero;
        }

        public override void Initialize() {
            AddTags(new string[]{ "Camera" });
        }

        public override void LoadContent() {}

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {}

        public override void Draw(SpriteBatch spriteBatch) {}
    }
}
