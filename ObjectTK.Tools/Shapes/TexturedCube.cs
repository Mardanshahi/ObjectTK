//
// TexturedCube.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using MINNOVAA.ObjectTK.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace MINNOVAA.ObjectTK.Tools.Shapes
{
    public class TexturedCube  : Cube , ITexturedShape
    {
        public Vector2[] TexCoords { get; set; }
        public Buffer<Vector2> TexCoordBuffer { get; set; }

        public TexturedCube()
        {
            DefaultMode = PrimitiveType.Triangles;


            var quad_uv_map = new[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0),
            };

         
            // Cube uses indexed vertices, TexturedShape assumes a flat vertices array
            // So we need to assemble the missing vertices ourself
            Vertices = Indices.Select(idx => Vertices[idx]).ToArray();

            // Use predefined uv texture mapping for vertices
            TexCoords = Enumerable.Range(0, Vertices.Length).Select(i => quad_uv_map[i % quad_uv_map.Length]).ToArray();
            
        }
        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            TexCoordBuffer = new Buffer<Vector2>();
            TexCoordBuffer.Init(BufferTarget.ArrayBuffer, TexCoords);
        }

        protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
            if (!manual) return;
            if (TexCoordBuffer != null) TexCoordBuffer.Dispose();
        }
    }

}
