//
// ColoredShape.cs
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
    public abstract class ColoredShape : IndexedShape , IColoredShape
    {
        public uint[] Colors { get; set; }
        public Buffer<uint> ColorBuffer { get; set; }

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