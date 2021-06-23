using System;

namespace eboatwright {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new Main())
                game.Run();
        }
    }
}
