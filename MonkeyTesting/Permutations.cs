using System.Collections.Generic;
using System.Linq;

namespace CuriousGeorge
{
    public class Permutations
    {
        public static List<object[]> AbdulsAlgorithm(List<List<object>> collectionOfValues)
        {
            var result = new List<object[]>();
            var numPossibilities = collectionOfValues.Aggregate(1, (current, entry) => current * entry.Count);
            var counters = GetCounters(collectionOfValues);
            var lastItemCounter = counters.Last();
            for (var i = 0; i < numPossibilities; i++)
            {
                var objects = new List<object>();
                for (var j = 0; j < collectionOfValues.Count; j++)
                {
                    var origElem = collectionOfValues.ElementAt(j);
                    var counterValue = counters.ElementAt(j).GetValue();
                    var value = origElem.ElementAt(counterValue);
                    objects.Add(value);
                }
                result.Add(objects.ToArray());
                lastItemCounter.Increment();
            }
            return result;
        }

        public static List<T[]> HeapsAlgorithm<T>(int n, T[] collectionOfItems)
        {
            var result = new List<T[]>();
            var c = new int[n];

            for (var j = 0; j < n; j += 1)
            {
                c[j] = 0;
            }
            AddToResult(result, collectionOfItems);
                        var i = 0;
            while (i < n)
            {
                if (c[i] < i)
                {
                    if (i % 2 == 0)   // if _n_ mod 2 then
                        collectionOfItems = Swap(collectionOfItems, 0, i);
                    else
                        collectionOfItems = Swap(collectionOfItems, c[i], i);

                    AddToResult(result, collectionOfItems);
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

        private static void AddToResult<T>(ICollection<T[]> result, T[] array)
        {
            var copyArray = new T[array.Length];
            array.CopyTo(copyArray, 0);
            result.Add(copyArray);
        }

        private static T[] Swap<T>(T[] array, int source, int destination)
        {
            var interim = array[source];
            array[source] = array[destination];
            array[destination] = interim;
            return array;
        }

        private static List<Counter> GetCounters(IReadOnlyCollection<List<object>> collectionOfItems)
        {
            var counters = new List<Counter>();
            Counter childCounter = null;
            for (var j = collectionOfItems.Count - 1; j >= 0; j--)
            {
                childCounter = new Counter(childCounter, collectionOfItems.ElementAt(j).Count, 0);
                counters.Add(childCounter);
            }
            counters.Reverse();
            return counters;
        }
    }
}
