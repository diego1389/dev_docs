using System;
using System.Linq;

namespace CollectionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime[] bankHols1 =
            {
                new DateTime(2021,1,1),
                new DateTime(2021,4,2)
            };

            DateTime[] bankHols2 =
            {
                new DateTime(2021,1,1),
                new DateTime(2021,4,2)
            };

            Console.WriteLine(bankHols1 == bankHols2);//false
            Console.WriteLine(bankHols1.SequenceEqual(bankHols2));//true
        }   
    }
}
