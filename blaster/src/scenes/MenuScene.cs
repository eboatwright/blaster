using Microsoft.Xna.Framework.Media;

namespace eboatwright {
    public class MenuScene : Scene {

        public MenuScene() {
            AddGameObject(new MenuHandler(this));
            AddGameObject(new Camera(this));

            Main.currentSong = Main.menuSong;
            Main.currentSong.Play();
        }
    }
}
