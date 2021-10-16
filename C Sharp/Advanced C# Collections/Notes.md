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
- Override Equality for keys
    - Override also the == operator toe create a more predictable behaviour (not needed for dictionary equality)
    - C# requires that if you override the Equals operator you should override the NotEquals operator.
    ```c#
       public class CountryCode{
        public string Value{get;}
        public CountryCode(string value){
            Value = value;
        }

        public override string ToString()=> Value;
        
        public override bool Equals(object obj){
            if(obj is CountryCode other)
                return StringComparer.OrdinalIgnoreCase.Equals(this.Value, other.Value);
            return false;
        }

        public static bool operator ==(CountryCode lhs, CountryCode rhs){
            if(lhs != null)
                return lhs.Equals(rhs);
            else
                return rhs == null;
        }

        public static bool !=(CountryCode lhs, CountryCode rhs){
            return !(lhs == rhs);
        }
    }
    ```
- Overriding Equals is not enough. You also have to override object.GetHashCode(). This method is around every single object and is virtual. The hash code is an integer representation that represents a compressed version of the value of the object. 
- Dictionaries rely on GetHashCode() extensively. Lookup will fail if key doesn't implement GetHashCode((). Easiest way is to use existing MS implementations. Also is easier to use standard MS types as keys. 
    ```c#
    public class CountryCode{
        public string Value{get;}
        public CountryCode(string value){
            Value = value;
        }

        public override string ToString()=> Value;
        
        public override bool Equals(object obj){
            if(obj is CountryCode other)
                return StringComparer.OrdinalIgnoreCase.Equals(this.Value, other.Value);
            return false;
        }

        public static bool operator ==(CountryCode lhs, CountryCode rhs){
            if(lhs != null)
                return lhs.Equals(rhs);
            else
                return rhs == null;
        }

        public static bool !=(CountryCode lhs, CountryCode rhs){
            return !(lhs == rhs);
        }

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(this.Value);
    }
    ```

## Linked lists

- Efficient for adding and removing elements.
- Complex.
- No direct element lookup.
- List<T> performs badly for modifications. 
- Each node contains a Next() reference to the next element of the list. 
- With LinkedLists the items can be anywhere in memory (no sequential order). 

|  LinkedList<T> |List<T>   |
|---|---|
| Definite order  | Definite order |
| Optimized for fast changes  | Optimized for fast lookup |

- The reason it is optimized for fast changes its that you need to move elements, just change the next() and previous() references of the elements to remove an element from the LinkedList. The unlinked element then gets GarbageCollected. 
- Same thing for adding an item. Just change the next and previous references and thats it, it is going to be fast no matter of the size of the linked list.
- However you lost the ability to lookup items quickly. This will scale O(n). 
- LinkedListNode<T> is a Wrapper to let you put stuff in linked lists. 
- Must use LinkedListNode<T> as an intermediary. 
    ```c#
    public class AppData{
        
        public List<Country> AllCountries {get; private set;}
        public Dictionary<CountryCode, Country> AllCountriesByKey{get; private set;}
        public LinkedList<Country> ItineraryBuilder {get; } = new LinkedList<Country>();

        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            this.AllCountries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountriesByKey = AllCountries.ToDictionary(x=> x.Code); 
        }
    }

    /*Add country*/
    private void btnAddToItinerary_Click(object sender, RoutedEventArgs e){
        int selectedIndex = this.lbxAllCountries.SelectedIndex;
        if(selectedIndex == -1)
            return;
        Country selectedCountry = AllData.AllCountries[selectedIndex];
        AllData.ItineraryBuilder.AddLast(selectedCountry);
        this.UpdateAllLists(); //WPF method
    }
    ```
- AddLast() is an O(1) operation. 
- Removing from a linked list
    ```c#
    private void btnRemoveFromItinerary_Click(object sender, RoutedEventArgs e){
        int selectedItinIndex = this.lbxItinerary.SelectedIndex;
        if(selectedItinIndex < 0)
            return;
        var nodeToRemove = AllData.ItineraryBuilder.GetNthNode(selectedItinIndex);
        AllData.ItineraryBuilder.Remove(nodeToRemove);

        this.UpdateAllLists();
    }

    public static class LinkedListExtension{
        public static LinkedListNode<T> GetNthNode<T>(this LinkedList<T> lst, int n){
            LinkedList<T> currentNode = lst.First;
            for(int i = 0; i<n; i++)
                currentNode = current.Next;
            return currentNode; 
        }
    }
    ```
