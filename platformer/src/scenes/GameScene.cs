namespace eboatwright {
    public class GameScene : Scene {

        public GameScene() {
            AddGameObject(new Map(this));
            AddGameObject(new Player(this));
            AddGameObject(new Camera(this));
        }
    }
}
