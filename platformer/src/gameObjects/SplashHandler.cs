using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class SplashHandler : GameObject {

        public Texture2D splashImg;
        private float yPosition;
        private float sinWaveTimer;
        public float timer = 80f;
        private bool xReleased;

        public SplashHandler(Scene scene) : base(scene) {}

        public override void Initialize() {}

        public override void LoadContent() {
            splashImg = Main.content.Load<Texture2D>("splash");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            timer -= deltaTime;
            if (keyboard.IsKeyDown(Keys.X)) {
                if (xReleased) {
                    xReleased = false;
                    LoadScene();
                }
            } else
                xReleased = true;

            if (timer <= 0f)
                LoadScene();

            sinWaveTimer += deltaTime;
            yPosition = (float)Math.Sin(sinWaveTimer / 20f) * 6f;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(splashImg, new Vector2(0f, yPosition), Color.White);
        }

        public void LoadScene() {
            Main.currentScene = new MenuScene();
            Main.currentScene.Initialize();
            Main.currentScene.LoadContent();
        }
    }
}
