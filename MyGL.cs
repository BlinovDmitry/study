using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ObjLoader.Loader.Loaders;
using System.IO;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;
using System.Drawing;
using MyGl.GlMath;

namespace MyGl
{
    public class Renderer
    {

        public LoadResult LoadObjFile(string fileName)
        {
            ObjLoaderFactory objLoaderFactory = new ObjLoaderFactory();         
            System.IO.FileStream fileStream = new FileStream(fileName, FileMode.Open);
            return objLoaderFactory.Create().Load(fileStream);
        }
       
        public void DrawPoint(Graphics graphics, int x, int y, Color color)
        {
            System.Drawing.Pen pen = new Pen(color, 1);
            graphics.DrawLine(pen, x, graphics.VisibleClipBounds.Bottom - y, x + 0.5f, graphics.VisibleClipBounds.Bottom - y);                        
        }

        public void DrawLine(Graphics graphics, int x1, int y1, int x2, int y2, Color color)
        {
            int m1, m2, n1, n2;
            bool Transposed = Math.Abs(x2 - x1) < Math.Abs(y2 - y1);
            if (Transposed)
            {
                n1 = y1; n2 = y2; m1 = x1; m2 = x2;
            }
            else
            {
                n1 = x1; n2 = x2; m1 = y1; m2 = y2;
            }
            if (n1 > n2)
            {
                int tmp = m1;
                m1 = m2;
                m2 = tmp;
                tmp = n1;
                n1 = n2;
                n2 = tmp;
            }

            int dm = m2 - m1;
            int dn = n2 - n1;
            int dn2 = 2 * dn;
            float dError2 = Math.Abs(dm) * 2;
            float error2 = 0;
            int m = m1;

            for (int n = n1; n <= n2; n += 1)
            {
                if (Transposed)
                    DrawPoint(graphics, m, n, color);
                else
                    DrawPoint(graphics, n, m, color);
                error2 += dError2;
                if (Math.Abs(error2) > Math.Abs(dn))
                {
                    m += (m2 > m1 ? 1 : -1);
                    error2 -= dn2;
                }
            }

        }

        public void DrawTriangle(Graphics graphics, int x1, int y1, int x2, int y2, int x3, int y3, Color color)
        {
            DrawLine(graphics, x1, y1, x2, y2, color);
            DrawLine(graphics, x2, y2, x3, y3, color);
            DrawLine(graphics, x3, y3, x1, y1, color);
        }

        public void Swap<T>(ref T param1, ref T param2)
        {
            T tmp = param1;
            param1 = param2;
            param2 = tmp;
        }

        public void FillTriangle(Graphics graphics, GlVector p1, GlVector p2, GlVector p3, GlVector uv1, GlVector uv2, GlVector uv3, double intensity, Bitmap texture, int[,] zBuffer)
        {            
            if (p1.Y > p2.Y)
            {
                Swap(ref p1, ref p2);
                Swap(ref uv1, ref uv2);
            }
            if (p1.Y > p3.Y)
            {
                Swap(ref p1, ref p3);
                Swap(ref uv1, ref uv3);
            }
            if (p2.Y > p3.Y)
            {
                Swap(ref p2, ref p3);
                Swap(ref uv2, ref uv3);
            }
            
            int totalHeight = p3.Y - p1.Y;

            GlVector secondLineP1 = p1;
            GlVector secondLineP2 = p2;
            GlVector secondLineUV1 = uv1;
            GlVector secondLineUV2 = uv2;
            for (int y = p1.Y; y <= p3.Y; y++) 
            {                
                if (y > p2.Y)
                {
                    secondLineP1 = p2;
                    secondLineP2 = p3;
                    secondLineUV1 = uv2;
                    secondLineUV2 = uv3;
                }
                if (secondLineP1.Y != secondLineP2.Y)
                {
                    double alpha = (double)(y - p1.Y) / totalHeight;
                    double beta = (double)(y - secondLineP1.Y) / (secondLineP2.Y - secondLineP1.Y);
                    GlVector PLeft = p1 + (p3 - p1) * alpha;
                    GlVector PRight = secondLineP1 + (secondLineP2 - secondLineP1) * beta;
                    GlVector UVLeft = uv1 + (uv3 - uv1) * alpha;
                    GlVector UVRight = secondLineUV1 + (secondLineUV2 - secondLineUV1) * beta;

                    if (PLeft.X > PRight.X)
                    {
                        Swap(ref PLeft, ref PRight);
                        Swap(ref UVLeft, ref UVRight);
                    }
                    int z = 0;
                    if (PLeft.X != PRight.X)
                        for (int x = PLeft.X; x <= PRight.X; x++)
                        {
                            double gamma = (double)(x - PLeft.X) / (PRight.X - PLeft.X);
                            z = PLeft.Z + (int)Math.Round((PRight.Z - PLeft.Z) * gamma);
                            if ((x >= zBuffer.GetLowerBound(0) && x <= zBuffer.GetUpperBound(0)) && (y >= zBuffer.GetLowerBound(1) && y <= zBuffer.GetUpperBound(1)))
                                if (zBuffer[x, y] < z)
                                {
                                    GlVector UV = UVLeft + (UVRight - UVLeft) * gamma;
                                    Color color = texture.GetPixel((int)Math.Round(UV.U * texture.Width), (int)Math.Round((1-UV.V) * texture.Height));
                                    //Color color = Color.White;
                                    color = Color.FromArgb((int)Math.Round(intensity * color.R), (int)Math.Round(intensity * color.G), (int)Math.Round(intensity * color.B));
                                    DrawPoint(graphics, x, y, color);
                                    zBuffer[x, y] = z;
                                }
                        }
                }
            }
        }

    }
}
