using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGl.GlMath;
using System.IO;
using MyGl.ObjLoader;
using System.Globalization;

namespace MyGl.Obj
{
    public class Model
    {
        private GlVector[] vertices = new GlVector[0];
        private GlVector[] uvs = new GlVector[0];
        private GlVector[] normals = new GlVector[0];
        private Face[] faces = new Face[0];

        public int FaceCount
        {
            get { return faces.Count(); }
        }

        public GlVector GetVertex(int index)
        {
            return vertices[index];
        }

        public GlVector GetUv(int index)
        {
            return uvs[index];
        }

        public GlVector GetNormal(int index)
        {
            return normals[index];
        }

        public Face GetFace(int index)
        {
            return faces[index];
        }

        public Model(string fileName)
        {            
            StreamReader file = new StreamReader(fileName);
            try
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Replace("  ", " ");
                    line = line.Replace("  ", " ");
                    string[] parts = line.Split(' ');
                    if (parts.Count() > 0)
                    {
                        double x;
                        double y;
                        double z;
                        switch (parts[0])
                        {
                            case "v":
                                Array.Resize(ref vertices, vertices.Count() + 1);
                                if (!Double.TryParse(parts[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out x))
                                    x = 0;
                                if (!Double.TryParse(parts[2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out y))
                                    y = 0;
                                if (!Double.TryParse(parts[3], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out z))
                                    z = 0;
                                vertices[vertices.Count() - 1] = new GlVector(new double[] { x, y, z });
                                break;
                            case "vt":
                                Array.Resize(ref uvs, uvs.Count() + 1);
                                if (!Double.TryParse(parts[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out x))
                                    x = 0;
                                if (!Double.TryParse(parts[2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out y))
                                    y = 0;
                                if (!Double.TryParse(parts[3], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out z))
                                    z = 0;
                                uvs[uvs.Count() - 1] = new GlVector(new double[] { x, y, z });
                                break;
                            case "vn":
                                Array.Resize(ref normals, normals.Count() + 1);
                                if (!Double.TryParse(parts[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out x))
                                    x = 0;
                                if (!Double.TryParse(parts[2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out y))
                                    y = 0;
                                if (!Double.TryParse(parts[3], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out z))
                                    z = 0;
                                normals[normals.Count() - 1] = new GlVector(new double[] { x, y, z });
                                break;
                            case "f":
                                Array.Resize(ref faces, faces.Count() + 1);
                                faces[faces.Count() - 1] = new Face();
                                for (int i = 1; i < parts.Count(); i++)
                                {
                                    string[] subparts = parts[i].Split('/');                                    
                                    if (subparts.Count() > 0)
                                    {
                                        int vertexIndex = 0;
                                        int uvIndex = 0;
                                        int normalIndex = 0;
                                        if (!Int32.TryParse(subparts[0], out vertexIndex))
                                            vertexIndex = 0;
                                        
                                        if (subparts.Count() > 1)
                                            if (!Int32.TryParse(subparts[1], out uvIndex))
                                                uvIndex = 0;

                                        if (subparts.Count() > 2)
                                            if (!Int32.TryParse(subparts[2], out normalIndex))
                                                normalIndex = 0;
                                        faces[faces.Count() - 1].AddVertex(vertexIndex-1, uvIndex-1, normalIndex-1);
                                    }
                                        
                                }
                                break;
                        }
                    }
                }
            }
            finally
            {
                file.Close();
            }
        }
    }
}
