# Design patterns:

- Are common architectural approaches.
- Creational patterns: builder, factories, prototype and singleton.
- Structural patterns: adapter, bridge, composite, decorator, facade, flyweight and proxy.
- Behavioral patterns: chain of responsability, command, interpreter, iterator, mediator, memento, null object, observer, state, strategy, template method, visitor.

## SOLID design principle:

- Single responsability principle:
    - Move the persistance methods to a separate class outside Journal because it had a lot of responsabilities.
    - Every class should have one responsability and one reason to change.
    ```c#
    namespace DesignPatterns
    {
        public class Journal
        {
            private readonly List<string> entries = new List<string>();
            private static int count = 0;
            public int AddEntry(string text)
            {
                entries.Add($"{++count}: {text}");
                return count;
            }

            public void RemoveEntry(int index)
            {
                entries.RemoveAt(index);
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, entries);
            }
        }

        public class Persistence
        {
            public void SaveToFile(Journal j, string filename, bool overwrite = false)
            {
                if(overwrite || !File.Exists(filename))
                    File.WriteAllText(filename, j.ToString());
            }
        }
        class Program
        {
            static void Main(string[] args)
            {
                var j = new Journal();
                j.AddEntry("I cried today");
                j.AddEntry("I ate a bug");
                Console.WriteLine(j);

                var p = new Persistence();
                var filename = @"journal.txt";
                p.SaveToFile(j, filename, true);
                Process.Start(filename);            }
        }
    }
    ```
- Open-Closed principle
    - Classes should be open for extension and close for modification. 
    - YOu can use inheritance (enhancing the capabilities using interfaces). 