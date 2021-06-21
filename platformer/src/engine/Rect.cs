using Microsoft.Xna.Framework;

namespace eboatwright {
    public class Rect {

        public int x, y, width, height;
        public int top, bottom, left, right;

        public Rect(int x, int y, int width, int height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            top = y;
            bottom = y + height;
            left = x;
            right = x + width;
        }

        public Rect(Vector2 position, int width, int height) {
            x = (int)position.X;
            y = (int)position.Y;
            this.width = width;
            this.height = height;

            top = y;
            bottom = y + height;
            left = x;
            right = x + width;
        }

        public bool Overlaps(Rect other) {
            return left < other.right &&
                    right > other.left &&
                    top < other.bottom &&
                    bottom > other.top;
        }
    }
}
