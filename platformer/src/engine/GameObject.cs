using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace eboatwright {
    public abstract class GameObject {

        public List<string> tags;
        public Scene scene;
        public Vector2 position;
        public bool destroyed;

        public GameObject(Scene scene) {
            tags = new List<string>();
            this.scene = scene;
        }

        public abstract void Initialize();

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(float deltaTime, MouseState mouse, KeyboardState keyboard);

        public abstract void Draw(SpriteBatch spriteBatch);

        public void Destroy() {
            destroyed = true;
        }

        public void AddTags(string[] tags) {
            foreach(string tag in tags) {
                this.tags.Add(tag);
            }
        }
    }
}
