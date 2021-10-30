using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINQSamples
{
  class Program
  {
    static async Task Main(string[] args)
    {
            Console.WriteLine("Starting");
            await Task.Factory.StartNew(() => {
                Task.Factory.StartNew(() => {
                    Thread.Sleep(1000);
                    Console.WriteLine("Completed 1");
                }, TaskCreationOptions.AttachedToParent);
                Task.Factory.StartNew(() => {
                    Thread.Sleep(2000);
                    Console.WriteLine("Completed 2");
                }, TaskCreationOptions.AttachedToParent);
                Task.Factory.StartNew(() => {
                    Thread.Sleep(3000);
                    Console.WriteLine("Completed 3");
                }, TaskCreationOptions.AttachedToParent);
            });

            Console.WriteLine("Completed");
            Console.ReadLine();
            /*// Instantiate the ViewModel
            SamplesViewModel vm = new SamplesViewModel
            {
              // Use Query or Method Syntax?
              UseQuerySyntax = false
            };

                  // Call a sample method
                  vm.SequenceEqualInteger();

            // Display Product Collection
            /*foreach (var item in vm.Products) {
              Console.Write(item.ToString());
            }*/

            // Display Result Text
            //Console.WriteLine(vm.ResultText);*/
        }
  }
}
