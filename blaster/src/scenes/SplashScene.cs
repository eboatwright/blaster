namespace eboatwright {
    public class SplashScene : Scene {

        public SplashScene() {
            AddGameObject(new SplashHandler(this));
        }
    }
}
