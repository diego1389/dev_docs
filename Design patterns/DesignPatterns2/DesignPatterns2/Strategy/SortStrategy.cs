using System.Collections.Generic;

namespace DesignPatterns2.Strategy
{
    public abstract class SortStrategy
    {
        public abstract void Sort(List<string> list);
    }
}
