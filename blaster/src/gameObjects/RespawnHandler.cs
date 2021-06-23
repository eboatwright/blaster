using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class RespawnHandler : GameObject {

        private static bool respawn;
        public float delay = 60f;
        private static SoundEffect gameOverSfx;

        public RespawnHandler(Scene scene) : base(scene) {}

        public override void Initialize() {}

        public override void LoadContent() {
            gameOverSfx = Main.content.Load<SoundEffect>("sfx/gameOver2");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if(respawn) {
                if(delay <= 0f) {
                    respawn = false;
                    Main.currentScene = new GameScene();
                    Main.currentScene.Initialize();
                    Main.currentScene.LoadContent();
                }
                delay -= deltaTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {}

        public static void Respawn() {
            respawn = true;
            gameOverSfx.Play();
        }
    }
}
