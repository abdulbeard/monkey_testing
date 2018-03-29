using System;
using System.Collections.Generic;
using System.Text;

namespace MonkeyTesting
{
    public class Permutations
    {
        public List<T[]> Permute<T>(int n, T[] A)
        {
            var result = new List<T[]>();
            var c = new int[n];

            for (var j = 0; j < n; j += 1)
            {
                c[j] = 0;
            }
            AddToResult(result, A);
                        var i = 0;
            while (i < n)
            {
                if (c[i] < i)
                {
                    if (i % 2 == 0)   // if _n_ mod 2 then
                        A = Swap(A, 0, i);
                    else
                        A = Swap(A, c[i], i);

                    AddToResult(result, A);
                    c[i] += 1;
                    i = 0;
                }
                else
                {
                    c[i] = 0;
                    i += 1;
                }
            }
            return result;
        }

        private void AddToResult<T>(List<T[]> result, T[] array)
        {
            var copyArray = new T[array.Length];
            array.CopyTo(copyArray, 0);
            result.Add(copyArray);
        }

        private T[] Swap<T>(T[] array, int source, int destination)
        {
            var interim = array[source];
            array[source] = array[destination];
            array[destination] = interim;
            return array;
        }
    }
}
