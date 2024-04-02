

using System.Drawing;

namespace MinimalExampleProject
{
    class MinimalExampleProject
    {
        static void Main()
        {

            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (var game = new Game())//(800, 600, "Chapter 1 - Textures"))
            {
                //game.WindowState = OpenTK.WindowState.Maximized;
                game.Size = new Size(1280, 720);
                game.Title = "minimal example";
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run(60.0);
            }





        }
    }
}
