## Vue exam notes:

1. What is VueJS?
    - Open-source progressive JS framework for building UI incrementally adoptable. 
2. Major features of VueJS?
    - **Virtual DOM:** uses virtual DOM similar to ReactJS. Light-weight in-memory tree representation of the original HTML DOM updating without affecting the DOM.
    - **Components:** used to create reusable custom elements.
    - **Templates:** HTML based templates that bind the DOM with the Vue instance data.
    - **Routing:** Navigation between pages is achieved through a vue-router.
    - **Light weight:** compared to other frameworks.
3. Lifecycle hooks:
    - **beforeCreate:** the very initialization of your component, observes data and initialization events (data properties are undefined here).
    - **created:** invoked when Vue has set up events and data observation.
    - **mounted:** most used hook since you will have full access to the reactive component, templates, and rendered DOM.
    - **beforeUpdate:** after data changes on your component and the updaate cycle begins, before the DOM is patched and re-rendered.
    - **updated:** runs after data changes on your component and the DOM re-renders.
    - **destroyed:** after your component has been destroyed, directives have been unbound and its event listeners have been removed.
4. What are conditional directives?
    - **v-if:** adds or removes DOM elements based on the given expression.
    - **v-else:** to display content only when the expression adjacent v-if resolves to false.
    - **v-else-if:** when you need more than two options to be checked.
    - **v-show:** similar to v-if but renders the elements and uses the CSS property to show/hide elements. v-if has higher cost while v-show has higher initial render cost. v-show has a performance advantage if the elements are switch on and off frequently.
5. What is the purpose of the v-for directive?
    - Allows to loop through items in an array or object. 
6. What is a vue instance?
    - Every vue application works by creating a new Vue instance with the Vue function:
    ```js
    var vm = new Vue({
        //options
    });
    ```
7. Why should not use if and for directives on the same element?
    - Because v-for directive has a higher priority than v-if.
    - Instead use a computed property to filter a list and then iterate it with v-for or move the condition to a parent by avoiding this check for each element.
8. Why do you need to use key attribute on for directive?
    - In order to track each node's identity and reuse and reorder existing elements.
9. Array mutation methods which trigger view updates: push(), pop(), shift(), unshift(), splice(), sort() and reverse().
10. Array detection non-mutation methods: filter(), concat() and slice().
11. Caveats of array changes detection. Vue cannot detect changes for the array in the following two cases:
    1. When you set an item directly with the index:
    ```js
    vm.todos[indexOfTodo] = newTodo;
    ```
    2. When you modify the length of the array:
    ```js
    vm.todos.length = todosLength;
    ```
    - You can overcome both using set and splice methods.
    ```js
    //first scenario
    Vue.set(vm.todos, indexOfTodo, newTodo);

    vm.todos.splice(indexOfTodo, 1, newTodo);

    //second scenario
    vm.todos.splice(todosLength);
    ```

- **Computed properties** are derived data. Often a subset of existing data used to move business logic out of the template.
- Difference between **computed property and method**: reactivity. The computed properties will be recalculated anytime some data change in the component. Also computed properties don't receive parameters.
- **Two ways to do two-way binding**: v-bind value and v-on method or just with v-model.
    - With v-model you don't need to assign the value directly.
- **Components:** reusable pieces of code. 
- **Component props:** pass data to the components. 
- You can use v-for directive in a component tag.
- Child parameter communication with events:
    ```js
    //child
    click(){
        this.$emit('chosen', this.number);//number is a prop
    }

    //parent
    <template>
        <num v-on:chosen="addNumber"/>
    </template>
    
    addNumber(number){
        //To do: something with the number
    }
    ```
- Style scoped: only applies to the component.
- You can define props as an array of strings or as an object.
- **Presentational component:** has no local state )it exists in the parent) and receives values via props and return back to the parent with this.$emit.
- **created hook:** it hasn't rendered anything yet.
- **mounted hook:** once it finishes rendering.
- **Modular components with slot:** reuse the card component with other things besides Pokemon:

## Vue course:
- Create your first Vue Hello world.
- Course source code: https://github.com/lmiller1990/complete-vuejs
- index.html
    ```html
    <!DOCTYPE html>
    <html lang="en">
    <head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    </head>
    <body>

    <div id="app">
        <h1>Hello {{msg}}</h1>
    </div>

    <script type="module">
        import * as Vue from 'vue/dist/vue.esm-bundler.js';

        const app = Vue.createApp({
        data(){
            return{
            msg: 'world'
            }
        }
        });
        app.mount('#app');
    </script>

    
    </body>
    </html>
    ```
- Move the data and the template to index.js file (and add the src attribute to the script tag):
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const app = Vue.createApp({
    template: `
    <h1>Hello {{msg}}</h1>
    `,
    data(){
        return{
        msg: 'world'
        }
    }
    });
    app.mount('#app');
    ```
- Methods
    - if you dont need to pass an argument you can just write the name of the method without ():
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const app = Vue.createApp({
    template: `
    <button v-on:click="increment(5)">Increment</button>
    <p>{{count}}</p>
    `,
    data(){
        return{
        count: 0
        }
    },
    methods:{
        increment(val){
        this.count += val;
        }
    }
    });
    app.mount('#app');
    ```
- Flow control:
    ```js
   import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const app = Vue.createApp({
    template: `
    <button v-on:click="increment">Increment</button>
    <p>{{count}}</p>
    <div v-if="isEven()">
        Even
    </div>
    <div v-else>
        Odd
    </div>
    `,
    data(){
        return{
            count: 0
        }
    },
    methods:{
        increment(){
            this.count += 1;
        },
        isEven(){
            return this.count % 2 === 0;
        }
    }
    });
    app.mount('#app');
    ```
- Loops with v-for directive:
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const app = Vue.createApp({
    template: `
    <button v-on:click="increment">Increment</button>
    <p>{{count}}</p>
    <div v-for="number in numbers">
        <div>
            {{number}} 
            <span v-if="isEven(number)"> is Even</span>
            <span v-else>is Odd</span>
        </div>
    </div>
    `,
    data(){
        return{
            count: 0,
            numbers: [1,2,3,4,5,6,7,8,9,10]
        }
    },
    methods:{
        increment(){
            this.count += 1;
        },
        isEven(number){
            return number % 2 === 0;
        }
    }
    });
    app.mount('#app');
    ```
- Computed properties. 
    - They are derived data. It is often going to be a subset of some existing data.
    - Computed properties are used to move your business logic out of your template.
    - A function with no values
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const app = Vue.createApp({
    template: `
    <button v-on:click="increment">Increment</button>
    <p>{{count}}</p>
    <div v-for="number in evenList">
    <div>
        {{number}} is Even!
    </div>
    </div>
    `,
    data(){
        return{
        count: 0,
        numbers: [1,2,3,4,5,6,7,8,9,10]
        }
    },
    computed:{
        evenList(){
            return this.numbers.filter(num => this.isEven(num))
        }
    },
    methods:{
        increment(){
            this.count += 1;
        },
        isEven(number){
            return number % 2 === 0;
        }
    }
    });
    app.mount('#app');
    ```
- Class bindings:
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const app = Vue.createApp({
    template: `
    <div>
    <button v-on:click="increment">Increment</button>
    <p>{{count}}</p>
    <div v-for="number in numbers"
        v-bind:class="getClass(number)">
        {{number}}
    <div>
    </div>
    `,
    data(){
        return{
        count: 0,
        numbers: [1,2,3,4,5,6,7,8,9,10]
        }
    },
    computed:{
        evenList(){
        return this.numbers.filter(num => this.isEven(num))
        }
    },
    methods:{
        getClass(number){
            return this.isEven(number) ? 'blue' : 'red';
        },
        increment(){
            this.count += 1;
        },
        isEven(number){
            return number % 2 === 0;
        }
    }
    });
    app.mount('#app');
    ```
- Input validation (two way binding)
    - Create a computed property for error instead of a new property in data section because it is derived data from the value property.
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const app = Vue.createApp({
    template: `
    <div>
    <button v-on:click="increment">Increment</button>
    <p>{{count}}</p>
    <input 
        v-bind:value="value"
        v-on:input="input"
    />
    <div class="red">
        {{error}}
    </div>
    <div v-for="number in numbers"
        v-bind:class="getClass(number)">
        {{number}}
    <div>
    </div>
    `,
    data(){
        return{
            count: 0,
            numbers: [1,2,3,4,5,6,7,8,9,10],
            value: 'user'
        }
    },
    computed:{
        evenList(){
            return this.numbers.filter(num => this.isEven(num))
        },
        error(){
            return (this.value.length < 5) ?
                'Must be greater than 5' :
                '';
        }
    },
    methods:{
        getClass(number){
            return this.isEven(number) ? 'blue' : 'red';
        },
        increment(){
            this.count += 1;
        },
        isEven(number){
            return number % 2 === 0;
        },
        input($event){
            this.value = $event.target.value;
        }
    }
    });
    app.mount('#app');
    ```
- The V-model
    - A faster way to do two-way binding.
    - You can get rid off v-bind and v-on and also of the input function. It does the binding automatically.
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const app = Vue.createApp({
    template: `
    <div>
    <button v-on:click="increment">Increment</button>
    <p>{{count}}</p>
    <input type="text"
        v-model="value"
    />
    <input type="radio"
        v-model="radioValue"
        value="a"/>
    <input type="radio"
        v-model="radioValue"
        value="b"/>
        {{radioValue}}
    
    <div class="red">
        {{error}}
    </div>
    <div v-for="number in numbers"
        v-bind:class="getClass(number)">
        {{number}}
    <div>
    </div>
    `,
    data(){
        return{
            count: 0,
            numbers: [1,2,3,4,5,6,7,8,9,10],
            value: 'user',
            radioValue : 'a'
        }
    },
    computed:{
        evenList(){
            return this.numbers.filter(num => this.isEven(num))
        },
        error(){
            return (this.value.length < 5) ?
                'Must be greater than 5' :
                '';
        }
    },
    methods:{
        getClass(number){
            return this.isEven(number) ? 'blue' : 'red';
        },
        increment(){
            this.count += 1;
        },
        isEven(number){
            return number % 2 === 0;
        }
    }
    });
    app.mount('#app');
    ```
- Components:
    - Reusable pieces of code.
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const Hello = {
    template : `
        <h3>Hello component</h3>
    `
    }
    const app = Vue.createApp({
    components: {
        Hello
    },
    template: `
    <div>
    <button v-on:click="increment">Increment</button>
    <p>{{count}}</p>
    <hello/>
    <input type="text"
        v-model="value"
    />
    <input type="radio"
        v-model="radioValue"
        value="a"/>
    <input type="radio"
        v-model="radioValue"
        value="b"/>
        {{radioValue}}
    
    <div class="red">
        {{error}}
    </div>
    <div v-for="number in numbers"
        v-bind:class="getClass(number)">
        {{number}}
    <div>
    </div>
    `,
    data(){
        return{
            count: 0,
            numbers: [1,2,3,4,5,6,7,8,9,10],
            value: 'user',
            radioValue : 'a'
        }
    },
    computed:{
        evenList(){
            return this.numbers.filter(num => this.isEven(num))
        },
        error(){
            return (this.value.length < 5) ?
                'Must be greater than 5' :
                '';
        }
    },
    methods:{
        getClass(number){
            return this.isEven(number) ? 'blue' : 'red';
        },
        increment(){
            this.count += 1;
        },
        isEven(number){
            return number % 2 === 0;
        }
    }
    });
    app.mount('#app');
    ```
