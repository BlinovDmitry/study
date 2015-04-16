using MyGl.GlMath;
using ObjLoader.Loader.Loaders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGl.Obj;
using MyGl.ObjLoader;

namespace MyGl
{
    public partial class Form1 : Form
    {
        private Bitmap bitmap;
        private Graphics graphics;
        private Renderer gl = new Renderer();

        private LoadResult model;
        private Model headModel;
        private Bitmap texture;

        private GlVector lightDirection = new GlVector(3);
        private GlVector cameraPos = new GlVector(new double[3] { 0.3, 0.2, 2 });
        private GlVector lookAt = new GlVector(new double[3] { 0, 0, 0 });
        private GlVector cameraUp = new GlVector(new double[3] { 0, 1, 0 });

        public Form1()
        {
            InitializeComponent();

            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(bitmap);
            pictureBox.Image = bitmap;


            /*gl.FillTriangle(graphics, 10, 70, 50, 160, 70, 80, Color.Blue);
            gl.FillTriangle(graphics, 180, 50, 150, 1, 70, 180, Color.Blue);
            gl.FillTriangle(graphics, 180, 150, 120, 160, 130, 180, Color.Blue);


            gl.DrawTriangle(graphics, 10, 70, 50, 160, 70, 80, Color.Red);
            gl.DrawTriangle(graphics, 180, 50, 150, 1, 70, 180, Color.White);
            gl.DrawTriangle(graphics, 180, 150, 120, 160, 130, 180, Color.Green);*/


            //model = gl.LoadObjFile("D:\\Dev\\C#\\MyGl\\MyGl\\Resources\\african_head.obj");
            headModel = new Model("D:\\Dev\\C#\\MyGl\\MyGl\\Resources\\african_head.obj");
            texture = new Bitmap("D:\\Dev\\C#\\MyGl\\MyGl\\Resources\\african_head_diffuse.bmp");

            lightDirection[0] = -0.5;
            lightDirection[1] = 0.1;
            lightDirection[2] = -1;
            lightDirection = lightDirection.Normalize();

            //Render();            
        }

        void Render()
        {
            int[,] zBuffer = new int[pictureBox.Width, pictureBox.Height];
            graphics.Clear(Color.Black);
            
            /*GlVector p1 = new GlVector(3);
            p1[0] = 250;
            p1[1] = 500;
            p1[2] = 0;
            GlVector uv1 = new GlVector(2);
            uv1[0] = 0.350;
            uv1[1] = 0.200;

            GlVector p2 = new GlVector(3);
            p2[0] = 0;
            p2[1] = 0;
            p2[2] = 0;
            GlVector uv2 = new GlVector(2);
            uv2[0] = 0.200;
            uv2[1] = 0.800;

            GlVector p3 = new GlVector(3);
            p3[0] = 500;
            p3[1] = 0;
            p3[2] = 0;
            GlVector uv3 = new GlVector(2);
            uv3[0] = 0.500;
            uv3[1] = 0.800;*/

            //gl.FillTriangle(graphics, p1, p2, p3, uv1, uv2, uv3, 1, texture, zBuffer);
            DrawSimpleLightened(gl, graphics, headModel, texture, 300, (int)(pictureBox.Width / 2), (int)(pictureBox.Height / 2), zBuffer);
            pictureBox.Refresh();
        }

        public void DrawWired(Renderer renderer, Graphics graphics, LoadResult loadResult, float scale, int baseX, int baseY, Color color)
        {
            /*foreach (Group group in loadResult.Groups)
            {
                foreach (Face face in group.Faces)
                {
                    Vertex lastVertex = loadResult.Vertices[face[face.Count - 1].VertexIndex - 1];
                    for (int i = 0; i < face.Count; i++)
                    {
                        Vertex vertex = loadResult.Vertices[face[i].VertexIndex - 1];
                        renderer.DrawLine(graphics, (int)Math.Round(lastVertex.X * scale) + baseX, (int)Math.Round(lastVertex.Y * scale) + baseY, (int)Math.Round(vertex.X * scale) + baseX, (int)Math.Round(vertex.Y * scale) + baseY, color);
                        lastVertex = vertex;
                    }
                }
            }*/
        }

