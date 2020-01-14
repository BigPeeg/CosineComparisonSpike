using System;
using System.Collections.Generic;
using GrattanDistances;

namespace CosineComparisonSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> l1 = new List<int>();
            List<int> l2 = new List<int>();

            Console.WriteLine("Similar");
            l1.Add(1);
            l1.Add(1);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);

            l2.Add(1);
            l2.Add(1);
            l2.Add(1);
            l2.Add(1);
            l2.Add(1);
            l2.Add(1);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            double dist1 = Cosine.Distance(l1, l2);
            Console.WriteLine(dist1);
            l1.Clear();
            l2.Clear();
            Console.WriteLine("Same");
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);
            l1.Add(2);

            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            double dist2 = Cosine.Distance(l1, l2);
            Console.WriteLine(dist2);
            l1.Clear();
            l2.Clear();
            Console.WriteLine("Dissimilar");
            l1.Add(1);
            l1.Add(1);
            l1.Add(1);
            l1.Add(1);
            l1.Add(1);
            l1.Add(1);
            l1.Add(1);
            l1.Add(1);
            l1.Add(1);

            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            l2.Add(2);
            double dist3 = Cosine.Distance(l1, l2);
            Console.WriteLine(dist3);
            Console.ReadKey();
        }
    }
}
