using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class PlayerHealthBar : GameObject {

        public const int SPRITE_WIDTH = 7, SPRITE_HEIGHT = 6;

        private Texture2D heartImg;

        public int health;

        public PlayerHealthBar(Scene scene) : base(scene) {}

        public override void Initialize() {}

        public override void LoadContent() {
            heartImg = Main.content.Load<Texture2D>("heart");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {}

        public override void Draw(SpriteBatch spriteBatch) {
            if (EndingHandler.fading || EndingHandler.showEndScreen) return;
            for(int i = 0; i < 4; i++)
                spriteBatch.Draw(heartImg, new Vector2(5 + (i * (SPRITE_WIDTH + 1)), 5), new Rectangle((i < health ? 0 : SPRITE_WIDTH), 0, SPRITE_WIDTH, SPRITE_HEIGHT), Color.White);
        }
    }
}
