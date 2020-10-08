## Introduction

1. Organize data in your SPA.
2. Predictable state container.
3. Redux: instead of modules redux has the idea of a store. In redux any change event is called an action.
4. The store is then updated by calling a reducer. A function that decides how to handle the current action.
5. The UI only gets re-rendered when the store changes. 

**Pure functions:** 

* Pure function relies only in the arguments passed (not global variables). 
* They also have no side effect (do not mutate states or changes anything globally).
* Given the same input you will always have the same output. 

**Immutability:**
* Its not a good practice to mutate your state directly because you loose your history.
* Its better to create a new state object with Object.assign
    ```js
    var state = {color : 'red', name : 'Adan', points : 5}; 

    var state2 = Object.assign({}, state, {points : 25});
    ```

* You can reload from url.
* Time travel.

## React crash course

1. Babel takes jsx (sintax extension to javascript) and transforms it into valid javascript. 
2. If you are extending a class, before you can use this you have to use **super** and pass the props in the constructor. It calls the parent constructor. If extending to React.Component it calls the constructor in React.Component FIRST.
3. Any time you change the state reacts calls the render() method. 
4. Webpack is a module bundler that combines our files. Instead of having, 6 or more script files we have a bundle.js script. 
5. Webpack.config.js is a javascript file that exports a configuration object.
6. Webpack dev-server which will start when you type npm start. It bundles up the client-side.js files and will recompile any time you make changes. 
 
 ## The problem with state

 1. Javascript descruturing assignment:

  ```js
  var {conversionRate, fee, total, originCurrency} = this.props;
  /*equals than*/

  var conversionRate = this.props.conversionRate;
  ```

  - Redux clarifies data flow.
  

   ## Redux basics

- First import redux:

     ```js
    import {createStore} from 'redux';
    ```

- Then you need a default state:

    ```js
    var defaultState = 0;
    ```

- Then we need a reducer (a function that will update the state).

    ```js
    var amount(state = this.defaultState, action){
        state = state;
        return state + 1;
    }
    ```
- After that we need a store, which is a data store where our state is saved. 

   ```js
    var store = createStore(amount); //we pass the reducer as the argument
    ```

- We can subscribe to updates of the store (passing a callback function); 

  ```js
  store.subscribe(function(){
    console.log("state", store.getState());
  })
    ```

- Setter (update the redux store)>

   ```js
    store.dispatch({type : ''})
    ```
- The result is state : 2 because when you create the store redux calls the reducers once. 

- Let's add an action type:

   ```js
   /*Modify the dispatch*/
    store.dispatch({type : 'INCREMENT'})

    /*Modify the amount reducer to check type*/
    amount(state = this.defaultState, action){

        if(action.type === "INCREMENT"){
            return state + 1;
        }
        return state;

    }

    /*Dispatch three times (fire three actions)*/

    store.dispatch({type : 'INCREMENT'}); //state : 1
    store.dispatch({type : ''}); //state : 1
    store.dispatch({type : ''}) //state : 1
    ```

- It uses the publish subscribe pattern. 

- In redux you cannot mutate the stata.

- An example using objects:

    ```js
     defaultState = {
        originAmount : '0.0'
    };

      //reducer
    amount(state = this.defaultState, action){

    if(action.type === "CHANGE_ORIGIN_AMOUNT"){
        let newState = 
        {...state, originAmount : action.data};
        //Object.assign({}, state, {originAmount : action.data})
        return newState;
    }

    return state
    }

     var store = createStore(this.amount.bind(this)); 
  
    store.subscribe(function(){
        console.log("state", store.getState());
    })

    store.dispatch({type : 'CHANGE_ORIGIN_AMOUNT', data : '600.65'});
    store.dispatch({type : ''});
    store.dispatch({type : ''})
    ```

- Create a separate file for the store (createStore.js).

 ```js
import {createStore} from 'redux';

var defaultState = {
    originAmount : '0.0'
  };

    //reducer
function amount(state = defaultState, action){

    if(action.type === "CHANGE_ORIGIN_AMOUNT"){
      let newState = {...state, originAmount : action.data};
      //Object.assign({}, state, {originAmount : action.data})
      return newState;
    }
  
    return state
  
  }


  var store = createStore(amount); 

  export default store;

```

