using System;
using System.Collections.Generic;
using System.Text;

namespace MathModeling.Utils
{
    public static class ArrayExtensions
    {
        public static IEnumerable<T> GetRow<T>(this T[,] array, int row)
        {
            for (var i = 0; i < array.GetLength(1); i++)
            {
                yield return array[row, i];
            }

            yield break;
        }
    }
}
