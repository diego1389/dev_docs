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
- Hoisting: a conceptual model for how JS works. Find the variables and move it to the top at compile time. 
    ```js
    a;//undefined
    b;//undefined
    var a = b;
    var b = 2;
    b;//2
    a;//undefined
    //---------------
    //equals to:
    //---------------
    var a;
    var b;
    a;//undefined
    b;//undefined
    a = b;
    b = 2;
    b;//2
    a;//undefined
    ```
- Hoisting example:
    - The function expression does not get hoisted whereas the regular function declaration does. So it throws an error because it tries to execute d in the second line but it is undefined (TypeError: d is not a function).
    - If you move c = d() below the d function declaration it would have worked.
    - Functions are hoisted first than variables.
    ```js
     var a = b();
    var c = d();

    function b(){
        return c;
    }
    var d = function(){
        return b();
    } 
    ```
    - This is how the compiler process it: 
    ```js
    function b(){
        return c;
    }

    var a;
    var c; 
    var d;
    a = b();
    c = d();

    d = function(){
        return b();
    } 
    ```
    - Lets don't hoist (if you change let baz for var baz it should work):
    ```js
    function foo(bar){
        if(bar){
            console.log(baz); //Cannot access 'baz' before initialization
            let baz = bar;
        }
    }

    foo("bar");
    ```
- The window object is supported by all browsers. It represents the browser's window.

- All global JavaScript objects, functions, and variables automatically become members of the window object.

- Global variables are properties of the window object.
- Exercise scope solution:
    ```js
    (function(){
        function C() {
            console.log("OOPS!");
        }

        function E(f) {
            console.log("E");
            f();
            var f = F;
        }

        function A() {
            console.log("A");
            B();
        };

        var C;

        function G() {
            console.log("G");
            H();

            function H() {
                console.log("H");
                I();
            };
        }

        var D = d;

        function d() {
            console.log("D");
            E(F);
        }

        function I() {
            console.log("I");
            J();
            J();
        }

        function B() {
            console.log("B");
            C();
        };

        var F = function() {
            console.log("F");
            G();
        };

        var rest = "KLMNOPQRSTUVWXYZ".split(""), obj = {};
        for (var i=0; i<rest.length; i++) {
            (function(i){
                // define the current function
                obj[rest[i]] = function() {
                    console.log(rest[i]);
                    if (i < (rest.length-1)) {
                        obj[rest[i+1]]();
                    }
                };
            })(i);
        }

        var J = function() {
            J = function() {
                console.log("J");
                obj.K();
            };
        };

        function C() {
            console.log("C");
            D();
        };

        return A;
    })()();
    ```
- The this keyword: every function, while executing, has a reference to its current execution **context** called this. 
    - Execution context: where the function is called. 
- The call site: the placing code where a function gets executed. 
- **Four rules of this**:
1. The New Keyword:
    - When we put the new keyword in front of any function call, it turns the function call into what you might call a constructor call. Fourth things happen:
        1. A brand new empty object will be created.
        2. The object gets linked to a different object. 
        3. The new object gets bound as the this keyword for the purposes of that function call.
        4. If the function does not return anything, then it will be implicitly inserted between lines three and four (return this):
        ```js
        function foo(){
            this.baz = "baz";
            console.log(this.bar + " " +baz);
        }

        var bar = "bar";
        var baz = new foo();//undefined undefined

        console.log(baz.baz);//baz
        ``` 
2. Explicit binding rule: 
    - If you use .call or .apply at the call site, both of those utilities take the first parameter a this binding.
    - In this example it says use obj as my this:
    ```js
    function foo(){
        console.log(this.bar);
    }

    var bar = "bar1";

    var obj = {
        bar : "bar2"
    };

    foo();//bar1
    foo.call(obj);//bar2
    ```
    - Hard binding:
    ```js
    function foo(){
        console.log(this.bar);
    }

    var obj = {
        bar : "bar"
    };

    var obj2 = {
        bar : "bar2"
    };

    var orig = foo;
    var bar = "bar3";

    foo = function(){
        orig.call(obj);
    };

    foo(); //bar
    foo.call(obj2);//bar
    ```
    - You can also create a function to do it:
    ```js
    function bind(fn,o){
        return function(){
            fn.call(o);
        }
    }

    function foo(){
        console.log(this.bar);
    }

    var obj = {
        bar : "bar"
    };

    var obj2 = {
        bar : "bar2"
    };
    
    foo = bind(foo, obj);

    foo();
    foo.call(obj2);
    ```
    - It is already added in the language since ES5:
    ```js
    /*function bind(fn,o){
        return function(){
            fn.call(o);
        }
    }*/

    function foo(){
        console.log(this.bar);
    }

    var obj = {
        bar : "bar"
    };

    var obj2 = {
        bar : "bar2"
    };


    foo = foo.bind(obj);

    foo();//bar
    foo.call(obj2);//bar

    ```