- Component props:
    - Move the numbers logic to a separate component using props.
    - You can directly use a v-for directive in the component tag (pass the number of each iteration as a prop).
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const Num = {
        props: ['number'],
        template : `
            <div 
            v-bind:class="getClass(number)"
            >
            {{number}}
            </div>`,
        methods: {
            getClass(number){
                return this.isEven(number) ? 'blue' : 'red';
            },
            isEven(number){
                return number % 2 === 0;
            }
        }
    }

    const app = Vue.createApp({
    components: {
        Num
    },
    template: `
    <div>
        <button v-on:click="increment">Increment</button>
        <p>{{count}}</p>
        <input type="text"
        v-model="value"
        />
        <input type="radio"
        v-model="radioValue"
        value="a"/>
        <input type="radio"
        v-model="radioValue"
        value="b"/>
        {{radioValue}}
        
        <div class="red">
        {{error}}
        </div>
        <num
            v-for="number in numbers" 
            v-bind:number="number"/>
    </div>
    `,
    data(){
        return{
            count: 0,
            numbers: [1,2,3,4,5,6,7,8,9,10],
            value: 'user',
            radioValue : 'a'
        }
    },
    computed:{
        evenList(){
            return this.numbers.filter(num => this.isEven(num))
        },
        error(){
            return (this.value.length < 5) ?
                'Must be greater than 5' :
                '';
        }
    },
    methods:{
        increment(){
            this.count += 1;
        }
    }
    });
    app.mount('#app');
    ```
- Child-parent communication with events (from the num component to the app component)
    - You can access the prop value using this.prop
    ```js
    import * as Vue from 'vue/dist/vue.esm-bundler.js';

    const Num = {
    props: ['number'],
    template : `
        <button 
        v-bind:class="getClass(number)"
        v-on:click="click"
        >
        {{number}}
        </button>`,
    methods: {
        click(){
        //you can access the prop value using this.prop
            this.$emit('chosen', this.number);
        },
        getClass(number){
            return this.isEven(number) ? 'blue' : 'red';
        },
        isEven(number){
            return number % 2 === 0;
        }
    }
    }
    const app = Vue.createApp({
    components: {
        Num
    },
    template: `
        <num
            v-for="number in numbers" 
            v-bind:number="number"
            v-on:chosen="addNumber"/>
            <hr/>
            <num
            v-for="number in numberHistory" 
            v-bind:number="number"/>
    `,
    data(){
        return{
            numbers: [1,2,3,4,5,6,7,8,9,10],
            numberHistory: []
        }
    },
    computed:{
        evenList(){
            return this.numbers.filter(num => this.isEven(num))
        },
    },
    methods:{
        addNumber(number){
            this.numberHistory.push(number);
        }
    }
    });
    app.mount('#app');
    ```
## User sign-up form with validation

- Single file components:
    - App.vue
    ```js
    <template>
        App
    </template>
    <script>
    export default {
    
    }
    </script>
    <style scoped>
    </style>
    ```
    - Index.html
    ```html
    <!DOCTYPE html>
    <html lang="en">
    <head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    </head>
    <body>

    <div id="app">
    </div>

    <script type="module" src="./index.js"> 
    </script>

    </body>
    </html>
    ```
    - index.js
    ```js
    import {createApp} from 'vue';
    import App from './App.vue';

    const app = createApp(App);

    app.mount('#app')
    ```
- Create and use MyButton custom component
    - You can define your props as an array of string (as in the previous examples) or as objects (where you can specify the type of the prop).
    - Style scoped means the styles are only applied inside the particular template.
    - MyButton.vue
    ```js
    <template>
    <button
        v-bind:style="{background, color}"
        v-bind:disabled="disabled"
    >Button</button>
    </template>
    <script>
    export default {
    props:{
        background:{
            type: String
        },
        color:{
            type: String
        },
        disabled:{
            type: Boolean
        }
    }
    }
    </script>
    <style scoped>
    button:disabled{
        opacity: 0.5;
    }
    button{
        background: none;
        color: black;
        border:none;
        border-radius: 5px;
        padding:10px 40px;
        font-size: 16px;
        cursor:pointer;
    }
    button:hover{
        filter:brightness(125%);
    }
    </style>
    ```
    - App.vue
    - MyInput.vue
    ```js
    <template>
    <my-button
        background="darkslateblue"
        color="white"
        :disabled="!valid"
    />
    </template>
    <script>
    import MyButton from './MyButton.vue';

    export default {
        data(){
            return{
                valid : true
            }
        },
    components:{
        MyButton
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- :for is short for v-bind:for. You can also pass javascript expressions with : (for example :rules)
- Create a new input component:
    ```js
    <template>
    <div class="label">
        <label :for="name"> {{name}}</label>
        <div class="error">{{error}}</div>
    </div>
    <input :id="name"
            v-model="value"/>
    </template>
    <script>
    export default {
    props:{
        name: {
            type: String,
            required: true
        },
        rules:{
            type: Object,
            default: {}
        }
        },
        data(){
            return{
                value: ''
            }
        },
        computed:{
            error(){
                if(this.rules.required && this.value.length === 0){
                    return 'Value is required'
                }
                if(this.rules.min && this.value.length < this.rules.min){
                    return `Value the min length is ${this.rules.min}`
                }
                return '';
            }
        }
    }
    </script>
    <style scoped>
        .input-wrapper{
            display: flex;
            flex-direction: column;
        }
        .error{
            color:red;
        }
        .label{
            display: flex;
            justify-content: space-between;
        }
        input{
            background: none;
            color: black;
            border: 1px solid silver;
            border-radius: 5px;
            padding: 10px;
            margin: 5px 0;
            font-size: 16px;
        }
    </style>
    ```
    - App.vue
    ```js
    <template>
    <my-input name="Username"
        :rules="{required : true, min: 5}"
        />
    <my-button
        background="darkslateblue"
        color="white"
        :disabled="!valid"
    /> 
    </template>
    <script>
    import MyButton from './MyButton.vue';
    import MyInput from './MyInput.vue';

    export default {
        data(){
            return{
                valid : true
            }
        },
    components:{
        MyButton,
        MyInput
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Rethinking the form state:
    - Moving the state from the components to the parent using events.
    - MyInput.vue
    ```js
    <template>
    <div class="label">
        <label :for="name"> {{name}}</label>
        <div class="error">{{error}}</div>
    </div>
    <input :id="name"
        :value="value"
        @input="input"
    />
    </template>
    <script>
    export default {
    props:{
        name: {
            type: String,
            required: true
        },
        value:{
            type: String,
            required: true
        },
        rules:{
            type: Object,
            default: {}
        }
        },
        methods:{
            input($event){
                this.$emit('update', {
                    name: this.name.toLowerCase(),
                    value: $event.target.value});
            }
        },
        computed:{
            error(){
                if(this.rules.required && this.value.length === 0){
                    return 'Value is required'
                }
                if(this.rules.min && this.value.length < this.rules.min){
                    return `Value the min length is ${this.rules.min}`
                }
                return '';
            }
        }
    }
    </script>
    <style scoped>
        .input-wrapper{
            display: flex;
            flex-direction: column;
        }
        .error{
            color:red;
        }
        .label{
            display: flex;
            justify-content: space-between;
        }
        input{
            background: none;
            color: black;
            border: 1px solid silver;
            border-radius: 5px;
            padding: 10px;
            margin: 5px 0;
            font-size: 16px;
        }
    </style>
    ```
    - App.vue
    ```js
   <template>
    <my-input name="Username"
        :rules="{required : true, min: 5}"
        :value="username.value"
        @update="update"
        />
    <my-input name="Password"
        :rules="{required : true, min: 10}"
        :value="password.value"
        @update="update"
        />
    <my-button
        background="darkslateblue"
        color="white"
        :disabled="!valid"
    /> 
    </template>
    <script>
    import MyButton from './MyButton.vue';
    import MyInput from './MyInput.vue';

    export default {
        data(){
            return{
                valid : true,
                username:{
                    value: 'user',
                    valid: false
                },
                password:{
                    value: 'pass',
                    valid: false
                }
            }
        },
        methods:{
            //getting the properties using destructuring
            update({name, value}){
                this[name].value = value;
            }
        },
        components:{
            MyButton,
            MyInput
        }
    }
    </script>
    <style scoped>
    </style>
    ```
- Input is considered a presentational component. It has no local state. 
- Validating the form:
    - MyInput.vue
    ```js
    <template>
    <div class="label">
        <label :for="name"> {{name}}</label>
        <div class="error">{{error}}</div>
    </div>
    <input :id="name"
        :value="value"
        @input="input"
    />
    </template>
    <script>
    export default {
    props:{
        name: {
            type: String,
            required: true
        },
        value:{
            type: String,
            required: true
        },
        rules:{
            type: Object,
            default: {}
        },
        error:{
            type : String
        }
        },
        created(){
            this.$emit('update', {
                name: this.name.toLowerCase(),
                value: this.value,
                error: this.validate(this.value)
            });
        },
        methods:{
            input($event){
                this.$emit('update', {
                    name: this.name.toLowerCase(),
                    value: $event.target.value,
                    error: this.validate($event.target.value)
                });
            },
            validate(value){
                if(this.rules.required && value.length === 0){
                    return 'Value is required'
                }
                if(this.rules.min && value.length < this.rules.min){
                    return `Value the min length is ${this.rules.min}`
                }
                return '';
            }
        }
    }

    </script>
    <style scoped>
        .input-wrapper{
            display: flex;
            flex-direction: column;
        }
        .error{
            color:red;
        }
        .label{
            display: flex;
            justify-content: space-between;
        }
        input{
            background: none;
            color: black;
            border: 1px solid silver;
            border-radius: 5px;
            padding: 10px;
            margin: 5px 0;
            font-size: 16px;
        }
    </style>
    ```
    - App.vue
    ```js
    <template>
    <my-input name="Username"
        :rules="{required : true, min: 5}"
        :value="username.value"
        :error="username.error"
        @update="update"
        />
    <my-input name="Password"
        :rules="{required : true, min: 10}"
        :value="password.value"
        :error="password.error"
        @update="update"
        />
    <my-button
        background="darkslateblue"
        color="white"
        :disabled="!valid"
    /> 
    </template>
    <script>
    import MyButton from './MyButton.vue';
    import MyInput from './MyInput.vue';

    export default {
        data(){
            return{
                valid : true,
                username:{
                    value: 'user',
                    error: ''
                },
                password:{
                    value: 'pass',
                    error: ''
                }
            }
        },
        methods:{
            update({name, value, error}){
                this[name].value = value;
                this[name].error = error;
            }
        },
    components:{
        MyButton,
        MyInput
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Submitting the form:
    - MyInput.vue
    ```js
    <template>
    <div class="label">
        <label :for="name"> {{name}}</label>
        <div class="error">{{error}}</div>
    </div>
    <input :id="name"
        :value="value"
        :type="type"
        @input="input"
    />
    </template>
    <script>
    export default {
    emits: ['update'],
    props:{
        name: {
            type: String,
            required: true
        },
        value:{
            type: String,
            required: true
        },
        rules:{
            type: Object,
            default: {}
        },
        error:{
            type : String
        },
        type:{
            type: String,
            default: 'text'
        }
        },
        created(){
            this.$emit('update', {
                name: this.name.toLowerCase(),
                value: this.value,
                error: this.validate(this.value)
            });
        },
        methods:{
            input($event){
                this.$emit('update', {
                    name: this.name.toLowerCase(),
                    value: $event.target.value,
                    error: this.validate($event.target.value)
                });
            },
            validate(value){
                if(this.rules.required && value.length === 0){
                    return 'Value is required'
                }
                if(this.rules.min && value.length < this.rules.min){
                    return `Value the min length is ${this.rules.min}`
                }
                return '';
            }
        }
    }
    </script>
    <style scoped>
        .input-wrapper{
            display: flex;
            flex-direction: column;
        }
        .error{
            color:red;
        }
        .label{
            display: flex;
            justify-content: space-between;
        }
        input{
            background: none;
            color: black;
            border: 1px solid silver;
            border-radius: 5px;
            padding: 10px;
            margin: 5px 0;
            font-size: 16px;
        }
    </style>
    ```
    - App.vue
    ```js
    <template>
    <form @submit.prevent="submit">
    <my-input name="Username"
        :rules="{required : true, min: 5}"
        :value="username.value"
        :error="username.error"
        @update="update"
        />
    <my-input name="Password"
        :rules="{required : true, min: 10}"
        :value="password.value"
        :error="password.error"
        type="password"
        @update="update"
        />
        <my-button
        background="darkslateblue"
        color="white"
        :disabled="!valid"
    /> 
    </form>
    </template>
    <script>
    import MyButton from './MyButton.vue';
    import MyInput from './MyInput.vue';

    export default {
        data(){
            return{
                username:{
                    value: 'user',
                    error: ''
                },
                password:{
                    value: 'pass',
                    error: ''
                }
            }
        },
        computed:{
            valid(){
                return (
                    !this.username.error &&
                    !this.password.error
                )
            }
        },
        methods:{
            update({name, value, error}){
                this[name].value = value;
                this[name].error = error;
            },
            submit(){
                console.log("Submit")
            }
        },
    components:{
        MyButton,
        MyInput
    },
    }
    </script>
    <style scoped>
    </style>
    ```
## Pokemon cards

- Fetch an array of pokemons from the pokemon api:
- Created and mounted life cycle hooks.
    - Created goes first. It hasn't rendered anything yet.
    - Mounted: ones the does finishes rendering. If you need to access something from the html you have to do it in the mounted hook. But it is not recommended to manipulate the html directly.
```html
<template>
    <div class="card">
        <div class="title">
            Title
        </div>
         <div class="content">
            Content
        </div>
         <div class="description">
            Description
        </div>
    </div>
</template>
<script>
const api = 'https://pokeapi.co/api/v2/pokemon';
const ids = [1, 4, 7];

export default {
    data(){
        return{
            pokemon : []
        }
    },
    created(){
        this.fetchData();
    },
   methods:{
       async fetchData(){
           const responses = await Promise.all(
                    ids.map(id=> window.fetch(`${api}/${id}`)
                ));
            const json = await Promise.all(
                responses.map(data => data.json()
            ));
           
           this.pokemon = json.map(data=>({
                id: data.id,
                name: data.name,
                sprite: data.sprites.other['official-artwork'].front_default,
                types: data.types.map(type => type.type.name)
           }));
       }
   }
}
</script>
<style scoped>
    .card{
        border: 1px solid silver;
        border-radius: 8px;
        max-width: 200px;
        margin: 0 5px;
        cursor: pointer;
        box-shadow: 0px 1px 3px darkgrey;
        transition: 0.2s;
    }
    .title, .content, .description{
        padding: 16px;
        text-transform: capitalize;
        text-align: center;
    }

    .title, .content{
        border-bottom: 1px solid silver;
    }
    .title{
        font-size: 1.25em;
    }
    .card:hover{
        transition: 0.2s;
        box-shadow: 0px 1px 9px darkgrey;
    }
</style>
```

- Render the Pokemons!
```html
<template>
    <div class="cards">
        <div 
            class="card"
            v-for="p in pokemon"
            :key="p.id">
            <div class="title">
                {{p.name}}
            </div>
            <div class="content">
                <img :src="p.sprite"/>
            </div>
            <div class="description">
                <div 
                    v-for="type of p.types" 
                    :key="type">
                {{type}}
                </div>
            </div>
        </div>
    </div>
</template>
<script>
const api = 'https://pokeapi.co/api/v2/pokemon';
const ids = [1, 4, 7];

export default {
    data(){
        return{
            pokemon : []
        }
    },
    created(){
        this.fetchData();
    },
   methods:{
       async fetchData(){
           const responses = await Promise.all(
                    ids.map(id=> window.fetch(`${api}/${id}`)
                ));
            const json = await Promise.all(
                responses.map(data => data.json()
            ));
           
           this.pokemon = json.map(data=>({
                id: data.id,
                name: data.name,
                sprite: data.sprites.other['official-artwork'].front_default,
                types: data.types.map(type => type.type.name)
           }));
       }
   }
}
</script>
<style scoped>
    .cards{
        display: flex;
    }
    .card{
        border: 1px solid silver;
        border-radius: 8px;
        max-width: 200px;
        margin: 0 5px;
        cursor: pointer;
        box-shadow: 0px 1px 3px darkgrey;
        transition: 0.2s;
    }
    .title, .content, .description{
        padding: 16px;
        text-transform: capitalize;
        text-align: center;
    }

    .title, .content{
        border-bottom: 1px solid silver;
    }
    .title{
        font-size: 1.25em;
    }
    .card:hover{
        transition: 0.2s;
        box-shadow: 0px 1px 9px darkgrey;
    }
    img{
        width: 100%;
    }
</style>
```
- Modular components with slots
    - Card.vue
    ```js
    <template>
        <div class="card">
            <div class="title">
                <slot name="title"/>
            </div>
            <div class="content">
                <slot name="content"/>
            </div>
            <div class="description">
                <slot name="description"/>
            </div>
        </div>
    </template>
    <script>
    const api = 'https://pokeapi.co/api/v2/pokemon';
    const ids = [1, 4, 7];

    export default {

    }
    </script>
    <style scoped>
        .card{
            border: 1px solid silver;
            border-radius: 8px;
            max-width: 200px;
            margin: 0 5px;
            cursor: pointer;
            box-shadow: 0px 1px 3px darkgrey;
            transition: 0.2s;
        }
        .title, .content, .description{
            padding: 16px;
            text-transform: capitalize;
            text-align: center;
        }

        .title, .content{
            border-bottom: 1px solid silver;
        }
        .title{
            font-size: 1.25em;
        }
        .card:hover{
            transition: 0.2s;
            box-shadow: 0px 1px 9px darkgrey;
        }
    </style>
    ```
    - App.vue
    ```js
    <template>
        <div class="cards">
            <card
                v-for="pokemon in pokemons"
                :key="pokemon.id">
                <template v-slot:title>
                    {{pokemon.name}}
                </template>
                <template v-slot:content>
                    <img :src="pokemon.sprite"/>
                </template>  
                <template v-slot:description>
                    <div 
                        v-for="type of pokemon.types" 
                        :key="type">
                            {{type}}
                    </div>
                </template>   
            </card>
        </div>
    </template>
    <script>
    import Card from './Card.vue';
    const api = 'https://pokeapi.co/api/v2/pokemon';
    const ids = [1, 4, 7];

    export default {
        components:{
            Card
        },
        data(){
            return{
                pokemons : []
            }
        },
        created(){
            this.fetchData();
        },
    methods:{
        async fetchData(){
            const responses = await Promise.all(
                        ids.map(id=> window.fetch(`${api}/${id}`)
                    ));
                const json = await Promise.all(
                    responses.map(data => data.json()
                ));
            
            this.pokemons = json.map(data=>({
                    id: data.id,
                    name: data.name,
                    sprite: data.sprites.other['official-artwork'].front_default,
                    types: data.types.map(type => type.type.name)
            }));
        }
    }
    }
    </script>
    <style scoped>
        img{
            width: 100%;
        }
        .cards{
            display: flex;
        }
    </style>
    ```
- Reusing code to fetch evolutions and class bindings. Separationg the business logic and the presentation components.
    -App.vue
    ```js
    <template>
    <pokemon-cards
        :pokemons="pokemons"
        @chosen="fetchEvolutions"
        :selectedId="selectedId"/> 
     <pokemon-cards
        :pokemons="evolutions"/> 
    </template>
    <script>
    import PokemonCards from './PokemonCards.vue';
    const api = 'https://pokeapi.co/api/v2/pokemon';
    const IDS = [1, 4, 7];

    export default {
        components:{
            PokemonCards
        },
        data(){
            return{
                pokemons : [],
                evolutions : [],
                selectedId : null
            }
        },
        async created(){
            this.pokemons = await this.fetchData(IDS);
        },
    methods:{
        async fetchData(ids){
            const responses = await Promise.all(
                        ids.map(id=> window.fetch(`${api}/${id}`)
                    ));
                const json = await Promise.all(
                    responses.map(data => data.json()
                ));
            
            return json.map(data=>({
                    id: data.id,
                    name: data.name,
                    sprite: data.sprites.other['official-artwork'].front_default,
                    types: data.types.map(type => type.type.name)
            }));
        },
            async fetchEvolutions(pokemon){
                this.evolutions = await this.fetchData(
                    [pokemon.id + 1, pokemon.id + 2]
                );
                this.selectedId = pokemon.id;
            }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
    - PokemonCards.vue
    ```js
    <template>
    <div class="cards">
        <card
            v-for="pokemon in pokemons"
            :key="pokemon.id"
            @click="click(pokemon)"
            :class="{opace :selectedId && pokemon.id !== selectedId}"
            class="card">
            <template v-slot:title>
                {{pokemon.name}}
            </template>
             <template v-slot:content>
                 <img :src="pokemon.sprite"/>
            </template>  
            <template v-slot:description>
                <div 
                    v-for="type of pokemon.types" 
                    :key="type">
                        {{type}}
                </div>
            </template>   
        </card>
    </div>
    </template>
    <script>
    import Card from './Card.vue';
    export default{
        props: {
            pokemons : {
                type: Array,
                default : []
            },
            selectedId :{
                type: Number
            }
        },
        components:{
            Card
        },
        methods:{
            click(pokemon){
                this.$emit('chosen', pokemon);
            }
        }
    }
    </script>
    <style scoped>
        .card:hover{
            opacity:1.0;
        }
        .opace{
            opacity : 0.5;
        }
        img{
            width: 100%;
        }
        .cards{
            display: flex;
        }
    </style>
    ```
    - Card.vue
    ```js
    <template>
    <div class="card">
        <div class="title">
            <slot name="title"/>
        </div>
        <div class="content">
            <slot name="content"/>
        </div>
        <div class="description">
            <slot name="description"/>
        </div>
    </div>
    </template>
    <script>
    const api = 'https://pokeapi.co/api/v2/pokemon';
    const ids = [1, 4, 7];

    export default {

    }
    </script>
    <style scoped>
        .card{
            border: 1px solid silver;
            border-radius: 8px;
            max-width: 200px;
            margin: 0 5px;
            cursor: pointer;
            box-shadow: 0px 1px 3px darkgrey;
            transition: 0.2s;
        }
        .title, .content, .description{
            padding: 16px;
            text-transform: capitalize;
            text-align: center;
        }

        .title, .content{
            border-bottom: 1px solid silver;
        }
        .title{
            font-size: 1.25em;
        }
        .card:hover{
            transition: 0.2s;
            box-shadow: 0px 1px 9px darkgrey;
        }
    </style>
    ```
## Composition API

- Previously we used the options API, where you put everything in specific places (methods, data and computed).
- In composition API everything goes inside the setup() method. It is more flexible.
    - You have to be more explicit. You need to return every variable that you want to use in the template.
    ```js
    <template>
    <div>
        {{msg}}
    </div>
    </template>
    <script>
    export default {
    setup(){
        const msg = "Hello world";
        return {
            msg
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Reactivity with ref
    - Decoupled reactivity. With the options API reactivity happened behind the scenes.
    - The setup() function is only called once, when the component is first created.
    - The options API is built on top of the composition API.
    - When you want to render something in the template you don't have to specify object.value. This is called **ref unwrapping**
    ```js
    <template>
    <button @click="increment">{{count}}</button>
    </template>
    <script>
    import {ref} from 'vue';
    export default {
    setup(){
        const count = ref(0);

        const increment = () =>{
            count.value++;
        }

        return {
            count,
            increment
        }
    }
    }
    </script>
    <style scoped>
    button{
        height: 200px;
        width: 200px;
        font-size: 40px;
    }
    </style>
    ```
- Reactive for Complex Values
    - For reactive you don't have to specify the value keyword.
    - For primitive data type you can only use ref. Reactive is used for objects.
    ```js
    <template>
    <button @click="increment">{{count}}</button>
    <button @click="increase('a')">{{numbers.a}}</button>
    <button @click="increase('b')">{{numbers.b}}</button>
    </template>
    <script>
    import {ref, reactive} from 'vue';
    export default {
    setup(){
        const count = ref(0);
        const numbers = reactive({
            a:0,
            b:0
        });

        const increment = () =>{
            count.value++;
        }

        const increase = (n) =>{
            numbers[n]++;
        }

        return {
            count,
            increment,
            numbers,
            increase
        }
    }
    }
    </script>
    <style scoped>
    button{
        height: 100px;
        width: 100px;
        font-size: 40px;
    }
    </style>
    ```
- Composing computed properties:
    ```js
    <template>
    <button @click="increment">{{count}}</button>
    <button @click="increase('a')">{{numbers.a}}</button>
    <button @click="increase('b')">{{numbers.b}}</button>
    <h3>{{total}}</h3>
    </template>
    <script>
    import {ref, reactive, computed} from 'vue';
    export default {
    setup(){
        const count = ref(0);
        const numbers = reactive({
            a:1,
            b:2
        });

        const increment = () =>{
            count.value++;
        }

        const increase = (n) =>{
            numbers[n]++;
        }

        const total = computed(()=>{
            return count.value + numbers.a + numbers.b; 
        });

        return {
            count,
            increment,
            numbers,
            increase,
            total
        }
    }
    }
    </script>
    <style scoped>
    button{
        height: 100px;
        width: 100px;
        font-size: 40px;
    }
    </style>
    ```
- Watch and Watch Effect
    - Every time a values change it fires the watch event. 
    - This doesn't fire the first time at least you specify immediate : true.
    ```js
    <template>
    <button @click="increment">{{count}}</button>
    <button @click="increase('a')">{{numbers.a}}</button>
    <button @click="increase('b')">{{numbers.b}}</button>
    <h3>{{total}}</h3>
    </template>
    <script>
    import {ref, reactive, computed, watch, watchEffect} from 'vue';
    export default {
    setup(){
        let count = ref(0);
        const numbers = reactive({
            a:1,
            b:2
        });

        const increment = () =>{
            count.value++;
        }

        const increase = (n) =>{
            numbers[n]++;
        }

        watch(numbers, (newVal) => {
            console.log(`a: ${newVal.a} b: ${newVal.b}`);
        })

        watchEffect(() =>{
            console.log(numbers.a);
        });

        const total = computed(()=>{
            return count.value + numbers.a + numbers.b; 
        });

        return {
            count,
            increment,
            numbers,
            increase,
            total
        }
    }
    }
    </script>
    <style scoped>
    button{
        height: 100px;
        width: 100px;
        font-size: 40px;
    }
    </style>
    ```
- Before and after with watch
    - You can only use it with ref objects not with  reactive ones.
    - You cannot pass an array into reactive.
    ```js
    <template>
    <button @click="increment">{{count}}</button>
    <button @click="a++">{{a}}</button>
    <button @click="b++">{{b}}</button>
    <h3>{{total}}</h3>
    <div 
        v-for="number in history"
        :key="number">
        {{number}}
    </div>
    </template>
    <script>
    import {ref, reactive, computed, watch, watchEffect} from 'vue';
    export default {
    setup(){
        let count = ref(0);
        let a = ref(0);
        let b = ref(0);
        const history = ref([]);

        const increment = () =>{
            count.value++;
        }

        watch([a,b], ([newA, newB], [oldA, oldB]) =>{
            if(newA !== oldA){
                history.value.push(`A: ${oldA} => ${newA}`);
            }
            if(newB !== oldB){
                history.value.push(`B: ${oldB} => ${newB}`);
            }
        })

        const total = computed(()=>{
            return count.value + a.value + b.value; 
        });

        return {
            a,
            b,
            count,
            increment,
            total,
            history
        }
    }
    }
    </script>
    <style scoped>
    button{
        height: 100px;
        width: 100px;
        font-size: 40px;
    }
    </style>
    ```
- The useNumbers Composable.
    - A composable to group functionality together.
    - useNumbers.js
    ```js
    import {ref, reactive, computed, watch, watchEffect} from 'vue';

    export function useNumbers(){
        let a = ref(0);
        let b = ref(0);
        const history = ref([]);
        
        watch([a,b], ([newA, newB], [oldA, oldB]) =>{
            if(newA !== oldA){
                history.value.push(`A: ${oldA} => ${newA}`);
            }
            if(newB !== oldB){
                history.value.push(`B: ${oldB} => ${newB}`);
            }
        })
        
        const total = computed(()=>{
            return a.value + b.value; 
        });

        return{
            a, b, history, total
        }
    }
    ```
    - App.vue
    ```js
    <template>
    <button @click="increment">{{count}}</button>
    <button @click="a++">{{a}}</button>
    <button @click="b++">{{b}}</button>
    <h3>{{total}}</h3>
    <div 
        v-for="number in history"
        :key="number">
        {{number}}
    </div>
    </template>
    <script>
    import {ref, reactive, computed, watch, watchEffect} from 'vue';
    import {useNumbers} from './useNumbers.js';

    export default {
        setup(){
            let count = ref(0);

                const increment = () =>{
                count.value++;
            }

                const {
                    a, b, total, history
                } = useNumbers();

            return {
                a,
                b,
                count,
                increment,
                total,
                history
            }
        }
    }
    </script>
    <style scoped>
    button{
        height: 100px;
        width: 100px;
        font-size: 40px;
    }
    </style>
    ```
    - You can even return the composable object directly:
    ```js
    <template>
    <button @click="a++">{{a}}</button>
    <button @click="b++">{{b}}</button>
    <h3>{{total}}</h3>
    <div 
        v-for="number in history"
        :key="number">
        {{number}}
    </div>
    </template>
    <script>
    import {useNumbers} from './useNumbers.js';

    export default {
        setup(){
            return useNumbers();
        }
    }
    </script>
    <style scoped>
    button{
        height: 100px;
        width: 100px;
        font-size: 40px;
    }
    </style>
    ```
- VueUse is a collection of Vue composition utilities you can import into your project.

## Composing a microblog

- Create a global state inside a store and display it to the screen:
    - store.js
    ```js
    import {reactive} from 'vue';

    class Store{
        constructor(){
            this.state = reactive({
                posts: [
                    {
                        id : 1,
                        title: 'Title',
                        content:  'Learning Vue.js 3'
                    }
                ]
            })
        }
    }

    export const store = new Store();
    ```
    - App.vue
    ```js
    <template>
    <div
        v-for="post in store.state.posts"
        :key="post.id">
        {{post.title}}
    </div>
    </template>
    <script>
    import {store} from './store.js';
    export default {
    setup(){
        return{
            store
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Render posts with the Card component
    - Controls.vue
    ```js
    <template>
    {{post}}
    </template>
    <script>

    export default {
        props:{
            post : {
                type : Object,
                required: true
            }
        },
    setup(props){
        console.log("props", props)
    }
    }
    </script>
    <style scoped>
    </style>
    ```
    - store.js
    ```js
    import {reactive} from 'vue';

    class Store{
        constructor(){
            this.state = reactive({
                posts: [
                    {
                        id : 1,
                        title: 'Title',
                        content:  'Learning Vue.js 3',
                        likes: 10,
                        hashtags: [
                            'vue',
                            'javascript',
                            'composition.API'
                        ]
                    }
                ]
            })
        }
    }

    export const store = new Store();
    ```
    - App.vue
    ```js
    <template>
    <card
        v-for="post in store.state.posts"
        :key="post.id">
        <template v-slot:title>
            {{post.title}}
        </template>
        <template v-slot:content>
            {{post.content}}
        </template>  
        <template v-slot:description>
            <controls :post="post"/>
        </template>  
    </card>
    </template>
    <script>
    import {store} from './store.js';
    import Card from '../pokemon/Card.vue';
    import Controls from './Controls.vue';

    export default {
    setup(){
            return{
                store,
                Controls,      
                Card
            }
        }
    }
    </script>
    <style scoped>
    </style>
    ```
- The hashtag component
    - Hashtag.vue
    ```js
    <template>
    <div>
        #{{hashtag}}
    </div>
    </template>
    <script>
    export default {
        props:{
            hashtag:{
                type : String
            }
        },
        setup() {
            
        }
    }
    </script>
    ```
    - Controls.vue
    ```js
    <template>
    <button>Like</button>{{post.likes}}
    <hashtag 
        v-for="hashtag in post.hashtags"
        :key="hashtag"
        :hashtag="hashtag"/>
    </template>
    <script>
    import Hashtag from './Hashtag.vue'

    export default {
        props:{
            post : {
                type : Object,
                required: true
            }
        },
    setup(props){
        return{
            Hashtag
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
    - store.js
    ```js
    import {reactive} from 'vue';
    import {testPosts} from './testPosts.js';

    class Store{
        constructor(){
            this.state = reactive({
                posts: testPosts
            })
        }
    }

    export const store = new Store();
    ```
- Emiting events with the composition API
    - Hashtag.vue
    ```js
    <template>
    <div 
        class="hashtag"
        @click="setHashtag">
        #{{hashtag}}
    </div>
    </template>
    <script>
    export default {
        props:{
            hashtag:{
                type : String,
                required: true
            }
        },
        emits: ['setHashtag'],
        setup(props, ctx) {
            const setHashtag = () =>{
                ctx.emit('setHashtag', props.hashtag);
            }

            return{
                setHashtag
            }
        }
    }
    </script>
    <style scoped>
        .hashtag{
            text-decoration: underline;
        }
        .hashtag:hover{
            color: cornflowerblue;
        }
    </style>
    ```
    - Controls.vue
    ```js
    <template>
    <button>Like</button>{{post.likes}}
    <hashtag 
        v-for="hashtag in post.hashtags"
        :key="hashtag"
        :hashtag="hashtag"
        @setHashtag="setHashtag"/>
    </template>
    <script>
    import Hashtag from './Hashtag.vue'

    export default {
        props:{
            post : {
                type : Object,
                required: true
            }
        },
        emits: ['setHashtag'],
    setup(props, ctx){
            const setHashtag = (hashtag) =>{
                ctx.emit('setHashtag', hashtag);
            }
            return{
                setHashtag,
                Hashtag
            }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
    - App.vue
    ```js
    <template>
    <card
        v-for="post in store.state.posts"
        :key="post.id">
        <template v-slot:title>
            {{post.title}}
        </template>
        <template v-slot:content>
            {{post.content}}
        </template>  
        <template v-slot:description>
            <controls :post="post"
                @setHashtag="setHashtag"/>
        </template>  
    </card>
    {{currentTag}}
    </template>
    <script>
    import {store} from './store.js';
    import Card from '../pokemon/Card.vue';
    import Controls from './Controls.vue';
    import {ref} from 'vue';

    export default {
    setup(){
        const currentTag = ref(null);
            const setHashtag = (hashtag) =>{
                currentTag.value = hashtag
            }
        return{
            store,
            Controls,      
            Card,
            setHashtag,
            currentTag
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Filtering posts with computed:
    - App.vue
    ```js
    <template>
    <card
        v-for="post in filteredPosts"
        :key="post.id">
        <template v-slot:title>
            {{post.title}}
        </template>
        <template v-slot:content>
            {{post.content}}
        </template>  
        <template v-slot:description>
            <controls :post="post"
                @setHashtag="setHashtag"/>
        </template>  
    </card>
    </template>
    <script>
    import {store} from './store.js';
    import Card from '../pokemon/Card.vue';
    import Controls from './Controls.vue';
    import {ref, computed} from 'vue';

    export default {
    setup(){
        const currentTag = ref(null);
            const setHashtag = (hashtag) =>{
                currentTag.value = hashtag
            }

            const filteredPosts = computed(()=>{
                if(!currentTag.value){
                    return store.state.posts;
                }
                return store.state.posts.filter(
                    post => post.hashtags.includes(currentTag.value)
                );
            })
        return{
            filteredPosts,
            Controls,      
            Card,
            setHashtag
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Refactoring with the posts store
    - store.js
    ```js
    import {reactive} from 'vue';
    import {testPosts} from './testPosts.js';

    class Store{
        constructor(){
            this.state = reactive({
                posts: testPosts,
                currentTag: null
            })
        }

        setHashtag(tag){
            this.state.currentTag = tag;
        }
    }

    export const store = new Store();
    ```
    - Hashtag.vue
    ```js
    <template>
    <div 
        class="hashtag"
        @click="setHashtag">
        #{{hashtag}}
    </div>
    </template>
    <script>
    import {store} from './store.js';
    export default {
        props:{
            hashtag:{
                type : String,
                required: true
            }
        },
        setup(props, ctx) {
            const setHashtag = () =>{
                store.setHashtag(props.hashtag);
            }

            return{
                setHashtag
            }
        }
    }
    </script>
    <style scoped>
        .hashtag{
            text-decoration: underline;
        }
        .hashtag:hover{
            color: cornflowerblue;
        }
    </style>
    ```
    - App.vue
    ```js
    <template>
    <card
        v-for="post in filteredPosts"
        :key="post.id">
        <template v-slot:title>
            {{post.title}}
        </template>
        <template v-slot:content>
            {{post.content}}
        </template>  
        <template v-slot:description>
            <controls :post="post"/>
        </template>  
    </card>
    </template>
    <script>
    import {store} from './store.js';
    import Card from '../pokemon/Card.vue';
    import Controls from './Controls.vue';
    import {computed} from 'vue';

    export default {
        components:{
            Controls,
            Card
        },
    setup(){
            const filteredPosts = computed(()=>{
                if(!store.state.currentTag){
                    return store.state.posts;
                }
                return store.state.posts.filter(
                    post => post.hashtags.includes(store.state.currentTag)
                );
            })
        return{
            filteredPosts
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
    - Controls.vue (doesn't need to emit the setHashtag anymore)
    ```js
    <template>
    <button>Like</button>{{post.likes}}
    <hashtag 
        v-for="hashtag in post.hashtags"
        :key="hashtag"
        :hashtag="hashtag"/>
    </template>
    <script>
    import Hashtag from './Hashtag.vue'

    export default {
        components:{
            Hashtag
        },
        props:{
            post : {
                type : Object,
                required: true
            }
        }
    }
    </script>
    <style scoped>
    </style>
    ```
- Liking a post:
    - Controls.vue
    ```js
    <template>
    <button @click="click">Like</button>{{post.likes}}
    <hashtag 
        v-for="hashtag in post.hashtags"
        :key="hashtag"
        :hashtag="hashtag"/>
    </template>
    <script>
    import Hashtag from './Hashtag.vue';
    import {store} from './store.js';

    export default {
        components:{
            Hashtag
        },
        props:{
            post : {
                type : Object,
                required: true
            }
        },
        setup(props){
            const click = () => {
                store.incrementLike(props.post);
            }
            return{
                click
            }
        }
    }
    </script>
    <style scoped>
    </style>
    ```
    - store.js
    ```js
    import {reactive} from 'vue';
    import {testPosts} from './testPosts.js';

    class Store{
        constructor(){
            this.state = reactive({
                posts: testPosts,
                currentTag: null
            })
        }

        setHashtag(tag){
            this.state.currentTag = tag;
        }

        incrementLike(post){
            const thePost = this.state.posts.find(x=> {return x.id == post.id});
            if(!thePost){
                return;
            }
            thePost.likes++;
        }
    }

    export const store = new Store();
    ```
- Searching for hashtags
    - App.vue
    ```js
    <template>
    <input 
        :value="currentTag"
        @input="setHashtag"/>
    <card
        v-for="post in filteredPosts"
        :key="post.id">
        <template v-slot:title>
            {{post.title}}
        </template>
        <template v-slot:content>
            {{post.content}}
        </template>  
        <template v-slot:description>
            <controls :post="post"/>
        </template>  
    </card>
    </template>
    <script>
    import {store} from './store.js';
    import Card from '../pokemon/Card.vue';
    import Controls from './Controls.vue';
    import {computed} from 'vue';

    export default {
        components:{
            Controls,
            Card
        },
    setup(){
        const setHashtag = ($event)=>{
            store.setHashtag($event.target.value)
        }
            const filteredPosts = computed(()=>{
                if(!store.state.currentTag){
                    return store.state.posts;
                }
                return store.state.posts.filter(
                    post => post.hashtags.includes(store.state.currentTag)
                );
            })
        return{
            filteredPosts,
            currentTag: computed(()=> store.state.currentTag),
            setHashtag
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Refactor
    - store.js
    ```js
    import {reactive, computed} from 'vue';
    import {testPosts} from './testPosts.js';

    class Store{
        constructor(){
            this.state = reactive({
                posts: testPosts,
                currentTag: null
            })
        }

        setHashtag(tag){
            this.state.currentTag = tag;
        }

        incrementLike(post){
            const thePost = this.state.posts.find(x=> {return x.id == post.id});
            if(!thePost){
                return;
            }
            thePost.likes++;
        }

        get filteredPosts(){
            if(!store.state.currentTag){
                return store.state.posts;
            }
            return store.state.posts.filter(
                post => post.hashtags.includes(store.state.currentTag)
            );
        }
    }

    export const store = new Store();
    ```
    - App.vue
    ```js
    <template>
    <input 
        :value="currentTag"
        @input="setHashtag"/>
    <card
        v-for="post in filteredPosts"
        :key="post.id">
        <template v-slot:title>
            {{post.title}}
        </template>
        <template v-slot:content>
            {{post.content}}
        </template>  
        <template v-slot:description>
            <controls :post="post"/>
        </template>  
    </card>
    </template>
    <script>
    import {store} from './store.js';
    import Card from '../pokemon/Card.vue';
    import Controls from './Controls.vue';
    import {computed} from 'vue';

    export default {
        components:{
            Controls,
            Card
        },
    setup(){
        const setHashtag = ($event)=>{
            store.setHashtag($event.target.value)
        }
        return{
            filteredPosts : computed(()=>store.filteredPosts),
            currentTag: computed(()=> store.state.currentTag),
            setHashtag
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```

    ## Vue router fundamentals

    - Create a router and register a new route
    - router.js
    ```js
    import {
    createWebHistory,
    createRouter,
    } from 'vue-router';
    import Hello from './Hello.vue';

    const router = createRouter({
        history: createWebHistory(),
        routes: [
            {
                path: '/hello',
                component: Hello,
            }
        ]
    })

    export {router};
    ```
    - Install the router plugin:
    - index.js
    ```js
    import {createApp} from 'vue';
    import App from './App.vue';
    import { router } from './router.js';
    const app = createApp(App);

    app.use(router)
    app.mount('#app')
    ```
    - Hello.vue
    ```js
    <template>
        Hello Route
    </template>
    <script>
    export default{
        setup() {
            
        }
    }
    </script>
    ```
    - App.vue. The <router-view> tag works as a slot.
    ```js
    <template>
        <router-view/>
    </template>
    <script>
    export default {
    
    }
    </script>
    <style scoped>
    </style>
    ```
- The Router Link component
    - router.js
    ```js
    import {
        createWebHistory,
        createRouter,
    } from 'vue-router';
    import Hello from './Hello.vue';
    import Posts from './Posts.vue';

    const router = createRouter({
        history: createWebHistory(),
        routes: [
            {
                path: '/hello',
                component: Hello,
            },
            {
                path: '/posts',
                component: Posts
            }
        ]
    })

    export {router};
    ```
    - App.vue
    ```js
    <template>
    <router-link to="/hello">
        Hello
    </router-link>
    <router-link to="/posts">
        Posts
    </router-link>
    <router-view/>
    </template>
    <script>
    export default {
    
    }
    </script>
    <style scoped>
    </style>
    ```
    - Posts.vue
    ```js
    <template>
    <h2>Posts</h2>
    </template>
    <script>

    export default {
        setup() {
            
        }
    }
    </script>
    ```
- Nested routes (posts/1) vs dynamic routes. Route params with the composition API:
    - router.js
    ```js
    import {
        createWebHistory,
        createRouter,
    } from 'vue-router';
    import Hello from './Hello.vue';
    import Posts from './Posts.vue';
    import Post from './Post.vue';

    const router = createRouter({
        history: createWebHistory(),
        routes: [
            {
                path: '/hello',
                component: Hello,
            },
            {
                path: '/posts',
                component: Posts,
                children: [
                    {
                        path: ':id',
                        component: Post
                    }
                ]
            }
        ]
    })

    export {router};
    ```
    - Posts.vue
    ```js
    <template>
    <h2>Posts</h2>
    <ul>
        <li v-for="post in testPosts"
         :key="post.id">
        <router-link        
            :to="`/posts/${post.id}`">
            {{post.title}}
        </router-link>
        </li>
 
    </ul>

    <router-view></router-view>
    </template>
    <script>
    import {testPosts} from '../microblog/testPosts.js'

    export default {
        setup() {
            return{
                testPosts
            }
        }
    }
    </script>
    ```
    - Post.vue
    ```js
   <template>
    <h4>{{post.title}}</h4>
    <p>{{post.content}}</p>
    </template>
    <script>
    import {computed} from 'vue';
    import {useRoute} from 'vue-router';
    import {testPosts} from '../microblog/testPosts.js'
    export default {
        setup(){
            const route = useRoute();
            const post = computed(() => testPosts.find(x => x.id === parseInt(route.params.id)))
            return {
                post
            }
        }
    }
    </script>
    ```
- New Post route
    - router.js
    ```js
    import {
    createWebHistory,
    createRouter,
    } from 'vue-router';
    import Hello from './Hello.vue';
    import Posts from './Posts.vue';
    import Post from './Post.vue';
    import NewPost from './NewPost.vue';

    const router = createRouter({
        history: createWebHistory(),
        routes: [
            {
                path: '/hello',
                component: Hello,
            },
            {
                path: '/posts',
                component: Posts,
                children: [
                    {
                        path: 'new',
                        component: NewPost
                    },
                    {
                        path: ':id',
                        component: Post
                    }
                ]
            }
        ]
    })

    export {router};
    ```
    - NewPost.vue
    ```js
    <template>
    <h3>New Post</h3>
    <form @submit.prevent="submit">
        <input 
            v-model="newPost.title"
            placeholder="Title"/>
        <br/>
        <textarea
            cols="50"
            rows="10"
            v-model="newPost.content"/>
        <br/>
        <button>Submit</button>
    </form>
    </template>
    <script>
    import {reactive} from 'vue';
    export default {
        setup() {
            const newPost = reactive({
                title : '',
                content : ''
            })

            const submit = () => {
                console.log("submit");
            }
            return{
                newPost,
                submit
            }
        },
    }
    </script>
    ```
- Posts.vue
    ```js
    <template>
    <h2>Posts</h2>
    <ul>
        <li v-for="post in testPosts"
         :key="post.id">
        <router-link        
            :to="`/posts/${post.id}`">
            {{post.title}}
        </router-link>
        </li>
 
    </ul>
    <router-link
        :to="`/posts/new`">
        New
    </router-link>
    <router-view></router-view>
    </template>
    <script>
    import {testPosts} from '../microblog/testPosts.js'

    export default {
        setup() {
            return{
                testPosts
            }
        }
    }
    </script>
    ```
- usePosts Composable / Add a new Post:
    - usePosts.js
    ```js
    import {testPosts} from '../microblog/testPosts.js'
    import { ref } from 'vue'

    export function usePosts(){
        const posts = ref(testPosts)

        const addPost = (post)=>{
            posts.value.push(post);
        }
        return {
            posts,
            addPost
        }
    }
    ```
    - Posts.vue
    ```js
    <template>
    <h2>Posts</h2>
    <router-link
        :to="`/posts/new`">
         New Post
    </router-link>
    <ul>
        <li v-for="post in posts"
         :key="post.id">
        <router-link        
            :to="`/posts/${post.id}`">
            {{post.title}}
        </router-link>
        </li>
 
    </ul>

    <router-view></router-view>
    </template>
    <script>
    import {usePosts} from './usePosts.js'

    export default {
        setup() {
            const postsStore = usePosts();
            return{
                posts : postsStore.posts
            }
        }
    }
    </script>
    ```
    - Post.vue
    ```js
    <template>
    <h4>{{post.title}}</h4>
    <p>{{post.content}}</p>
    </template>
    <script>
    import {computed} from 'vue';
    import {useRoute} from 'vue-router';
    import {usePosts} from './usePosts.js'

    export default {
        setup(){
            const route = useRoute();
            const postsStore = usePosts();
            const post = computed(() => postsStore.posts.value.find(x => x.id === parseInt(route.params.id)))
            return {
                post
            }
        }
    }
    </script>
    ```
    - NewPost.vue
    ```js
    <template>
    <h3>New Post</h3>
    <form @submit.prevent="submit">
    <input 
        v-model="newPost.title"
        placeholder="Title"/>
    <br/>
    <textarea
        cols="50"
        rows="10"
        v-model="newPost.content"/>
    <br/>
    <button>Submit</button>
    </form>
    </template>
    <script>
    import {reactive} from 'vue';
    import {usePosts} from './usePosts.js'

    export default {
        setup() {
            const postsStore = usePosts();

            const newPost = reactive({
                title : '',
                content : ''
            })

            const submit = () => {
                postsStore.addPost({
                    id : postsStore.posts.value.length +1,
                    title: newPost.title,
                    content: newPost.content
                })
            }
            return{
                newPost,
                submit
            }
        },
    }
    </script>
    ```
- Redirects with Vue router:
    - NewPost.vue
    ```js
    <template>
    <h3>New Post</h3>
    <form @submit.prevent="submit">
    <input 
        v-model="newPost.title"
        placeholder="Title"/>
    <br/>
    <textarea
        cols="50"
        rows="10"
        v-model="newPost.content"/>
    <br/>
    <button>Submit</button>
    </form>
    </template>
    <script>
    import {reactive} from 'vue';
    import {usePosts} from './usePosts.js'
    import {useRouter} from 'vue-router';

    export default {
        setup() {
            const postsStore = usePosts();
            const router = useRouter();

            const newPost = reactive({
                title : '',
                content : ''
            })

            const submit = () => {
                const id = postsStore.posts.value.length +1;
                postsStore.addPost({
                    id,
                    title: newPost.title,
                    content: newPost.content
                })
                router.push(`/posts/${id}`);
            }
            return{
                newPost,
                submit
            }
        },
    }
    </script>
    ```
- Refactor to options API:
    - NewPost.vue
    ```js
    <template>
    <h3>New Post</h3>
    <form @submit.prevent="submit">
    <input 
        v-model="newPost.title"
        placeholder="Title"/>
    <br/>
    <textarea
        cols="50"
        rows="10"
        v-model="newPost.content"/>
    <br/>
    <button>Submit</button>
    </form>
    </template>
    <script>
    export default {
        data(){
            return{
                newPost : {
                    title : '',
                    content: ''
                }
            }
        },
        emits: ['createPost'],
        methods:{
                submit(){
                    this.$emit('createPost',
                    {
                        title: this.newPost.title,
                        content: this.newPost.content
                    })
                }
            }
        }
    </script>
    ```
    - Posts.vue
    ```js
    <template>
    <h2>Posts</h2>
    <router-link
        :to="`/posts/new`">
         New Post
    </router-link>
    <ul>
        <li v-for="post in posts"
         :key="post.id">
        <router-link        
            :to="`/posts/${post.id}`">
            {{post.title}}
        </router-link>
        </li>
 
    </ul>

    <router-view
        :posts="posts"
        @createPost="createPost"/>
    </template>
    <script>
    import {testPosts} from '../microblog/testPosts.js'

    export default {
    data(){
        return{
            posts: testPosts
        }
    },
    methods:{
        createPost(newPost){
            const id = this.posts.length +1;
            this.posts.push({
                id,
                title: newPost.title,
                content: newPost.content
            })
                this.$router.push(`/posts/${id}`);
        }
    }
    }
    </script>
    ```
    - Post.vue
    ```js
    <template>
    <h4>{{post.title}}</h4>
    <p>{{post.content}}</p>
    </template>
    <script>


    export default {
        props:{
            posts : {
                type : Array,
                default : []
            }
        },
        computed:{
            post(){
                return this.posts.find(x => x.id === parseInt(this.$route.params.id))
            }
        }
    }
    </script>
    ```
## Vuex fundamentals

- State management. Create your first store:

    - store.js
    ```js
    import {createStore} from 'vuex';

    export const store = createStore({
        state(){
            return{
                count : 0
            }
        }
    })
    ```
    -App.vue
    ```js
    <template>
    {{store.state.count}}
    </template>
    <script>
    import {useStore} from 'vuex';
    export default {
    setup(){
        const store = useStore()
        return{
            store
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Do not manipulate the state directly. Use a mutation to have one centralize the state manipuation. This is more scalable.
- A mutation is a special method to update the state. You don't call the mutation directly, you rather commit it.
- You can pass the value to update using a payload. If you need to pass multiple values you can pass an object. 
    - store.js
    ```js
    import {createStore} from 'vuex';

    export const store = createStore({
        state(){
            return{
                count : 0
            }
        },
        mutations:{
            //destructuring the payload
            increment(state, {number}){
                state.count+= number;
            }
        }
    })
    ```
    - App.vue
    ```js
    <template>
    {{store.state.count}}
    <button @click="click">Increment</button>
    </template>
    <script>
    import {useStore} from 'vuex';
    export default {
    setup(){
        const store = useStore()

        const click = () =>{
            store.commit('increment', {number:7});
        }
        return{
            store,
            click
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- More mutations:
    - store.js
    ```js
    import {createStore} from 'vuex';

    export const store = createStore({
        state(){
            return{
                postId : null
            }
        },
        mutations:{
            setPostId(state, postId){
                state.postId = postId;
            }
        }
    })
    ```
    - App.vue
    ```js
    <template>
    <button
        v-for="post in posts"
        :key="post.id"
        @click="click(post)">
        {{post.title}}
        </button>
        {{postId}}
    </template>
    <script>
    import {useStore} from 'vuex';
    import {computed} from 'vue';
    export default {
    setup(){
        const store = useStore()
        const posts =[
            {id: 1, title: 'Post #1'},
            {id: 2, title: 'Post #2'}
        ]

        const click = (post)=>{
            store.commit('setPostId', post.id);
        }
        return{
            postId: computed(()=>store.state.postId),
            posts,
            click
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Dispatching actions
    - store.js
    ```js
    import {createStore} from 'vuex';
    const delay = () => new Promise(res => setTimeout(res,1000));
    import {testPosts} from '../microblog/testPosts';
    export const store = createStore({
        state(){
            return{
                postId : null
            }
        },
        mutations:{
            setPostId(state, postId){
                state.postId = postId;
            }
        },
        actions:{
            async fetchPosts(ctx, payload){
                await delay();
                console.log(payload);
            }
        }
    })
    ```
    - App.vue
    ```js
    <template>
    <button
        v-for="post in posts"
        :key="post.id"
        @click="click(post)">
        {{post.title}}
        </button>
        {{postId}}
    </template>
    <script>
    import {useStore} from 'vuex';
    import {computed, onMounted} from 'vue';
    export default {
    setup(){
        const store = useStore()
        const posts =[
            {id: 1, title: 'Post #1'},
            {id: 2, title: 'Post #2'}
        ]

        const click = (post)=>{
            store.commit('setPostId', post.id);
        }

        const fetchData = () => {
            store.dispatch('fetchPosts', 'POST');
        }

        onMounted(()=>{
            fetchData();
        });

        return{
            postId: computed(()=>store.state.postId),
            posts,
            click
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Mocking the server and fetching posts
    - store.js
    ```js
    import {createStore} from 'vuex';
    const delay = () => new Promise(res => setTimeout(res,1000));
    import {testPosts} from '../microblog/testPosts';
    export const store = createStore({
        state(){
            return{
                postId : null,
                posts : []
            }
        },
        mutations:{
            setPostId(state, postId){
                state.postId = postId;
            },
            setPosts(state, posts){
                state.posts = posts
            }
        },
        actions:{
            async fetchPosts(ctx){
                await delay();
                ctx.commit('setPosts', testPosts);
            }
        }
    })
    ```
    - App.vue
    ```js
    <template>
    <button
        v-for="post in posts"
        :key="post.id"
        @click="click(post)">
        {{post.title}}
        </button>
        {{postId}}
    </template>
    <script>
    import {useStore} from 'vuex';
    import {computed, onMounted} from 'vue';
    export default {
    setup(){
        const store = useStore()

        const click = (post)=>{
            store.commit('setPostId', post.id);
        }

        const fetchData = () => {
            store.dispatch('fetchPosts');
        }

        onMounted(()=>{
            fetchData();
        });

        return{
            postId: computed(()=>store.state.postId),
            posts : computed(()=> store.state.posts),
            click
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Showing the current post / Vuex getters
    - store.js
    ```js
    import {createStore} from 'vuex';
    const delay = () => new Promise(res => setTimeout(res,1000));
    import {testPosts} from '../microblog/testPosts';
    export const store = createStore({
        state(){
            return{
                postId : null,
                posts : []
            }
        },
        mutations:{
            setPostId(state, postId){
                state.postId = postId;
            },
            setPosts(state, posts){
                state.posts = posts
            }
        },
        actions:{
            async fetchPosts(ctx){
                await delay();
                ctx.commit('setPosts', testPosts);
            }
        },
        getters:{
            currentPost(state){
                return state.posts.find(p => p.id === state.postId);
            }
        }
    })
    ```
    - App.vue
    ```js
    <template>
    <button
        v-for="post in posts"
        :key="post.id"
        @click="click(post)">
        {{post.title}}
        </button>
        {{postId}}
    <div v-if="currentPost">
        <h2>
            {{currentPost.title}}
        </h2>
        <h4>
             {{currentPost.content}}
        </h4>
    </div>
    </template>
    <script>
    import {useStore} from 'vuex';
    import {computed, onMounted} from 'vue';
    export default {
    setup(){
        const store = useStore()

        const click = (post)=>{
            store.commit('setPostId', post.id);
        }

        const fetchData = () => {
            store.dispatch('fetchPosts');
        }

        onMounted(()=>{
            fetchData();
        });

        return{
            postId: computed(()=>store.state.postId),
            posts : computed(()=> store.state.posts),
            currentPost: computed(()=> store.getters.currentPost),
            click
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Scaling Vuex with modules
    - Always keep your state in modules don't use a single global store.
    - store.js
    ```js
    import {createStore} from 'vuex';
    import {testPosts} from '../microblog/testPosts';

    const delay = () => new Promise(res => setTimeout(res,1000));
    const posts ={
        namespaced:true,
        state(){
            return{
                postId : null,
                all : []
            }
        },
        mutations:{
            setPostId(state, postId){
                state.postId = postId;
            },
            setPosts(state, posts){
                state.all = posts
            }
        },
        actions:{
            async fetch(ctx){
                await delay();
                ctx.commit('setPosts', testPosts);
            }
        },
        getters:{
            currentPost(state){
                return state.all.find(p => p.id === state.postId);
            }
        }
    }
    export const store = createStore({
        modules:{
            posts
        }
    })
    ```
    - App.vue
    ```js
    <template>
    <button
        v-for="post in posts"
        :key="post.id"
        @click="click(post)">
        {{post.title}}
        </button>
        {{postId}}
    <div v-if="currentPost">
        <h2>
            {{currentPost.title}}
        </h2>
        <h4>
             {{currentPost.content}}
        </h4>
    </div>
    </template>
    <script>
    import {useStore} from 'vuex';
    import {computed, onMounted} from 'vue';
    export default {
    setup(){
        const store = useStore()

        const click = (post)=>{
            store.commit('posts/setPostId', post.id);
        }

        const fetchData = () => {
            store.dispatch('posts/fetch');
        }

        onMounted(()=>{
            fetchData();
        });

        return{
            postId: computed(()=>store.state.posts.postId),
            posts : computed(()=> store.state.posts.all),
            currentPost: computed(()=> store.getters['posts/currentPost']),
            click
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```
- Refactor to the options API
    - App.vue
    ```js
    <template>
    <button
        v-for="post in posts"
        :key="post.id"
        @click="click(post)">
        {{post.title}}
        </button>
        {{postId}}
    <div v-if="currentPost">
        <h2>
            {{currentPost.title}}
        </h2>
        <h4>
             {{currentPost.content}}
        </h4>
    </div>
    </template>
    <script>
    export default {
    created(){
        this.$store.dispatch('posts/fetch');
    },
    computed:{
        posts(){
                return this.$store.state.posts.all;         
        },
        currentPost(){
            return this.$store.getters['posts/currentPost'];
        }
    },
    methods:{
        click(post){
            this.$store.commit('posts/setPostId', post.id);
        }
    }
    }
    </script>
    <style scoped>
    </style>
    ```

## Capstone final project

- Setup the store and the store modules:
    - albums.js
    ```js
    export const albums = {
        namespaced: true,
        state(){
            return{

            }
        },
        mutations:{

        },
        actions:{
            
        }
    }
    ```    
    - photos.js
    ```js
    export const photos = {
        namespaced: true,
        state(){
            return{

            }
        },
        mutations:{

        },
        actions:{
            
        }
    }
    ```
    - store.js
    ```js
    import {createStore} from 'vuex';
    import {albums} from './albums.js';
    import {photos} from './photos.js';

    export const store = createStore({
        modules:{
            albums,
            photos
        }
    })
    ```
- Create the app generic layout
    - Layout.vue
    ```js
    <template>
    <div class="wrapper">
        <div class="header">
            <slot name="header"/>
        </div>
        <div class="main">
            <div class="sidebar">
                <slot name="sidebar"/>
            </div>
            <div class="content">
                <slot name="content"/>
            </div>
            
        </div>
    </div>
    </template>
    <style scoped>
        .wrapper {
            height: 100vh;
        }
        .header {
            height: 50px;
            display: flex;
            align-items: center;
            padding: 0 0 0 20px;
            font-size: 20px;
            font-family: Arial;
        }
        .main {
            height: calc(100vh - 50px);
            border-top: 2px solid silver;
            display: flex;
        }
        .sidebar { 
            padding: 10px;
            flex-grow: 0;
            flex-shrink: 0;
            flex-basis: 300px;
            border-right: 2px solid silver;
        }
        .content {
            width: 100%;
            margin: 10px;
        }
    </style>
    ```
- Fetching albums:
    - App.vue
    ```js
    <template>
    <layout>
        <template v-slot:header>
            Header
        </template>
        <template v-slot:sidebar>
            <div 
                v-for="album in albums"
                :key="album.id">
                {{album.title}}
            </div>
        </template>
        <template v-slot:content>
            Content
        </template>
    </layout>
    </template>
    <script>
    import Layout from './Layout.vue';
    import {ref, onMounted} from 'vue';
    export default {
    components:{
        Layout
    },
    setup(){
        const albums = ref([]);
        onMounted(async() =>{
            const res = await window.fetch('https://jsonplaceholder.typicode.com/albums');
            const json = await res.json();
                albums.value = json;
        });

        return{
            albums
        }
    }
    }
    </script>
    <style scoped>
    *{
        box-sizing: border-box;
    }
    body{
        margin: 0;
    }
    </style>
    ```
- Data fetching albums with vuex
    - album.js
    ```js
    export const albums = {
    namespaced: true,
    state(){
        return{
            all: []
        }
    },
    mutations:{
        setAlbums(state, albums){
            state.all = albums
        }

    },
    actions:{
        async fetch(ctx){
            const res = await window.fetch('https://jsonplaceholder.typicode.com/albums');
            const json = await res.json();
            ctx.commit('setAlbums', json);
        }
        }
    }
    ```
    - App.vue
    ```js
    <template>
    <layout>
        <template v-slot:header>
            Header
        </template>
        <template v-slot:sidebar>
            <div 
                v-for="album in albums"
                :key="album.id">
                {{album.title}}
            </div>
        </template>
        <template v-slot:content>
            Content
        </template>
    </layout>
    </template>
    <script>
    import Layout from './Layout.vue';
    import {computed, onMounted} from 'vue';
    import {useStore} from 'vuex';
    export default {
    components:{
        Layout
    },
    setup(){
        const store = useStore();
        
        const albums = computed(()=>{
            return store.state.albums.all
        })
        onMounted(() =>{
                store.dispatch('albums/fetch');
        });

        return{
            albums
        }
    }
    }
    </script>
    <style scoped>
    *{
        box-sizing: border-box;
    }
    body{
        margin: 0;
    }
    </style>
    ```
- Create the Album component

    - Album.vue
    ```js
    <template>
    <button @click="click">{{album.title}}</button>
    </template>
    <script>
    import {useStore} from 'vuex';
    export default {
        setup(props) {
            const store = useStore();
            const click = () =>{
                store.dispatch('photos/getByAlbum', {album : props.album});
            }
            return{
                click
            }
        },
        props:{
            album:{
                type: Object,
                required: true
            }
        }
    }
    </script>
    <style scoped>
    button {
    background: darkcyan;
    color: white;
    border: none;
    padding: 10px;
    margin: 0 10px 5px 0;
    font-size: 18px;
    border-radius: 5px;
    transition: .1s;
    width: 100%;
    display: block;
    text-align: center;
    text-decoration: none;
    font-family: Arial;
    }
    button:hover {
    filter: brightness(120%);
    cursor: pointer;
    transition: .1s;
    }
    </style>
    ```
- Fetching thousands of photos:
    - photo.js
    ```js
    export const photos = {
        namespaced: true,
        state(){
            return{
                all : []
            }
        },
        mutations:{
            setPhotosForCurrentAlbum(state, photos){
                state.all = photos;
            }
        },
        actions:{
            async getByAlbum(ctx, {album}){
                const res = await window.fetch(`https://jsonplaceholder.typicode.com/photos?album=${album.id}`);
                const json = await res.json();
                ctx.commit('setPhotosForCurrentAlbum', json);
            }
        }
    }
    ```
    - App.vue
    ```js
    <template>
    <layout>
        <template v-slot:header>
            Header
        </template>
        <template v-slot:sidebar>
            <album 
                v-for="album in albums"
                :key="album.id"
                :album="album"/>
        </template>
        <template v-slot:content>
            <img
                v-for="photo in photos"
                :key="photo.id"
                :src="photo.thumbnailUrl"/>
        </template>
    </layout>
    </template>
    <script>
    import Layout from './Layout.vue';
    import Album from './Album.vue';
    import {computed, onMounted} from 'vue';
    import {useStore} from 'vuex';
    export default {
    components:{
        Layout,
        Album
    },
    setup(){
        const store = useStore();

        onMounted(() =>{
                store.dispatch('albums/fetch');
        });

        const albums = computed(()=>{
            return store.state.albums.all
        })

            const photos = computed(() =>{
                return store.state.photos.all
            });

        return{
            albums,
            photos
        }
    }
    }
    </script>
    <style scoped>
    *{
        box-sizing: border-box;
    }
    body{
        margin: 0;
    }
    </style>
    ```
- Adding routing:
    - PhotoApp.vue
    ```js
    <template>
    <layout>
        <template v-slot:header>
            Header
        </template>
        <template v-slot:sidebar>
            <album 
                v-for="album in albums"
                :key="album.id"
                :album="album"/>
        </template>
        <template v-slot:content>
            <img
                v-for="photo in photos"
                :key="photo.id"
                :src="photo.thumbnailUrl"/>
        </template>
    </layout>
    </template>
    <script>
    import Layout from './Layout.vue';
    import Album from './Album.vue';
    import {computed, onMounted} from 'vue';
    import {useStore} from 'vuex';
    export default {
    components:{
        Layout,
        Album
    },
    setup(){
        const store = useStore();

        onMounted(() =>{
                store.dispatch('albums/fetch');
        });

        const albums = computed(()=>{
            return store.state.albums.all
        })

            const photos = computed(() =>{
                return store.state.photos.all
            });

        return{
            albums,
            photos
        }
    }
    }
    </script>
    <style scoped>
    *{
        box-sizing: border-box;
    }
    body{
        margin: 0;
    }
    </style>
    ```
    - App.vue
    ```js
    <template>
        <router-view/>
    </template>
    ```
    - routes.js
    ```js
    import {createRouter, createWebHistory} from 'vue-router';
    import PhotoApp from './PhotoApp.vue';

    export const router = createRouter({
        history: createWebHistory(),
        routes: [
            {
                path: '/',
                component: PhotoApp
            }
        ]
    });
    ```
    - index.js
    ```js
    import {createApp} from 'vue';
    import App from './App.vue';
    import {store} from './store.js';
    import {router} from './router.js';

    const app = createApp(App);

    app.use(store)
    app.use(router)
    app.mount('#app')
    ```
- Improved routing.
    - <router-view/> will be dynamically replaced with whatever component is matched up to.
    - WatchEffect to monitor the changes in the url
    - router.js
    ```js
    import {createRouter, createWebHistory} from 'vue-router';
    import PhotoApp from './PhotoApp.vue';
    import PhotoView from './PhotoView.vue';

    export const router = createRouter({
        history: createWebHistory(),
        routes: [
            {
                path: '/',
                component: PhotoApp,
                children: [
                    {
                    path: 'albums/:id',
                    component: PhotoView
                    }
                ]
            }
        ]
    });
    ```
    - photo.js
    ```js
    export const photos = {
        namespaced: true,
        state(){
            return{
                all : []
            }
        },
        mutations:{
            setPhotosForCurrentAlbum(state, photos){
                state.all = photos;
            }
        },
        actions:{
            async getByAlbum(ctx, {albumId}){
                const res = await window.fetch(`https://jsonplaceholder.typicode.com/photos?albumId=${albumId}`);
                const json = await res.json();
                ctx.commit('setPhotosForCurrentAlbum', json);
            }
        }
    }
    ```
    - Album.vue
    ```js
    <template>
    <router-link :to="`/albums/${album.id}`">
        {{album.title}}
    </router-link>
    </template>
    <script>
    import {useStore} from 'vuex';
    export default {
        props:{
            album:{
                type: Object,
                required: true
            }
        }
    }
    </script>
    <style scoped>
    a {
    background: darkcyan;
    color: white;
    border: none;
    padding: 10px;
    margin: 0 10px 5px 0;
    font-size: 18px;
    border-radius: 5px;
    transition: .1s;
    width: 100%;
    display: block;
    text-align: center;
    text-decoration: none;
    font-family: Arial;
    }
    a:hover {
    filter: brightness(120%);
    cursor: pointer;
    transition: .1s;
    }
    </style>    
    ```
    - PhotoApp.vue
    ```js
    <template>
    <layout>
        <template v-slot:header>
            Header
        </template>
        <template v-slot:sidebar>
            <album 
                v-for="album in albums"
                :key="album.id"
                :album="album"/>
        </template>
        <template v-slot:content>
            <router-view/>
        </template>
    </layout>
    </template>
    <script>
    import Layout from './Layout.vue';
    import Album from './Album.vue';
    import {computed, onMounted} from 'vue';
    import {useStore} from 'vuex';
    export default {
    components:{
        Layout,
        Album
    },
    setup(){
        const store = useStore();

        onMounted(() =>{
                store.dispatch('albums/fetch');
        });

        const albums = computed(()=>{
            return store.state.albums.all
        })

        return{
            albums
        }
    }
    }
    </script>
    <style scoped>
    *{
        box-sizing: border-box;
    }
    body{
        margin: 0;
    }
    </style>
    ```
    - PhotoView.vue
    ```js
    <template>       
    <img
        v-for="photo in photos"
        :key="photo.id"
        :src="photo.thumbnailUrl"/>
    </template>
    <script>
    import {computed, watchEffect} from 'vue';
    import {useStore} from 'vuex';
    import {useRoute} from 'vue-router';

    export default{
    setup(){
            const store = useStore();
            const route = useRoute();

            watchEffect(()=>{
                const id = route.params.id;
                if(!id){
                    return;
                }
                store.dispatch('photos/getByAlbum', {albumId :id});
            })
    
            const photos = computed(() =>{
                return store.state.photos.all
            });

            return{
                photos
            }
    }
        
    }
    </script>
    <style scoped>
    </style>
    ```
- Vuex level caching
    - photos.js
    ```js
    export const photos = {
        namespaced: true,
        state(){
            return{
                all : [],
                cache: {}
            }
        },
        mutations:{
            setPhotosForCurrentAlbum(state, {photos, albumId}){
                state.all = photos;
                state.cache[albumId] = photos;
            }
        },
        actions:{
            async getByAlbum(ctx, {albumId}){
                if(ctx.state.cache[albumId]){
                    ctx.commit('setPhotosForCurrentAlbum', {photos: ctx.state.cache[albumId], albumId });
                    return;
                }
                const res = await window.fetch(`https://jsonplaceholder.typicode.com/photos?albumId=${albumId}`);
                const json = await res.json();
                ctx.commit('setPhotosForCurrentAlbum', {photos: json, albumId });
            }
        }
    }
    ```
