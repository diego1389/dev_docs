using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns2.Strategy
{
    public class ReverseSort : SortStrategy
    {
        public override void Sort(List<string> list)
        {
            //list.ShellSort();  not-implemented
            list.Reverse();


            Console.WriteLine("ReverseSort list ");
        }
    }
}
