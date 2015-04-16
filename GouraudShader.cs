using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGl
{
    class GouraudShader : IShader
    {

        public GlMath.GlVector Vertex(ObjLoader.Face face, int vertexIndex)
        {
            throw new NotImplementedException();
        }

        public bool Fragment(GlMath.GlVector bar, System.Drawing.Color color)
        {
            throw new NotImplementedException();
        }
    }
}
