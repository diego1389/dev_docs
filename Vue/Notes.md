- **Vue template:** describes the structure and content of our app. Showing content to the users (HTML)
    ```html
    <div id="app">
        <h3>My identicon generator</h3>
        Input:
        <input v-on:input="onInput"/>
        <div>
        Output:
        </div>
    </div>
    ```
    - You can also define your template in the javascript side of the application.
- **Vue instance:** describes what happens when a user interacts with our app. (JS)
- el property: element. It is used to tie one view instance to a view template through an id.
    ```js
    new Vue({
        el: '#app',
        methods: {
            onInput : function(event){
                console.log(event.target.value);
            }
        }
    });
    ```
- A method is a function that it is going to be tied to our instance. 
- Vue directives: a piece of template syntax inside of Vue that enhances the behaviour of normal html code.
- Imperative vs declarative programming:
    - Imperative: step by step. 
    - Declarative: rules that our application should follow. 
        - We start with an initial state and apply a set of rules to that state. 
- **Data:** We are gonna define a data property inside of our Vue instance. The initial state of our instance.
- **Methods:** function that update our data.
- **Computed:** consume the data and get it into the actual template. Defines how to turn the current data into viewable values.
    - Every time we want to calculate something before showing it on the screen we need to do it in the computed section. 
- Final identicon demo:
    - View template
    ```html
    <div id="app">
        <h3>My identicon generator</h3>
        Input:
        <input v-on:input="onInput"/>
        <div>
        Output:
        <div v-html="identicon"></div>
        </div>
    </div>
    ```
    - Vue instance:
    ```js
    new Vue({
        el: '#app',
        data:{
          textInput : ''
        },
        computed:{
          identicon : function(){
            return jdenticon.toSvg(this.textInput, 200);
          }
        },
        methods: {
            onInput : function(event){
                this.textInput = event.target.value;
            }
        }
    });
    ```
- Template placement:
    - The template doesn't have to be inside an html file. You can use a template property:
    ```html
    <div id="app">
    </div>
    ```
    ```js
    new Vue({
        el: '#app',
        data:{
          textInput : ''
        },
        computed:{
          identicon : function(){
            return jdenticon.toSvg(this.textInput, 200);
          }
        },
        methods: {
            onInput : function(event){
                this.textInput = event.target.value;
            }
        },
        template:`
        <div>
        <h3>My identicon generator</h3>
        Input:
        <input v-on:input="onInput"/>
        <div>
        Output:
        <div v-html="identicon"></div>
        </div>
        </div>
        `
    });
    ```