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
12. Use a directive with range:
    ```js
    <div>
        <span v-for="n in 20">{{n}}</span>
    </div>
    ```
13. Access native event through event handler (to prevent default behaviour in this case).
    ```js
    <button v-on:click="show('Welcome to VueJS world', $event)">
    Submit
    </button>

    methods: {
    show: function (message, event) {
        // now we have access to the native event
        if (event) event.preventDefault()
        console.log(message);
    }
    }
    ```
14. Event modifiers provided by vue: stop, prevent, capture, self, once, passive:
    ```js
    //<!-- the click event's propagation will be stopped -->
    <a v-on:click.stop="methodCall"></a>
    ```
15. Key modifiers for handling keyboard events:
    ```js
    //<!-- only call `vm.show()` when the `keyCode` is 13 -->
    <input v-on:keyup.13="show">
    ```
16. Use v-model directive to implement two-way binding on form input, text area and select elements. 
    ```js
    //<input v-model="message" placeholder="Enter input here">
    <p>The message is: {{ message }}</p>
    ```
17. Supported modifiers on model:
    1. lazy: by default v-model syncs the data after each input event. You can add the lazy modifier to instad sync after change events:
    v-model.lazy
    2. number: user input to automatically typecast as a number: v-model.number
    3. trim: if you want user input to be trimmed automatically.

18. In Vue.JS every component must have a single root element (usually the <div> tag).
In Vue 3.x components can have multiple root nodes.
19. Implement model on custom components:
    - Bind the value attribute to a value prop
    - On input, emit its own custom input event with the new value:
    - Component input: 
    ```js
    Vue.component('custom-input', {
        props: ['value'],
        template: `
            <input
            v-bind:value="value"
            v-on:input="$emit('input', $event.target.value)"
            />
        `
    })
    ```
    - Now you can use v-model with this component:
    ```js
    <custom-input v-model="searchInput"></custom-input>
    ```
20. slots: content distribution outlets for dynamic content insertion:
    ```js
    Vue.component('alert', {
        template: `
            <div class="alert-box">
            <strong>Error!</strong>
            <slot></slot>
            </div>
        `
    })
    ```
    - Now you can insert dynamic content as below:
    ```js
    <alert>
        There is an issue with in application.
    </alert>
    ```
21. Prop types: String, Number, Boolean, Array, Object.
22. The child component should not mutate the prop. You can create a local variable and assign it the parent prop as default value and mutate that variable or you can use a computed property:
    ```js
    props: ['environment'],
    computed: {
    localEnvironment: function () {
        return this.environment.trim().toUpperCase()
    }
    }
    ```
23. Props validations: Vue provides validations such as types, required fields, default values along with customized validations. 
    ```js
    Vue.component('user-profile', {
    props: {
        // Basic type check (`null` matches any type)
        age: Number,
        // Multiple possible types
        identityNumber: [String, Number],
        // Required string
        email: {
        type: String,
        required: true
        },
        // Number with a default value
        minBalance: {
        type: Number,
        default: 10000
        },
        // Object with a default value
        message: {
        type: Object,
        // Object or array defaults must be returned from
        // a factory function
        default: function () {
            return { message: 'Welcome to Vue' }
        }
        },
        // Custom validator function
        location: {
        validator: function (value) {
            // The value must match one of these strings
            return ['India', 'Singapore', 'Australia'].indexOf(value) !== -1
        }
        }
    }
    })
    ```
