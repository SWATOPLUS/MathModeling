using System;

namespace Lab3MonteCarlo
{
    class Program
    {
        static void Main(string[] args)
        {
            var exactResult = 0.567544;

            var n = 30001;
            var a = new double[n];
            var az = new double[n];
            var m = 15.0;
            var betta = Math.Pow(2.0, m) + 3.0;
            var M = (int)Math.Pow(2, m - 1);

            az[0] = betta;
            var a1 = 0.0;
            Console.WriteLine($"betta = {betta}");
            for (var t = 0; t < n - 1; t++)
            {
                az[t + 1] = (betta * az[t]) % M;
                a[t + 1] = az[t + 1] / M;
                /*Console.WriteLine("a[");
                Console.WriteLine(t);
                Console.WriteLine("]= ");
                Console.WriteLine(a[t]);*/
            }
            for (var j = 1; j < n; j++)
            {
                a[j - 1] = a[j - 1] * 3;
                a1 = a1 + Math.Exp(-1 * a[j - 1]) * Math.Cos(a[j - 1]) * Math.Cos(a[j - 1]);
            }
            a1 = 3 * a1 / n;

            Console.WriteLine($"Точное решение = {exactResult}");
            Console.WriteLine("Решение методом Монте Карло = " + a1);
            Console.WriteLine("Погрешность = " + Math.Abs(a1 - exactResult));
            Console.ReadLine();
        }
    }
}
