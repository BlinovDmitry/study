using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGl.GlMath
{
    public class GlVector : List<double>
    {
        public int Size { get { return Count; } }

        public GlVector(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Add(0);
            }
        }

        public GlVector(double[] newItems)
        {
            for (int i = 0; i < newItems.Count(); i++)
            {
                Add(newItems[i]);
            }
        }
        
        public static void Swap(ref GlVector vector1, ref GlVector vector2)
        {
            GlVector tmp = vector1;
            vector1 = vector2;
            vector2 = tmp;
        }

        public Point AsPoint()
        {
            if (Size < 2)
                throw new Exception("Not enough vector elements");

            return new Point((int)Math.Round(this[0]), (int)Math.Round(this[1]));
        }

        public GlMatrix AsMatrix(bool column = true)
        {
            if (column)
            {
                GlMatrix result = new GlMatrix(Size, 1);
                result.SetColumn(0, this);
                return result;
            }
            else
            {
                GlMatrix result = new GlMatrix(1, Size);
                result.SetRow(0, this);
                return result;
            }
        }

        public static GlVector operator +(GlVector vector1, GlVector vector2)
        {
            GlVector result = new GlVector(0);
            for (int i = 0; i < Math.Max(vector1.Size, vector2.Size); i++)
            {
                if (i >= vector1.Size)
                    result.Add(vector2[i]);
                else if (i >= vector2.Size)
                    result.Add(vector1[i]);
                else
                    result.Add(vector1[i] + vector2[i]);
            }
            return result;
        }

        public static GlVector operator -(GlVector vector1, GlVector vector2)
        {
            GlVector result = new GlVector(0);
            for (int i = 0; i < Math.Max(vector1.Size, vector2.Size); i++)
            {
                if (i >= vector1.Size)
                    result.Add(-vector2[i]);
                else if (i >= vector2.Size)
                    result.Add(vector1[i]);
                else
                    result.Add(vector1[i] - vector2[i]);
            }
            return result;
        }

        public static double operator ^(GlVector vector1, GlVector vector2)
        {
            if (vector1.Size != vector2.Size)
                throw new Exception("Vectors have unequal length on multiplication");
            double result = 0;
            for (int i = 0; i < vector1.Size; i++ )
            {
                result += vector1[i] * vector2[i];
            }
            return result;
        }

        public static GlVector operator *(GlVector vector1, GlVector vector2)
        {
            if (vector1.Size != vector2.Size)
                throw new Exception("Vectors have unequal length on multiplication");
            if (vector1.Size != 3)
                throw new Exception("Unimplemented for this vertor size");
            GlVector result = new GlVector(vector1.Size);
            result[0] = vector1[1]*vector2[2]-vector1[2]*vector2[1];
            result[1] = vector1[2]*vector2[0]-vector1[0]*vector2[2];
            result[2] = vector1[0]*vector2[1]-vector1[1]*vector2[0];
            return result;
        }

        public static GlVector operator *(GlVector vector, double scalar)
        {
            GlVector result = new GlVector(vector.Size);
            for (int i = 0; i < vector.Size; i++)
            {
                result[i] = vector[i] * scalar;
            }
            return result;
        }

        public static GlVector operator *(double scalar, GlVector vector)
        {
            return vector * scalar;
        }    

        public double Length 
        {
            get 
            {
                double result = 0;
                for (int i = 0; i < Size; i++)
                    result += this[i] * this[i];
                return Math.Sqrt(result);
            }             
        }

        public static GlVector operator /(GlVector vector, double scalar)
        {
            GlVector result = new GlVector(vector.Size);
            for (int i = 0; i < vector.Size; i++)
            {
                result[i] = vector[i] / scalar;
            }
            return result;
        }

        public static GlVector operator /(double scalar, GlVector vector)
        {
            return vector / scalar;
        }


        public GlVector Normalize()
        {
            GlVector result = new GlVector(Size);
            if (Length != 0)
                for (int i = 0; i < Size; i++)
                    result[i] = this[i] / Length;
            return result;
        }

        public int X
        {
            get { return (int)Math.Round(this[0]); }
        }

        public int Y
        {
            get { return (int)Math.Round(this[1]); }
        }

        public int Z
        {
            get { return (int)Math.Round(this[2]); }
        }

        public double U
        {
            get { return this[0]; }
        }

        public double V
        {
            get { return this[1]; }
        }
    }
}
