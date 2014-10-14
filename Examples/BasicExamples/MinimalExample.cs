﻿using System;
using System.Drawing;
using DerpGL.Buffers;
using DerpGL.Shaders;
using Examples.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Examples.BasicExamples
{
    [ExampleProject("Minimal example on shader and buffer usage")]
    public class MinimalExample
        : ExampleWindow
    {
        private ExampleProgram _program;
        private VertexArray _vao;
        private Buffer<Vector3> _vbo;

        public MinimalExample()
            : base("Shader and buffer usage")
        {
            Load += OnLoad;
            RenderFrame += OnRenderFrame;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // initialize shader (load sources, create/compile/link shader program, error checking)
            // when using the factory method the shader sources are retrieved from the ShaderSourceAttributes
            _program = ProgramFactory.Create<ExampleProgram>();
            // this program will be used all the time so just activate it once and for all
            _program.Use();
            
            // create vertices for a triangle
            var vertices = new[] { new Vector3(-1,-1,0), new Vector3(1,-1,0), new Vector3(0,1,0) };
            
            // create buffer object and upload vertex data
            _vbo = new Buffer<Vector3>();
            _vbo.Init(BufferTarget.ArrayBuffer, vertices);

            // create and bind a vertex array
            _vao = new VertexArray();
            _vao.Bind();
            // set up binding of the shader variable to the buffer object
            _vao.BindAttribute(_program.InPosition, _vbo);

            // set a nice clear color
            GL.ClearColor(Color.MidnightBlue);
        }

        private void OnRenderFrame(object sender, FrameEventArgs e)
        {
            // set up viewport
            GL.Viewport(0, 0, Width, Height);
            // clear the back buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // set up modelview and perspective matrix
            SetupPerspective();

            // calculate the MVP matrix and set it to the shaders uniform
            _program.ModelViewProjectionMatrix.Set(ModelView*Projection);
            // draw the buffer which contains the triangle
            _vao.DrawArrays(PrimitiveType.Triangles, _vbo.ElementCount);

            // swap buffers
            SwapBuffers();
        }
    }
}