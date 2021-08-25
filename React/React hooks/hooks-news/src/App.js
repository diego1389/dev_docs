import React, {useState, useEffect, useRef} from 'react';
import axios from 'axios';

export default function App(){
const [query, setQuery] = useState("reacthooks")
const [results, setResults] = useState([]);
const [isLoading, setIsLoading] = useState(false);

const searchInputRef = useRef();
  useEffect( ()=>{
    getResults()
  }, []);

  const getResults = async () => {
    setIsLoading(true);
    const response = await axios.get(`https://hn.algolia.com/api/v1/search?query=${query}`)
    setResults(response.data.hits);
    setIsLoading(false);
  }

  const handleSearch = event => {
    event.preventDefault();
    getResults();
  }

  const handleClearSearch = () => {
    setQuery("");
    searchInputRef.current.focus();
  }

  return(
    <>
    <form onSubmit={handleSearch}>
    <input type="text" 
    onChange={event => setQuery(event.target.value)} 
    value={query}
    ref={searchInputRef}/>
   <button type="submit">Search</button>
   <button type="button" onClick={handleClearSearch}>Clear</button>
    </form> 
    {isLoading ? (<div>Loading results</div>) : (<ul>
    {results.map(result =>(
      <li key={result.objectID}>
        <a href={result.url}>{result.title}</a>
      </li>
    ))}
    </ul>)}
    </>
  )
}