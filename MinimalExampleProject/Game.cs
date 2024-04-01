using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

//using Examples.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using ObjectTK.Tools.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinimalExampleProject
{
    public class Game : ExampleWindow
    {
        private Texture2D _texture;

        //private SimpleTextureProgram _textureProgram;
        private SimpleColorProgram _colorProgram;

        private ColorCube _cube;
        private VertexArray _cubeVao;

        private Matrix4 _baseView;
        private Matrix4 _objectView;

        private Vector3[] _rotateVectors = new[] { Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.One };
        private const int _defaultRotateIndex = 4;

        private int _rotateIndex = _defaultRotateIndex;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public Game()
        {
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.R:
                    _objectView = _baseView = Matrix4.Identity;
                    _rotateIndex = _defaultRotateIndex;
                    _stopwatch.Restart();
                    break;

                case Keys.Space:
                    _baseView = _objectView;
                    _rotateIndex = (_rotateIndex + 1) % _rotateVectors.Length;
                    _stopwatch.Restart();
                    break;

                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                    _baseView = _objectView;
                    _rotateIndex = (e.Key - Keys.D0) % _rotateVectors.Length;
                    _stopwatch.Restart();
                    break;
            }
        }

        protected override void OnLoad()
        {
            // load texture from file
            using (var bitmap = new Bitmap("Data/Textures/crate.png"))
            {
                BitmapTexture.CreateCompatible(bitmap, out _texture);
                _texture.LoadBitmap(bitmap);
            }

            // initialize shaders
            //_textureProgram = ProgramFactory.Create<SimpleTextureProgram>();
            _colorProgram = ProgramFactory.Create<SimpleColorProgram>();

            // initialize cube object and base view matrix
            _objectView = _baseView = Matrix4.Identity;

            // initialize demonstration geometry
            _cube = new ColorCube();
            uint ToRgba32(Color color)
            {
                return (uint)(color.A << 24 | color.B << 16 | color.G << 8 | color.R);
            }

            _cube.Colors = new List<Color>
            {
                Color.White,
                Color.DarkRed,
                Color.Gold,
                Color.Gold,
                Color.DarkRed,
                Color.DarkRed,
                Color.Gold,
                Color.Gold
            }.Select(_ => ToRgba32(_)).ToArray();
            _cube.UpdateBuffers();

            // set up vertex attributes for the quad
            // set up vertex attributes for the cube
            _cubeVao = new VertexArray();
            _cubeVao.Bind();
            _cubeVao.BindAttribute(_colorProgram.InPosition, _cube.VertexBuffer);
            _cubeVao.BindAttribute(_colorProgram.InColor, _cube.ColorBuffer);
            _cubeVao.BindElementBuffer(_cube.IndexBuffer);

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

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupPerspective();

            // determinate object view rotation vectors and apply them
            _objectView = _baseView;
            //var rotation = _rotateVectors[_rotateIndex];
            //if (rotation != Vector3.Zero)
            //    _objectView *= Matrix4.CreateFromAxisAngle(_rotateVectors[_rotateIndex], (float)(_stopwatch.Elapsed.TotalSeconds * 1.0));

            // set transformation matrix
            _colorProgram.Use();
            _colorProgram.ModelViewProjectionMatrix.Set(_objectView * ModelView * Projection);

            // render cube with texture
            _cubeVao.Bind();
            _cubeVao.DrawElements(PrimitiveType.Triangles, _cube.IndexBuffer.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}
