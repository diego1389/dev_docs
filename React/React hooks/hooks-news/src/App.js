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