import React, {useEffect, useState} from 'react';

const InputElement = () => {
    const [isLoading, setIsLoading] = useState(true);

    useEffect(()=> {
        setTimeout(()=>{
            setIsLoading(false)
        }, 2000);
    })
    return isLoading ? <div>Loading...</div> : <input placeholder="Enter Some Text"/>;
};

export default InputElement;
/*
const InputElement = () => {
    const [inputText, setInputText] = useState("");
    const [historyList, setHistoryList] = useState([]);

    return <div>
    <input onChange={(e)=>{
        setInputText(e.target.value);
        setHistoryList([...historyList, e.target.value]);
    }}placeholder="Enter some text"/><br/>
    {inputText}
    <hr/><br/>
    <ul>
        {historyList.map((rec)=> {
            return <div>{rec}</div>
        })}
    </ul>
    </div>
};

export default InputElement;*/