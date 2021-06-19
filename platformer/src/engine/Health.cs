namespace eboatwright {
    public abstract class Health {

        public int health;

        public Health(int startHealth) {
            this.health = startHealth;
        }

        public void Damage(int damage) {
            health -= damage;
            if(health <= 0) {
                Death();
            }
        }

        public abstract void Death() {}
    }
}
