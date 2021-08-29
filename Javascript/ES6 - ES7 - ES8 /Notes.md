- "use strict" rule.
- js by default uses the global scope.
    ```js
    for(var i = 0; i <=9; i++){
    }

    console.log("value after loop "+ i);  //value after loop 10  
    ```
- Hoisting: Hoisting is a JavaScript mechanism where variables and function declarations are moved to the top of their scope before code execution. Inevitably, this means that no matter where functions and variables are declared, they are moved to the top of their scope regardless of whether their scope is global or local.
    ```js
    console.log(i); //undefined
    for(var i = 0; i <=9; i++){
    }

    console.log("value after loop "+ i);  //value after loop 10  
    ```
- **let** is a block variable, not a function variable.
    ```js
    for(let i = 0; i <=9; i++){
    }

    console.log("value after loop "+ i); //Error, i is not defined
    ```
- **const** when something should change.
    ```js
    const i = 2;
    console.log(i);
    i = 3;//throws an error
    ```
- To compile code in different languages: https://replit.com/languages.
- Const means it doesn't bind the variable to other object (but you can change it without an equal sign). You can change what's inside just don't change what the variable is assigned to.
    ```js
    const myArray = [1, 2, 3, 4];
    console.log(myArray);
    //myArray = [1,2,3,4,5];//throws an error
    myArray.push(5);//this works
    ```
- Interpolation: you're trying to take something and insert it into something that's different.
    ```js
    const name = 'Diego';
    const selectQuery = 'SELECT * from users where name = "' + name + '"';
    console.log(selectQuery);
    ```
- Template literals: 