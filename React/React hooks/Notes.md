- **Hooks:** use state and lifecycle methods inside a React functional component.
Build 100 % of your application with React functional components (without javascript classes).
useState:
    ```js
    import React, {useState} from 'react';


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

    export default InputElement;
    ```
    - **useRef:** primarly used to allow access directly to an element in the DOM.
        * It is normally best to avoid using direct html access.
        * ImageChangeOnMouseOver.js
            ```js
            import React from 'react';
            import ImageToggleOnMouseOver from '../src/ImageToggleOnMouseOver';

            const ImageChangeOnMouseOver = () => {
                return (
                    <div>
                        <ImageToggleOnMouseOver primaryImg="/static/speakers/bw/Speaker-187.jpg" 
                        secondaryImg="/static/speakers/Speaker-187.jpg" alt=""/>
                        &nbsp;&nbsp;&nbsp;
                        <ImageToggleOnMouseOver primaryImg="/static/speakers/bw/Speaker-1124.jpg" 
                        secondaryImg="/static/speakers/Speaker-1124.jpg" alt=""/> 
                    </div>
                );
            };

            export default ImageChangeOnMouseOver;
            ```
        * ImageToggleOnMouseOver.js
            ```js
            import React, {useRef} from 'react';

            const ImageToggleOnMouseOver = ({primaryImg, secondaryImg}) =>{
                const imageRef = useRef(null);

                return(
                    <img onMouseOver ={()=> {
                        imageRef.current.src = secondaryImg
                    }} onMouseOut={()=>{
                        imageRef.current.src = primaryImg
                    }} 
                    src={primaryImg}
                    alt="" ref={imageRef}/>
                );

            };

            export default ImageToggleOnMouseOver;
            ```
