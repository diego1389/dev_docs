using System;
namespace DesignPatterns2.IteratorPattern
{
    public interface IAbstractCollection
    {
        Iterator CreateIterator();
    }
}
