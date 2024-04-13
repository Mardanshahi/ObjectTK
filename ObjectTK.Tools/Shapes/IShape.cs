//
// Shape.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using MINNOVAA.ObjectTK.Buffers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MINNOVAA.ObjectTK.Tools.Shapes
{
    /// <summary>
    /// TODO: needs a total refactoring
    /// does not prevent multiple calls to UpdateBuffers() and causes resource leaks
    /// does not fit well to an inheritance chain
    /// there may be a shape just with vertices
    /// there may be an indexed shape with vertices and indices
    /// there may be a colored, indexed shape with vertices, indices and colors
    /// there may be a colored shape with vertices and colors but no indices...
    /// </summary>
    public interface IShape  : IGLResource
    {
        PrimitiveType DefaultMode { get; set; }
        Vector3[] Vertices { get; set; }
        Buffer<Vector3> VertexBuffer { get; set; }

        void UpdateBuffers();

      
   
    }
}