- Insert before method:
    ```c#
    private void btnInsertInItinerary_Click(object sender, RoutedEventArgs e){
        int selectedIndex = this.lbxAllCountries.SelectedIndex;
        int selectedItinIndex = this.lbxItinerary.SelectedIndex;

        Country selectedCountry = AllData.AllCountries[selectedIndex];
        var insertBeforeNode = AllData.ItineraryBuilder.GetNthNode(selectedItinIndex);
        AllData.ItineraryBuilder.AddBefore(insertBeforeNode, selectedCountry);
        this.UpdateAllLists();
    }
    ```
|  Pros |Cons  |
|---|---|
| Very scalable insertions and deletions | Need to wrap elements in LinkedListNode<T> |
|  | No direct indexed element access |

- Adding elements in the middle nullify the performance gains because you have to search through all the elements. 

    ```c#
    public string Name{get;}
    public Country[] Itinerary{get;}

    public override string ToString() => $"{Name} ({Itinerary.Length} countries)}"

    public Tour(string name, Country[] itinerary){
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tour must have a non-whitespace name", nameof(name))
        //other validations
        this.Name = name;
        this.Itinerary = itinerary;
    }
    ```
- Taking advantage of dictionary
- Converting the LinkedList into an Array because the data to persist is not gonna change (no need to operate in it because it is for saving). 
    ```c#
     public class AppData{
        
        public List<Country> AllCountries {get; private set;}
        public Dictionary<CountryCode, Country> AllCountriesByKey{get; private set;}
        public LinkedList<Country> ItineraryBuilder {get; } = new LinkedList<Country>();
        public SortedDictionary<string, Tour> AllTours {get; private set;} = new SortedDictionary<string, Tour>();

        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            this.AllCountries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountriesByKey = AllCountries.ToDictionary(x=> x.Code); 
        }
    }

    /*Save tour*/
    private void btnSaveTour_Click(object sender, RoutedEventAArgs e){
        string name = this.tbxTourName.Text.Trim();
        Country[] itinerary = AllData.ItineraryBuilder.ToArray();

        try{
            Tour newTour = new Tour(name, itinerary);
            AllData.AllTours.Add(newTour.Name, newTour);
        }catch(Exception e){
            //handle exception
        }
    }
    ```
## Stacks

- Items remved as they are processed. 
- You only retrieve the latest change.
- You push and pop items from a stack. 

    ```c#
    public enum ChangeType {Append, Insert, Remove}

    public class ItineraryChange{
        public ChangeType ChangeType{get;}
        public Country Value {get;}
        public int Index {get;}

        public ItineraryChange(ChangeType changeType, int index, Country countryRemoved){
            this.ChangeType = changeType;
            this.Index = index;
            this.Value = countryRemovedl
        }
    }

    public class AppData{
        
        public List<Country> AllCountries {get; private set;}
        public Dictionary<CountryCode, Country> AllCountriesByKey{get; private set;}
        public LinkedList<Country> ItineraryBuilder {get; } = new LinkedList<Country>();
        public SortedDictionary<string, Tour> AllTours {get; private set;} = new SortedDictionary<string, Tour>();
        public Stack<ItineraryChange> ChangeLog {get;} = new Stack<ItineraryChange>();

        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            this.AllCountries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountriesByKey = AllCountries.ToDictionary(x=> x.Code); 
        }
    }

    /*Add country*/
    private void btnAddToItinerary_Click(object sender, RoutedEventArgs e){
        int selectedIndex = this.lbxAllCountries.SelectedIndex;
        if(selectedIndex == -1)
            return;
        Country selectedCountry = AllData.AllCountries[selectedIndex];
        AllData.ItineraryBuilder.AddLast(selectedCountry);

        var change = new ItineraryChange(ChangeType.Append, AllData.ItineraryBuilder.Count, selectedCountry); /*For appending at the end of the linked list the position = the number of items*/
        AllData.ChangeLog.Push(change); 

        this.UpdateAllLists(); //WPF method
    }

    /*Remove from itinerary*/
    private void btnRemoveFromItinerary_Click(object sender, RoutedEventArgs e){
        int selectedItinIndex = this.lbxItinerary.SelectedIndex;
        if(selectedItinIndex < 0)
            return;
        var nodeToRemove = AllData.ItineraryBuilder.GetNthNode(selectedItinIndex);
        AllData.ItineraryBuilder.Remove(nodeToRemove);

        var change = new ItineraryChange(ChangeType.Remove, selectedIndex, noteToRemove.Value);
        AllData.ChangeLog.Push(change);
        this.UpdateAllLists();
    }

      private void btnInsertInItinerary_Click(object sender, RoutedEventArgs e){
        int selectedIndex = this.lbxAllCountries.SelectedIndex;
        int selectedItinIndex = this.lbxItinerary.SelectedIndex;

        Country selectedCountry = AllData.AllCountries[selectedIndex];
        var insertBeforeNode = AllData.ItineraryBuilder.GetNthNode(selectedItinIndex);
        AllData.ItineraryBuilder.AddBefore(insertBeforeNode, selectedCountry);

        var change = new ItineraryChange(ChangeType.Insert, selectedItindex, selectedCountry);
        AllData.ChangeLog.Push(change);
        this.UpdateAllLists();
    }
    ```
