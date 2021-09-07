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
- Now we can create static methods:
    ```js
    class Hero{
        constructor(name, weapon, strength){
            this._name = name;
            this._weapon = weapon;
            this._strength = strength;
        }

        static getHero(){
            return true;
        }
    }

    const LuBuDetails = ["Lu Bu", "Spear", 80];
    const hero1 = new Hero(...LuBuDetails);

    console.log(Hero.getHero());//true
    ```
- Subclasses and prototypal inheritance:
    - super() is going to run the parent's constructor.
    - in case of having variables or methods with the same name it goes child first.
    ```js
    class Hero{
        constructor(name, weapon, strength){
            this._name = name;
            this._weapon = weapon;
            this._strength = strength;
        }

        static getHero(){
            return true;
        }
    }

    class Human extends Hero{
        constructor(health, ...heroDetails){
            super(...heroDetails);
            this._health = health;
        }
    }


    const LuBuDetails = [60, "Lu Bu", "Spear", 80];
    const AdanDetails = ["Adan", "Brass Knuckles", 90];
    const hero1 = new Human(...LuBuDetails);
    const hero2 = new Hero(...AdanDetails);
    console.log(hero1);
    /*Human { _name: 'Lu Bu', _weapon: 'Spear', _strength: 80, _health: 60 }*/
    console.log(hero2);
    /*Hero { _name: 'Adan', _weapon: 'Brass Knuckles', _strength: 90 }*/
    ```
- **Arrays:**
    - Different ways to iterate them (the first one doesnt iterate):
        ```js
        const array = [1, 3, 5, 7, 9];

        for(let i = 0; i < array.length; i++){
            console.log(array[i]);
        }
        //iterates over the keys
        for(let key in array){
            console.log(array[key]);
        }
        //iterates over the values
        for(let value of array){
            console.log(value);
        }

        array.forEach(value =>{
            console.log(value);
        })
        ```
    - Map will push and return an array.
    - Array.from = creates an array from an array-like or iterable object.
        - You can also passs a map function to call on every element of the array

        ```js
        const array = Array.from(`Diego`);
        console.log(array);//[ 'D', 'i', 'e', 'g', 'o' ]

        console.log(Array.from([1, 2, 3], x => x + x));//[ 2, 4, 6 ]
        ```
    - .of() //create a new array with a variable number of arguments:
        ```js
        const array = Array.of(7,6);
        console.log(array);//[ 7, 6 ]

        ```
    - fill(): it mutates the array. Arguments:
        - 1. Value you want to fill
        - 2. starting index
        - 3. stopping index
        - If you pass a negative value it starts counting from the end.
        ```js
        const array = [1, "a", 3, 4, "b", "c"];
        const array2 = [...array];
        array.fill("d", 2);

        console.log(array); //[ 1, 'a', 'd', 'd', 'd', 'd' ]

        array2.fill("d", 2, 3);
        console.log(array2);//[ 1, 'a', 'd', 4, 'b', 'c' ]
        ```
    - slice doesnt mutate the array

        ```js
        const array = [1, "a", 3, 4, "b", "c"];
        const array2 = array.slice(1,2);
        console.log(array2);//[ 'a' ]
        console.log(array);//[ 1, 'a', 3, 4, 'b', 'c' ]
        ```
    - splice does mutate the array. Arguments:
        - 1. Where to start
        - 2. How many to
        - 3. What to insert
        ```js
        const monthsDiscount = ["Jan", "Mar", "Jul", "Nov"];
        monthsDiscount.splice(2, 0, "Jun");//[ 'Jan', 'Mar', 'Jun', 'Jul', 'Nov' ]
        monthsDiscount.splice(2, 1, "Jun");//[ 'Jan', 'Mar', 'Jun', 'Nov' ]
        ```
    - find(): we provide a callback, if it returns true returns the (first) element that was on when the callback returned true.
        ```js
        const donut = {
            "id": "0001",
            "type": "donut",
            "name": "Cake",
            "ppu": 0.55,
            "batters":
                {
                    "batter":
                        [
                            { "id": "1001", "type": "Regular" },
                            { "id": "1002", "type": "Chocolate" },
                            { "id": "1003", "type": "Blueberry" },
                            { "id": "1004", "type": "Devil's Food" }
                        ]
                },
            "topping":
                [
                    { "id": "5001", "type": "None" },
                    { "id": "5002", "type": "Glazed" },
                    { "id": "5005", "type": "Sugar" },
                    { "id": "5007", "type": "Powdered Sugar" },
                    { "id": "5006", "type": "Chocolate with Sprinkles" },
                    { "id": "5003", "type": "Chocolate" },
                    { "id": "5004", "type": "Maple" }
                ]
        };

        const glazedTopping = donut.topping.find((t)=> {
            return t.id === "5002";
        });

        const glazedToppingIndex = donut.topping.findIndex((t)=> {
            return t.id === "5002";
        });


        console.log(glazedTopping); //{ id: '5002', type: 'Glazed' }
        console.log(glazedToppingIndex); //1
        ```