3. Implicit binding rule:
    - When there is an object property reference at the call site (o2.foo()).
    - The rule says that object (the containing object) becomes the this keyword. It means its going to take "bar2" value when you do this.bar.
    ```js
    var o1 = {
        bar : "bar1",
        foo : function(){
            console.log(this.bar);
        }
    }

    var o2 = {
        bar : "bar2",
        foo : o1.foo
    }

    var bar = "bar3";
    var foo = o1.foo;

    o1.foo(); //bar1
    o2.foo();//bar2
    foo(); //bar3 //takes the bar value from the global context
    ``` 
4. Default binding rule:
    - If you are in strict mode, default the this keyword to the undefined value. If you're not in strict mode default the this keyword to the global object. 
    - It's not the strict mode of the entire program but the strict-mode of the foo function in this case. 
    ```js
    //'use strict';
    function foo(){
        //'use strict';
        console.log(this.bar); 
    }

    var bar = "bar1";
    var o2 = {bar : "bar2", foo : foo};
    var o3 = {bar : "bar3", foo : foo};
    foo(); /*bar1 but with 'use strict' it would have been cannot read bar of undefined.*/
    o2.foo();//bar2
    o3.foo();//bar3
        ```
- Binding confusion:
    - The this reference gets set by the call side of the function call (foo). Where does foo gets called and how does it gets called? It is in the global context so the this.bar is taken from the global context. 
    ```js
   function foo(){
        var bar = "bar1";
        baz();
        
    }

    function baz(){
        var bar = "bar3";
        console.log(this.bar);
    }
    var bar = "bar2";

    foo(); //bar2
    ```
- The four question to ask to deal with this in JS:
    1. Was the function called with `new`? Use that object.
    2. Was it called with `call` or `apply`? If so, use that object.
    3. Was the function called via containing/owning object (context)? USe that object. 
    4. Default: global object (except strict mode).
    ***The new keyword is able to override hard binding. 

# Closure: 

-  When a function remembers its lexical scope even when the function is executed outside that lexical scope. It is created when an inner function is transported outside of the outer function.  
- Example:
    - It is able to access the bar value even if the baz() function is executed outside its lexical scope (foo). It is passed as a parameter to bam).
    - It's not just a copy of the lexical scope is a reference to the existing lexical scope. It's kept alive. 
    ```js
    function foo(){
        var bar = "bar";
        function baz(){
            console.log(bar);
        }
        bam(baz);
    }

    function bam(baz){
        baz();//bar (has access to bar even though is outside of its lexical scope - foo function)
    }

    foo();
    ```
- Another way is with a function that returns a function and still has access to its lexical scope:
    ```js
    function foo(){
        var bar = "bar";
        return function baz(){
            console.log(bar);
        }
    }

    function bam(){
        foo()();//bar
    }

    bam();
    ```
- No matter how many functions have a clousure over the scope the all have a closure over the same scope:
    ```js
    function foo(){
        var bar = 0;
        setTimeout(function(){
            console.log(bar++); //++ at the end first prints, then updates
        }, 100);
        setTimeout(function(){
            console.log(bar++);
        }, 200);
    }

    foo(); //0  1 

    //------------------------------
    function foo(){
        var bar = 0;
        setTimeout(function(){
            var baz = 1;
            console.log(bar++);
            setTimeout(function(){
                console.log(bar+baz);
            }, 200);
            
        }, 100);
    }

    foo();//0 2
    ```
