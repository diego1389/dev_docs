using System;
using System.Collections.Generic;

namespace DesignPatterns2.Strategy
{
    public class SortedList
    {
        private List<string> list = new List<string>();
        private SortStrategy sortstrategy;
        public void SetSortStrategy(SortStrategy sortstrategy)
        {
            this.sortstrategy = sortstrategy;
        }
        public void Add(string name)
        {
            list.Add(name);
        }
        public void Sort()
        {
            sortstrategy.Sort(list);
            // Iterate over list and display results
            foreach (string name in list)
            {
                Console.WriteLine(" " + name);
            }
            Console.WriteLine();
        }
    }
}