- Undo button
    ```c#
    private void btnUndo_Click(object sender, RoutedEventArgs e){
        if(AllData.ChangeLog.Count === 0){
            return
        }

        ItineraryChange lastChange = AllData.ChangeLog.Pop();
        ChangeUndoer.Undo(AllData.ItineraryBuilder, lastChange);
        this.UpdateAllLists();
    }

    public static void Undo(LinkedList<Country> itinerary, ItineraryChange changeToUndo){
        switch(changeToUndo.ChangeType){
            case ChangeType.Append:
                //change was to append final country so we need to remove it
                itinerary.RemoveLast();
                break;
            case ChangeType.Insert:
                LinkListNode<Country> insertion = itinerary.GetNthNode(changeToUndo.Index);
                itinerary.Remove(insertion);
                break;
            case ChangeType.Remove:
                itinerary.AddLast(cangeToUndo.Value);
                break;
        }
    }
    ```
- There is no look-up.
- The stack decides which item you get next (the most recently added). 

## Queues

- Always supplies the item that has been waiting in the collection the longest. 
    ```c#
    public class Customer{
        public string Name {get;}
        public List<Tour> BookedTours{get;} = new List<Tour>();
        public Customer(string name){
            this.Name = name;
        }
        public override string ToString()=> Name;

    }

    public class AppData{
        
        public List<Country> AllCountries {get; private set;}
        public Dictionary<CountryCode, Country> AllCountriesByKey{get; private set;}
        public List<Customer> Customers{get; private set;} = new List<Customer>(){
            new Customer("Simon"), new Customer("Kim")
        }
        public LinkedList<Country> ItineraryBuilder {get; } = new LinkedList<Country>();
        public SortedDictionary<string, Tour> AllTours {get; private set;} = new SortedDictionary<string, Tour>();
        public Stack<ItineraryChange> ChangeLog {get;} = new Stack<ItineraryChange>();
        public Queue<(Customer TheCustomer, Tour TheTour)> BookingRequests {get;} = new Queue<(Customer, Tour)>();

        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            this.AllCountries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountriesByKey = AllCountries.ToDictionary(x=> x.Code); 
        }
    }

    private void btnBookTour_Click(object sender, RoutedEventArgs e){
        Customer customer = this.lbxCustomer.SelectedItem as Customer;
        List<Tour> requestedTour = GetRequestedTour();

        foreach(Tour tour in requestedTours){
            this.AllData.BookingRequests.Enqueue((customer, tour));
        }
    }

    private void btnApprovedRequestClick(object sender, RoutedEventArgs e){
        var request = AllData.BookingRequests.Dequeue();
        request.TheCustomer.BookedTours.Add(request.TheTour);

    }
    ```
- Dequeue() will throw an exception if the queue is empty.
- Peeking the queue.
    ```c#
    private GetLatestBookingRequestText(){
        return AllData.BookingRequests.Peek().ToString();
    }
    ```
- Stack<T> also supports a Peek() method. You can enumerate both collections with a foreach loop but they don;t provide lookup.

## Concurrency and concurrent collections

