using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Player : GameObject, IDamageable {

        enum ANIMATION_STATES {
            IDLE = 0,
            WALK = 1,
            JUMP = 2,
            SHOOT = 3,
        }

        public const int SPRITE_WIDTH = 16, SPRITE_HEIGHT = 16;
        public const int COLLISION_WIDTH = 17, COLLISION_HEIGHT = 17;
        public const float MOVE_SPEED = 0.69f, FRICTION = 0.7f, GRAVITY = 0.34f, JUMP_HEIGHT = -5.6f, COYOTE_TIME = 8, GUN_RECOIL = 0.8f;

        public Vector2 SHOOT_RIGHT_OFFSET = new Vector2(11, 5), SHOOT_LEFT_OFFSET = new Vector2(-1, 5);

        private bool jumpReleased, shootReleased;
        private float lastGrounded = 0f;
        
        private Vector2 velocity;

        private Texture2D playerImg;

        private Camera camera;
        private Map map;

        private bool flipSprite;

        public Animator animator;



        public int Health { get; set; }

        public void Damage() {
            Health--;
            if(Health <= 0)
                Destroy();
        }



        public Player(Scene scene) : base(scene) {}

        public override void Initialize() {
            AddTags(new string[]{ "Player" });
            map = (Map)scene.FindGameObjectWithTag("Map");
            animator = new Animator(new Animation[]{
                new Animation(new int[] { 0, 1 }, 10f),
                new Animation(new int[] { 3, 2 }, 7.2f),
                new Animation(new int[] { 4 }, 1f),
                new Animation(new int[] { 5 }, 7f)
            });
            Health = 3;
        }

        public override void LoadContent() {
            playerImg = Main.content.Load<Texture2D>("player");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            if (keyboard.IsKeyDown(Keys.Left)) {
                velocity.X -= MOVE_SPEED * deltaTime;
                if (!animator.doingUninterruptableAnimation) animator.ChangeAnimation((int)ANIMATION_STATES.WALK);
                flipSprite = true;
            } else if (keyboard.IsKeyDown(Keys.Right)) {
                velocity.X += MOVE_SPEED * deltaTime;
                if (!animator.doingUninterruptableAnimation) animator.ChangeAnimation((int)ANIMATION_STATES.WALK);
                flipSprite = false;
            } else if (!animator.doingUninterruptableAnimation)
                animator.ChangeAnimation((int)ANIMATION_STATES.IDLE);

            if (keyboard.IsKeyDown(Keys.X)) {
                if (jumpReleased) {
                    jumpReleased = false;
                    if (lastGrounded > 0f) {
                        lastGrounded = 0f;
                        velocity.Y = JUMP_HEIGHT;
                    }
                }
            } else
                jumpReleased = true;

            if (lastGrounded <= 0f && !animator.doingUninterruptableAnimation)
                animator.ChangeAnimation((int)ANIMATION_STATES.JUMP);

            if (keyboard.IsKeyDown(Keys.Z)) {
                if (shootReleased) {
                    shootReleased = false;
                    animator.ChangeAnimation((int)ANIMATION_STATES.SHOOT, true);
                    velocity.X *= GUN_RECOIL;

                    Vector2 shootPosition = position + (flipSprite ? SHOOT_LEFT_OFFSET : SHOOT_RIGHT_OFFSET);
                    scene.AddGameObject(new Projectile(scene, true, !flipSprite, shootPosition));
                }
            } else
                shootReleased = true;

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
                                lastGrounded = COYOTE_TIME;
                                position.Y = y * Map.TILE_SIZE - COLLISION_HEIGHT + 1;
                            } else if (velocity.Y < 0f)
                                position.Y = y * Map.TILE_SIZE + Map.TILE_SIZE;
                            velocity.Y = 0f;
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
                            if (velocity.X > 0f)
                                position.X = x * Map.TILE_SIZE - COLLISION_WIDTH + 1;
                            else if (velocity.X < 0f)
                                position.X = x * Map.TILE_SIZE + Map.TILE_SIZE;
                            velocity.X = 0f;
                        }
                    }
                }

            animator.Update(deltaTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (camera == null) {
                camera = (Camera)scene.FindGameObjectWithTag("Camera");
                return;
            }
            spriteBatch.Draw(playerImg, position - camera.scroll, new Rectangle(animator.animationFrame * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT), Color.White, 0f, Vector2.Zero, 1f, (flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
        }
    }
}
