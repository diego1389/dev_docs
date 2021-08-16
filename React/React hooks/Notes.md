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
        - **useEffect:** similar to componentDidMount(useEffect(()=> {})), componentDidUpdate and componentWillUnmount (useEffect(()=> {... return() => {...}})) from react class components.
            - useEffect causes side effects to functional components. 
            - Useful for adding and removing DOM listeners.
            - The second paramenter of useEffect is a list of dependencies (useEfect(()=>{}, [])). If it's left out, then the function in the first parameter it's executed both when the component is first rendered and then on every subsequent component update. If this array it's empty then the function associated with useEffect as the first parameter is only run once the component is first rendered. If you do want the component to be rendered based on certain conditions, you need to have all the values in this array that change.

            ```js
            import React, {useEffect} from "react";

            const Syntax = () =>{

                const [checkBoxValue, setCheckBoxValue] = useState(false);

                useEffect(()=>{
                    console.log('in useEffect');
                    return()=>{
                        console.log('in useEffect cleanup');
                    }
                }, [checkBoxValue]);
                return(<div></div>);
            };

            export default Syntax;
            ```
        - useEffect dependency array is used for optimization (running only the first time).
        - ImageToggleOnScroll:
            ```js
            import React, {useEffect, useRef, useState} from 'react';

            const ImageToggleOnScroll = ({primaryImg, secondaryImg}) =>{
                const imageRef = useRef(null);
                const [isLoading, setIsLoading] = useState(true);
                
                const isInView = () => {
                    const react = imageRef.current.getBoundingClientRect();
                    return react.top >= 0 && react.bottom <= window.innerHeight;
                };

                const [inView, setInView] = useState(false);
                useEffect(()=> {
                    setIsLoading(false);
                    setInView(isInView());
                    window.addEventListener("scroll", scrollHandler);
                    return () => {
                        window.removeEventListener("scroll", scrollHandler);
                    };
                }, []);

                const scrollHandler = () => {
                    setInView(isInView());
                }

                return(
                    <img 
                    src={isLoading ? 'data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw=='
                    : inView ? secondaryImg : primaryImg}
                    alt="" ref={imageRef}/>
                );

            };

            export default ImageToggleOnScroll;
            ```
        - ImageChangeOnScroll.js

            ```js
            import React from 'react';
            import ImageToggleOnScroll from '../src/ImageToggleOnScroll';

            const ImageChangeOnScroll= () => {
                return (
                    <div>
                        {[1124, 187, 823, 1269, 1530].map((speakerId)=>{
                            return(
                                <div key={speakerId}>
                                    <ImageToggleOnScroll primaryImg={`/static/speakers/bw/Speaker-${speakerId}.jpg`}
                                    secondaryImg={`/static/speakers/Speaker-${speakerId}.jpg`} alt=""/>
                                </div>
                            );
                        })}
                    </div>
                );
            };

            export default ImageChangeOnScroll;
            ```
        - ImageChangeOnScroll.js
        
            ```js
            import React, {useState, useEffect} from 'react';
            import ImageToggleOnScroll from '../src/ImageToggleOnScroll';

            const ImageChangeOnScroll= () => {

                const [currentSpeakerId, setCurrentSpeakerId] = useState(0);
                const [mouseEventCount, setMouseEventCount] = useState(0);
                useEffect(()=>{
                    window.document.title = `Speaker id: ${currentSpeakerId}`;        console.log(`useEffect: setting title to ${currentSpeakerId}`);
                }, [currentSpeakerId]);
                return (
                    <div>
                        <span>mouseEventCount : {mouseEventCount}</span>
                        {[1124, 187, 823, 1269, 1530].map((speakerId)=>{
                            return(
                                <div key={speakerId} onMouseOver={() => {
                                    setCurrentSpeakerId(speakerId);
                                    setMouseEventCount(mouseEventCount + 1);
                                    console.log(`onMouseOver:${speakerId}`);
                                    }}>
                                    <ImageToggleOnScroll primaryImg={`/static/speakers/bw/Speaker-${speakerId}.jpg`}
                                    secondaryImg={`/static/speakers/Speaker-${speakerId}.jpg`} alt=""/>
                                </div>
                            );
                        })}
                    </div>
                );
            };

            export default ImageChangeOnScroll;
            ```
    * Hooks can only be used in functional React components and they must be called just at the top level. No nesting no calling them inside other functions. 
    * Eslint plugin to identify react hooks that don't meet the basic rules.
    