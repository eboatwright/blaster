using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Rover : GameObject, IDamageable {


        enum ANIMATION_STATES {
            IDLE = 0,
            WALK = 1,
            SHOOT = 2
        }


        public const float MOVE_SPEED = 0.36f, FRICTION = 0.7f, GRAVITY = 0.34f, GUN_RECOIL = 0.7f;
        public const int SPRITE_WIDTH = 18, SPRITE_HEIGHT = 16;
        public const int COLLISION_WIDTH = 19, COLLISION_HEIGHT = 17;

        private Vector2 SHOOT_RIGHT_OFFSET = new Vector2(15, 8), SHOOT_LEFT_OFFSET = new Vector2(-2, 9);

        private int direction = 1;

        private Texture2D roverImg;

        private Animator animator;
        private bool flipSprite;

        private Vector2 velocity;

        private SoundEffect hitSfx, explodeSfx, shootSfx;
        private float shootTime = 26f, shootTimer;


        public int Health { get; set; }

        public void Damage() {
            Camera.Shake(2f);

            Health--;
            if (Health <= 0) {
                Camera.Shake(2f);
                Destroy();
                explodeSfx.Play();

                for (int i = 0; i < 3; i++) {
                    Vector2 particlePosition = position + new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2) + new Vector2((float)Main.random.NextDouble() * 10f - 5f, (float)Main.random.NextDouble() * 10f - 5f);
                    Vector2 particleVelocity = new Vector2((float)Main.random.NextDouble() * 3 - 1.5f, (float)Main.random.NextDouble() * 3 - 2.5f);
                    scene.AddGameObject(new Particle(scene, particlePosition, particleVelocity, 0.05f, 0.9f, 16f, new ParticleData[]{ new ParticleData(new Color(43, 73, 90), 1f, 0), new ParticleData(new Color(43, 73, 90), 0.5f, 2) }));
                }

                return;
            }
            hitSfx.Play();

            for (int i = 0; i < 1; i++) {
                Vector2 particlePosition = position + new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2) + new Vector2((float)Main.random.NextDouble() * 10f - 5f, (float)Main.random.NextDouble() * 10f - 5f);
                Vector2 particleVelocity = new Vector2((float)Main.random.NextDouble() * 3 - 1.5f, (float)Main.random.NextDouble() * 3 - 2.5f);
                scene.AddGameObject(new Particle(scene, particlePosition, particleVelocity, 0.05f, 0.9f, 16f, new ParticleData[] { new ParticleData(new Color(255, 255, 255), 1f, 0), new ParticleData(new Color(255, 255, 255), 0.5f, 2) }));
            }
        }


        public Rover(Scene scene) : base(scene) {}

        public override void Initialize() {
            animator = new Animator(new Animation[]{
                new Animation(new int[] { 0 }, 1f),
                new Animation(new int[] { 1, 0 }, 16f),
                new Animation(new int[] { 2 }, 8f)
            });
            animator.ChangeAnimation((int)ANIMATION_STATES.WALK, false);
            AddTags(new string[]{ "Rover" });
            Health = 5;
        }

        public override void LoadContent() {
            roverImg = Main.content.Load<Texture2D>("enemyRover");
            hitSfx = Main.content.Load<SoundEffect>("sfx/hitEnemy");
            explodeSfx = Main.content.Load<SoundEffect>("sfx/enemyExplode");
            shootSfx = Main.content.Load<SoundEffect>("sfx/shoot");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            velocity.X += direction * MOVE_SPEED;

            velocity.Y += GRAVITY * deltaTime;
            position.Y += velocity.Y * deltaTime;

            Rect roverRect = new Rect(position, COLLISION_WIDTH - 1, COLLISION_HEIGHT);

            for (int y = 0; y <= Map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= Map.mapValues.GetUpperBound(1); x++) {
                    if (Map.mapValues[y, x] > 0 && Map.mapValues[y, x] < 14) {
                        Rect TileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (roverRect.Overlaps(TileRect)) {
                            if (velocity.Y > 0f)
                                position.Y = y * Map.TILE_SIZE - COLLISION_HEIGHT + 1;
                            else if (velocity.Y < 0f)
                                position.Y = y * Map.TILE_SIZE + Map.TILE_SIZE;
                            velocity.Y = 0f;
                        }
                    }
                }

            velocity.X *= FRICTION;
            position.X += velocity.X;

            roverRect = new Rect(position, COLLISION_WIDTH, COLLISION_HEIGHT - 1);

            for (int y = 0; y <= Map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= Map.mapValues.GetUpperBound(1); x++) {
                    if (Map.mapValues[y, x] > 0 && Map.mapValues[y, x] < 14) {
                        Rect TileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (roverRect.Overlaps(TileRect)) {
                            if (velocity.X > 0f) {
                                position.X = x * Map.TILE_SIZE - COLLISION_WIDTH + 1;
                                direction = -1;
                            }
                            else if (velocity.X < 0f) {
                                position.X = x * Map.TILE_SIZE + Map.TILE_SIZE;
                                direction = 1;
                            }
                            velocity.X = 0f;
                        }
                    }
                }

            if(shootTimer <= 0f) {
                shootTimer = shootTime;
                velocity.X *= GUN_RECOIL;

                Vector2 shootPosition = position + (flipSprite ? SHOOT_LEFT_OFFSET : SHOOT_RIGHT_OFFSET);
                scene.AddGameObject(new Projectile(scene, false, !flipSprite, shootPosition));

                shootSfx.Play();
            }
            shootTimer -= deltaTime;

            animator.Update(deltaTime);
            flipSprite = direction < 0;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(roverImg, position - Main.camera.scroll, new Rectangle(animator.animationFrame * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT), Color.White, 0f, Vector2.Zero, 1f, (flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
        }
    }
}
