using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Player : GameObject {

        public const int COLLISION_WIDTH = 13, COLLISION_HEIGHT = 14;
        public const float MOVE_SPEED = 0.85f, FRICTION = 0.7f, GRAVITY = 0.34f, JUMP_HEIGHT = -6f, COYOTE_TIME = 8;

        private bool jumpReleased;
        private float lastGrounded = 0f;

        private Vector2 velocity;

        private Texture2D playerImg;

        private Camera camera;
        private Map map;

        public Player(Scene scene) : base(scene) { }

        public override void Initialize() {
            tags.Add("Player");
            map = (Map)scene.FindGameObjectWithTag("Map");
        }

        public override void LoadContent(ContentManager content) {
            playerImg = content.Load<Texture2D>("player");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) velocity.X -= MOVE_SPEED * deltaTime;
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) velocity.X += MOVE_SPEED * deltaTime;
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up)) {
                if (jumpReleased) {
                    jumpReleased = false;
                    if (lastGrounded > 0f) {
                        lastGrounded = 0f;
                        velocity.Y += JUMP_HEIGHT;
                    }
                }
            } else {
                jumpReleased = true;
            }

            velocity.Y += GRAVITY * deltaTime;
            position.Y += velocity.Y * deltaTime;
            lastGrounded -= deltaTime;

            Rect PlayerRect = new Rect((int)Math.Floor(position.X), (int)Math.Floor(position.Y), COLLISION_WIDTH - 1, COLLISION_HEIGHT);

            for (int y = 0; y <= map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= map.mapValues.GetUpperBound(1); x++) {
                    if (map.mapValues[y, x] > 0 && map.mapValues[y, x] < 14) {
                        Rect TileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (PlayerRect.Overlaps(TileRect)) {
                            if (velocity.Y > 0f) {
                                velocity.Y = 0f;
                                lastGrounded = COYOTE_TIME;
                                position.Y = y * Map.TILE_SIZE - COLLISION_HEIGHT + 1;
                            } else if (velocity.Y < 0f) {
                                velocity.Y = 0f;
                                position.Y = y * Map.TILE_SIZE + Map.TILE_SIZE;
                            }
                        }
                    }
                }

            velocity.X *= FRICTION;
            position.X += velocity.X;

            PlayerRect = new Rect((int)Math.Floor(position.X), (int)Math.Floor(position.Y), COLLISION_WIDTH, COLLISION_HEIGHT - 1);

            for (int y = 0; y <= map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= map.mapValues.GetUpperBound(1); x++) {
                    if (map.mapValues[y, x] > 0 && map.mapValues[y, x] < 14) {
                        Rect TileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (PlayerRect.Overlaps(TileRect)) {
                            if (velocity.X > 0f) {
                                velocity.X = 0f;
                                position.X = x * Map.TILE_SIZE - COLLISION_WIDTH + 1;
                            } else if (velocity.X < 0f) {
                                velocity.X = 0f;
                                position.X = x * Map.TILE_SIZE + Map.TILE_SIZE;
                            }
                        }
                    }
                }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (camera == null) {
                camera = (Camera)scene.FindGameObjectWithTag("Camera");
                return;
            }
            spriteBatch.Draw(playerImg, position - camera.scroll, new Rectangle(0, 0, 12, 13), Color.White);
        }
    }
}
