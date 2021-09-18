# Scope: 
- Where to look for things. 
- Javascript is compiled every single time it is run.
- Batch is an interpreted language (goes from top to bottom). 
- Javascript does an initial pass through the code to compile that code and then another one and then in a final pass through does its execution.
- Javascript has function scope only.
    ```js
    var foo = "bar";//For the compiler these are two statements.

    function bar(){
        var foo = "baz";
    }

    function baz(){
        foo = "bam";
        bam = "yay";
    }
    ```
- First the compiler search for declarations and its scope. first foo declaration in global scope -> bar function declaration in global scope -> foo variable declaration in the bar() scope, foo local variable (argument) in the baz scope.
- var foo = "bar"; foo variable is an LHS reference (left hand side of the assignment =). "bar" is the RHS reference.
- The LHS is the target and the RHS is the source. 
- The compiler in non-strict mode creates bam variable in the global scope.
- undefined != undeclared. Undefined: it is declared but not initialized. Undefined is an actual value (not an abscense of a value).
- The global undefined property represents the primitive value undefined. It is one of JavaScript's primitive types.
    ```js
    var foo = "bar";

    function bar(){
        var foo = "baz";
        
        function baz(foo){
            foo = "bam";
            bam = "yay";
        }
        bar();
        console.log(foo); //bar
        console.log(bam); //yay
        baz();//Reference error
    }
    ```
    - function bar() is an RHS because is not assigned it is used. 
    - foo declaration in line 29 is called shadowing because we cannot access the foo from the global scope in there.
    - function declaration vs function expression. (function declaration is the very first thing in the "sentence"). 
    ```js
    var foo = function bar(){ //named function expression
        var foo = "baz";
        function baz(foo){
            foo = bar;
            foo;
        }
        baz();
        
    }

    foo();
    //bar(); //Error!
    ```
    - Anonymous function expressions:
        - We don't have a way to refer to itself (we can't use recurssion).
        - Using named function to use it in the stack traces.
        - It self-documents code. 
    - Lexical scope vs dynamic scope:
        - Lex = the parsing stage called lexing that occurs in the compiler when it's parsing through your code. 
    - eval keyword to cheat on the lexical scope (which already is defined and immutable at compile time):
        ```js
        var bar = "bar";
        function foo(str){
            eval(str);
            console.log(bar);//42
        }

        foo("var bar = 42;");
        ```
    - It pretends that the variable bar was declared at compile time. 
    - It's not good for optimization.
    - Set time out with string references does eval under the hood.
- with keyword:
    ```js
    var obj = {
        a : 2,
        b : 3,
        c : 4
    };

    obj.a = obj.b + obj.c; //a: 7
    obj.c = obj.b + obj.a; //c: 10

    with(obj){
        a = b + c; //a: 13
        d = 3
    }
    console.log(obj.d);//undefined
    console.log(d);//3
    ```
    - The with keyword creates a new lexical scope at runtime (bad).
- IIFE Pattern (creates a function and immediately execute it):
    - Immediately invoked function expression.
    ```js
    var foo = "foo";

    (function(){
        var foo = "foo2";
        console.log(foo);
    })();

    console.log(foo);
    /*
    foo2
    foo
    */
    ```
    - We can pass things into our IIFE
    ```js
    var foo = "foo";

    (function(bar){
        var foo = bar;
        console.log(foo);
    })(foo);

    console.log(foo);
    /*
    foo
    foo
    */
    ```
