using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class EndingHandler : GameObject {

        public static bool fading, showEndScreen;
        private bool doneInitialFade;

        private Player player;

        private Texture2D blackScreenImg, endImg;

        private float transparency;

        public EndingHandler(Scene scene) : base(scene) {}

        public override void Initialize() {}

        public override void LoadContent() {
            blackScreenImg = Main.content.Load<Texture2D>("blackScreen");
            endImg = Main.content.Load<Texture2D>("end");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if (player == null) {
                player = (Player)scene.FindGameObjectWithTag("Player");
                return;
            }
            if (player.position.X > 240 * 12 && !doneInitialFade) {
                fading = true;
                doneInitialFade = true;
            }

            if (transparency >= 0.92f) {
                fading = false;
                showEndScreen = true;
            }

            if (fading)
                transparency = MathHelper.Lerp(transparency, 1f, 0.05f * deltaTime);
            else
                transparency = MathHelper.Lerp(transparency, 0f, 0.05f * deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(blackScreenImg, Vector2.Zero, new Color(0f, 0f, 0f, transparency));
            if(showEndScreen) {
                spriteBatch.Draw(endImg, Vector2.Zero, Color.White);
            }
        }
    }
}
