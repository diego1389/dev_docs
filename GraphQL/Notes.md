- A query language for your API.
    - Query language for APIs and runtime for fulfilling those queries with your existing data. 
    - Ask for what you need (which fields to get from your API). Avoids overfetching.
    - GraphQL queries always return predictable results.
    - Get many resources in a single request. 
    - You can describe GraphQL Apis using types and fields, not endpoings:
    ```js
    type Query{
        hero: Character
    }
    type Character{
        name: String,
        frieds: [Character],
        homeWorld: Plan
    }
    type Planet{
        name: String
        climate: String
    }
    ```
    - Evolve API without versioons (add new fields and types without impacting existing queries).
    - GraphQL provides an API layer but you don't need to rewrite your entire code base. GraphQL engines available in many languages.

# Project
    
- Create server.js file and create type definitions and resolver functions:
    ```js
    const typeDefs = `
        type Query{
            greeting: String
        }`;

    //  greeting resolves the value of the greeting field defined above
    const resolvers = {
        Query: {
            greeting : () => 'Hello world!'
        }
    }
    ```
- To serve the API over http use Apollo Server (popular implementation for js).
- Install:
    ```batch
    npm install @apollo/server
    ```
- Build the API with Apollo server:
    - server.js
    ```js
    import {ApolloServer} from '@apollo/server';
    import {startStandaloneServer} from '@apollo/server/standalone'

    const typeDefs = `#graphql
        type Query{
            greeting: String
        }
    `
    ;
    // greeting resolves the value of the greeting field defined above
    const resolvers = {
        Query: {
            greeting : () => 'Hello world!'
        }
    }

    const server = new ApolloServer({typeDefs, resolvers });
    const {url} = await startStandaloneServer(server, {listen: {port: 9000}});
    console.log(`Server running at ${url}`);
    ```
    - Serve it with:
    ```batch
    node server.js
    ```
- Go to http://localhost:9000 and there is a Sandbox to write graphQL queries:
    ```graphql
    query{
        greeting
    }
    ```
    - Response:
    ```json
    {
        "data": {
            "greeting": "Hello world!"
        }
    }
    ```
    - GraphQL response can contain other properties additional to data. For example:
    ```json
    {
        "data": {},
        "errors": [
            {
            "message": "Cannot query field \"greetings\" on type \"Query\". Did you mean \"greeting\"?",
            "locations": [
                {
                "line": 2,
                "column": 3
                }
            ],
            "extensions": {
                "code": "GRAPHQL_VALIDATION_FAILED",
                "stacktrace": [
                    "GraphQLError: Cannot query field \"greetings\" on type \"Query\". Did you mean \"greeting\"?"
                ]
            }
            }
        ]
    }
    ```
    - query keyword in the response correspons to the query operation on the schema:
        ```js
        type Query{
            greeting: String
        }
        ```
- GraphQL over HTTP
    
    - Turn on schema polling means it will execute the request every second (you can turn it off).
    - All GraphQL will use HTTP Post method
- GraphQL Client:
    - Create client project and html file:
    ```html
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="UTF-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>GraphQL Client</title>
    </head>
    <body>
        <h1>GraphQL Client</h1>
        <p>
            The server says:
            <strong id="greeting">
                Loading...
            </strong>
        </p>
    <script src="app.js"></script>
    </body>
    </html>
    ```
    - app.js
    ```js
    async function fetchGreeting(){
        const response = await fetch('http://localhost:9000/',{
            method: 'POST',
            headers:{
                'Content-Type':'application/json'
            },
            body: JSON.stringify({
                query:'query{greeting}'
            })
        })
        const {data} = await response.json();
        return data.greeting;
    }

    fetchGreeting().then((greeting) =>{
        document.getElementById('greeting').textContent = greeting;
    });
    ```