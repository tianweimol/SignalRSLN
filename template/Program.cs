using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace template
{
    class Program
    {
        static int threadFlag = 0;
        static void Main(string[] args)
        {
            Fun();
            Console.Read();
        }

        static async void Fun()
        {
            Console.WriteLine("主线程开始了");
            PrintNumber(100);
            Console.WriteLine("主线程线束了");
        }

        static async Task PrintNumber(int number)
        {
            threadFlag++;
            await Task.Run(()=> {
                Thread.Sleep(1000);
                for (int i = 0; i < number; i++)
                {
                    Console.WriteLine("线程：{0}，输出:{1}", threadFlag, i);
                }
                Thread.Sleep(1000);
            });
        }
    }
}
