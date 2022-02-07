- Create your first Vue Hello world.
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
