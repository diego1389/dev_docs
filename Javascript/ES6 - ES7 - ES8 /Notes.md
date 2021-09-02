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
- To compile code in different languages: https://replit.com/languages
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
- rest operator. ... operator, it has to come last.
    ```js
    function findHighest(upperLimit, ...restValues){
        console.log("arguments: ");
        console.log(arguments);
        console.log("rest of values (excluding the first upper limit): ");
        console.log(restValues);
    }

    findHighest(80, 15, 80, 70, 55, 40, 30);
    /*
    arguments: 
    [Arguments] {
    '0': 80,
    '1': 15,
    '2': 80,ß
    '3': 70,
    '4': 55,
    '5': 40,
    '6': 30
    }
    rest of values (excluding the first upper limit): 
    [ 15, 80, 70, 55, 40, 30 ]
    */
    ```
- Spread syntax:
- Reduce function to return a single thing.
    ```js
    /*rest way*/
    function sum(...numberList){
        return numberList.reduce(
            function(total, num){
                return total+num
            }
        )
    }

    const total = sum(10,5,3);
    console.log(total);//18
    //------------------------------------
    function sum(a, b, c, d, e){
	return a + b + c + d + e;
    }

    const numbersArray = [2, 3, 1, 4, 0];
    const total = sum(...numbersArray);
    console.log(total); //10
    ```
- Another spread operator example:
    ```js
    function aReducer(state, action){
        switch(action.type){
            case 'ADD':
                //let newState = Object.assign({}, state); //makes a copy of state
                let newState = {...state}; //makes a copy of state
                newState.c = action.payload;
                return newState;
            default:
                return state;
        }
    }

    const currState = {
        a: 1,
        b: 2
    };

    const action = {
        type : 'ADD',
        payload : 3
    }

    console.log(aReducer(currState, action)); //{ a: 1, b: 2, c: 3 }
    ```
- Yet another example:
    - With spread operator you can make copies of objects and arrays.
    ```js
    const numberSet = [5, 2, 1, 4, 3];
    console.log(Math.min(...numberSet));
    /*Math.min definition: Math.min(value0, value1, ... , valueN)*/

    //You can merge arrays with spread operator
    const letters = ['a', 'b', 'c'];
    const numbersAndLetters = [1, 2, 3, ...letters, 4];

    console.log(numbersAndLetters);
    /*
    [
    1,   2,   3, 'a',
    'b', 'c', 4
    ]
    */
    const myArray1 = [1, 2, 3];
    const myArray2 = myArray1;
    myArray1.push(4);
    console.log(myArray2); //[ 1, 2, 3, 4 ] (it mutated the array!)


    const myArray1 = [1, 2, 3];
    const myArray2 = [...myArray1];
    myArray1.push(4);
    console.log(myArray2); //[ 1, 2, 3 ]
    ```
- Anonymous functions: In computer programming, an anonymous function is a function definition that is not bound to an identifier. Anonymous functions are often arguments being passed to higher-order functions.
    ```js
    const myArray1 = [2, 4, 6];

    myArray1.map((n, i)=>{
        console.log(i); //0 1 2
    });
    ```
- The regular function creates a new this, the arrow function doesn't.
- If you use an arrow function inside a regular function it takes the this (the context) of the regular function. If that's not the desired behaviour use a regular function inside as well. For example: 
    ```js
    function Timer(){
        setInterval(()=> {
            console.log(this); //The this is the Timer constructor (function)
        }, 250)
    }

    const timer = new Timer();
    /*
    Timer {}
    Timer {}
    Timer {}
    Timer {}
    */
    /*-------------------------*/
    function Timer(){
        setInterval(function(){
            console.log(this); //the this is the setInterval function
        }, 500)
    }

    const timer = new Timer();
    /*Timeout {
    _idleTimeout: 500,
    _idlePrev: null,
    _idleNext: null,
    _idleStart: 89,
    _onTimeout: [Function],
    _timerArgs: undefined,
    _repeat: 500,
    _destroyed: false,
    [Symbol(refed)]: true,
    [Symbol(kHasPrimitive)]: false,
    [Symbol(asyncId)]: 2,
    [Symbol(triggerId)]: 1
    }*/
    ```

- Object literal. Three primary ways to create an object.
    - let x = new Object();
    - let x = Object.create();
    - And object literal: let x = {};
    ```js
    const stuff = {
        name : "Diego",
        lastName : "Guillen",
        job: function(){
            console.log("Developer");
        }
    };
    stuf.name;
    stuff.job();//Developer
    ```
- Example without destructuring:
    ```js
    const someJSON = {
        "userId": 1,
        "id": 1,
        "title": "delectus aut autem",
        "completed": false
    };

    const userId = someJSON.userId;
    const id = someJSON.id;
    const title = someJSON.title;

    const newThing = {
        userId,
        id,
        title
    };

    function processSomeData(data){
        console.log(data);
    };

    processSomeData(newThing);//{ userId: 1, id: 1, title: 'delectus aut autem' }
    ```