- All datatypes are primitives and objects. 
    - Primitives: string, number, bool, null, undefined and Symbol.
    ```js
    let str = "Popeye";
    let name = str; //we copied the value (because they are primitives)
    str = "Olive Oil";
    console.log(name);//Popeye

    /*Objects are stored by ref*/
    let cartoon = obj;
    console.log(obj);//{ name: 'Popeye' }
    console.log(cartoon);//{ name: 'Popeye' }
    obj.girlfriend = "Olive Oil";
    console.log(obj);//{ name: 'Popeye', girlfriend: 'Olive Oil' }
    console.log(cartoon);//{ name: 'Popeye', girlfriend: 'Olive Oil' }
    /*To make an actual copy and not copy the reference: */
    let cartoon = {...obj};
    ```
- The value of the variable primitive is inmutable.
- Map is a key-value pair. Its more protected than a regular object and comes with specific methods. It is also iterable:
    ```js
    let myContacts = new Map();
    myContacts.set("Diego", "83122155");
    const rob = myContacts.get("Diego");
    console.log(rob);//83122155
    ```
- You can use a function as a key (just for Map not for regular objects):
    ```js
    keyFunction = () =>{
        console.log("Hello from key. function");
    }

    let myContacts = new Map();
    myContacts.set("Diego", "83122155");
    myContacts.set(keyFunction, "Some value");
    const someValue = myContacts.get(keyFunction);
    console.log(someValue);//Some value

    //to get the size of the Map
    console.log(myContacts.size);
    // we can iterate:
    for(value of myContacts){
        console.log(value);
    }
    //clear method
    myContacts.clear();
    console.log(myContacts);//Map {}
    //get all the entries:
    console.log(myContacts.entries()); /*[Map Entries] {
    [ 'Diego', '83122155' ],
    [ [Function: keyFunction], 'Some value' ]
    }*/

    //get keys:
    console.log(myContacts.keys());/*[Map Iterator] { 'Diego', [Function: keyFunction] }*/

    //get values

    console.log(myContacts.values()); //[Map Iterator] { '83122155', 'Some value' }
    ```
- weakMap will allow garbage collection, if the key has no reference.
    - it's not iterable and only has get, set, has delete.
- Set object lets you store unique values of anytype (primitive or object).
    - sets are iterable.
    ```js
  let employeesIds = new Set([`a1`, `b2`, `c3`, `b2`]);
    console.log(employeesIds);//Set { 'a1', 'b2', 'c3' }

    let employeeSet = new Set();
    let employee1 = {
        name: `Employee name`,
        job: `Cashier`
    };
    let employee2 = employee1;

    employeeSet.add(employee1);
    employeeSet.add(employee2);

    console.log(employeeSet);//Set { { name: 'Employee name', job: 'Cashier' } }
    ```
    - WeakSet can only hold objects. Garbage collection.
        - It's not iterable, is no get method, size is always 0.
- **Symbol:**
    - A primitive, an identifier. Symbols are often used to add unique property keys to an object that won’t collide with keys any other code might add to the object, and which are hidden from any mechanisms other code will typically use to access the object. That enables a form of weak encapsulation, or a weak form of information hiding.
    ```js
    const CARCOLOR = Symbol();
    const CARMAKE = Symbol();
    const CARMODEL = Symbol();

    class Car{
        constructor(color, make, model){
            this[CARCOLOR] = color;
            this[CARMAKE]= make;
            this[CARMODEL] = model;
        }

        get color(){
            return this[CARCOLOR];
        }
    }

    let myCar = new Car("red", "Suzuki","Vitara");
    console.log(myCar.color);
    ```
