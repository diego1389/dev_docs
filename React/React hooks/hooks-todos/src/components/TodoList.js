import React, {useContext} from 'react';
import TodosContext from '../context';

export default function TodoList(){
    const{state, dispatch} = useContext(TodosContext);
    const title = state.todos.length > 0 
    ? `${state.todos.length} Todos`
    : "Nothing To Do!"
    return(
        <div className="container mx-auto max-w-md text-center font-mono">
            <h1 className="bold">
                {title}
            </h1>
            <ul className="list-reset text-white p-0">
                {state.todos.map(todo=>(
                    <li className="bg-yellow-600 border-black border-dashed border-2
                    my-2 py-4 flex items-centered" key={todo.id}>
                        <span 
                        className={`flex-1 m1-12 cursor-pointer ${todo.complete &&  "line-through text-gray-500"}`}
                        onDoubleClick={() => dispatch({type : "TOGGLE_TODO", payload: todo})}>{todo.text}</span>
                        <button onClick={()=> dispatch({type : "SET_CURRENT_TODO", payload:todo})}>
                        <img src="https://img.icons8.com/material-outlined/24/000000/edit--v4.png" 
                        alt="Edit"
                        className="h-6"/>
                        </button>
                        <button onClick={() => dispatch({type: "REMOVE_TODO", payload : todo})}>
                        <img src="https://img.icons8.com/material-outlined/24/000000/delete--v4.png" 
                        alt="Delete"
                        className="h-6"/>
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    )

}