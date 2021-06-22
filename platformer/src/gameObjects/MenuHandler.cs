using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class MenuHandler : GameObject {

        public Texture2D menuImg;

        private float yPosition;

        private float sinWaveTimer, startTimer = 46f;

        private bool xReleased, start;

        private SoundEffect startSfx;

        public MenuHandler(Scene scene) : base(scene) {}

        public override void Initialize() {}

        public override void LoadContent() {
            menuImg = Main.content.Load<Texture2D>("menu");
            startSfx = Main.content.Load<SoundEffect>("sfx/startGame");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if(!start) {
                if (keyboard.IsKeyDown(Keys.X)) {
                    if (xReleased) {
                        xReleased = false;
                        Camera.Shake(5f);
                        startSfx.Play();
                        start = true;
                    }
                } else
                    xReleased = true;
            } else {
                if (startTimer <= 0f)
                    LoadScene();
                startTimer -= deltaTime;
            }

            sinWaveTimer += deltaTime;
            yPosition = (float)Math.Sin(sinWaveTimer / 20f) * 6f;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(menuImg, new Vector2(0f, yPosition) - Main.camera.scroll, Color.White);
        }

        public void LoadScene() {
            Main.currentScene = new GameScene();
            Main.currentScene.Initialize();
            Main.currentScene.LoadContent();
        }
    }
}