- Concurrent queue
    ```c#
     private async void btnBookTour_Click(object sender, RoutedEventArgs e){
        Customer customer = this.lbxCustomer.SelectedItem as Customer;
        List<Tour> requestedTour = GetRequestedTour();

        List<Task> tasks = new List<Task>();
        foreach(Tour tour in requestedTours){
            Task task = Task.Run(
                () => this.AllData.BookingRequests.Enqueue((customer, tour))
            );
            await Task.WhenAll(tasks);
        }
    }

     public class AppData{
        
        public List<Country> AllCountries {get; private set;}
        public Dictionary<CountryCode, Country> AllCountriesByKey{get; private set;}
        public List<Customer> Customers{get; private set;} = new List<Customer>(){
            new Customer("Simon"), new Customer("Kim")
        }
        public LinkedList<Country> ItineraryBuilder {get; } = new LinkedList<Country>();
        public SortedDictionary<string, Tour> AllTours {get; private set;} = new SortedDictionary<string, Tour>();
        public Stack<ItineraryChange> ChangeLog {get;} = new Stack<ItineraryChange>();
        public ConcurrentQueue<(Customer TheCustomer, Tour TheTour)> BookingRequests {get;} = new ConcurrentQueue<(Customer, Tour)>();

        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            this.AllCountries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountriesByKey = AllCountries.ToDictionary(x=> x.Code); 
        }
    }
    ```
- YOu enqueue to a concurrent queue the same way as with a generic regular queue.
- Concurrent queue has a trydequeue because there is no way to guarantee that another thread empties the queue just before you access it so the count property validation wont work for concurrent queue.
    ```c#
    private void btnApprovedRequestClick(object sender, RoutedEventArgs e){
        //var request = AllData.BookingRequests.Dequeue();
        bool success = AllData.BookingRequests.TryDequeue(out var request);
        if(success){
            request.TheCustomer.BookedTours.Add(request.TheTour);
        }
    }
    ```
- Try Peek
    ```c#
    private GetLatestBookingRequestText(){
        var success = AllData.BookingRequests.TryPeek(out var request);
        return success ? request.ToString() : null;
    }
    ```

## Merging data with Hashsets and SortedSets

- Remove duplicate countries (you can use Linq Distinct() but it is not very scalable)
    ```c#
    private HashSet<Country> GetCOuntriesInSelection(){
        var countries = new HashSet<Country>();

        List<Tour> selectedTours = GetRequestedTours();
        foreach(Tour tour in selectedTours){
            foreach(Country country in tour.Itinerary){
                countries.Add(country);
            }
        }
        return countries;
    }
    ```
- Comparing sets and dictionary

|Dictionary<TKey, TValue> |HashSet<T>  |
|---|---|
| Have keys | Dont have keys |
| Key-based lookuup| Do not support lookup|
| Keys are unique| Values are unique|
| Adding duplicates throws exception| Duplicates are ignored|
| Must compare items for equality| Must compare items for ordering|

- Sorting items with SortedSet<T>
    - Works just as a Set but it is sorted by default (but you need to define a comparer for complex objects).
    ```c#
    private SortedSet<Country> GetCOuntriesInSelection(){
        var countries = new SortedSet<Country>(CountryNameComparer.Instance);

        List<Tour> selectedTours = GetRequestedTours();
        foreach(Tour tour in selectedTours){
            foreach(Country country in tour.Itinerary){
                countries.Add(country);
            }
        }
        return countries;
    }

    public class CountryNameComparer : IComparer<Country>{
        public static CountryNameComparer Instance {get;} = new CountryNameComparer();
        private CountryNameComparer(){}
        public int Compare(Country x, Country y){
            return x.Name.CompareTo(y.Name);
        }
    }
    ```
- Merging sets with UnionWith()
      ```c#
    private SortedSet<Country> GetCOuntriesInSelection(){
       List<Tour> selectedTours = GetRequestedTours();
        if(selectedTours.Count == 0){
            return new SortedSet<Country>(CountryNameComparer.Instance);
        }

        var allSets = new List<SortedSet<Country>>();

        foreach(Tour tour in selectedTours){
            SortedSet<Country> tourCountries = new SortedSet<Country>(tour.Itinerary, CountryName.Comparer.Instance);
            allSets.Add(tourCountries);
        }

        SortedSet<Country> result = allSest[0];
        for(int i = 1; i < allSets.Count; i++ ){
            result.UnionWith(allSets[i]);
        }
        return result;

    }
    ```
