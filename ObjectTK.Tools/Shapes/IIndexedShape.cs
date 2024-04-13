//
// IndexedShape.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using MINNOVAA.ObjectTK.Buffers;
using OpenTK.Graphics.OpenGL;

namespace MINNOVAA.ObjectTK.Tools.Shapes
{
    public interface IIndexedShape : IShape
    {
        uint[] Indices { get; set; }
        Buffer<uint> IndexBuffer { get; set; }

        
      
    }
}