- Example with destructuring
    ```js
   const someJSON = {
        "userId": 1,
        "id": 1,
        "title": "delectus aut autem",
        "completed": false
    };

        //you can change variable name title for newTitle like this:
    const {userId, id,  title : newTitle } = someJSON;

    const newThing = {
        userId,
        id,
        newTitle
    };

    function processSomeData({userId, id, newTitle}){
        console.log(`UserId: ${userId}, id: ${id} and title: ${newTitle}`);
    };

    processSomeData(newThing);//UserId: 1, id: 1 and title: delectus aut autem
    ```
- We can destructure nested objects (and arrays) as well with "inner destructuring":
    ```js
    const someJSON = {
        "userId": 1,
        "id": 1,
        "title": "delectus aut autem",
        "completed": false,
        "numbers" : [
            3,
            6,
            9
        ],
        "pets" : {
            "dogName" : "Xabi"
        }
    };

    const {numbers : {[0] : firstNumber, [2] : thirdNumber}} = someJSON;
    console.log(firstNumber, thirdNumber); //3 9
    const {pets : {dogName}} = someJSON;
    console.log(dogName); //Xabi
    ```
- We can use destructuring as a workaround for named parameters:
    ```js
    getArea({height : 30, width : 50});
    function getArea({width, height}){
        console.log(width); //50
    }
    ```
- We can destructure arrays:
    ```js
    const someJSON = {
        "userId": 1,
        "id": 1,
        "title": "delectus aut autem",
        "completed": false,
        "numbers" : [
            3,
            6,
            9,
            12,
            15
        ],
        "pets" : {
            "dogName" : "Xabi"
        }
    };

    const [first, second, third] = someJSON.numbers;

    console.log(`${first+second+third} `); //18
    ```
- And also you can skip values and use the rest operator:
    ```js
    const someJSON = {
        "numbers" : [
            3,
            6,
            9,
            12,
            15
        ]
    };

    const [, second,, ...others] = someJSON.numbers; //skip first and third values
    console.log(second);//6
    console.log(others);//[12, 15]
    ```
- Regular javscript "constructure". This instance variable (cannot use destructuring here).
    - Every time you create a new hero it will create a new copy of the "static" goodHero property and the powerUp function. 
    ```js
    function Hero(name, weapon, strength){
        this.name = name;
        this.weapon = weapon;
        this.strength = strength;

        this.goodHero = true;
        this.powerUp = function(){
            this.strength += 5;
        }
    }

    const hero1 = new Hero("Lu Bu", "Spear", 80);
    hero1.powerUp();
    console.log(hero1);
    /*
    Hero {
    name: 'Lu Bu',
    weapon: 'Spear',
    strength: 85,
    powerUp: [Function]
    }
    */
    ```
- To create a shared variable (a static variable you use the prototype keyword):
    ```js
    function Hero(name, weapon, strength){
        this.name = name;
        this.weapon = weapon;
        this.strength = strength;
    }

    Hero.prototype.powerUp =  function(){
            this.strength += 5;
        }

    const hero1 = new Hero("Lu Bu", "Spear", 80);
    hero1.powerUp();
    console.log(hero1);//Hero { name: 'Lu Bu', weapon: 'Spear', strength: 85 }
    ```
- Now JS has classes (syntactic sugar). 
    ```js
    class Hero{
        constructor(name, weapon, strength){
            this.name = name;
            this.weapon = weapon;
            this.strength = strength;
        }

        powerUp(){
            this.strength += 5;
        }
        
    }

    const hero1 = new Hero("Lu Bu", "Spear", 80);
    hero1.powerUp();
    console.log(hero1);//Hero { name: 'Lu Bu', weapon: 'Spear', strength: 85 }
    ```
- Setters and getters. 
    - Private variables in js start with underscore, otherwise they create an infinite loop in the setter (you set the name and calls the set and so on).
    ```js
    class Hero{
        constructor(name, weapon, strength){
            this._name = name;
            this._weapon = weapon;
            this._strength = strength;
        }

        powerUp(){
            this._strength += 5;
        }
        
        get name(){
            console.log("Getting name...");
        }

        set name(newName){
            console.log("Setting name...");
            this._name = newName;
        }
    }

    const LuBuDetails = ["Lu Bu", "Spear", 80];
    const hero1 = new Hero(...LuBuDetails);
    hero1.name = "Adan";
    hero1.powerUp();
    console.log(hero1);
    /*
    Setting name...
    Hero { _name: 'Adan', _weapon: 'Spear', _strength: 85 }
    */
    ```