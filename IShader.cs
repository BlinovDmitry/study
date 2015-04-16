using MyGl.GlMath;
using MyGl.ObjLoader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGl
{
    public interface IShader
    {
        public GlVector Vertex(Face face, int vertexIndex);

        public bool Fragment(GlVector bar, Color color);
    }
}
