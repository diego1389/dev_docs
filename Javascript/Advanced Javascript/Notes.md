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
        baz();
    }
    ```
    - function bar() is an RHS because is not assigned it is used. 
    - foo declaration in line 29 is called shadowing because we cannot access the foo from the global scope in there.

