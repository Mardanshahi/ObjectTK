//
// ColorCube.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MINNOVAA.ObjectTK.Buffers;
using OpenTK.Graphics.OpenGL;

namespace MINNOVAA.ObjectTK.Tools.Shapes
{
    public class ColorCube  : Cube , IColoredShape
    {
        public uint[] Colors { get; set; }
        public Buffer<uint> ColorBuffer { get; set; }
        public ColorCube()
        {
            DefaultMode = PrimitiveType.Triangles;

            // add color to the vertices
            Colors = new List<Color>
            {
                Color.DarkRed,
                Color.DarkRed,
                Color.Gold,
                Color.Gold,
                Color.DarkGreen,
                Color.DarkGreen,
                Color.Silver,
                Color.Silver
            }.Select(_ => _.ToRgba32()).ToArray();

            // Cube uses indexed vertices, TexturedShape assumes a flat vertices array
            // So we need to assemble the missing vertices ourself
            //Vertices = Indices.Select(idx => Vertices[idx]).ToArray();

        }
        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            ColorBuffer = new Buffer<uint>();
            ColorBuffer.Init(BufferTarget.ArrayBuffer, Colors);
        }

        protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
            if (!manual) return;
            if (ColorBuffer != null) ColorBuffer.Dispose();
        }
    }
}