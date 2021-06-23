using Microsoft.Xna.Framework;

namespace eboatwright {
    public class ParticleData {

        public Color color;
        public float lifePercentage;
        public int spriteIndex;

        public ParticleData(Color color, float lifePercentage, int spriteIndex) {
            this.color = color;
            this.lifePercentage = lifePercentage;
            this.spriteIndex = spriteIndex;
        }
    }
}