- Set intersection
    ```c#
    private SortedSet<Country> GetCOuntriesInSelection(){
       List<Tour> selectedTours = GetRequestedTours();
        if(selectedTours.Count == 0){
            return new SortedSet<Country>(CountryNameComparer.Instance);
        }

        var allSets = new List<SortedSet<Country>>();

        foreach(Tour tour in selectedTours){
            SortedSet<Country> tourCountries = new SortedSet<Country>(tour.Itinerary, CountryName.Comparer.Instance);
            allSets.Add(tourCountries);
        }

        SortedSet<Country> result = allSest[0];
        for(int i = 1; i < allSets.Count; i++ ){
            result.IntersectWith(allSets[i]);/*Returns only the common element*/
        }
        return result;
    }
    ```
## Read-only and immutable collections

- Preventing modifications.  
- If the collection can be modified it opens a path for bugs. 
- System.Collections.ObjectModel has more specialized collections.
    ```c#
    public class AppData{
        
        public ReadOnlyCollection<Country> AllCountries {get; private set;}
        public ReadOnlyDictionary<CountryCode, Country> AllCountriesByKey{get; private set;}
        public List<Customer> Customers{get; private set;} = new List<Customer>(){
            new Customer("Simon"), new Customer("Kim")
        }
        public LinkedList<Country> ItineraryBuilder {get; } = new LinkedList<Country>();
        public SortedDictionary<string, Tour> AllTours {get; private set;} = new SortedDictionary<string, Tour>();
        public Stack<ItineraryChange> ChangeLog {get;} = new Stack<ItineraryChange>();
        public ConcurrentQueue<(Customer TheCustomer, Tour TheTour)> BookingRequests {get;} = new ConcurrentQueue<(Customer, Tour)>();

        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            var countries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountries = countries.AsReadOnly();
            this.AllCountriesByKey = new ReadOnlyDictionary<CountryCode,  Country>(AllCountries.ToDictionary(x=> x.Code)); 
        }
    }
    ```
- The loophole in Read-only collection. If you modify the original list or dictionary it affects the read only collection as well (it changes its data set to put it some way). Read-only collections can be modified if you have a reference to the underlying collection. 
- Immutable collections don't have this behavior.     
    ```c#
    public class AppData{
        
        public ImmutableArray<Country> AllCountries {get; private set;}
        public ImmutableDictionary<CountryCode, Country> AllCountriesByKey{get; private set;}
        public List<Customer> Customers{get; private set;} = new List<Customer>(){
            new Customer("Simon"), new Customer("Kim")
        }
        public LinkedList<Country> ItineraryBuilder {get; } = new LinkedList<Country>();
        public SortedDictionary<string, Tour> AllTours {get; private set;} = new SortedDictionary<string, Tour>();
        public Stack<ItineraryChange> ChangeLog {get;} = new Stack<ItineraryChange>();
        public ConcurrentQueue<(Customer TheCustomer, Tour TheTour)> BookingRequests {get;} = new ConcurrentQueue<(Customer, Tour)>();

        public void Initialize(string csvFilePath){
            CsvReader reader = new CsvReader(csvFilePath);
            var countries = reader.ReadAllCountries().OrderBy(x => x.Name).ToList();
            this.AllCountries = countries.ToImmutableArray();
            this.AllCountriesByKey = AllCountries.ToImmutableDictionary(x => x.Code); 

            countries.Add(new Country("Lilliput", "LIL", "somewhere", 1_000_000));
        }
    }
    ```
- Majority of collections have an immutable version. 
- You can circumvent with reflection or unmanaged code though.

|Immutable collections |Read-only collections  |
|---|---|
| Collections in their own right| Wrappers that guard other collections |
| Thread-safe| Not thread-safe|

## Collection interfaces

- IList<T> support must list features. 
- IReadOnly<T> doesn't allow features to modify the collection. 
- IEnumerable<T> allows something to be enumerated. All collections implement it. 
    ```c#
    IEnumerable<Tour> GetRequestedTours() => this.lbxToursToBook.SelectedItems.Cast<Tour>();//.ToList() ;

    var selectedTours = GetRequestedTours();
        //if(selectedTours.Count == 0){
        if(selectedTours.Any()){ //IEnumerable doesnt have a Count
            return new SortedSet<Country>(CountryNameComparer.Instance);
        }

        var allSets = new List<SortedSet<Country>>();

        foreach(Tour tour in selectedTours){
            SortedSet<Country> tourCountries = new SortedSet<Country>(tour.Itinerary, CountryName.Comparer.Instance);
            allSets.Add(tourCountries);
        }

        SortedSet<Country> result = allSest[0];
        for(int i = 1; i < allSets.Count; i++ ){
            result.UnionWith(allSets[i]);
        }
        return result;

    }
    ```
- 
