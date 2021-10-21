using System;

namespace LINQSamples
{
  class Program
  {
    static void Main(string[] args)
    {
      // Instantiate the Samples ViewModel
      SamplesViewModel vm = new SamplesViewModel
      {
        // Use Query or Method Syntax?
        UseQuerySyntax = false
      };

            // Call a sample method
            //vm.GetSpecificColumns();
            //vm.WhereExtensionMethod();
            vm.SingleOrDefault();

      // Display Product Collection
         /*   foreach (var item in vm.Products) {
        Console.Write(item.ToString());
      }*/

      // Display Result Text
      Console.WriteLine(vm.ResultText);
    }
  }
}
