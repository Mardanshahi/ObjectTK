﻿using System;
using System.Diagnostics;
using System.Drawing;
using MinimalExampleProject.Properties;

//using Examples.Shaders;
using MINNOVAA.ObjectTK.Buffers;
using MINNOVAA.ObjectTK.Shaders;
using MINNOVAA.ObjectTK.Textures;
using MINNOVAA.ObjectTK.Tools.Cameras;
using MINNOVAA.ObjectTK.Tools.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MinimalExampleProject
{
    public class Game : ExampleWindow
    {
        private Texture2D _texture;

        private SimpleTextureProgram _textureProgram;

        private TexturedCube _cube;
        private VertexArray _cubeVao;

        private Matrix4 _baseView;
        private Matrix4 _objectView;

        private Vector3[] _rotateVectors = new[] { Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.One };
        private const int _defaultRotateIndex = 4;

        private int _rotateIndex = _defaultRotateIndex;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public Game()
        {
            Load += OnLoad;
            Unload += OnUnload;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // load texture from file
            using (var bitmap = new Bitmap("Data/Textures/crate.png"))
            {
                BitmapTexture.CreateCompatible(bitmap, out _texture);
                _texture.LoadBitmap(bitmap);
            }

            // initialize shaders
            //ProgramFactory.BasePath = "Shaders";
            _textureProgram = ProgramFactory.Create<SimpleTextureProgram>(Resources.SimpleTexture_vert, Resources.SimpleTexture_frag);
            //_textureProgram = ProgramFactory.Create<SimpleTextureProgram>();

            // initialize cube object and base view matrix
            _objectView = _baseView = Matrix4.Identity;

            // initialize demonstration geometry
            _cube = new TexturedCube();
            _cube.UpdateBuffers();

            // set up vertex attributes for the quad
            _cubeVao = new VertexArray();
            _cubeVao.Bind();
            _cubeVao.BindAttribute(_textureProgram.InPosition, _cube.VertexBuffer);
            _cubeVao.BindAttribute(_textureProgram.InTexCoord, _cube.TexCoordBuffer);

            // Enable culling, our cube vertices are defined inside out, so we flip them
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            // initialize camera position
            Camera.DefaultState.Position = new Vector3(0, 0, 4);
            Camera.ResetToDefault();

            // set nice clear color
            GL.ClearColor(Color.MidnightBlue);

            _stopwatch.Restart();
        }
        private void OnUnload(object sender, EventArgs e)
        {
            // Always make sure to properly dispose gl resources to prevent memory leaks.
            // Most of the examples do not explicitly dispose resources, because
            // the base class (ExampleWindow) calls GLResource.DisposeAll(this).
            // This will automatically dispose all objects referenced by class fields
            // which derive from GLResource. Everything else still has to be disposed manually.
            _textureProgram.Dispose();
            _cubeVao.Dispose();
            _cube.Dispose();
        }
        protected  void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // determinate object view rotation vectors and apply them
            _objectView = _baseView;
            //var rotation = _rotateVectors[_rotateIndex];
            //if (rotation != Vector3.Zero)
            //    _objectView *= Matrix4.CreateFromAxisAngle(_rotateVectors[_rotateIndex], (float)(_stopwatch.Elapsed.TotalSeconds * 1.0));

            // set transformation matrix
            _textureProgram.Use();
            _textureProgram.ModelViewProjectionMatrix.Set(_objectView * ModelView * Projection);

            // render cube with texture
            _cubeVao.Bind();
            _cubeVao.DrawArrays(_cube.DefaultMode, 0, _cube.VertexBuffer.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}
