﻿using MINNOVAA.ObjectTK.Shaders;
using MINNOVAA.ObjectTK.Shaders.Sources;
using MINNOVAA.ObjectTK.Shaders.Variables;
using MINNOVAA.ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Examples.Shaders
{
    [VertexShaderSource("Skybox.Vertex")]
    [FragmentShaderSource("Skybox.Fragment")]
    public class SkyboxProgram
        : Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public TextureUniform<TextureCubemap> Texture { get; protected set; }
    }
}