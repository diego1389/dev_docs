## Async and await

- Use HttpClient instead of WebClient for asynchornous functionalities. 
- Always use async and await together. 
- Calling Result or Wait instead of await will block and potentially deadlock your application. It will run the application synchronously. 
- The await validates the success of the operation. Continuation is back on calling thread. 
- The code inside the continuation will be executed once the task completes successfully. 
- The await will re-throw exceptions that occurs inside the Task.
- Avoid "async void" methods (just use it for event handlers), use "async Task" instead. Methods marked with async Task will automatically have a Task returned without explicitly having to return anything. 
- If we don't add the await keyword the exceptions will be swallowed (this also happens if we use async void instead of async Task).

## Using the Task Parallel library 

- A Task represents a single asynchronous operation. 
    - Execute work on a different thread.
    - Get the result from the asynchronous operation.
    - Subscribe to when the operation is done by introducing a continuation. 
    - Task provides a static method: Task.Run(SomeMethod); It queues the method on the thread pool for execution. 
    - Approach when you use a library that doesn't provide asynchronous methods. This example is with Continuation but it can also be done using async and await keywords.
    - The **Result** can be called if it is available (in ContinueWith for example) but if it is not available (if the asynchronouws operation hasn't finished) it can block the program.
    ```c#
    //Task.Run(()=> {/*Heavy operation*/})
    var loadLinesTask = Task.Run(()=> {
        var lines = File.ReadAllLines("StockPrices_Small.csv");
        return lines;
    })

    var processStocksTask = loadLinesTask.ContinueWith((completedTask)=>{
        var lines = completedTask.Result;
        var data = new List<StockPrice>();
        foreach(var line in lines.Skip(1)){/*do something*/}

        Dispatcher.Invoke(()=>{ /*Update the UI thread WPF*/
            Stocks.ItemsSource = data.Where(/*something*/); 
        })
    });

    processStocksTask.ContinueWith(_ => {
        Dispatcher.Invoke(()=>{
            AfterLoadingStockData();
        })
    });
    ```
- You can chain multiple ContinueWith.
- async & await is a much more readable and maintanable approach than ContinueWith.
- You can create asynchronous operations inside asynchronous operations.
- You can mark an anonymous method as async (it's not async void):
    ```c#
    var loadLinesTask = Task.Run(async () =>{
        using var(stream = new StreamReader(File.OpenRead("StockPrices_Small.csv"))){
            var lines = new List<string>();
            string line;
            while((line = await strea.ReadLineAsync()) != null){
                lines.Add(line);
            }
            return lines;
        }
    })
    ```
- ContinueWith executes when the Task is completed, no matter if it is successful, faulted or cancelled. 
- TaskContinuationOptions (to continue only i Succeded, only if faulted, etc).
    ```c#
        var loadLinesTask = Task.Run(()=> {
            var lines = File.ReadAllLines("StockPrices_Small.csv");
            return lines;
        })

        loadLinesTask.ContinueWith(t=>{Dispatcher.Invoke(()=>{/*display error message*/})}, TaskContinuationOptions.OnlyOnFaulted);

       var processStocksTask = loadLinesTask.ContinueWith((completedTask)=>{
        var lines = completedTask.Result;
        var data = new List<StockPrice>();
        foreach(var line in lines.Skip(1)){/*do something*/}

        Dispatcher.Invoke(()=>{ /*Update the UI thread WPF*/
            Stocks.ItemsSource = data.Where(/*something*/); 
        })
    }, TaskContinuationOptions.OnlyOnRanToCompletion); //to avoid Faulted and Cancelled
    ```
- Always validate your asynchronous operations.
- CancellationTokerSource = signals to a cancellationtken that it should be canceled.
    - It has Cancel() method to cancel immediately and also a CancelAfter() method. 
    ```c#
    CancellationTokenSource cancellationTokenSource;
    CancellationToken token = cancellationTokenSource.Token;

    Task.Run(()=> {}, token);
    ```
- Calling Cancel() will not automatically terminate the asynchronous operation. We need to handle the cancellations gracefully:
    ```c#
    using var(stream = new StreamReader(File.OpenRead("StockPrices_Small.csv"))){
        var lines = new List<string>();
        string line;
        while((line = await strea.ReadLineAsync()) != null){
            if(cancellationToken.IsCancellationRequested){
                break;
            }
            lines.Add(line);
        }
        return lines;
    }
    ```
- You can also use cancellationToken.ThrowIfCancellationRequested.
- You can also pass the cancellation token to the Continuation (you also need another parameter: TaskScheduler.[Current]):
    ```c#
    task.ContinueWith((t)=> {}, token);
    ```
- CancellationToken allow us to register a delegate that will be called when the CancellationToken is canceled:
    ```c#
    cancellationTokenSource.Token.Register(()=>{
        Notes.Text = "The task was cancelled";
    });
    ```
## Exploring useful methods in the Task Parallel Library

-  When all tasks are completed:
    ```c#
    var loadingTasks = new List<Task<IEnumerable<StockPrice>>>();

    foreach(var identifier in identifiers){
        var loadTask = service.GetStockPricesFor(identifier, cancellationTokenSource.Token);/*If we await this it means we will be waiting one by one*/
        loadingTasks.Add(loadTask); 
    }
    var allStocks = await Task.WhenAll(loadingTasks);
    Stocks.ItemsSource = allStocks.SelectMany(stocks => stocks);
    ```
- Delay example and WhenAny to determine which of those tasks completed first:
    ```c#
    var loadingTasks = new List<Task<IEnumerable<StockPrice>>>();

    foreach(var identifier in identifiers){
        var loadTask = service.GetStockPricesFor(identifier, cancellationTokenSource.Token);/*If we await this it means we will be waiting one by one*/
        loadingTasks.Add(loadTask); 
    }
    var timeoutTask = Task.Delay(2000);
    var allStocksLoadingTask = Task.WhenAll(loadingTasks);

    var completedTask = await Task.WhenAny(timeoutTask, allStocksLoadingTask);

    if(completedTask == timeoutTask){
        cancellationTokenSource.Cancel();
        throw new OperationCancelException("Timeout!");
    }
    Stocks.ItemsSource = allStocksLoadingTask.Result.SelectMany(stocks => stocks);
    ```
- Precomputed results of a task:
    ```c#
    public class MockStockService : IStockService{
        public Task<IEnumerable<StockPrice>> GetStockPricesFor(string stockIdentifier,
            CancellationToken cancellationToken){
                var stocks = new List<StockPrice>{
                    new StockPrice{/*mocked data...*/}
                }

                //var task = Task.CompletedTask;
                var task = Task.FromResult(stocks.Where(stock => stock.Identifier == stockIdentifier));
                return task;
            }
    }
    ```
- Process Task as they complete (to fill the datagrid progressively).
    - List<T> is not thread safe consider using ConcurrentBag<T> instead.
    ```c#
    var loadingTasks = new List<Task<IEnumerable<StockPrice>>>();
    var stocks = new ConcurrentBag<StockPrice>();

    foreach(var identifier in identifiers){
        var loadTask = service.GetStockPricesFor(identifier, cancellationTokenSource.Token);
        
        loadTask = loadTask.ContinueWith(t=>{
            var aFewStocks = t.Result.Take(5);
            foreach(var stock in aFewStocks){
                stocks.Add(stock);
            }
            Dispatcher.Invoke(()=>{
                Stocks.ItemsSource = stocks.ToArray();
            });
            return aFewStocks;
        })
        
        loadingTasks.Add(loadTask); 
    }
    //var timeoutTask = Task.Delay(2000);
    var allStocksLoadingTask = await Task.WhenAll(loadingTasks);

    //var completedTask = await Task.WhenAny(timeoutTask, allStocksLoadingTask);

    //if(completedTask == timeoutTask){
      //  cancellationTokenSource.Cancel();
        //throw new OperationCancelException("Timeout!");
    //}
    //Stocks.ItemsSource = allStocksLoadingTask.Result.SelectMany(stocks => stocks);
    ```
- Execution context and controlling execution.
- ConfigureAwait configures how the continuation will be executed. Continue on the captured context? true / false.
    ```c#
    var data = await service.GetStockPricesFor(identifier, CancellationToken.None).ConfigureAwait(false); //this doesn't come back to the UI thread and causes an exception
    Notes.Text = "Stockes loaded!";
    return data.Take(5);
    ```
    - ConfigureAwait only affects the continuation in the method that you are operating in.
    - Slightly improve performance. 
    - The continuation shouldn't depend on the original context. 
    - .Net core doesn't use synchronization context so COnfigureAwait(false) is useless.
    
## Async await advanced topics:

- IAsyncEnumerable<t> exposes an enumerator that provides asynchronous iteration over values of a specified type. 
    ```c#
    public class MockStockStreamService{
        public async IAsyncEnumerable<StockPrice> GetAllStockPrices([IEnumeratorCancellation]CancellationToken cancellationToken = default){
            await Task.Delay(500, cancellationToken);
            yield return new StockPrice{...};
            await Task.Delay(500, cancellationToken);
            yield return new StockPrice{...};
        }
    }

    private async void Search_Click(object sender, RoutedEventArgs e){
        try{
            var identifiers = StockIdentifier.Text.Split(' ', ',');
            var data = new ObservableCollection<StockPrice>();
            Stocks.ItemsSource = data;

            var service = new MockStockStramService();
            var enumerator = service.GetAllStockPrices();
            await foreach(var price in enumerator.WithCancellation(CancellationToken.None)){
                if(identifiers.Contains(price.Identifier)){
                    data.Add(price);
                }
            }

        }catch(Exception ex){
            Notes.Text = ex.Message;
        }
    }

    /*Real implementation:*/
    public async IAsyncEnumerable<StockPrice> GetAllStockPrices([IEnumeratorCancellation]CancellationToken cancellationToken = default){
        using var stream = new StreamReader(File.OpenRead("file.csv"));
        await stream.ReadLineAsync(); //Skip header line
        string line;
        while((line = strea.ReadLineAsync()) != null){
            if(cancellationToken.IsCancellationRequested) bread;
            yield return StockPrice.FromCSV(line);
        }
    }
    ```
- StateMachine: 
    - Keeps track of tasks.
    - Executes the continuation.
    - Provides the continuation with a result.
    - Handles context switching. 
    - Reports errors.
- Each method marked as async will generate a state machine for that method. 
- A deadlock may occur if two threads depend on each other and one is blocked. 

## Asynchronous programming deep dive
- Out of the box the Task does not automatically report progress. 
- Progress<T> provides an IProgress<T> that invokes callbacks for each reported progress value. 
    ```c#
    try{
        var progress = new Progress<IEnumerable<StockPrice>>();
        progress.ProgressChanged += (_, stocks) =>{
            Stock.Progress.Value += 1;
        };
        await SearchForStocks(progress);
    }catch(Exception ex){

    }

    private async Task SearchForStocks(IProgress<IEnumerable<StockPrice>>){
        var service = new StockService();
         var loadingTasks = new List<Task<IEnumerable<StockPrice>>>();

    foreach(var identifier in identifiers.Text.Split(' ', ',')){
        var loadTask = service.GetStockPricesFor(identifier, CancellationToken.None);
        loadingTasks.Add(loadTask); 
        loadTask.ContinueWith(completedTask =>{
            progress?.Report(completedTask.Result);
            return completedTask.Result;
        });
    }

    }
    ```
- Work with attached and detacched tasks:
    ```c#
     static async Task Main(string[] args)
    {
            Console.WriteLine("Starting");
            await Task.Factory.StartNew(() => {
                Task.Factory.StartNew(() => {
                    Thread.Sleep(1000);
                    Console.WriteLine("Completed 1");
                });
                Task.Factory.StartNew(() => {
                    Thread.Sleep(2000);
                    Console.WriteLine("Completed 2");
                });
                Task.Factory.StartNew(() => {
                    Thread.Sleep(3000);
                    Console.WriteLine("Completed 3");
                });
            });

            Console.WriteLine("Completed");
            Console.ReadLine();
            /*Starting
            Completed
            Completed1
            Completed2
            Completed3*/
    }
    ```
- By default a child task executes independently of its parent. Use AttachedToParent to make them sinchronized. (This doesn't work if the parent is configured as DenyChildAttached (its default)). 
    ```c#
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
        /*Starting
            Completed1
            Completed2
            Completed3
            Completed*/
    ```
# Task parallel library

## Getting started with parallel programing

- Task executes only in one thread. 
- Parallel Linq (PLINQ).
- Parallel extensions: Build on-top of the Task in the Task Parallel library. 
- The methods on Parallel will efficiently distribute the work on the available cores. 
    ```c#
    var bag = new ConcurrentBag<StockCalculation>();
    Parallel.Invoke(
        ()=>{
            var msft = Calculate(stocks['MSFT']);
            bag.Add(msft);
        },
        ()=>{
            var googl = Calculate(stocks['GOOGL']);
            bag.Add(googl);
        },
        ()=>{
            var amaz = Calculate(stocks['AMAZ']);
            bag.Add(amaz);
        },
    );

    Stocks.ItemsSource = bag;
    ```
- ParallelOptions = configure maxdegreeofparalellism (number of cores/threads to use). 
- Parallel.Invoke, Parallel.For and Parallel.ForEach block the calling thread until all the parallel operations completed. To avoid that you can use Task.Run to move it to another context (this reduces in one the number of thread available to process).
    ```c#
    private async void Search_Click(object sender, RoutedEventArgs e){
        var bag = new ConcurrentBag<StockCalculation>();
        await Task.Run(()=>{
            Parallel.Invoke(
                ()=>{
                    var msft = Calculate(stocks['MSFT']);
                    bag.Add(msft);
                },
                ()=>{
                    var googl = Calculate(stocks['GOOGL']);
                    bag.Add(googl);
                },
                ()=>{
                    var amaz = Calculate(stocks['AMAZ']);
                    bag.Add(amaz);
                },
            );
        });
        Stocks.ItemsSource = bag;
    } 
    ```
- Aggregate exception:
    - This will give you every exception from a different parallel operation as inner exceptions.
    - Throwing an exception doesn't cancel a parallel operation. The exception is thrown until the entire collection of parallel operations is completed. 
    ```c#
    private async void Search_Click(object sender, RoutedEventArgs e){
        var bag = new ConcurrentBag<StockCalculation>();
        try{
            await Task.Run(()=>{
                    try{
                        Parallel.Invoke(
                            ()=>{
                                var msft = Calculate(stocks['MSFT']);
                                bag.Add(msft);
                            },
                            ()=>{
                                var googl = Calculate(stocks['GOOGL']);
                                bag.Add(googl);
                            },
                            ()=>{
                                var amaz = Calculate(stocks['AMAZ']);
                                bag.Add(amaz);
                            },
                        );
                    }
                    catch(Exception ex){
                        throw ex;
                    }
                    
                });
        }catch(Exception ex){
            Notes.Text = ex.Message;
        }
    
        Stocks.ItemsSource = bag;
    } 
    ```