23. What is Vue router?
    - Official routing library for single-page applications designed for use with the framework.
    - Features: nested route/view mappping, modular, component based router config, route params, etc.
    **Steps to use it:**
        1. Configure router link and router view in the template:
        ```js
        <script src="https://unpkg.com/vue/dist/vue.js"></script>
        <script src="https://unpkg.com/vue-router/dist/vue-router.js"></script>

        <div id="app">
        <h1>Welcome to Vue routing app!</h1>
        <p>
            <!-- use router-link component for navigation using `to` prop. It rendered as an `<a>` tag -->
            <router-link to="/home">Home</router-link>
            <router-link to="/services">Services</router-link>
        </p>
        <!-- route outlet in which component matched by the route will render here -->
        <router-view></router-view>
        </div>
        ```
        2. Import Vuew and VueRouter packages and then apply router:
        ```js
        import Vue from 'vue';
        import VueRouter from 'vue-router';

        Vue.use(VueRouter)
        ```
        3. Define or import route components:
        ```js
        const Home = { template: '<div>Home</div>' }
        const Services = { template: '<div>Services</div>' }
        ```
        4. Define your route where each one maps to a component:
        ```js
        const routes = [
        { path: '/home', component: Home },
        { path: '/services', component: Services }
        ]
        ```
        5. Create the router instance and pass the routes option
        ```js
        const router = new VueRouter({
        routes // short for `routes: routes`
        })
        ```
        6. Create and mount the root instance:
        ```js
        const app = new Vue({
        router
        }).$mount('#app')
        ```
24. Dynamic route matching based on a pattern (use a colon):
    ```js
    const User = {
    template: '<div>User {{ $route.params.name }}, PostId: {{ route.params.postid }}</div>'
    }

    const router = new VueRouter({
    routes: [
        // dynamic segments start with a colon
        { path: '/user/:name/post/:postid', component: User }
    ]
    })
    ```
25. Single file components: encapsulate the structure, styling and behaviour into one file:
    ```js
    <template>
    <div>
        <h1>Welcome {{ name }}!</h1>
    </div>
    </template>

    <script>
    module.exports = {
    data: function() {
        return {
        name: 'John'
        }
    }
    }
    </script>

    <style scoped>
    h1 {
    color: #34c779;
    padding: 3px;
    }
    </style>
    ```
    - It solves common problems like: global definitions force unique names for every component, string templates lack syntax highlighting and require ugly slashes.
26. What are filters? Used to apply common text formatting, they should be appended to the end of the Javascript expression, denoted by the pipe symbol (|).
    - Define a local filter named capitalize:
    ```js
    filters: {
    capitalize: function (value) {
        if (!value) return ''
            value = value.toString()
            return value.charAt(0).toUpperCase() + value.slice(1)
        }
    }
    ```
    - Use the filter:
    ```js
    <!-- in mustaches -->
    {{ username | capitalize }}

    <!-- in v-bind -->
    <div v-bind:id="username | capitalize"></div>
    ```
    - You can create local filters in the component options as the previous example and it is applicable to that specific component.
    - You can also define a filter globally before creating the Vue instance. It is applicable to all the components:
    ```js
    Vue.filter('capitalize', function (value) {
    if (!value) return ''
        value = value.toString()
        return value.charAt(0).toUpperCase() + value.slice(1)
    })

    new Vue({
    // ...
    })
    ```
    -  You can also chain filters one after another to perform multiple manipulations on the expression. The first filter takes the expression as a single argument and the result of the expression becomes an argument for the second filter and so on and so forth:
    ```js
    {{ birthday | dateFormat | uppercase }}
    ```
    - You can even pass arguments to a filter similar to a javascript function:
    ```js
    {{ 2 | exponentialStrength(10) }} <!-- prints 2 power 10 = 1024 -->
    ```
