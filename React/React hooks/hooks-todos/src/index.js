import React, {useContext, useEffect, useReducer, useState} from 'react';
import ReactDOM from 'react-dom';
import reportWebVitals from './reportWebVitals';
import TodosContext from './context';
import reducer from './reducer'; 
import TodoList from './components/TodoList';
import TodoForm from './components/TodoForm';
import axios from 'axios';

const useApi = endPoint => {
  const[data, setData]=  useState([]);

  useEffect(()=> {
    getData();
  }, [])

  const getData = async () => {
    const response = await axios.get(endPoint);

    setData(response.data);
  }

  return data;
}

const App = ()=> {
  const initialState = useContext(TodosContext);
  const [state, dispatch] = useReducer(reducer, initialState);
  const savedTodos = useApi("https://hooks-api-dtcefwvsg-diego1389.vercel.app/todos");

  useEffect(() => {
    dispatch({
      type : "GET_TODOS",
      payload : savedTodos
    })
  }, [savedTodos])

  return(
    <TodosContext.Provider value={{state, dispatch}}>
      <TodoForm/>
      <TodoList/>
    </TodosContext.Provider>
  );
}

ReactDOM.render(
    <App/>
  ,document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