- Javascript works with an event queue separate from the main thread. The events queue can check for things to execute.
- Functions are first class objects:
    - You can pass them around
    - Store them in a variable, etc.
    ```js
    printUpper = function(text){
        console.log(text.toUpperCase());
    }

    function run(callback, input){
        callback(input);
    }

    run(printUpper, `Diego`);//DIEGO
    ```
- Callback its a function that will be call later on, it will be called back.
    ```js
    function a(x){
        console.log(x);
        return function(y){
            console.log(x+y);
        }
    }
    a(2)(3);//2
    //5

    function b(num){
        const objectToReturn = {
            run : `RUN!`
        }
        return objectToReturn;
    }
    //RUN!
    ```
- Javascript is a single threaded language, but it is non-blocking, IO / event driven.
- Callback hell: a callback inside a callback inside a callback (inside the then for example).
- Use promises to avoid callback hell.
- Promises improve readability.
- A promise is a js constructor. The Promise object represents the eventual completion (or failure) of an asynchronous operation and its resulting value. It gives you a few methods:
    - then
    - catch
    - all
    - race
    - resolve, reject.
- A promise constructor expects one parameter: a callback function.
    ```js
    let myPromise = new Promise((resolve, reject)=> {
        console.log("Inside the promise");
        resolve()
    })



    myPromise.then(()=> {
        console.log("Promised finished");
    })

    console.log("Final line");
    /*Inside the promise
    Final line
    Promised finished*/
    ```
- The .then() method takes up to two arguments; the first argument is a callback function for the resolved case of the promise, and the second argument is a callback function for the rejected case. Each .then() returns a newly generated promise object. 
- Whatever argument you send to the resolve callback (it can be any datatype). When you make a call to an api the data is send to the then() through the resolve().
- Whatever argument you send in the reject you get in the catch.
    ```js
    const myPromise = new Promise((resolve, reject) => {
    setTimeout(() => {
        resolve('foo');
    }, 300);
    });

    const handleResolvedA = (value)=>{
        console.log(`handled resolved A value: ${value}`);
    }

    const handleRejectedA = () => {
        console.log("handled resolved B");
    }

    myPromise
    .then(handleResolvedA, handleRejectedA);
    //handled resolved A value: foo
    ```
- Promise.all takes an array of promises as an argument. It's an iterable of promises. The then of the all will wait until all the promises finish.
- Promise.race takes one parameter as well (an array of promises) and finishes as soon as the first promise finishes.
    ```js
    const myPromiseA = new Promise((resolve, reject) => {
    setTimeout(() => {
        resolve('foo');
    }, 2000);
    });

    const myPromiseB = new Promise((resolve) =>{
        setTimeout(() => {
        resolve('bar');
    }, 1000);
    })

    const handleResolvedA = (value)=>{
        console.log(`handled resolved A value: ${value}`);
    }

    const handleResolvedB = (value)=>{
        console.log(`handled resolved B value: ${value}`);
    }

    const handleRejectedA = () => {
        console.log("handled resolved B");
    }

    myPromiseA
    .then(handleResolvedA, handleRejectedA);

    myPromiseB
    .then(handleResolvedB);

    const promiseArray = [myPromiseA, myPromiseB];

    Promise.all(promiseArray).then((data) => {
        console.log("All promises finished", data);
    });

    Promise.race(promiseArray).then((data) => {
        console.log("Race finished", data);
    });
    /*handled resolved B value: bar
    Race finished bar
    handled resolved A value: foo
    All promises finished [ 'foo', 'bar' ]*/
    ```
- Array.from() to create a shallow-copied Array instance from an array-like or iterable object.
- Chaining promises: To avoid callback hell when you need the result from one promise to use it in another promise. 
    ```js
    //...
    function getCast(movie){
        return new Promise((resolve, reject)=>{
            $.ajax({
                url: `${castUrl}/${movie.id}/credits?api_key=${apiKey}`,
                method : `get`,
                success :(castData)=>{
                    resolve(castData.cast[0]);
                }
            })
        })
    }
    const moviePromise = getMovieData(movieElem[0].value); //gets a promise
    moviePromise.then((movieData)=>{
        return getCast(movieData);//gets another promise
    }).then((personInfo)=>{

    })

    ```