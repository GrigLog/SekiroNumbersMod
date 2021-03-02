using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace SekiroNumbersMod.Scripts.Data {
    public class Matrix {
        public const double DoubleComparisonDelta = 0.0000001;
        private double[,] data;
        private double precalculatedDeterminant = double.NaN;

        private int m;
        public int M;
        private int n;
        public int N;
        public bool IsSquare { get => this.M == this.N; }

        public static Matrix getViewMatrix(V3 eye, V3 lookpt) {
            V3 up = new V3(0, 1, 0);
            V3 zaxis = (lookpt - eye).normalize();
            V3 xaxis = V3.cross(up, zaxis).normalize();
            V3 yaxis = -V3.cross(zaxis, xaxis).normalize();
            /*Matrix res = new Matrix(4, 4, new double[4,4] {
                { xaxis.x, yaxis.x, zaxis.x, 0 },
                { xaxis.y, yaxis.y, zaxis.y, 0 },
                { xaxis.z, yaxis.z, zaxis.z, 0 },
                { -(xaxis * eye), -(yaxis * eye), -(zaxis * eye), 1 }
            });*/

            Matrix res = new Matrix(4, 4, new double[4, 4] {
                { xaxis.x, xaxis.y, xaxis.z, -(xaxis * eye) },
                { yaxis.x, yaxis.y, yaxis.z, -(yaxis * eye) },
                { zaxis.x, zaxis.y, zaxis.z, -(zaxis * eye) },
                { 0, 0, 0, 1 }
            });
            return res;

        }

        public static Matrix getRotationY(double a) {
            return new Matrix(4, 4, new double[4, 4] {
                {Cos(a),  0, Sin(a), 0},
                {  0,     1,   0,    0 },
                {-Sin(a), 0, Cos(a), 0 },
                {  0,     0,   0,    1 }
            });
        }

        public static V3 rotateY(V3 v, double a) {
            return new V3(v.x * Cos(a) + v.z * Sin(a), v.y, v.x * -Sin(a) + v.z * Cos(a));
        }

        public static V3 getProjected(V3 v) {
            double near = 1;
            double far = 1000;
            double fovx = 1.347; //in radians?...
            double fovy = 0.5;
            return new V3(v.x / Tan(fovx / 2) / v.z, v.y / Tan(fovy / 2) / v.z, ((far + near) + 2*near*far/-v.z) / (near - far));
        }
        /*public static Matrix getProjMatrix() {
            double near = 1;
            double far = 1000;
            Matrix res = new Matrix(4, 4, new double[,] {

            });
        }*/

        public override string ToString() {
            string res = "[";
            for (int y = 0; y < M; y++) {
                res += "[";
                for (int x = 0; x < N; x++) {
                    res += data[y, x] + " ";
                }
                res = res.Trim();
                res += "]\n";
            }
            res = res.Trim();
            res += "]";
            return res;
        }

        public Matrix(int m, int n) {
            this.m = m;
            this.M = m;
            this.n = n;
            this.N = n;
            this.data = new double[m, n];
            this.ProcessFunctionOverData((i, j) => this.data[i, j] = 0);
        }

        public Matrix(int m, int n, double[,] arr) {
            this.M = m;
            this.m = m;
            this.N = n;
            this.n = n;
            this.data = arr;
        }

        public void ProcessFunctionOverData(Action<int, int> func) {
            for (var i = 0; i < this.M; i++) {
                for (var j = 0; j < this.N; j++) {
                    func(i, j);
                }
            }
        }

        public static Matrix CreateIdentityMatrix(int n) {
            var result = new Matrix(n, n);
            for (var i = 0; i < n; i++) {
                result[i, i] = 1;
            }
            return result;
        }

        public Matrix CreateTransposeMatrix() {
            var result = new Matrix(this.N, this.M);
            result.ProcessFunctionOverData((i, j) => result[i, j] = this[j, i]);
            return result;
        }

        

        public double this[int x, int y] {
            get {
                return this.data[x, y];
            }
            set {
                this.data[x, y] = value;
                this.precalculatedDeterminant = double.NaN;
            }
        }

        public double CalculateDeterminant() {
            if (!double.IsNaN(this.precalculatedDeterminant)) {
                return this.precalculatedDeterminant;
            }
            if (!this.IsSquare) {
                throw new InvalidOperationException("determinant can be calculated only for square matrix");
            }
            if (this.N == 2) {
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }
            double result = 0;
            for (var j = 0; j < this.N; j++) {
                result += (j % 2 == 1 ? 1 : -1) * this[1, j] *
                    this.CreateMatrixWithoutColumn(j).CreateMatrixWithoutRow(1).CalculateDeterminant();
            }
            this.precalculatedDeterminant = result;
            return result;
        }

        public Matrix CreateInvertibleMatrix() {
            if (this.M != this.N)
                return null;
            var determinant = CalculateDeterminant();
            if (Math.Abs(determinant) < DoubleComparisonDelta)
                return null;

            var result = new Matrix(M, M);
            ProcessFunctionOverData((i, j) =>
            {
                result[i, j] = ((i + j) % 2 == 1 ? -1 : 1) * CalculateMinor(i, j) / determinant;
            });

            result = result.CreateTransposeMatrix();
            return result;
        }

        private double CalculateMinor(int i, int j) {
            return CreateMatrixWithoutColumn(j).CreateMatrixWithoutRow(i).CalculateDeterminant();
        }

        private Matrix CreateMatrixWithoutRow(int row) {
            if (row < 0 || row >= this.M) {
                throw new ArgumentException("invalid row index");
            }
            var result = new Matrix(this.M - 1, this.N);
            result.ProcessFunctionOverData((i, j) => result[i, j] = i < row ? this[i, j] : this[i + 1, j]);
            return result;
        }

        private Matrix CreateMatrixWithoutColumn(int column) {
            if (column < 0 || column >= this.N) {
                throw new ArgumentException("invalid column index");
            }
            var result = new Matrix(this.M, this.N - 1);
            result.ProcessFunctionOverData((i, j) => result[i, j] = j < column ? this[i, j] : this[i, j + 1]);
            return result;
        }

        public static Matrix operator *(Matrix matrix, double value) {
            var result = new Matrix(matrix.M, matrix.N);
            result.ProcessFunctionOverData((i, j) => result[i, j] = matrix[i, j] * value);
            return result;
        }

        public static Matrix operator *(Matrix matrix, Matrix matrix2) {
            if (matrix.N != matrix2.M) {
               Console.WriteLine("matrixes can not be multiplied");
            }
            var result = new Matrix(matrix.M, matrix2.N);
            result.ProcessFunctionOverData((i, j) =>
            {
                for (var k = 0; k < matrix.N; k++) {
                    result[i, j] += matrix[i, k] * matrix2[k, j];
                }
            });
            return result;
        }

        public static Matrix operator +(Matrix matrix, Matrix matrix2) {
            if (matrix.M != matrix2.M || matrix.N != matrix2.N) {
                throw new ArgumentException("matrixes dimensions should be equal");
            }
            var result = new Matrix(matrix.M, matrix.N);
            result.ProcessFunctionOverData((i, j) => result[i, j] = matrix[i, j] + matrix2[i, j]);
            return result;
        }

        public static Matrix operator -(Matrix matrix, Matrix matrix2) {
            return matrix + (matrix2 * -1);
        }
    }
}
