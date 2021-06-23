using Microsoft.Xna.Framework;

namespace eboatwright {
    public class GameScene : Scene {

        public GameScene() {
            AddGameObject(new Background(this, new Vector2(-240f, 0f)));
            AddGameObject(new Background(this, new Vector2(0f, 0f)));
            AddGameObject(new Background(this, new Vector2(240f, 0f)));
            AddGameObject(new Background(this, new Vector2(480f, 0f)));
            AddGameObject(new Map(this));
            AddGameObject(new Boss(this));
            AddGameObject(new Player(this));
            AddGameObject(new Camera(this));
            AddGameObject(new RespawnHandler(this));
            AddGameObject(new ScoreCounter(this));

            Main.currentSong = Main.gameSong;
            Main.currentSong.Play();
        }
    }
}