- Other example:
    - They share the same lexical scope (the same i value).
    ```js
    /*This is why it gets 6 in the for: */
    for(var i = 1; i <= 5; i++){
        //console.log(i);
    }

    console.log(i);//i
    /*Here starts the actual example: */
    for(var i = 1; i <= 5; i++){
        setTimeout(function(){
            console.log("i"+i);
        }, i*1000);
    }
    /*
    i6
    i6
    i6
    i6
    i6
    i6
    */
    ```
    - You can use IIFE to create a new lexical scope (a different copy of i every time): 
    ```js
    for(var i = 1; i <= 5; i++){
        (function(i){
            setTimeout(function(){
                console.log("i"+i);
            }, i*1000);
        })(i)
    }
    /*
    i1
    i2
    i3
    i4
    i5 
    */
    ```
    - The other solution is create the loop with the let keyword (creates a new lexical scope (works as an IIFE).
- Module patterns:
    -  Closure: clasic module pattern:
        1. There must be an outer wrapping function that gets executed.
        2. There must be one or more function that get returned from that function call. So one or more inner functions that have a closure over the inner private scope. 
        ```js
        var foo = (function(){
            var o = {bar : "bar"};

            return {
                bar : function(){
                    console.log(o.bar)
                }
            }
        })();

        foo.bar(); //bar
        ```
    - Moified module pattern (add a reference to modify, update properties and values).
        ```js
        var foo = (function(){
            var publicApi = {
                bar : function(){
                    publicApi.baz();
                },
                baz : function(){
                    console.log("baz");
                }
            }
            return publicApi;
        })();

        foo.bar(); //baz
        ```
        - foo and publicApi are references to the same object. 
    - Modern module pattern:
        ```js
        define("foo", function(){
            var o = {bar : "bar"};

            return {
                bar : function(){
                    console.log(o.bar)
                }
            }
        });
        ```
    - ES6 module pattern (foo.js):
        - It is syntactic sugar it asumes the whole file is a module.
        ```js
        var o = {bar: "bar"};
        export function bar(){
            return o.bar;
        }
        ```
        - There is two ways to import them from the module:
        ```js
        import bar from "foo"; //imports only the bar from foo
        bar(); //bar

        module foo from "foo"; //imports the whole module (rejected)
        foo.bar();//bar 
        ```
    - Benefits of the module pattern: encapsulation (rule of least exposure).
    - Tradeoff: difficult to test inner functions. 
- How to take a big spaguetti class and wrap it in a module (create an IIFE) and expose an API with just an init method and a method to load the data. Also keep the data private.
    ```js
    var NotesManager = (function(){
        function addNote(note) {
            $("#notes").prepend(
                $("<a href='#'></a>")
                .addClass("note")
                .text(note)
            );
        }

        function addCurrentNote() {
            var current_note = $("#note").val();

            if (current_note) {
                notes.push(current_note);
                addNote(current_note);
                $("#note").val("");
            }
        }

        function showHelp() {
            $("#help").show();

            document.addEventListener("click",function __handler__(evt){
                evt.preventDefault();
                evt.stopPropagation();
                evt.stopImmediatePropagation();

                document.removeEventListener("click",__handler__,true);
                hideHelp();
            },true);
        }

        function hideHelp() {
            $("#help").hide();
        }

        function handleOpenHelp(evt) {
            if (!$("#help").is(":visible")) {
                evt.preventDefault();
                evt.stopPropagation();

                showHelp();
            }
        }

        function handleAddNote(evt) {
            addCurrentNote();
        }

        function handleEnter(evt) {
            if (evt.which == 13) {
                addCurrentNote();
            }
        }

        function handleDocumentClick(evt) {
            $("#notes").removeClass("active");
            $("#notes").children(".note").removeClass("highlighted");
        }

        function handleNoteClick(evt) {
            evt.preventDefault();
            evt.stopPropagation();

            $("#notes").addClass("active");
            $("#notes").children(".note").removeClass("highlighted");
            $(evt.target).addClass("highlighted");
        }

        function init() {
            // build the initial list from the existing `notes` data
            var html = "";
            for (i=0; i<notes.length; i++) {
                html += "<a href='#' class='note'>" + notes[i] + "</a>";
            }
            $("#notes").html(html);

            // listen to "help" button
            $("#open_help").bind("click",handleOpenHelp);

            // listen to "add" button
            $("#add_note").bind("click",handleAddNote);

            // listen for <enter> in text box
            $("#new_note").bind("keypress",handleEnter);

            // listen for clicks outside the notes box
            $(document).bind("click",handleDocumentClick);

            // listen for clicks on note elements
            $("#notes").on("click",".note",handleNoteClick);
        }

        function loadData(data){
            notes = notes.concat(data)
        } 

        var notes = []

        var publicAPI = {
            init : init,
            loadData : loadData
        }

        return publicAPI;
    })();
    // assume this data came from the database
    NotesManager.loadData(
        [
            "This is the first note I've taken!",
            "Now is the time for all good men to come to the aid of their country.",
            "The quick brown fox jumped over the moon."
        ]
    );

    $(document).ready(NotesManager.init);
    ```
# Object orienting

--- Complete object orienting...

