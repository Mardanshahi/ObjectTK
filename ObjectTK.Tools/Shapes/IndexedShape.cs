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
    public abstract class IndexedShape  : Shape , IIndexedShape
    {
        public uint[] Indices { get; set; }
        public Buffer<uint> IndexBuffer { get; set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            IndexBuffer = new Buffer<uint>();
            IndexBuffer.Init(BufferTarget.ElementArrayBuffer, Indices);
        }

        protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
            if (!manual) return;
            if (IndexBuffer != null) IndexBuffer.Dispose();
        }
    }
}