using MINNOVAA.ObjectTK.Buffers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINNOVAA.ObjectTK.Tools.Shapes
{
    public interface ITexturedShape
    {
        Vector2[] TexCoords { get; set; }
        Buffer<Vector2> TexCoordBuffer { get; set; }
    }
}