- Examples:
    - App.vue:
    ```js
    <template>
    <div id="app">
        <img alt="Vue logo" src="./assets/logo.png" width="25%" />
        <HelloWorld msg="hello2 ">
        {{ testData | toUpper | addStars }}
        </HelloWorld>
    </div>
    </template>
    <script>
    import HelloWorld from "./components/HelloWorld";

    export default {
    name: "App",
    data: function () {
        return {
        testData: "yello from df",
        };
    },
    filters: {
        capitalize: function (value) {
        if (!value) return "";
        const newVal = value;
        return newVal.charAt(0).toUpperCase() + newVal.slice(1);
        },
        addStars: function (value) {
        if (!value) return "";
        return value.padEnd(25, "*");
        },
    },
    components: {
        HelloWorld,
    },
    };
    </script>

    <style>
    #app {
    font-family: "Avenir", Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
    margin-top: 60px;
    }
    </style>
    ```
    - main.js
    ```js
    import Vue from "vue";
    import App from "./App.vue";

    Vue.config.productionTip = false;
    Vue.filter("toUpper", function (value) {
        if (!value) return "";
        return value.toUpperCase();
    });

    new Vue({
    render: (h) => h(App)
    }).$mount("#app");
    ```
    - HelloWorld.vue
    ```js
    <template>
    <div class="alert-box">
        <strong>Error!</strong>
        <br />
        <slot></slot>
        <br />
        {{ msg }}
    </div>
    </template>

    <script>
    export default {
    name: "HelloWorld",
    props: {
        msg: {
        required: true,
        type: String,
        },
    },
    };
    </script>

    <!-- Add "scoped" attribute to limit CSS to this component only -->
    <style scoped>
    h3 {
    margin: 40px 0 0;
    }
    ul {
    list-style-type: none;
    padding: 0;
    }
    li {
    display: inline-block;
    margin: 0 10px;
    }
    a {
    color: #42b983;
    }
    </style>

    ```

27. Plugins provide global-level functionality to Vue application. For example: add some global methods or properties, add one or more global assets (directives, filters, transitions), add some componen options by global mixin (for example vue router), 
    - You can use a plugin by passing your plugin to Vue's global method
    ```js
    Vue.use(myPlugin);
    ```
28. What are mixings? A way to distribute reusable functionalities in Vue components. These reusable functions are merged with existing functions. 
- They can contain any component options. Multiple mixins can be specified in the mixin array of the component.

    ```js
    const myMixin = {
    created(){
        console.log("Welcome to Mixins!")
    }
    }
    var app = new Vue({
    el: '#root',
    mixins: [myMixin]
    })
    ```
- **Global mixins:** apply to all vue components available in our application (use them sparsely and carefully). Example:
    ```js
    Vue.mixin({
        created(){
            console.log('Write global mixins');
        }
    })

    new Vue({
        el: '#app'
    })
    ```
29. Mixin mergin strategies:
- When mixin and the component itself contain overlapping options:
    - Data objects undergo a recursive merge, with the component's data taking priority over mixins in case of conflicts.
    - Hook functions merged into an array so that all of them will be called. Mixin hooks will be called before the component's own hooks.
30. Custom directives: tiny commands that you can attach to DOM elements, prefixed with v- to let the library know you're using a special bit of markup and to keep syntax consistent. Custom focus directive to provide focus on specific form element during page load-time:
    ```js
    // Register a global custom directive called `v-focus`
    Vue.directive('focus', {
    // When the bound element is inserted into the DOM...
        inserted: function (el) {
            // Focus the element
            el.focus()
        }
    })
    ```
    - Use it:
    ```js
    <input v-focus>
    ```
    - You can register directives locally using directives option in component as below:
    ```js
    directives: {
        focus: {
            // directive definition
            inserted: function (el) {
                el.focus()
            }
        }
    }
    ```
    - Hook functions provided by directives: bind, inserted, update, componentUpdated and unbind.
    - Pass multiple values to a directive:
        ```js
        <div v-avatar="{ width: 500, height: 400, url: 'path/logo', text: 'Iron Man' }"></div>
        ```
        - Configure avatar directive globally:
        ```js
        Vue.directive('avatar', function (el, binding) {
            console.log(binding.value.width) // 500
            console.log(binding.value.height)  // 400
            console.log(binding.value.url) // path/logo
            console.log(binding.value.text)  // "Iron Man"
        })
        ```
31. What are render function and what are they used for? Normal functions which receive a createElement method as it's first argument use to create virtual nodes. Vue.js templates are syntactic sugar for render functions since templates compile down to render functions. They are used to create components dynamically.
    - Template version: 
    ```js
    <template>
    <div :class="{'is-rounded': isRounded}">
        <p>Welcome to Vue render functions</p>
    </div>
    </template>
    ```
    - **Render function version:**
    ```js
    render: function (createElement) {
    return createElement('div', {
        'class': {
            'is-rounded': this.isRounded
        }
    }, [
        createElement('p', 'Welcome to Vue render functions')
    ]);
    }
    ```
