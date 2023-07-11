using System;
using MINNOVAA.ObjectTK;
using MINNOVAA.ObjectTK.Shaders;
using MINNOVAA.ObjectTK.Tools;
using MINNOVAA.ObjectTK.Tools.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Examples
{
    /// <summary>
    /// Provides common functionality for the examples.
    /// </summary>
    public class ExampleWindow
        : DerpWindow
    {
        protected Camera Camera;
        protected Matrix4 ModelView;
        protected Matrix4 Projection;
        protected string OriginalTitle { get; private set; }

        public ExampleWindow()
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(800, 600), Title = "" })
        {
            // disable vsync
            VSync = VSyncMode.Off;
            // set up camera
            Camera = new Camera();
            Camera.SetBehavior(new ThirdPersonBehavior(this));
            Camera.DefaultState.Position.Z = 5;
            Camera.ResetToDefault();
            Camera.Enable(this);
            ResetMatrices();
            // hook up events
        }

        protected override void OnLoad()
        {
            // maximize window
            WindowState = WindowState.Maximized;
            // remember original title
            OriginalTitle = Title;
            // set search path for shader files and extension
            ProgramFactory.BasePath = "Data/Shaders/";
            ProgramFactory.Extension = "glsl";
        }

        protected override void OnUnload()
        {
            // release all gl resources on unload
            GLResource.DisposeAll(this);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // display FPS in the window title
            Title = string.Format("ObjectTK example: {0} - FPS {1}", OriginalTitle, FrameTimer.FpsBasedOnFramesRendered);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            // close window on escape press
            if (e.Key == Keys.Escape) Close();
            // reset camera to default position and orientation on R press
            if (e.Key == Keys.R) Camera.ResetToDefault();
        }

        /// <summary>
        /// Resets the ModelView and Projection matrices to the identity.
        /// </summary>
        protected void ResetMatrices()
        {
            ModelView = Matrix4.Identity;
            Projection = Matrix4.Identity;
        }

        /// <summary>
        /// Sets a perspective projection matrix and applies the camera transformation on the modelview matrix.
        /// </summary>
        protected void SetupPerspective()
        {
            // setup perspective projection
            var aspectRatio = Size.X / (float)Size.Y;
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000);
            ModelView = Matrix4.Identity;
            // apply camera transform
            ModelView = Camera.GetCameraTransform();
        }
    }
}