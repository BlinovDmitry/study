using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGl.GlMath
{
    public class GlMatrix
    {
        private double[,] items;

        
        public GlMatrix(int rowCount, int columnCount)
        {
            items = new double[rowCount, columnCount];
        }

        public GlMatrix(double[,] newItems)
        {
            items = newItems;
        }

        public static GlMatrix CreateIdentity(int rank)
        {
            GlMatrix result = new GlMatrix(rank, rank);
            for (int i = 0; i < rank; i++)
                result[i, i] = 1;
            return result;
        }

        public static GlMatrix CreatePerspective3D(double zCameraDistance)
        {
            return new GlMatrix(new double[,] {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 0, -1/zCameraDistance, 1}
            });
        }

        public static GlMatrix CreateScale3D(double scaleX, double scaleY, double scaleZ)
        {
            return new GlMatrix(new double[,] {
                {scaleX, 0, 0, 0},
                {0, scaleY, 0, 0},
                {0, 0, scaleZ, 0},
                {0, 0, 0, 1}
            });
        }

        public static GlMatrix CreateMove3D(double moveX, double moveY, double moveZ)
        {
            return new GlMatrix(new double[,] {
                {1, 0, 0, moveX},
                {0, 1, 0, moveY},
                {0, 0, 1, moveZ},
                {0, 0, 0, 1}
            });
        }

        public static GlMatrix CreateView(GlVector cameraPos, GlVector lookAt, GlVector upDirection)
        {           
            GlVector z = (cameraPos - lookAt).Normalize();
            GlVector x = (upDirection * z).Normalize();
            GlVector y = (z * x).Normalize();
            GlMatrix mInv = CreateIdentity(4);
            GlMatrix tr = CreateIdentity(4);
            for (int i = 0; i < 3; i++ )
            {
                mInv[0, i] = x[i];
                mInv[1, i] = y[i];
                mInv[2, i] = z[i];
                tr[i, 3] = -lookAt[i];
            }
            return mInv * tr;
        }

        public double this[int rowNumber, int columnNumber]
        {
            get { return items[rowNumber, columnNumber]; }
            set { items[rowNumber, columnNumber] = value; }
        }

        public int RowCount
        {
            get { return items.GetUpperBound(0) - items.GetLowerBound(0) + 1; }
        }

        public int ColumnCount
        {
            get { return items.GetUpperBound(1) - items.GetLowerBound(1) + 1; }
        }

        public GlVector GetRow(int rowIndex)
        {
            GlVector result = new GlVector(ColumnCount);
            for (int i = 0; i < ColumnCount; i++)
            {
                result[i] = this[rowIndex, i];
            }
            return result;
        }

        public void SetRow(int rowIndex, GlVector row)
        {
            for (int i = 0; i < ColumnCount; i++)
            {
                this[rowIndex, i] = row[i];
            }
        }

        public GlVector GetColumn(int columnIndex)
        {
            GlVector result = new GlVector(RowCount);
            for (int i = 0; i < RowCount; i++)
            {
                result[i] = this[i, columnIndex];
            }
            return result;
        }

        public void SetColumn(int columnIndex, GlVector column)
        {
            for (int i = 0; i < RowCount; i++)
            {
                this[i, columnIndex] = column[i];
            }
        }

        public static GlMatrix operator *(GlMatrix matrixA, GlMatrix matrixB)
        {
            if (matrixA.ColumnCount != matrixB.RowCount)
            {
                throw new Exception("First matrix column count and second matrix row count must be equal!");
            }

            GlMatrix result = new GlMatrix(matrixA.RowCount, matrixB.ColumnCount);

            for (int i = 0; i < result.RowCount; i++ )
            {
                for (int j = 0; j < result.ColumnCount; j++)
                {
                    result[i, j] = matrixA.GetRow(i) ^ matrixB.GetColumn(j);
                }
            }
            return result;
        }

        public static GlMatrix operator *(double scalar, GlMatrix matrix)
        {
            GlMatrix result = matrix.Clone();
            for (int i = 0; i < result.RowCount; i++)
            {
                for (int j = 0; j < result.ColumnCount; j++)
                {
                    result[i, j] *= scalar;
                }
            }
            return result;
        }

        public static GlMatrix operator *(GlMatrix matrix, double scalar)
        {
            return scalar * matrix;
        }

        public double GetDeterminant()
        {
            if (RowCount != ColumnCount)
                throw new Exception("Matrix must be square to cald determinant!");
            double result = 0;

            if (RowCount == 1)
                result = this[0, 0];
            else
                for (int i = 0; i < ColumnCount; i++)
                {
                    result += this[0, i] * GetAlgAddition(0, i).GetDeterminant();
                }
            return result;
        }

        public GlMatrix Clone()
        {
            GlMatrix result = new GlMatrix(RowCount, ColumnCount);
            for (int i=0; i < RowCount; i++)
                for (int j=0; j < ColumnCount; j++)
                {
                    result[i, j] = this[i, j];
                }
            return result;
        }

        public GlMatrix GetMinor(int rowIndex, int columnIndex)
        {
            GlMatrix result = new GlMatrix(RowCount-1, ColumnCount-1);
            int k = 0;
            int l;
            for (int i = 0; i < RowCount; i++)
                if (i != rowIndex)
                {
                    l = 0;
                    for (int j = 0; j < ColumnCount; j++)
                        if (j != columnIndex)
                        {
                            result[k, l] = this[i, j];
                            l++;
                        }                    
                    k++;
                }
            return result;
        }

        public GlMatrix GetAlgAddition(int rowIndex, int columnIndex)
        {
            return GetMinor(rowIndex, columnIndex) * Math.Pow(-1, rowIndex + columnIndex);
        }

        public GlMatrix AddRow(GlVector row)
        {
            GlMatrix result = new GlMatrix(RowCount+1, ColumnCount);
            for (int i = 0; i < RowCount; i++)
                for (int j = 0; j < ColumnCount; j++)
                    result[i, j] = this[i, j];
            for (int j = 0; j < ColumnCount; j++)
                result[RowCount, j] = row[j];
            return result;
        }
            
    }
}