32. What are functional components? Simple functions to create simple components just by passing a context.
    - They are stateless (don't keep any state by itself).
    - Instanceless (has no instance, thus no this).
    ```js
    Vue.component('my-component', {
    functional: true,
    // Props are optional
    props: {
        // ...
    },
    // To compensate for the lack of an instance,
    // we are now provided a 2nd context argument.
    render: function (createElement, context) {
        // ...
    }
    })
    ```
33. Vue.js vs React:

    - **Similarities:**
        - Virtual DOM model.
        - Component-based structure and reactivity.
        - Root library and additional tasks are transferred to other libaries (routing, state management, etc).
    - **Differences:**
        | VueJS      | React |
        | ----------- | ----------- |
        | JS MVVM framework      | Javascript library       |
        | Web development   | Web and native (mobile)        |
        |Easier to learn| Steep learning curve|
        |Simpler| More Complex|
        |Bootstrap application is vue-cli|Bootstrap application is Create React App|
        * Bootstrap meaning: a technique of loading a program into a computer by means of a few initial instructions which enable the introduction of the rest of the program from an input device.
    - **Advantages:**
        - Smaller and faster.
        - Convenient templates to ease the process of developing.
        - Simpler javascript syntax without learning JSX.
    - **Disadvantages:**
        - Stronger community.
        - More flexibility in large apps developing.
        - Mobile apps creation.
34. **Dynamic components:** dynamically switch between multiple components without updating the route of the application. It works by using <component> tag with v-bind:is attribute which accept dynamic component:
    ```js
    new Vue({
    el: '#app',
    data: {
        currentPage: 'home'
    },
    components: {
        home: {
            template: "<p>Home</p>"
        },
        about: {
            template: "<p>About</p>"
        },
        contact: {
            template: "<p>Contact</p>"
        }
    }
    })
    //Use it:
    <div id="app">
    <component v-bind:is="currentPage">
        <!-- component changes when currentPage changes! -->
        <!-- output: Home -->
    </component>
    </div>
    ```
    - The component will be unmounted when it is switched away but it is possible to force the inactive component alive using <keep-alive> component
35. Async components:
    - Divide the app into smaller chunks and only load a component from the server when it's needed. Define your component as a factory function that asynchronously resolves your component definition. 
    ```js
    Vue.component('async-webpack-example', function (resolve, reject) {
        // Webpack automatically split your built code into bundles which are loaded over Ajax requests.
        require(['./my-async-component'], resolve)
    })
    ```
    - Async component factory example:
    ```js
    const AsyncComponent = () => ({
        // The component to load (should be a Promise)
        component: import('./MyComponent.vue'),
        // A component to use while the async component is loading
        loading: LoadingComponent,
        // A component to use if the load fails
        error: ErrorComponent,
        // Delay before showing the loading component. Default: 200ms.
        delay: 200,
        // The error component will be displayed if a timeout is
        // provided and exceeded. Default: Infinity.
        timeout: 3000
    })
    ```
36. **Recursive component:** components that recursively can invoke themselves (careful with infinite loops)
    ```js
    Vue.component('recursive-component', {
        template: `<!--Invoking myself!-->
                <recursive-component></recursive-component>`
    });
    ```
37. To avoid circular dependencies between components use beforeCreate hook to register the child component:
    ```js
    beforeCreate: function () {
        this.$options.components.componentB = require('./component-b.vue').default
    }
    ```
38. What is the purpose of vuejs compiler? Responsible for compiling template strings into JS render functions. 
    - Template string:
    ```js
    // this requires the compiler
    new Vue({
        template: '<div>{{ message }}</div>'
    })
    ```
39. What is the purpose of the vuejs once directive? If you want to render a lot of static content and you need to make sure it is only evaluated once and then cached after. Not to overuse unless there is slow rendering due to a lot of static content.
40. How to access the root instance. Use: $root property. It is recommended to use Vuex to manage state instead of using root instance as a global store. 
41. What is the purpose of renderError? If the render function encounters an error then you can use renderError as an alternative render output. The error will be passed as a second argument:
    ```js
    new Vue({
    render (h) {
        throw new Error('An error')
    },
    renderError (h, err) {
        return h('div', { style: { color: 'red' }}, err.stack)
    }
    }).$mount('#app')
    ```
42. How to access parent instance? Use the $parent object to access the inmmediate outer scope. 
43. **Vuex:** state management pattern + library for Vue.js applications. Serves as a centralized store for all the components in an applications, which rules to ensure the state can only be mutated in a predictable fasion. 
44. Major components of State Management Pattern:
    1. **state:** the source of truth that drives our app.
    2. **view:** declarative mapping of data.
    3. **actions:** possible ways the state could change.
    ```js
    new Vue({
    // state
    data () {
        return {
        count: 0
        }
    },
    // view
    template: `
        <div>{{ count }}</div>
    `,
    // actions
    methods: {
        increment () {
        this.count++
        }
    }
    })
    ```
45. scoped CSS: prevents styles form leaking out of the current component and affecting other uintended components on your page.
    - You can mix global and local styles:
    ```js
    <style>
    /* global styles */
    </style>

    <style scoped>
    /* local styles */
    </style>
    ```
46. What is hot reloading? When you edit a *.vue file all instances of that component will be swapped in without reloading the page (improves development experience).
    - To enable hot reloading is enabled by default with the following command:
    ```batch
    webpack-dev-server --hot
    ```
    - State preservation rules in hot reloading:
        1. When editing the \<template> of a component, instances of the edited component will re-render in place, preserving all current private state.
        2. When editing the \<script> part of a component, instances of the edited component will be destroyed and re-created in place.
        3. When editing the \<style> hot reload operates on its own via vue-style-loader without affecting application state.
47. Unit testing? 
    - Vue-cli offers pre-configured unit testing and e2e testing setups.
    - Manual setup using mocha-webpack or jest
48. Eslint plugin? Support for both the template and the script parts of a Vue single file. Configure:
    ```js
    // .eslintrc.js
    module.exports = {
        extends: [
            "plugin:vue/essential"
        ]
    }
    ```
    - Rules of stylint:
        1. 160 built-in rules to catch errors, enforce stylistic conventions.
        2. Extracts embedded styles from html, markdown and css in js object and template literals.
        3. 
49. **Principles of vuex application:**
    1. Application-level state is centralized in the store.
    2. The only way to mutate the state is by commiting mutations, which are synchronous transactions.
    3. Asynchronoues logic should be encapsulated in, and can be composed with actions.
50. How do you test mutations? Keep mutations inside your store.js file and export the mutations as a named export apart from default export. 
    ```js
    // mutations.js
    export const mutations = {
        increment: state => state.counter++
    }

    // mutations.spec.js
    import { expect } from 'chai'
    import { mutations } from './store'

    // destructure assign `mutations`
    const { increment } = mutations

    describe('mutations', () => {
    it('INCREMENT', () => {
        // mock state
        const state = { counter: 10 }
        // apply mutation
        increment(state)
        // assert result
        expect(state.counter).to.equal(11)
    })
    })
    ```
51. Test getters? Recommended to test them if they have complicated computation.
    ```js
    // getters.js
    export const getters = {
        filterTodos (state, status) {
            return state.todos.filter(todo => {
            return todo.status === status
            })
        }
    }

    // getters.spec.js
    import { expect } from 'chai'
    import { getters } from './getters'

    describe('getters', () => {
    it('filteredTodos', () => {
        // mock state
        const state = {
        todos: [
            { id: 1, title: 'design', status: 'Completed' },
            { id: 2, title: 'testing', status: 'InProgress' },
            { id: 3, title: 'development', status: 'Completed' }
        ]
        }
        // mock getter
        const filterStatus = 'Completed'

        // get the result from the getter
        const result = getters.filterTodos(state, filterStatus)

        // assert the result
        expect(result).to.deep.equal([
            { id: 1, title: 'design', status: 'Completed' },
            { id: 2, title: 'development', status: 'Completed' }
        ])
    })
    })
    ```
52. What is the purpose of strict mode in vuex. Whenever vuex state is mutated outside of mutation handlers, an error will be thrown. Enabling it by passing strict: true while creating the store:
    ```js
    const store = new Vuex.Store({
    // ...
        strict: true
    })
    ```
    - It is not recommended to use it in production environment since it is quite expensive when you perform large amount of mutations. 
53. What is Vuex store? A container that holds your application's state.
    - Steps to create it:
        1. Configure vuex in vuejs ecosystem:
        ```js
        import Vuex from "vuex";
        Vue.use(Vuex)
        ```
        2. Provide an initial state object and some mutations:
        ```js
        // Make sure to call Vue.use(Vuex) first if using a module system

        const store = new Vuex.Store({
            state: {
                count: 0
            },
            mutations: {
                increment (state) {
                state.count++
                }
            }
        })
        ```
        3. Trigger state change with commit and access state variables:
        ```js
        store.commit('increment')
        console.log(store.state.count) // -> 1
        ```
        - **Differences of vuex store and plain global object:**
            1. Vuex stores are reactive (components will reactively get updated).
            2. Cannot directly mutate the store's state. 
        - We want to track application state in order to implement tools that can log every mutation, take state snapshots of even perform time travel debugging.
54. How do you install vuex?
    ```batch
    npm install vuex --save
    (or)
    yarn add vuex
    ```
    - You can also install it using CDN links  (\<script> tags).
55. Do I need promise for vuex? Yes, otherwise like IE6 you can use a polyfill.
56. How do you display store state in vue components? You can retrieve state from store by simply returning store's state from withina computed property. 
    ```js
    //Inject store into our app component:
    const app = new Vue({
        el: '#app',
        // provide the store using the "store" option.
        // this will inject the store instance to all child components.
        store,
        components: { Greeting },
        template: `
            <div class="app">
            <greeting></greeting>
            </div>
        `
    })
    // let's create a hello world component and use the store
    const Greeting = {
        template: `<div>{{ greet }}</div>`,
        computed: {
            greet () {
                return this.$store.state.msg
            }
        }
    }
    ```
57. What is mapState helper? Creating a computed property everytime whenever we want to access the store's state is going to be repetitive, so we can use mapState helper to do that, it generates computed getter functions for us.
    ```js
    state: {
        honorific: 'Mr.',
        firstName: 'Johnny',
        lastName: 'Bravo'
    }

    <script>
    import {mapState} from 'vuex';
    export default {
        name: 'show-name',
        computed: {
            fullName() {
                return this.firstName + ' ' + this.lastName;
            },
            ...mapState(['honorific', 'firstName', 'lastName'])
        }
    }
    </script>
    //and this translates to:
    <script>
    import {mapState} from 'vuex';
    export default {
        name: 'show-name',
        computed: {
            fullName() {
                return this.firstName + ' ' + this.lastName;
            },
            honorific() {
                return this.$store.state.honorific;
            },
            firstName() {
                return this.$store.state.firstName;
            },
            lastName() {
                return this.$store.state.lastName;
            }
        }
    }
    </script>
    ```
58. Do you need to replace entire local state with vuex? No, if a piece state strictly belongs to a single component just leave it as local state. 
59. What are vuex getters? Computed properties for stores to compute derived state base on store state. 
    ```js
    const store = new Vuex.Store({
    state: {
        todos: [
        { id: 1, text: 'Vue course', completed: true },
        { id: 2, text: 'Vuex course', completed: false },
        { id: 2, text: 'Vue Router course', completed: true }
        ]
    },
    getters: {
        completedTodos: state => {
        return state.todos.filter(todo => todo.completed)
        }
    }
    })
    ```
60. What is mapGetter helper? It is a helper that simply maps store getters to local computed properties.
    ```js
    import { mapGetters } from 'vuex'

    export default {
        computed: {
            // mix the getters into computed with object spread operator
            ...mapGetters(['completedTodos','todosCount'])
        }
    }
    ```
61. What are mutations? Similar to any events with a string type and a handler. The handler function is where we perform actual state modifications, and it will receive the state as the first argument.
    ```js
    const store = new Vuex.Store({
    state: {
        count: 0
    },
    mutations: {
        increment (state) {
            // mutate state
            state.count++
        }
    }
    })
    ```
    - You cannot directly invoke a mutation you need to call store.commit:
    ```js
    store.commit('increment');
    ```
    - You can also pass payload for the mutation as an additional argument to the commit:
    ```js
    mutations: {
        increment (state, payload) {
            state.count += payload.increment
        }
    }
    ```
    - Trigger commit:
    ```js
    store.commit('increment', {
        increment: 20
    })
    ```
    - You can also commit a mutation by directly using an object that has a **type** property (without any changes to the handler):
    ```js
    store.commit({
        type: 'increment',
        value: 20
    })
    ```
    - Mutations should be synchronous.
    - For reactivity reasons you should add new properties to state Object using set method or spread syntax.
- How do you perform mutations in components? You can use this.$store.commit('mutation-name', payload) or use mapMutations helper:
    ```js
    import { mapMutations } from 'vuex'

    export default {
    methods: {
        ...mapMutations([
        'increment', // map `this.increment()` to `this.$store.commit('increment')`

        // `mapMutations` also supports payloads:
        'incrementBy' // map `this.incrementBy(amount)` to `this.$store.commit('incrementBy', amount)`
        ]),
        ...mapMutations({
        add: 'increment' // map `this.add()` to `this.$store.commit('increment')`
        })
    }
    }
    ```
62. Is it mandatory to use constants for mutation types? No but this convention is just a preference and useful to take advantage of tooling like linters and putting all constants in a single file. Example:  
    ```js
    // mutation-types.js
    export const SOME_MUTATION = 'SOME_MUTATION';

    // store.js
    import Vuex from 'vuex'
    import { SOME_MUTATION } from './mutation-types'

    const store = new Vuex.Store({
        state: { ... },
        mutations: {
            // ES2015 computed property name feature to use a constant as the function name
            [SOME_MUTATION] (state) {
                // mutate state
            }
        }
    })
    ```
63. How do you perform asynchronous operations in Vuex? Use actions instead of mutations. 
    - Mutations perform mutations on the state, and actions commit mutations to ultimately update the state. 
    - Actions receive context object as an argument which has same properties and methods of store instance. 
    ```js
    const store = new Vuex.Store({
        state: {
            count: 0
        },
        mutations: {
            increment (state) {
            state.count++
            }
        },
        actions: {
            increment (context) {
            context.commit('increment')
            }
        }
    })
    ```
    - Actions are triggered with the store.dispatch method:
    ```js
    store.dispatch('increment')
    ```
    - You can dispatch an action using payload or object style:
    ```js
    // dispatch with a payload
    store.dispatch('incrementAsync', {
        amount: 10
    })

    // dispatch with an object
    store.dispatch({
        type: 'incrementAsync',
        amount: 10
    })
    ```
    - You can dispatch actions in components using this.$store.dispatch('action-name') or use mapActions helper:
    ```js
    import { mapActions } from 'vuex'
    export default {
    // ...
        methods: {
            ...mapActions([
                'increment', // map `this.increment()` to `this.$store.dispatch('increment')`
                // `mapActions` also supports payloads:
                'incrementBy' // map `this.incrementBy(amount)` to `this.$store.dispatch('incrementBy', amount)`
            ]),
            ...mapActions({
                add: 'increment' // map `this.add()` to `this.$store.dispatch('increment')`
            })
        }
    }
    ```
    - Compose actions: write multiple actions together to handler more complex async flows either by chaining promises or using async/await. 
        ```js
        actions: {
            async actionOne ({ commit }) {
                commit('first mutation', await getDataAsPromise())
            },
            async actionTwo ({ dispatch, commit }) {
                await dispatch('actionOne') // wait for `actionA` to finish
                commit('second mutation', await getSomeDataAsPromise())
            }
        }
        ```
64. What are modules in Vuex? When store get really bloated Vuex allows you to divide our store into modules. Each module can contain its own state, mutations, actions, getters and even nested modules. 
    ```js
    const moduleOne = {
        state: { ... },
        mutations: { ... },
        actions: { ... },
        getters: { ... }
    }

    const moduleTwo = {
        state: { ... },
        mutations: { ... },
        actions: { ... },
        getters: { ... }
    }

    const store = new Vuex.Store({
        modules: {
            one: moduleOne,
            two: moduleTwo
        }
    })
    //Access different modules
    store.state.one // -> `moduleOne's state
    store.state.two // -> `moduleTwo's state
    ```
65. Namespacing in Vuex? By default actions, mutations and getters inside modules are still registered under the global namespace. 
66. Principles enforced by vuex?
    - Application-level state needs to be centralized in the store.
    - The state should be mutated by commiting mutations only (sync transactions).
    - Actions should be used for async transactions. 
67. How to use model directive with two way computed property?
    ```js
     <input v-model="username">
    computed: {
    username: {
        get () {
            return this.$store.state.user.username
        },
        set (value) {
            this.$store.commit('updateProfile', value)
        }
    }
    }
    mutations: {
        updateProfile (state, username) {
            state.user.username = username
        }
    }
    ```
68. How do you install plugins in an existing Vue CLI project?
    ```batch
    vue add @vue/eslint
    ```
69. How do you create reactive objects in Vue 3?
    ```js
    const reactiveState = reactive({
        count: 0
    })
    ```
    - In Vue 2 you can create it them with Vue.observable() global API
    ```js
    const reactiveState = Vue.observable({
        count: 0
    })
    ```
70. New slot directive (v-slot) to replace old slot syntax:
    ```js
    <!-- old -->
    <user>
    <template slot="header" slot-scope="{ msg }">
        text slot: {{ msg }}
    </template>
    </user>

    <!-- new -->
    <user>
    <template v-slot:header="{ msg }">
        text slot: {{ msg }}
    </template>
    </user>
    ```
71. What does nextTick do in VueJS?
    - A way to execute a function after the data has been set and the DOM has been updated.
    ```js
    // modify data
    vm.msg = 'Welcome to Vue'
    // DOM not updated yet
    Vue.nextTick(function () {
    // DOM updated
    })

    // usage as a promise (2.1.0+)
    Vue.nextTick()
        .then(function () {
            // DOM updated
        })
    ```
72. Difference between method and computed property? CP are cached and invoke/change only when their dependencies change. A method will evaluate every time it's called.
73. What is vuetify? framework that provides clean, semantic and reusable components that make building application easier. 
74. How do you watch for nested data changes? You can use deep watcher by setting deep: true in the options object.
    ```js
    vm.$watch('someObject', callback, {
        deep: true
    })
    vm.someObject.nestedValue = 123
    // callback is fired
    ```
75. How do you watch route object changes? Setup a watcher on the $route in your component. 
    ```js
    watch:{
        $route (to, from){
            this.message = 'Welcome';
        }
    }
    ```
76. How can I use imported constants in template section? The variables need to be exposed on your data in order to use them in template section
    ```js
    <span>
    CREATE: {{CREATE_PROP}}
    UPDATE: {{UPDATE_PROP}}
    DELETE: {{DELETE_PROP}}
    </span>
    <script>
    import {CREATE_DATA, UPDATE_DATA, DELETE_DATA} from 'constants';
    new Vue({
        ...
        data:{
            CREATE_PROP: CREATE_DATA,
            UPDATE_PROP: UPDATE_DATA,
            DELETE_PROP: DELETE_DATA
        }
        ...
    })
    </script>
    ```
77. Why the component data must be a function? Because each instance needs to maintain an independed copy of the returned data object to not impact the data of all other instances when making changes to it.
    ```js
    data: { // Bad
        message: 'Hello'
    }
    data: function () { //Good
        return {
            message: 'Hello'
        }
    }
    ```
78. Can I use composition api on Vue 2? Yes, you can do it running (npm install @vue/composition-api) command.
79. What is composition Api? Set of additive, function-based APIs that allow flexible composition of component logic.
80. What is the best way to re-render a component? Set :key on the component. Whenever the component to be re-rendered just change the value of the key.
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
