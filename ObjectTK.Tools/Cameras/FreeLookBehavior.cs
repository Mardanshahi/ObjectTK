//
// FreeLookBehavior.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MINNOVAA.ObjectTK.Tools.Cameras
{
    public class FreeLookBehavior
        : CameraBehavior
    {
        public FreeLookBehavior(GameWindow window) : base(window) { }

        public override void UpdateFrame(CameraState state, float step)
        {
            var keyboard = Window.KeyboardState;
            
            var dir = Vector3.Zero;
            var leftRight = Vector3.Cross(state.Up, state.LookAt).Normalized();
            if (keyboard.IsKeyDown(Keys.W)) dir += state.LookAt;
            if (keyboard.IsKeyDown(Keys.S)) dir -= state.LookAt;
            if (keyboard.IsKeyDown(Keys.A)) dir += leftRight;
            if (keyboard.IsKeyDown(Keys.D)) dir -= leftRight;
            if (keyboard.IsKeyDown(Keys.Space)) dir += state.Up;
            if (keyboard.IsKeyDown(Keys.LeftControl)) dir -= state.Up;
            // normalize dir to enforce consistent movement speed, independent of the number of keys pressed
            if (dir.LengthSquared > 0) state.Position += dir.Normalized() * step;
        }

        public override void MouseMove(CameraState state, Vector2 delta)
        {
            var mouse = Window.MouseState;
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                HandleFreeLook(state, delta);
            }
        }
    }
}