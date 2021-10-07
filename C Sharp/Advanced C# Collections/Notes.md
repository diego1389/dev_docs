## Arrays, Lists and Collection Equality

- Almost all collections use reference equality: Same memory location, not same values.
- For string MS overrode default reference behaviour for strings. It compares values even though they are a reference type.
- SequenceEqual() extension method (in System.Linq) to check for collection's value equality.
    ```c#
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
    ```
- Arrays are fixed size (don't have an Add method).
    - The only way to look up an item is by index.
    - Single block of memory.
    - Items are stored sequentially
    - Arrays are very fast to look up / enumerate.
    - Most collections use arrays under the hood.
- Lists
    - Lists are arrays internally. If you try to add a new element and it doesn't fit the memory allocation of the array it allocates a new memory segment big enough to support all the data and copy all the elements there. Then the old array gets Garbage collected. This operation doesn't happen every time you add a new element. 

## Collection performance

- If you use myList.RemoveAt(0) you have to move the list (the internal array) because it cannot have empty spaces. 
- Lists are O(N) operations, o stands for 'order of', or the number of times you need to move things. O(n) means time to execute grows directly with the size of  the collection. 
- Big O Notation tells how collection performance scales as collection gets bigger. 
- O(1) = same calculation no matter how big the array is. Retrieving and setting the value of this property (List<T>.Item[Int32] property) is an O(1) operation. 
- Beware of putting O(N) operations inside loops. 
```c#
/*DONT! O(N^2)*/
for(int i = lst.Count -1; i>=0; i-- ){
    if(someExpression(bankHols[i]))
        lst.RemoveAt(i)
}

/*BETTER USE O(N)*/
lst.RemoveAll(x => someExpression(x));
```
- O(log N) almost as good as O(1) Look up item in array or list
- O(N log N) almost as good as O(N) REmove items from list, enumerate.
- Avoid O(N^2)
- LINQ Find() uses O(N) (not bad for small to medium collections).
- List.Sort() is a bit faster than sorting using LINQ OrderBy but performance doesn't matter as much if only doing once (not repetedly).
- Here ToList() makes a new collection, not just copy the reference:
    ```c#
    public void Initialize(string csvFilePath){
        CsvReader reader = new CsvReader(csvFilePath);
        this.AllCountries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
    }
    ```
- Lists are not good for lookup. Fix this by using a dictionary.
    ```c#
    public class AppData{
        
        public List<Country> AllCountries {get; private set;}
        public Dictionary<string, Country> AllCountriesByKey{get; private set;}
        
        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            this.AllCountries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountriesByKey = AllCountries.ToDictionary(x=> x.Code); //specify key
        }
    }
    /*Search method:*/
    private Country GetCountryWithCode(string code){
        //return AllData.AllCountries.Find(x => x.Code == code);
        //return AllData.AllCountriesByKey[code]; //bad idea, throws exception if invalid code
        AllData.AllCountriesByKey.TryGetValue(code, out Country result);
        return result;
    }
    ```
- TryGetValue approaches an O(1) operation. 
- To deal with case sensitivity you have to specify in the dictionary initialization. You have to pass an equality comparer to the dictionary.
    ```c#
    this.AllCountriesByKey = AllCountries.ToDictionary(x=> x.Code, StringComparer.OrdinalIgnoreCase); 
    ```
- Enumerating the dictionary.
- To get only the values of the dictionary instead of the [TKey, TValue] representation just call the .Values property. 
- You can't rely on dictionary enumeration order.
- SortedDictionary automatically sorts items. It guarantees order when enumerating. 
- The ToDictionary() method doesn't work for SortedDictionary. 
- Sorted dictionaries sort the items by key.
- SortedList<TKey, TValue> is a type of dictionary but internally it uses a list (that's the name reason).
- SortedList and SortedDictionary are functionally the same. SortedList uses less memory but performs worse at inserting and deleting items
- It is better to use lists to enumerate and dictionaries to lookup. 
- Using custom type as key: equality must be implemented properly (developer's responsability).
    ```c#
    public class CountryCode{
        public string Value{get;}
        public CountryCode(string value){
            Value = value;
        }
        public override string ToString()=> Value;
    }


    public class AppData{
        
        public List<Country> AllCountries {get; private set;}
        public Dictionary<CountryCode, Country> AllCountriesByKey{get; private set;}
        
        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            this.AllCountries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountriesByKey = AllCountries.ToDictionary(x=> x.Code); 
        }
    }

    /*Search method:*/
    private Country GetCountryWithCode(string code){
        AllData.AllCountriesByKey.TryGetValue(new CountryCode(code), out Country result);
        return result;
    }

    ```
