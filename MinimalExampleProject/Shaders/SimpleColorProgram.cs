﻿using MINNOVAA.ObjectTK.Shaders;
using MINNOVAA.ObjectTK.Shaders.Sources;
using MINNOVAA.ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MinimalExampleProject
{
    [VertexShaderSource("SimpleColor.Vertex")]
    [FragmentShaderSource("SimpleColor.Fragment")]
    public class SimpleColorProgram
        : Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }
        [VertexAttrib(4, VertexAttribPointerType.UnsignedByte, true)]
        public VertexAttrib InColor { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
    }
}