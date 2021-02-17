using System;
using System.Runtime.Serialization;

namespace MatrixLibrary
{
    [Serializable]
    public class MatrixException : Exception
    {
        public MatrixException()
        {
        }

        public MatrixException(string message) : base(message)
        {
        }

        public MatrixException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MatrixException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class Matrix : ICloneable
    {
        /// <summary>
        /// Number of rows.
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// Number of columns.
        /// </summary>
        public int Columns { get; }
        
        /// <summary>
        /// Gets an array of floating-point values that represents the elements of this Matrix.
        /// </summary>
        public double[,] Array { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Matrix(int rows, int columns)
        {
            if (rows < 0) 
                throw new ArgumentOutOfRangeException(nameof(rows), "rows cant be less then zero");

            if (columns < 0) 
                throw new ArgumentOutOfRangeException(nameof(columns), "columns cant be less then zero");

            Rows = rows;
            Columns = columns;
            Array = new double[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Array[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with the specified elements.
        /// </summary>
        /// <param name="array">An array of floating-point values that represents the elements of this Matrix.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Matrix(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), "array cant be null");

            Rows = array.GetLength(0);
            Columns = array.GetLength(1);
            Array = array;
        }

        /// <summary>
        /// Allows instances of a Matrix to be indexed just like arrays.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <exception cref="ArgumentException"></exception>
        public double this[int row, int column]
        {
            get
            {
                if (row < 0 || row > Array.GetLength(0))
                    throw new ArgumentException(nameof(row));

                if (column < 0 || column > Array.GetLength(1))
                    throw new ArgumentException(nameof(column));

                return Array[row, column];
            }
            set
            {
                if (row < 0 || row > Array.GetLength(0))
                    throw new ArgumentException(nameof(row));

                if (column < 0 || column > Array.GetLength(1))
                    throw new ArgumentException(nameof(column));

                Array[row, column] = value;
            }
        }

        /// <summary>
        /// Creates a deep copy of this Matrix.
        /// </summary>
        /// <returns>A deep copy of the current object.</returns>
        public object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns>New <see cref="Matrix"/> object which is sum of two matrices.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1 == null)
                throw new ArgumentNullException(nameof(matrix1));

            if(matrix2 == null)
                throw new ArgumentNullException(nameof(matrix2));

            if (matrix1.Rows != matrix2.Rows || matrix1.Columns != matrix2.Columns)
                throw new MatrixException($"Cannot compute sum, matrix dimensions do not match:\n\t" +
                                          $"matrix1 : {matrix1.Rows}x{matrix1.Columns}\n\t" +
                                          $"matrix2 : {matrix2.Rows}x{matrix2.Columns}");

            var result = new Matrix(matrix1.Rows, matrix1.Columns);

            for (int i = 0; i < matrix1.Rows; i++)
            {
                for (int j = 0; j < matrix2.Columns; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns>New <see cref="Matrix"/> object which is subtraction of two matrices</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1 == null)
                throw new ArgumentNullException(nameof(matrix1));

            if (matrix2 == null)
                throw new ArgumentNullException(nameof(matrix2));

            if (matrix1.Rows != matrix2.Rows || matrix1.Columns != matrix2.Columns)
                throw new MatrixException($"Cannot compute subtract, matrix dimensions do not match:\n\t" +
                                          $"matrix1 : {matrix1.Rows}x{matrix1.Columns}\n\t" +
                                          $"matrix2 : {matrix2.Rows}x{matrix2.Columns}");

            var result = new Matrix(matrix1.Rows, matrix1.Columns);

            for (int i = 0; i < matrix1.Rows; i++)
            {
                for (int j = 0; j < matrix2.Columns; j++)
                {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns>New <see cref="Matrix"/> object which is multiplication of two matrices.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1 == null)
                throw new ArgumentNullException(nameof(matrix1));

            if (matrix2 == null)
                throw new ArgumentNullException(nameof(matrix2));

            if (matrix1.Columns != matrix2.Rows)
                throw new MatrixException($"Cannot compute product, matrix dimensions do not match:\n\t" +
                                          $"matrix1 : {matrix1.Rows}x{matrix1.Columns}\n\t" +
                                          $"matrix2 : {matrix2.Rows}x{matrix2.Columns}");

            var result = new Matrix(matrix1.Rows, matrix2.Columns);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    for (int k = 0; k < matrix1.Columns; k++)
                    {
                        result[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adds <see cref="Matrix"/> to the current matrix.
        /// </summary>
        /// <param name="matrix"><see cref="Matrix"/> for adding.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public Matrix Add(Matrix matrix)
        {
            return this + matrix;
        }

        /// <summary>
        /// Subtracts <see cref="Matrix"/> from the current matrix.
        /// </summary>
        /// <param name="matrix"><see cref="Matrix"/> for subtracting.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public Matrix Subtract(Matrix matrix)
        {
            return this - matrix;
        }

        /// <summary>
        /// Multiplies <see cref="Matrix"/> on the current matrix.
        /// </summary>
        /// <param name="matrix"><see cref="Matrix"/> for multiplying.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MatrixException"></exception>
        public Matrix Multiply(Matrix matrix)
        {
            return this * matrix;
        }

        /// <summary>
        /// Tests if <see cref="Matrix"/> is identical to this Matrix.
        /// </summary>
        /// <param name="obj">Object to compare with. (Can be null)</param>
        /// <returns>True if matrices are equal, false if are not equal.</returns>
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
