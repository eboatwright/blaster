using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public const float MOVE_SPEED = 0.69f, FRICTION = 0.7f, GRAVITY = 0.34f, JUMP_HEIGHT = -5.6f, COYOTE_TIME = 8, GUN_RECOIL = 0.7f;

        private Vector2 SHOOT_RIGHT_OFFSET = new Vector2(11, 5), SHOOT_LEFT_OFFSET = new Vector2(-1, 5);

        private bool jumpReleased, shootReleased;
        private float lastGrounded = 0f;
        
        private Vector2 velocity;

        private Texture2D playerImg;

        private bool flipSprite;

        public Animator animator;
        private PlayerHealthBar healthBar;

        private SoundEffect jumpSfx, shootSfx, footstepSfx;
        private float footstepTime = 21f, footstepTimer;
        private float shootTime = 10f, shootTimer;


        public int Health { get; set; }

        public void Damage() {
            Camera.Shake(2f);

            Health--;
            healthBar.health = Health;
            if (Health <= 0) {
                Camera.Shake(2f);

                RespawnHandler.Respawn();
                Destroy();

                for (int i = 0; i < 3; i++) {
                    Vector2 particlePosition = position + new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2) + new Vector2((float)Main.random.NextDouble() * 10f - 5f, (float)Main.random.NextDouble() * 10f - 5f);
                    Vector2 particleVelocity = new Vector2((float)Main.random.NextDouble() * 3 - 1.5f, (float)Main.random.NextDouble() * 3 - 2.5f);
                    scene.AddGameObject(new Particle(scene, particlePosition, particleVelocity, 0.05f, 0.9f, 16f, new ParticleData[] { new ParticleData(new Color(46, 96, 182), 1f, 0), new ParticleData(new Color(46, 96, 182), 0.5f, 2) }));
                }

                return;
            }

            for (int i = 0; i < 1; i++) {
                Vector2 particlePosition = position + new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2) + new Vector2((float)Main.random.NextDouble() * 10f - 5f, (float)Main.random.NextDouble() * 10f - 5f);
                Vector2 particleVelocity = new Vector2((float)Main.random.NextDouble() * 3 - 1.5f, (float)Main.random.NextDouble() * 3 - 2.5f);
                scene.AddGameObject(new Particle(scene, particlePosition, particleVelocity, 0.05f, 0.9f, 16f, new ParticleData[] { new ParticleData(new Color(255, 255, 255), 1f, 0), new ParticleData(new Color(255, 255, 255), 0.5f, 2) }));
            }
        }


        public Player(Scene scene) : base(scene) {}

        public override void Initialize() {
            AddTags(new string[]{ "Player" });
            animator = new Animator(new Animation[]{
                new Animation(new int[] { 0, 1 }, 10f),
                new Animation(new int[] { 3, 2 }, 7.2f),
                new Animation(new int[] { 4 }, 1f),
                new Animation(new int[] { 5 }, 7f)
            });
            healthBar = (PlayerHealthBar)scene.AddGameObject(new PlayerHealthBar(scene));
            Health = healthBar.health = 3;

            position = new Vector2(16, 64);
        }

        public override void LoadContent() {
            playerImg = Main.content.Load<Texture2D>("player");
            jumpSfx = Main.content.Load<SoundEffect>("sfx/jump3");
            shootSfx = Main.content.Load<SoundEffect>("sfx/shoot");
            footstepSfx = Main.content.Load<SoundEffect>("sfx/footstep");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {float xInput = 0f;
            if (keyboard.IsKeyDown(Keys.Left)) {
                velocity.X -= MOVE_SPEED * deltaTime;
                if (!animator.doingUninterruptableAnimation) animator.ChangeAnimation((int)ANIMATION_STATES.WALK);
                flipSprite = true;
                xInput = -1f;
            } else if (keyboard.IsKeyDown(Keys.Right)) {
                velocity.X += MOVE_SPEED * deltaTime;
                if (!animator.doingUninterruptableAnimation) animator.ChangeAnimation((int)ANIMATION_STATES.WALK);
                flipSprite = false;
                xInput = 1f;
            } else if (!animator.doingUninterruptableAnimation)
                animator.ChangeAnimation((int)ANIMATION_STATES.IDLE);

            if (keyboard.IsKeyDown(Keys.X)) {
                if (jumpReleased) {
                    jumpReleased = false;
                    if (lastGrounded > 0f) {
                        lastGrounded = 0f;
                        velocity.Y = JUMP_HEIGHT;
                        jumpSfx.Play();
                    }
                }
            } else
                jumpReleased = true;

            if (lastGrounded <= 0f && !animator.doingUninterruptableAnimation)
                animator.ChangeAnimation((int)ANIMATION_STATES.JUMP);

            if (keyboard.IsKeyDown(Keys.Z) && shootTimer <= 0f) {
                if (shootReleased) {
                    shootTimer = shootTime;
                    shootReleased = false;
                    animator.ChangeAnimation((int)ANIMATION_STATES.SHOOT, true);
                    velocity.X *= GUN_RECOIL;

                    Vector2 shootPosition = position + (flipSprite ? SHOOT_LEFT_OFFSET : SHOOT_RIGHT_OFFSET);
                    scene.AddGameObject(new Projectile(scene, true, !flipSprite, shootPosition));

                    shootSfx.Play();
                }
            } else
                shootReleased = true;
            shootTimer -= deltaTime;

            velocity.Y += GRAVITY * deltaTime;
            position.Y += velocity.Y * deltaTime;
            lastGrounded -= deltaTime;

            Rect playerRect = new Rect(position, COLLISION_WIDTH - 1, COLLISION_HEIGHT);

            for (int y = 0; y <= Map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= Map.mapValues.GetUpperBound(1); x++) {
                    if (Map.mapValues[y, x] > 0 && Map.mapValues[y, x] < 14) {
                        Rect TileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (playerRect.Overlaps(TileRect)) {
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

            playerRect = new Rect(position, COLLISION_WIDTH, COLLISION_HEIGHT - 1);

            for (int y = 0; y <= Map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= Map.mapValues.GetUpperBound(1); x++) {
                    if (Map.mapValues[y, x] > 0 && Map.mapValues[y, x] < 14) {
                        Rect TileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (playerRect.Overlaps(TileRect)) {
                            if (velocity.X > 0f)
                                position.X = x * Map.TILE_SIZE - COLLISION_WIDTH + 1;
                            else if (velocity.X < 0f)
                                position.X = x * Map.TILE_SIZE + Map.TILE_SIZE;
                            velocity.X = 0f;
                        }
                    }
                }

            animator.Update(deltaTime);

            if (lastGrounded > 0f && xInput != 0f) {
                if (footstepTimer <= 0f) {
                    footstepTimer = footstepTime;
                    footstepSfx.Play();
                }
                footstepTimer -= deltaTime;
            } else
                footstepTimer = 0f;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(playerImg, position - Main.camera.scroll, new Rectangle(animator.animationFrame * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT), Color.White, 0f, Vector2.Zero, 1f, (flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
        }
    }
}
