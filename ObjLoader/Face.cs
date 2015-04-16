using MyGl.GlMath;
using MyGl.Obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGl.ObjLoader
{
    public class Face
    {
        private int[] vertices = new int[0];
        private int[] uvs = new int[0];
        private int[] normals = new int[0];

        public int VertrexCount
        {
            get { return vertices.Count(); }
        }

        public int GetVertexIndex(int index)
        {
            return vertices[index];
        }

        public int GetUvIndex(int index)
        {
            return uvs[index];
        }

        public int GetNormalIndex(int index)
        {
            return normals[index];
        }

        public GlVector GetVertex(Model model, int index)
        {
            return model.GetVertex(GetVertexIndex(index));
        }

        public GlVector GetUv(Model model, int index)
        {
            return model.GetUv(GetUvIndex(index));
        }

        public GlVector GetNormal(Model model, int index)
        {
            return model.GetNormal(GetNormalIndex(index));
        }

        public void AddVertex(int vertexIndex, int uvIndex, int normalIndex)
        {
            Array.Resize(ref vertices, vertices.Count() + 1);
            vertices[vertices.Count() - 1] = vertexIndex;
            Array.Resize(ref uvs, uvs.Count() + 1);
            uvs[uvs.Count() - 1] = uvIndex;
            Array.Resize(ref normals, normals.Count() + 1);
            normals[normals.Count() - 1] = normalIndex;
        }
    }
}
