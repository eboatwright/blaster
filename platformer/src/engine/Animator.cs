using System;

namespace eboatwright {
    public class Animator {

        public Animation currentAnimation;
        public int animationIndex = 0, animationFrame = 0;
        private float animationTimer = 0f;

        public bool doingUninterruptableAnimation;

        private Animation[] animations;

        public Animator(Animation[] animations) {
            this.animations = animations;
            ChangeAnimation(0);
        }

        public void Update(float deltaTime) {
            if (animationTimer <= 0f) {
                animationTimer = currentAnimation.frameDuration;
                animationIndex++;
            } else animationTimer -= deltaTime;

            if (animationIndex >= currentAnimation.frameIndexes.Length) {
                animationIndex = 0;
                if (doingUninterruptableAnimation) {
                    doingUninterruptableAnimation = false;
                    ChangeAnimation(0);
                }
            }

            animationFrame = currentAnimation.frameIndexes[animationIndex];
        }

        public void ChangeAnimation(int animation, bool isUninterruptable = false) {
            if (Array.IndexOf(animations, currentAnimation) != animation) {
                currentAnimation = animations[animation];
                animationIndex = 0;
                animationTimer = animations[animation].frameDuration;
                if (isUninterruptable) doingUninterruptableAnimation = true;
            }
        }
    }
}
