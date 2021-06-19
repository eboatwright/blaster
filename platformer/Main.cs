﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Main : Game {

        public const int SCREEN_SCALE = 4, SCREEN_WIDTH = 240, SCREEN_HEIGHT = 150;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Scene currentScene;

        public static ContentManager content;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            content = Content;
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH * SCREEN_SCALE;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT * SCREEN_SCALE;
            graphics.ApplyChanges();
            Window.Title = "Platformer - eboatwright";

            currentScene = new GameScene();

            currentScene.Initialize();

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentScene.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentScene.Update((float)gameTime.ElapsedGameTime.TotalSeconds * 60, Mouse.GetState(), Keyboard.GetState());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(SCREEN_SCALE));

            currentScene.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}