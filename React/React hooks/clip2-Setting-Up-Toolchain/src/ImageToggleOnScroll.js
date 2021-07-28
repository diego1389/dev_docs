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