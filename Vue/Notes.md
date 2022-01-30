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