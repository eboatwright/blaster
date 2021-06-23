using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class ScoreCounter : GameObject {

        public static int score;

        public ScoreCounter(Scene scene) : base(scene) {}

        public override void Initialize() {
            score = 0;
        }

        public override void LoadContent() {}

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {}

        public override void Draw(SpriteBatch spriteBatch) {
            int digits = 3;
            if (score > 9) digits--;
            if (score > 99) digits--;
            if (score > 999) digits--;

            string scoreText = "";
            for (int i = 0; i < digits; i++) scoreText += "0";
            spriteBatch.DrawString(Main.font, "SCORE: " + scoreText + score, new Vector2(175, -1), new Color(121, 169, 189));
        }
    }
}
