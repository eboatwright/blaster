using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public class Scene {

        public List<GameObject> gameObjects;

        public Scene() {
            gameObjects = gameObjects = new List<GameObject>();
        }

        public void Initialize() {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Initialize();
        }

        public void LoadContent() {
            foreach (GameObject gameObject in gameObjects)
                gameObject.LoadContent();
        }

        public void Update(float deltaTime, MouseState mouse, KeyboardState keyboard) {
            for(int i = gameObjects.Count - 1; i >= 0; i--) {
                if(gameObjects[i].destroyed)
                    gameObjects.RemoveAt(i);
                else
                    gameObjects[i].Update(deltaTime, mouse, keyboard);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(spriteBatch);
        }

        public GameObject FindGameObjectWithTag(string tag) {
            foreach(GameObject gameObject in gameObjects)
                if (gameObject.tags.Contains(tag))
                    return gameObject;
            return null;
        }

        public List<GameObject> FindGameObjectsWithTag(string tag) {
            List<GameObject> found = new List<GameObject>();
            foreach (GameObject gameObject in gameObjects)
                if (gameObject.tags.Contains(tag))
                    found.Add(gameObject);
            return found;
        }

        public GameObject AddGameObject(GameObject newGameObject) {
            gameObjects.Add(newGameObject);
            return newGameObject;
        }
    }
}
