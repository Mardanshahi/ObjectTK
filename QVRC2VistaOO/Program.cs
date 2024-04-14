

namespace Qvrc2VistaOO
{
    class Qvrc2VistaOO
    {
        static void Main()
        {

            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (var game = new Game(600, 400, "Textures Slice Classification"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run(60.0);
            }





        }
    }
}
