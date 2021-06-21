using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Map : GameObject {

        public const int TILE_SIZE = 16;

        public static int[,] mapValues;

        private Texture2D tilesetImg;

        public Map(Scene scene) : base(scene) {}

        public override void Initialize() {
            mapValues = new int[,]{
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,1,2,3,0,0,0,0,0,0},
                {2,2,3,0,0,0,7,8,9,0,0,0,1,2,2},
                {5,5,6,0,0,0,0,0,0,0,0,0,4,5,5},
                {5,5,10,2,2,2,2,2,2,2,2,2,11,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}
            };

            AddTags(new string[]{ "Map" });
        }

        public override void LoadContent() {
            tilesetImg = Main.content.Load<Texture2D>("levelTileset");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {}

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            for (int y = 0; y <= mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= mapValues.GetUpperBound(1); x++)
                    if (mapValues[y, x] > 0 && mapValues[y, x] < 14)
                        spriteBatch.Draw(tilesetImg, new Vector2(x * TILE_SIZE, y * TILE_SIZE) - Main.camera.scroll, new Rectangle((mapValues[y, x] - 1) * TILE_SIZE, 0, TILE_SIZE, TILE_SIZE), Color.White);
        }
    }
}
