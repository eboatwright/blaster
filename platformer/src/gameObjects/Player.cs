using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Player : GameObject {

        public const int SPRITE_WIDTH = 12, SPRITE_HEIGHT = 13;
        public const int COLLISION_WIDTH = 13, COLLISION_HEIGHT = 14;
        public const float MOVE_SPEED = 0.85f, FRICTION = 0.7f, GRAVITY = 0.34f, JUMP_HEIGHT = -6f, COYOTE_TIME = 8;

        private bool jumpReleased, shootReleased, shooting;
        private float lastGrounded = 0f;

        private Vector2 velocity;

        private Texture2D playerImg;

        private Camera camera;
        private Map map;

        private bool flipSprite;


        // ANIMATION STUFF
        public int animationIndex, animationFrame;
        public float animationTimer;
        public Animation idleAnimation = new Animation(new int[]{ 0, 1 }, 10f);
        public Animation walkAnimation = new Animation(new int[]{ 2, 3, 4, 3 }, 10f);
        public Animation jumpAnimation = new Animation(new int[]{ 5 }, 1f);
        public Animation shootAnimation = new Animation(new int[]{ 6, 6 }, 2f);
        public Animation currentAnimation;



        public Player(Scene scene) : base(scene) {}

        public override void Initialize() {
            tags.Add("Player");
            map = (Map)scene.FindGameObjectWithTag("Map");
            currentAnimation = idleAnimation;
        }

        public override void LoadContent(ContentManager content) {
            playerImg = content.Load<Texture2D>("player");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if(!shooting) {
                if (keyboard.IsKeyDown(Keys.Left)) {
                    velocity.X -= MOVE_SPEED * deltaTime;
                    ChangeAnimation(walkAnimation);
                    flipSprite = true;
                } else if (keyboard.IsKeyDown(Keys.Right)) {
                    velocity.X += MOVE_SPEED * deltaTime;
                    ChangeAnimation(walkAnimation);
                    flipSprite = false;
                } else
                    ChangeAnimation(idleAnimation);

                if (keyboard.IsKeyDown(Keys.X)) {
                    if (jumpReleased) {
                        jumpReleased = false;
                        if (lastGrounded > 0f) {
                            lastGrounded = 0f;
                            velocity.Y += JUMP_HEIGHT;
                        }
                    }
                } else
                    jumpReleased = true;
            }

            if (keyboard.IsKeyDown(Keys.Z)) {
                if (shootReleased) {
                    shootReleased = false;
                    ChangeAnimation(shootAnimation);
                    shooting = true;
                }
            } else
                shootReleased = true;

            if (lastGrounded <= 0f)
                ChangeAnimation(jumpAnimation);

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


            // ANIMATION STUFF
            if(animationTimer <= 0f) {
                animationTimer = currentAnimation.frameDuration;
                animationIndex++;
            } else animationTimer -= deltaTime;

            if (animationIndex >= currentAnimation.frameIndexes.Length) {
                animationIndex = 0;
                if(shooting) {
                    shooting = false;
                    ChangeAnimation(idleAnimation);
                }
            }

            animationFrame = currentAnimation.frameIndexes[animationIndex];
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (camera == null) {
                camera = (Camera)scene.FindGameObjectWithTag("Camera");
                return;
            }
            spriteBatch.Draw(playerImg, position - camera.scroll, new Rectangle(animationFrame * SPRITE_WIDTH, 0, 12, 13), Color.White, 0f, Vector2.Zero, 1f, (flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
        }

        public void ChangeAnimation(Animation animation) {
            if(currentAnimation != animation) {
                currentAnimation = animation;
                animationIndex = 0;
                animationTimer = animation.frameDuration;
            }
        }
    }
}
