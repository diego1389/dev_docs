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
    ```js
    const name = 'Diego';
    const selectQuery =`SELECT * from users where name = "${name}"`;
    console.log(selectQuery);
    ```
- Multiline:
    ```js
    const text = `It's a rainy day
    a very rainy day`;

    console.log(text);
    ```
- Expressions (you can use regular javascript code inside template literals):
    ```js
    const anExpression = `10 * 3 = ${10*3}`;
    console.log(anExpression);
    const isMember = false;
    const anotherExpression = `Your price is ${isMember ? 10 : 12}`;
    console.log(anotherExpression);
    ```
- Tagged templates give us the ability to package up html and pre process it. You pass the 
    ```js
    let expression1 = "First expression";
    let expression2 = "Second expression";
    let expression3 = "Third expression";

    let lines = [expression1, expression2, expression3];

    //function buildHtml(strings, expression1, expression2)
    function buildHtml(strings, expressions){
        console.log(expressions); //array with expression1, expression2 and expression3
    }

    //const result = buildHtml`<li>${lines[0]}${lines[1]}</li>`;
    const result = buildHtml`<li>${lines}</li>`;
    ```
- Build and pre process html with tagged templates:
    ```js
    let expression1 = "First expression";
    let expression2 = "Second expression";
    let expression3 = "Third expression";

    let lines = [expression1, expression2, expression3];

    function buildHtml(strings, expressions){
        const newHtml = expressions.map((expression)=>{
            return `${strings[0]}${expression}${strings[1]}`;
        });
        return newHtml;
    }

    const result = buildHtml`<li>${lines}</li>`;
    console.log(result);
    /*
    [
    '<li>First expression</li>',
    '<li>Second expression</li>',
    '<li>Third expression</li>'
    ]
    */
    ```
- You cannot get functions result using just template literals:
    ```js
    function testFunction(){
        return "some other demo text";
    }

    const result = `Some text and ${() =>testFunction()}`; 
    console.log(result); //Some text and () =>testFunction()
    ```
- But with tagged literals you can:
    ```js
    function testFunction(){
        return "some other demo text";
    }

    const result = `Some text and ${() =>testFunction()}`; 
    console.log(result); //Some text and () =>testFunction()

    function taggedCallback(strings, func){
        return strings[0]+func();
    }

    const taggedLiteralCallback = taggedCallback`Some text and ${() =>testFunction()}`;
    console.log(taggedLiteralCallback);//Some text and some other demo text
    ```
- Optional parameters:
    ```js
    function getArea(x,y,s='r'){
        if(s === 'r'){
            return x*y;
        }
    }

    console.log(getArea(2,5));
    ```