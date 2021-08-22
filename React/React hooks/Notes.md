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
    ----------------------

    * **useState:** it is a function and we need to execute at the beggining of our functional component.
        ```js
        const App =()=> {

        const [count, setCount] = useState(0);

        const incrementCount = () =>{
            setCount(count +1);
        }

        return (
            <button onClick={incrementCount}>Clicked {count} times</button>
        );
        }

        export default App;
        ```
    * Use previous state:
        ```js
        import React, {useState} from 'react';

        const App =()=> {

        const [count, setCount] = useState(0);

        const incrementCount = () =>{
            setCount(prevCount => prevCount +1);
        }

        return (
            <button onClick={incrementCount}>Clicked {count} times!</button>
        );
        }

        export default App;

        ```
    * useState example:
        ```js
        import React, {useState} from 'react';

        const App =()=> {

        const [count, setCount] = useState(0);
        const [isOn, setIsOn] = useState(false);

        const incrementCount = () =>{
            setCount(prevCount => prevCount +1);
        }

        const toggleLight = () =>{
            setIsOn(prevIsOn => !prevIsOn);
        }

        return (
            <>
            <h2>Counter</h2>
            <button onClick={incrementCount}>Clicked {count} times!</button>
            <h2>Toggle Light</h2>
            <div 
            style={{
            height: "50px",
            width: "50px",
            background: isOn ? "yellow" : "grey"
            }}
            onClick={toggleLight} alt="Lightbulb"></div>
            </>
        );
        }

        export default App;
        ```
    - useEffect stands for useEffect (sideeffects, calling api, outside world).
        ```js
        import React, {useState, useEffect} from 'react';

        const App =()=> {

        const [count, setCount] = useState(0);
        const [isOn, setIsOn] = useState(false);

        useEffect(()=> {
            document.title = `You have clicked ${count} times`;
        });

        const incrementCount = () =>{
            setCount(prevCount => prevCount +1);
        }

        const toggleLight = () =>{
            setIsOn(prevIsOn => !prevIsOn);
        }

        return (
            <>
            <h2>Counter</h2>
            <button onClick={incrementCount}>Clicked {count} times!</button>
            <h2>Toggle Light</h2>
            <div 
            style={{
            height: "50px",
            width: "50px",
            background: isOn ? "yellow" : "grey"
            }}
            onClick={toggleLight} alt="Lightbulb"></div>
            </>
        );
        }

        export default App;

        ```
    - With the return function inside useEfect (at the end) we can specify something that we want to perform when component unmounts. F.e: remove event listener.
    - useEfect example:
        ```js
        import React, {useState, useEffect} from 'react';

        const App =()=> {

        const [count, setCount] = useState(0);
        const [isOn, setIsOn] = useState(false);
        const[mousePosition, setMousePosition] = useState({x: null, y:null});

        useEffect(()=> {
            document.title = `You have clicked ${count} times`;
            window.addEventListener('mousemove', handleMouseMove);

            return() =>{
            window.removeEventListener('mousemove', handleMouseMove);
            }
        }, [count]);

        const handleMouseMove = event =>{
            setMousePosition({
            x : event.pageX,
            y: event.pageY
            })
        }
        const incrementCount = () =>{
            setCount(prevCount => prevCount +1);
        }

        const toggleLight = () =>{
            setIsOn(prevIsOn => !prevIsOn);
        }

        return (
            <>
            <h2>Counter</h2>
            <button onClick={incrementCount}>Clicked {count} times!</button>
            <h2>Toggle Light</h2>
            <div 
            style={{
            height: "50px",
            width: "50px",
            background: isOn ? "yellow" : "grey"
            }}
            onClick={toggleLight} alt="Lightbulb"></div>
            <h2>Mouse position</h2>
            {JSON.stringify(mousePosition, null, 2)}
            <br/>
            </>
        );
        }

        export default App;

        ```
    - Another example:
        ```js
        import React, {useState, useEffect} from 'react';
        const App =()=> {

        const [count, setCount] = useState(0);
        const [isOn, setIsOn] = useState(false);
        const[mousePosition, setMousePosition] = useState({x: null, y:null});
        const [status, setStatus] = useState(navigator.onLine);

        useEffect(()=> {
            document.title = `You have clicked ${count} times`;
            window.addEventListener('mousemove', handleMouseMove);
            window.addEventListener('online', handleOnline);
            window.addEventListener('offline', handleOffline);

            return() =>{
            window.removeEventListener('mousemove', handleMouseMove);
            window.removeEventListener('online', handleOnline);
            window.removeEventListener('offline', handleOffline);
            }
        }, [count]);

        const handleMouseMove = event =>{
            setMousePosition({
            x : event.pageX,
            y: event.pageY
            })
        }

        const handleOnline = () =>{
            setStatus(true);
        }

        const handleOffline = () =>{
            setStatus(false);
        }

        const incrementCount = () =>{
            setCount(prevCount => prevCount +1);
        }

        const toggleLight = () =>{
            setIsOn(prevIsOn => !prevIsOn);
        }

        return (
            <>
            <h2>Counter</h2>
            <button onClick={incrementCount}>Clicked {count} times!</button>
            <h2>Toggle Light</h2>
            <div 
            style={{
            height: "50px",
            width: "50px",
            background: isOn ? "yellow" : "grey"
            }}
            onClick={toggleLight} alt="Lightbulb"></div>
            <h2>Mouse position</h2>
            {JSON.stringify(mousePosition, null, 2)}
            <br/>
            <h2>Network Status</h2>
            <p>You are <strong>{status? "online" : "offline"}</strong></p>
            </>
        );
        }

        export default App;

        ```
    - Another example of useEffect using geolocation:
        ```js
        import React, {useState, useEffect} from 'react';
        const initialLocationState = {
        latitude : null,
        longitude : null,
        speed : null
        }

        const App =()=> {

        const [count, setCount] = useState(0);
        const [isOn, setIsOn] = useState(false);
        const[mousePosition, setMousePosition] = useState({x: null, y:null});
        const [status, setStatus] = useState(navigator.onLine);
        const [{latitude, longitude, speed}, setLocation] = useState(initialLocationState);
        let mounted = true;

        useEffect(()=> {
            document.title = `You have clicked ${count} times`;
            window.addEventListener('mousemove', handleMouseMove);
            window.addEventListener('online', handleOnline);
            window.addEventListener('offline', handleOffline);
            navigator.geolocation.getCurrentPosition(handleGeolocation);
            const watchId = navigator.geolocation.watchPosition(handleGeolocation);

            return() =>{
            window.removeEventListener('mousemove', handleMouseMove);
            window.removeEventListener('online', handleOnline);
            window.removeEventListener('offline', handleOffline);
            navigator.geolocation.clearWatch(watchId);
            mounted = false;
            }
        }, [count]);

        const handleMouseMove = event =>{
            setMousePosition({
            x : event.pageX,
            y: event.pageY
            })
        }

        const handleGeolocation = event =>{
            if(mounted){
            setLocation({
                latitude : event.coords.latitude,
                longitude : event.coords.longitude,
                speed : event.coords.speed
            });
            }
        }

        const handleOnline = () =>{
            setStatus(true);
        }

        const handleOffline = () =>{
            setStatus(false);
        }

        const incrementCount = () =>{
            setCount(prevCount => prevCount +1);
        }

        const toggleLight = () =>{
            setIsOn(prevIsOn => !prevIsOn);
        }

        return (
            <>
            <h2>Counter</h2>
            <button onClick={incrementCount}>Clicked {count} times!</button>
            <h2>Toggle Light</h2>
            <div 
            style={{
            height: "50px",
            width: "50px",
            background: isOn ? "yellow" : "grey"
            }}
            onClick={toggleLight} alt="Lightbulb"></div>
            <h2>Mouse position</h2>
            {JSON.stringify(mousePosition, null, 2)}
            <br/>
            <h2>Network Status</h2>
            <p>You are <strong>{status? "online" : "offline"}</strong></p>
            <h2>Geolocation</h2>
            <p>Latitude is {latitude}</p>
            <p>Longitude is {longitude}</p>
            <p>Speed is {speed? speed : 0}</p>
            </>
        );
        }

        export default App;

        ```
    - Login form:
        ```js
        import React, {useState} from 'react';

        export default function Login(){
            const[username, setUsername] = useState("");
            const[password, setPassword] = useState("");
            const [user, setUser] = useState(null);

            const handleOnSUbmit = event => {
                event.preventDefault();
                const userData = {
                    username,
                    password
                }
                setUser(userData);
                setUsername("");
                setPassword("");
            }

            return(
                <div style={{
                    textAlign : 'center'
                }}>
                    <h2>Login</h2>
                    <form
                        style={{
                            display: 'grid',
                            alignItems : 'center',
                            justifyItems : 'center'
                        }}
                        onSubmit={handleOnSUbmit}>
                        <input type="text" placeholder="Username" onChange={event => setUsername(event.target.value)} value={username}></input>
                        <input type="text" placeholder="Password" onChange={event => setPassword(event.target.value)} value={password}></input>
                        <button type="submit">Submit</button>
                    </form>
                    {user && JSON.stringify(user, 2, null)}
                </div>
            );
        }
        ```
    - Register (handle form as an object)
        ```js
        import React, {useState} from 'react';
        const initialFormState = {
            username: "",
            email: "",
            password: ''
        };

        export default function Register(){
            const [form, setForm] = useState(initialFormState);

            const [user, setUser] = useState(null);

            const handleChange = event =>{
                setForm({
                    ...form,
                    [event.target.name] : event.target.value
                });
            }

            const handleSubmit = event => {
                event.preventDefault();
                setUser(form);
                setForm(initialFormState);
            }

            return( <div style={{
                textAlign : 'center'
            }}>
                <h2>Register</h2>
                <form
                    style={{
                        display: 'grid',
                        alignItems : 'center',
                        justifyItems : 'center'
                    }}
                    onSubmit={handleSubmit}>
                    <input type="text" placeholder="Username" name="username" onChange={handleChange}
                    value ={form.username}/>
                    <input type="email" placeholder="Email address" name = "email" onChange={handleChange}
                    value={form.email}/>
                    <input type="text" placeholder="Password" name="password" onChange={handleChange}
                    value={form.password}/>
                    <button type="submit" >Submit</button>
                </form>
                {user && JSON.stringify(user, null, 2)}
            </div>)
        }
        ```
    - For managing unrelated pieces of state you can use multiple state values. For related pieces of state you can use multiple state values. 
    - useEffect to call an external API and display:
        ```js
        import React, {useState, useEffect} from 'react';
        import axios from 'axios';

        export default function App(){

        const [results, setResults] = useState([]);
        useEffect(()=>{
            axios.get('https://hn.algolia.com/api/v1/search?query=reacthooks')
                .then(response =>{
                setResults(response.data.hits);
                })
        }, [])
        return(
            <>
            <ul>
            {results.map(result =>(
            <li key={result.objectID}>
                <a href={result.url}>{result.title}</a>
            </li>
            ))}
            </ul>
            </>
        )
        }
        ```
    - Retrieve the data using async / await
        ```js
        import React, {useState, useEffect} from 'react';
        import axios from 'axios';

        export default function App(){

        const [results, setResults] = useState([]);
        useEffect( ()=>{
            getResults()
        }, []);

        const getResults = async () => {
            const response = await axios.get('https://hn.algolia.com/api/v1/search?query=reacthooks')
            setResults(response.data.hits);
        }
        
        return(
            <>
            <ul>
            {results.map(result =>(
            <li key={result.objectID}>
                <a href={result.url}>{result.title}</a>
            </li>
            ))}
            </ul>
            </>
        )
        }
        ```
    - Search different queries:
        ```js
        import React, {useState, useEffect} from 'react';
        import axios from 'axios';

        export default function App(){
        const [query, setQuery] = useState("reacthooks")
        const [results, setResults] = useState([]);
        useEffect( ()=>{
            getResults()
        }, [query]);

        const getResults = async () => {
            const response = await axios.get(`https://hn.algolia.com/api/v1/search?query=${query}`)
            setResults(response.data.hits);
        }

        return(
            <>
            <ul>
            <input type="text" onChange={event => setQuery(event.target.value)}/>
            {results.map(result =>(
            <li key={result.objectID}>
                <a href={result.url}>{result.title}</a>
            </li>
            ))}
            </ul>
            </>
        )
        }
        ```
    - Add a button to search (you can search with Enter enclosing the input and the button tags within form tags and changing the button type to submit).
        ```js
        import React, {useState, useEffect} from 'react';
        import axios from 'axios';

        export default function App(){
        const [query, setQuery] = useState("reacthooks")
        const [results, setResults] = useState([]);
        useEffect( ()=>{
            getResults()
        }, []);

        const getResults = async () => {
            const response = await axios.get(`https://hn.algolia.com/api/v1/search?query=${query}`)
            setResults(response.data.hits);
        }

        const handleSearch = event => {
            event.preventDefault();
            getResults();
        }

        return(
            <>
            <form onSubmit={handleSearch}>
            <input type="text" onChange={event => setQuery(event.target.value)} value={query}/>
        <button type="submit">Search</button>
            </form> 
            <ul>
            {results.map(result =>(
            <li key={result.objectID}>
                <a href={result.url}>{result.title}</a>
            </li>
            ))}
            </ul>
            </>
        )
        }
        ```