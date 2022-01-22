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
- ** Vue instance:** describes what happens when a user interacts with our app. (JS)
- el property: element. It is used to tie one view instance to a view template through an id.
    ```js
    new Vue({
        el: '#app',
        methods: {
            onInput : function(){
                console.log("Input");
            }
        }
    });
    ```
- A method is a function that it is going to be tied to our instance. 
- Vue directives