- Pass the store as a component property App.jsx (you have to subscribe and set empty state to render the child componet):

 ```js
export class App extends React.Component {
  constructor(props){
    super(props);
  }

  componentDidMount(){
    store.subscribe(()=> {
      this.setState({})
    })
  }

  render(){

    return (
      <div className="App">
        <Component originAmount = {store.getState().originAmount}/>
      </div>
    );
  }
}

export default App;
 ```

- Component.jsx:

```js
import React from 'react';
import './App.css';
import store from './createStore';

export class Component extends React.PureComponent {
    constructor(props){
        super(props);
    
      }

    HandleClick(){
        store.dispatch({type : 'CHANGE_ORIGIN_AMOUNT', data :  '600.65'});
        console.log("handle click!!");
    }

    componentWillReceiveProps(nextProps){
        console.log("nextProps", nextProps);
    }
    render(){
        return(
        <>
        <button onClick={this.HandleClick.bind(this)}>Click me</button>
        <h3>{this.props.originAmount}</h3>
        </>
        );
    }
}

export default Component;
```

- React redux npm module to make things easier.

- Remove the subscribe in the parent as well as the property that passes the store and add Provider wrapper.


```js
//App.jsx
import React from 'react';
import store from './createStore';
import Component from './Component';
import {Provider} from 'react-redux';


export class App extends React.Component {

  
  constructor(props){
    super(props);
  }

  render(){
    return (
      <div className="App">
        <Provider store={store}>
              <Component message="Hello component!"/>
        </Provider>
      </div>
    );
  }
}

export default App;
```

- Add a connect to the Component, the connect receives a callback function that executes anytime redux updates the store.

```js
import React from 'react';
import './App.css';
import {connect} from 'react-redux';

    export class Component extends React.PureComponent {
        constructor(props){
            super(props);
        }

        HandleClick(){
            //current state and the action object
            this.props.dispatch({type : 'CHANGE_ORIGIN_AMOUNT', data :  '600.65'});
        }


        render(){
            return(
            <>
            <button onClick={this.HandleClick.bind(this)}>Click me</button>
            <h3>{this.props.originAmount}</h3>
            </>
            );
        }
    }

    export default connect((state, props)=> {
        return {
            originAmount : state.originAmount
        }
    })(Component);
```

- For optimization it is recommended to use react-redux instead of subscribe.

- Separate smart (container) and dumb (presentational)components. Container handles the store and presentational doesnt know anything of redux it just receives the props and render them. 

 ## Asynchronous actions

 - Middleware is a way where you can inject your own code when certain events happen.

 - npm install --save-dev redux-logger
 
  ```js
  import {applyMiddleware, createStore} from 'redux';
  import logger from 'redux-logger';

  var defaultState = {
      originAmount : '0.0'
    };

  function amount(state = defaultState, action){
      if(action.type === "CHANGE_ORIGIN_AMOUNT"){
        let newState = {...state, originAmount : action.data};
        return newState;
      }
      return state
    }

    var store = createStore(amount, 
      applyMiddleware(logger)); 

    export default store;
  ```

- Redux thunk allows us to use asynchronous actions. A thunk is a function that wraps an expression to delay its evaluation.

- Logger has to be the last middleware. 

- Action creators to abstract the things that don't change. 

 ```js
 //Actions.js
 export function changeOriginAmount(newAmount){
    return {
        type : 'CHANGE_ORIGIN_AMOUNT', 
        data :  newAmount
    }
}
 ```
- Import the action in the container.

  ```js
  import * as actions from './Actions';

  export class Component extends React.PureComponent {
      constructor(props){
          super(props);
        }

      HandleClick(){
          this.props.dispatch(function(dispatch){
              dispatch({type : "SOME_ACTION", data : "SOMEDATA"});

              setTimeout(function(){
                  dispatch(actions.changeOriginAmount('505.5'),3000)
              })
      });
  }
  ...}
  ```
- Keymirror npm where the value is equal than the key.

  ```js
  import keyMirror from 'keymirror';

  export var ActionTypes = keyMirror({
    CHANGE_ORIGIN_AMOUNT : null
  }); 
  ```