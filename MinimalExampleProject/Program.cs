

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MinimalExampleProject
{
    class MinimalExampleProject
    {
        static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                WindowState = WindowState.Normal,
                Size = new Vector2i(800, 600),
                Title = "Minimal Example Project",
                API = ContextAPI.OpenGL,
                APIVersion = new System.Version(3, 3),
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
            };
            //var gameWindowSettings = new GameWindowSettings()
            //{
            //    RenderFrequency = 20,
            //    UpdateFrequency = 10,
            //};
            // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
            using (var window = new Game())
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                window.Run();
            }





        }
    }
}
