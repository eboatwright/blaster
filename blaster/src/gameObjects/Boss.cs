using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Boss : GameObject {

        enum ANIMATION_STATES {
            WALK = 0,
            SHOOT = 1
        }


        public const float MOVE_SPEED = 0.36f, FRICTION = 0.7f, GRAVITY = 0.34f, GUN_RECOIL = 0.7f;
        public const int SPRITE_WIDTH = 32, SPRITE_HEIGHT = 32;
        public const int COLLISION_WIDTH = 33, COLLISION_HEIGHT = 33;

        private Vector2 SHOOT_RIGHT_OFFSET = new Vector2(27, 12), SHOOT_LEFT_OFFSET = new Vector2(-5, 12);

        private int direction = 1;

        private Texture2D bossImg;

        private Animator animator;
        private bool flipSprite;

        private Vector2 velocity;

        private SoundEffect hitSfx, explodeSfx, shootSfx;
        private float shootTime = 36f, shootTimer;

        private bool playedMusic;

        private Player player;

        private float shockwaveCooldown = 0f;


        public int Health { get; set; }

        public void Damage() {
            Camera.Shake(2f);

            Health--;
            if (Health <= 0) {
                Main.currentSong.Stop();

                ScoreCounter.score += 1000;

                Camera.Shake(12f);
                explodeSfx.Play();

                for (int i = 0; i < 26; i++) {
                    Vector2 particlePosition = position + new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2) + new Vector2((float)Main.random.NextDouble() * 24f - 12f, (float)Main.random.NextDouble() * 24f - 12f);
                    Vector2 particleVelocity = new Vector2((float)Main.random.NextDouble() * 5 - 2.5f, (float)Main.random.NextDouble() * 5 - 3.5f);
                    scene.AddGameObject(new Particle(scene, particlePosition, particleVelocity, 0.05f, 0.9f, 30f, new ParticleData[] { new ParticleData(new Color(43, 73, 90), 1f, 0), new ParticleData(new Color(43, 73, 90), 0.5f, 2) }));
                }

                for(int i = 0; i < 10; i++)
                    scene.AddGameObject(new Gem(scene, position + new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2) + new Vector2((float)Main.random.NextDouble() * 24f - 12f, (float)Main.random.NextDouble() * 16f - 8f)));

                Map.mapValues[4, 179] = 7;
                Map.mapValues[8, 179] = 2;
                for (int y = 5; y < 8; y++)
                    Map.mapValues[y, 179] = 0;

                Destroy();
                return;
            }
            hitSfx.Play();

            for (int i = 0; i < 1; i++) {
                Vector2 particlePosition = position + new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2) + new Vector2((float)Main.random.NextDouble() * 10f - 5f, (float)Main.random.NextDouble() * 10f - 5f);
                Vector2 particleVelocity = new Vector2((float)Main.random.NextDouble() * 3 - 1.5f, (float)Main.random.NextDouble() * 3 - 2.5f);
                scene.AddGameObject(new Particle(scene, particlePosition, particleVelocity, 0.05f, 0.9f, 16f, new ParticleData[] { new ParticleData(new Color(255, 255, 255), 1f, 0), new ParticleData(new Color(255, 255, 255), 0.5f, 2) }));
            }
        }


        public Boss(Scene scene) : base(scene) {}

        public override void Initialize() {
            animator = new Animator(new Animation[]{
                new Animation(new int[] { 4, 5, 6, 7 }, 8f),
                new Animation(new int[] { 8, 9 }, 8f)
            });
            animator.ChangeAnimation((int)ANIMATION_STATES.WALK, false);
            AddTags(new string[] { "Boss" });
            Health = 32;

            position = new Vector2((960 / 4) * 11.5f, 16 * 6);
        }

        public override void LoadContent() {
            bossImg = Main.content.Load<Texture2D>("boss/boss");
            hitSfx = Main.content.Load<SoundEffect>("sfx/hitEnemy");
            explodeSfx = Main.content.Load<SoundEffect>("boss/bossExplode");
            shootSfx = Main.content.Load<SoundEffect>("sfx/shoot");
        }

        public override void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            Rect bossRect = new Rect(position, COLLISION_WIDTH, COLLISION_HEIGHT);
            Rect screenRect = new Rect(Main.camera.smoothedScroll, 240, 150);
            if (!bossRect.Overlaps(screenRect)) return;
            if(!playedMusic) {
                playedMusic = true;

                Main.currentSong.Stop();
                Main.currentSong = Main.bossSong;
                Main.currentSong.Play();
            }

            player = (Player)scene.FindGameObjectWithTag("Player");
            if(player != null) {
                if (shockwaveCooldown <= 0f) {
                    shockwaveCooldown = 10f;
                    if (Vector2.Distance(position, player.position) <= 25f)
                        scene.AddGameObject(new Shockwave(scene));
                } else
                    shockwaveCooldown -= deltaTime;
            }

            velocity.X += direction * MOVE_SPEED;

            velocity.Y += GRAVITY * deltaTime;
            position.Y += velocity.Y * deltaTime;

            bossRect = new Rect(position, COLLISION_WIDTH - 1, COLLISION_HEIGHT);

            for (int y = 0; y <= Map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= Map.mapValues.GetUpperBound(1); x++) {
                    if (Map.mapValues[y, x] > 0 && Map.mapValues[y, x] < 14) {
                        Rect tileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (bossRect.Overlaps(tileRect)) {
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

            bossRect = new Rect(position, COLLISION_WIDTH, COLLISION_HEIGHT - 1);

            for (int y = 0; y <= Map.mapValues.GetUpperBound(0); y++)
                for (int x = 0; x <= Map.mapValues.GetUpperBound(1); x++) {
                    if ((Map.mapValues[y, x] > 0 && Map.mapValues[y, x] < 14) || (Map.mapValues[y, x] == 16)) {
                        Rect TileRect = new Rect(x * Map.TILE_SIZE, y * Map.TILE_SIZE, Map.TILE_SIZE, Map.TILE_SIZE);
                        if (bossRect.Overlaps(TileRect)) {
                            if (velocity.X > 0f) {
                                position.X = x * Map.TILE_SIZE - COLLISION_WIDTH + 1;
                                direction = -1;
                            } else if (velocity.X < 0f) {
                                position.X = x * Map.TILE_SIZE + Map.TILE_SIZE;
                                direction = 1;
                            }
                            velocity.X = 0f;
                        }
                    }
                }

            if (shootTimer <= 0f) {
                shootTimer = shootTime;
                velocity.X *= GUN_RECOIL;

                Vector2 shootPosition = position + (flipSprite ? SHOOT_LEFT_OFFSET : SHOOT_RIGHT_OFFSET);
                scene.AddGameObject(new BossProjectile(scene, !flipSprite, shootPosition));

                shootSfx.Play();
            }
            shootTimer -= deltaTime;

            animator.Update(deltaTime);
            flipSprite = direction < 0;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (Main.camera == null) return;
            spriteBatch.Draw(bossImg, position - Main.camera.scroll, new Rectangle(animator.animationFrame * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT), Color.White, 0f, Vector2.Zero, 1f, (flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
        }
    }
}
