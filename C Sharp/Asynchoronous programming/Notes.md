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