﻿//
// TexturedShape.cs
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
    public abstract class TexturedShape   : Shape , ITexturedShape
    {
        public Vector2[] TexCoords { get; set; }
        public Buffer<Vector2> TexCoordBuffer { get; set; }

        public override void UpdateBuffers()
        {
            base.UpdateBuffers();
            TexCoordBuffer = new Buffer<Vector2>();
            TexCoordBuffer.Init(BufferTarget.ArrayBuffer, TexCoords);
        }

        protected override void Dispose(bool manual)
        {
            base.Dispose(manual);
            if (TexCoordBuffer != null) TexCoordBuffer.Dispose();
        }
    }
}