//
// DerpWindow.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MINNOVAA.ObjectTK.Tools
{
    /// <summary>
    /// Provides basic functionality to an OpenTK GameWindow such as camera controls,
    /// ModelView and Projection matrices and improved timing.
    /// </summary>
    public abstract class DerpWindow
        : GameWindow
    {
        private static readonly Logging.IObjectTKLogger Logger = Logging.LogFactory.GetLogger(typeof(DerpWindow));

        protected readonly FrameTimer FrameTimer;

        /// <summary>
        /// Initializes a new instance of the DerpWindow class.
        /// </summary>
        protected DerpWindow(GameWindowSettings gws, NativeWindowSettings nws)
            : base(gws, nws)
        {
            // log some OpenGL information
            Logger?.Info("OpenGL context information:");
            Logger?.InfoFormat("{0}: {1}", StringName.Vendor, GL.GetString(StringName.Vendor));
            Logger?.InfoFormat("{0}: {1}", StringName.Renderer, GL.GetString(StringName.Renderer));
            Logger?.InfoFormat("{0}: {1}", StringName.Version, GL.GetString(StringName.Version));
            Logger?.InfoFormat("{0}: {1}", StringName.ShadingLanguageVersion, GL.GetString(StringName.ShadingLanguageVersion));
            int numExtensions;
            GL.GetInteger(GetPName.NumExtensions, out numExtensions);
            Logger?.DebugFormat("Number available extensions: {0}", numExtensions);
            for (var i = 0; i < numExtensions; i++) Logger?.DebugFormat("{0}: {1}", i, GL.GetString(StringNameIndexed.Extensions, i));
            Logger?.InfoFormat("Initializing game window: {0}", nws.Title);
            // set up GameWindow events
            // set up frame timer
            FrameTimer = new FrameTimer();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            Size = e.Size;
            Logger?.InfoFormat("Window resized to: {0}x{1}", Size.X, Size.Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            FrameTimer.Time();
        }
    }
}
