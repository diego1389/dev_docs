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