using System;
using System.Threading;

namespace thread
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(SayHello));
            t1.Start();
            t1.Join();
        }

        static void SayHello()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