        public void DrawFlat(Renderer renderer, Graphics graphics, LoadResult loadResult, float scale, int baseX, int baseY, Color color)
        {
            /*foreach (Group group in loadResult.Groups)
            {
                foreach (Face face in group.Faces)
                {
                    Point point1 = new Point((int)Math.Round(loadResult.Vertices[face[0].VertexIndex - 1].X * scale) + baseX, (int)Math.Round(loadResult.Vertices[face[0].VertexIndex - 1].Y * scale) + baseY);
                    Point point2 = new Point((int)Math.Round(loadResult.Vertices[face[1].VertexIndex - 1].X * scale) + baseX, (int)Math.Round(loadResult.Vertices[face[1].VertexIndex - 1].Y * scale) + baseY);
                    Point point3 = new Point((int)Math.Round(loadResult.Vertices[face[2].VertexIndex - 1].X * scale) + baseX, (int)Math.Round(loadResult.Vertices[face[2].VertexIndex - 1].Y * scale) + baseY);
                    //FillTriangle(graphics, point1, point2, point3, color);                    
                }

            }*/

        }

        public void DrawSimpleLightened(Renderer renderer, Graphics graphics, Model model, Bitmap texture, float scale, int baseX, int baseY, int[,] zBuffer)
        {
            for (int i = zBuffer.GetLowerBound(0); i <= zBuffer.GetUpperBound(0); i++)
                for (int j = zBuffer.GetLowerBound(1); j <= zBuffer.GetUpperBound(1); j++)
                {
                    zBuffer[i, j] = int.MinValue;
                }

            GlMatrix worldMatrix = new GlMatrix(3, 3);
            GlMatrix textureMatrix = new GlMatrix(2, 3);
            GlMatrix projectionMatrix = GlMatrix.CreatePerspective3D((cameraPos - lookAt).Length);
            GlMatrix viewMatrix = GlMatrix.CreateView(cameraPos, lookAt, cameraUp);
            GlMatrix viewportMatix = GlMatrix.CreateMove3D(baseX, baseY, 0) * GlMatrix.CreateScale3D(scale, scale, scale);
            GlMatrix preparedMatix = viewportMatix * projectionMatrix * viewMatrix;

            for (int i = 0; i < model.FaceCount; i++ )
            {
                Face face = model.GetFace(i);
                if (face.VertrexCount == 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        worldMatrix.SetColumn(j, face.GetVertex(model, j));
                        textureMatrix.SetColumn(j, face.GetUv(model, j));
                    }
                    GlVector normal = (worldMatrix.GetColumn(2) - worldMatrix.GetColumn(0)) * (worldMatrix.GetColumn(1) - worldMatrix.GetColumn(0));
                    normal = normal.Normalize();
                    double intensity = (normal ^ lightDirection);

                    GlMatrix resultMatrix = preparedMatix * worldMatrix.AddRow(new GlVector(new double[] { 1, 1, 1 }));
                    if (intensity > 0)
                        renderer.FillTriangle(graphics, resultMatrix.GetColumn(0) / resultMatrix.GetColumn(0)[3], resultMatrix.GetColumn(1) / resultMatrix.GetColumn(1)[3], resultMatrix.GetColumn(2) / resultMatrix.GetColumn(2)[3], textureMatrix.GetColumn(0), textureMatrix.GetColumn(1), textureMatrix.GetColumn(2), intensity, texture, zBuffer);
                }
            }
            
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            GlVector mousePos = new GlVector(3);
            GlVector centerPos = new GlVector(3);
            mousePos[0] = e.X;
            mousePos[1] = pictureBox.Height - e.Y;
            mousePos[2] = 300;

            centerPos[0] = pictureBox.Width / 2;
            centerPos[1] = pictureBox.Height / 2;
            centerPos[2] = 0;

            lightDirection = (centerPos - mousePos).Normalize();            

            Render();
        }
    }
}
