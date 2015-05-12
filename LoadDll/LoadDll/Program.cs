using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dll;
namespace LoadDll
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DllClass.func().ToString());
            Console.ReadLine();
        }
    }
}
