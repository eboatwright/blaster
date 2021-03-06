using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace eboatwright {
    public class Main : Game {

        public const int SCREEN_SCALE = 4, SCREEN_WIDTH = 240, SCREEN_HEIGHT = 150;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static Scene currentScene;
        public static ContentManager content;
        public static Random random;
        public static Color lightGreyColor = new Color(120, 172, 187);
        public static Camera camera;
        public static SpriteFont font;
        public static SoundEffectInstance menuSong, gameSong, bossSong, currentSong;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            content = Content;
            random = new Random();

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH * SCREEN_SCALE;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT * SCREEN_SCALE;
            graphics.ApplyChanges();
            Window.Title = "Blaster - eboatwright";

            currentScene = new SplashScene();
            currentScene.Initialize();

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            menuSong = Content.Load<SoundEffect>("music/space").CreateInstance();
            gameSong = Content.Load<SoundEffect>("music/robotdance").CreateInstance();
            bossSong = Content.Load<SoundEffect>("music/likeclockwork").CreateInstance();
            currentScene.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            camera = (Camera)currentScene.FindGameObjectWithTag("Camera");

            currentScene.Update((float)gameTime.ElapsedGameTime.TotalSeconds * 60, Mouse.GetState(), Keyboard.GetState());
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(33, 44, 54));

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(SCREEN_SCALE));

            currentScene.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
