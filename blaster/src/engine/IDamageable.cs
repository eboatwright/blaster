namespace eboatwright {
    public interface IDamageable {

        public int Health { get; set; }
        public void Damage();
    }
}
