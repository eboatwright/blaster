namespace eboatwright {
    public class MenuScene : Scene {

        public MenuScene() {
            AddGameObject(new MenuHandler(this));
        }
    }
}
