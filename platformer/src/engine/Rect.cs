namespace eboatwright {
    public class Rect {

        public int x, y, width, height;
        public int top, bottom, left, right;

        public Rect(int x, int y, int width, int height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            this.top = y;
            this.bottom = y + height;
            this.left = x;
            this.right = x + width;
        }

        public bool Overlaps(Rect other) {
            return this.left < other.right &&
                    this.right > other.left &&
                    this.top < other.bottom &&
                    this.bottom > other.top;
        }
    }
}
