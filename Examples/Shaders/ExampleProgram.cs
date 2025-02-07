﻿using MINNOVAA.ObjectTK.Shaders;
using MINNOVAA.ObjectTK.Shaders.Sources;
using MINNOVAA.ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Examples.Shaders
{
    [VertexShaderSource("ExampleShader.Vertex")]
    [FragmentShaderSource("ExampleShader.Fragment")]
    public class ExampleProgram
        : Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
    }
}