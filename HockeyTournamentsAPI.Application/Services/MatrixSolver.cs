using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HockeyTournamentsAPI.Application.Services
{
    public static class MatrixSolver
    {
        /// <summary>
        /// Решает систему линейных уравнений методом Гаусса
        /// </summary>
        /// <param name="matrix">Расширенная матрица системы (коэфф + свободные члены)</param>
        /// <returns>Массив решений или null, если система не имеет решений</returns>
        public static double[] Solve(double[,] matrix)
        {
            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1) - 1;

            // Прямой ход метода Гаусса
            for (int pivot = 0; pivot < rowCount; pivot++)
            {
                // Поиск строки с максимальным элементом в текущем столбце
                int maxRow = FindPivotRow(matrix, pivot, rowCount);

                if (Math.Abs(matrix[maxRow, pivot]) < 1e-10)
                    continue; // Пропускаем нулевые столбцы

                // Перестановка строк
                SwapRows(matrix, pivot, maxRow, colCount + 1);

                // Нормализация текущей строки
                NormalizeRow(matrix, pivot, colCount + 1);

                // Исключение переменной из других строк
                EliminateVariable(matrix, pivot, rowCount, colCount + 1);
            }

            // Проверка на совместность системы
            if (!IsConsistent(matrix, rowCount, colCount))
                return null;

            // Обратный ход - извлечение решений
            return ExtractSolutions(matrix, rowCount, colCount);
        }

        private static int FindPivotRow(double[,] matrix, int col, int rowCount)
        {
            int maxRow = col;
            for (int i = col + 1; i < rowCount; i++)
            {
                if (Math.Abs(matrix[i, col]) > Math.Abs(matrix[maxRow, col]))
                    maxRow = i;
            }
            return maxRow;
        }

        private static void SwapRows(double[,] matrix, int row1, int row2, int length)
        {
            for (int j = 0; j < length; j++)
            {
                double temp = matrix[row1, j];
                matrix[row1, j] = matrix[row2, j];
                matrix[row2, j] = temp;
            }
        }

        private static void NormalizeRow(double[,] matrix, int row, int length)
        {
            double pivot = matrix[row, row];
            for (int j = row; j < length; j++)
                matrix[row, j] /= pivot;
        }

        private static void EliminateVariable(double[,] matrix, int pivot, int rowCount, int length)
        {
            for (int i = 0; i < rowCount; i++)
            {
                if (i != pivot && Math.Abs(matrix[i, pivot]) > 1e-10)
                {
                    double factor = matrix[i, pivot];
                    for (int j = pivot; j < length; j++)
                        matrix[i, j] -= factor * matrix[pivot, j];
                }
            }
        }

        private static bool IsConsistent(double[,] matrix, int rowCount, int colCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                bool allZero = true;
                for (int j = 0; j < colCount; j++)
                {
                    if (Math.Abs(matrix[i, j]) > 1e-10)
                    {
                        allZero = false;
                        break;
                    }
                }
                if (allZero && Math.Abs(matrix[i, colCount]) > 1e-10)
                    return false;
            }
            return true;
        }

        private static double[] ExtractSolutions(double[,] matrix, int rowCount, int colCount)
        {
            double[] solutions = new double[colCount];
            for (int i = 0; i < colCount; i++)
            {
                solutions[i] = matrix[i, colCount];
            }
            return solutions;
        }
    }
}
