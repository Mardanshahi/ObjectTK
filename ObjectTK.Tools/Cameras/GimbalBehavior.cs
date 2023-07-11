//
// GimbalBehavior.cs
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
    /// <summary>
    /// TODO: Does not yet work like it should.
    /// </summary>
    public class GimbalBehavior
        : ThirdPersonBehavior
    {
        public GimbalBehavior(GameWindow gw) : base(gw) { }

        public override void MouseMove(CameraState state, Vector2 delta)
        {
            var mouse = Window.MouseState;
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                base.MouseMove(state, delta);
                var leftRight = Vector3.Cross(state.Up, state.LookAt);
                Vector3.Cross(state.LookAt, leftRight, out state.Up);
                state.Up.Normalize();
            }
        }
    }
}