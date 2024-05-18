using MINNOVAA.ObjectTK.Tools.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace MINNOVAA.ObjectTK.Tools.Shapes
{
    public class VolumeCube22 : IndexedShape
    {

        public VolumeCube22()
        {
            DefaultMode = PrimitiveType.TriangleStrip;

            // For cube we would need only 8 vertices but we have to
            // duplicate vertex for each face because texture coordinate
            // is different.
            Vertices = new[]{
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 1.0f),
                new Vector3(0.5f, 1.0f, 0.5f),
                new Vector3(0.5f, 1.0f, 1.0f),
                new Vector3(1.0f, 0.5f, 0.5f),
                new Vector3(1.0f, 0.5f, 1.0f),
                new Vector3(1.0f, 1.0f, 0.5f),
                new Vector3(1.0f, 1.0f, 1.0f)
                
            };

            Indices = new uint[] {
                1,5,7, 7,3,1, 0,2,6,
                6,4,0, 0,1,3, 3,2,0,
                7,5,4, 4,6,7, 2,3,7,
                7,6,2, 1,0,4, 4,5,1
            };
        }
    }
}
