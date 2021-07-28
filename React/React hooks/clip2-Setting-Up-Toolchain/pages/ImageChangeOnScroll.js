import React, {useState, useEffect} from 'react';
import ImageToggleOnScroll from '../src/ImageToggleOnScroll';

const ImageChangeOnScroll= () => {

    const [currentSpeakerId, setCurrentSpeakerId] = useState(0);
    const [mouseEventCount, setMouseEventCount] = useState(0);
    useEffect(()=>{
        window.document.title = `Speaker id: ${currentSpeakerId}`;
    });

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