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