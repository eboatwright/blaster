using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Camera : GameObject {

        public Vector2 scroll;
        public Vector2 offset = new Vector2(1792, 1200);
        private Player player;

        public Camera(Scene scene) : base(scene) {
            scroll = new Vector2(0f, 0f);
        }

        public override void Initialize() {
            AddTags(new string[]{ "Camera" });
        }

        public override void LoadContent(ContentManager content) {}

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if (player == null) {
                player = (Player)scene.FindGameObjectWithTag("Player");
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {}
    }
}
