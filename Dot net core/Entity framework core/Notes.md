## Questions

1. What is Entity framework? Is an open-source Object-relational mapper for .Net applications. It is a set of utilities and mechanisms that work towards optimizing data-driven applications. 
    - Little coding and high level ob absorption when dealing with data (unlike ADO.Net).
    - It fits between the business entities (domain classes) and the database.
    - Cross platform: used on Linux, Windows or Mac
    - Modelling.
    - Querying: LINQ transformed into database specific query languages. 
    - Change tracking: SaveChanges method of the context. EF tracks the changes of the entities and relationships.
    - Saving: Upon calling SaveChanges() EF executes insert, update and delete operations. 
    - Concurrency: optimistic concurrency to prevent unknown user from overwriting data from the db. 
    - Transaction: can be cutomized. 
    - Caching: repeated queries will retrieve data from the cache rather than the database. 
    - Built In convention.
    - Configuration: data annotation attribute or Fluent API we can configure the EF model and override default conventions.
    - Migration.
2. Entity framework advantages:
    - Auto-migration it is simple to create a database or modify it.
    - Reduces the code length with alternate commands.
    - Reduces development time, development cost and provides auto-generated code. 
    - A unique syntax (LINQ).
    - Enables mapping of multiple conceptual models to a single storage schema.
3. Entity framework disadvantages:
    - Slower form of the ORM.
    - Some RDMS do not offer this feature.
    - Lazy loading.
4. What are the main components of EF?
    - **Entity Data Model (EDM):** abstracts logical or relational schema and expose conceptual schema of data with a three-layered model: conceptual, mapping and storage.
        - Conceptual: Conceptual data definition language layer. Model classes (entities) and their relationships. Your database table design will not be affected by this.
        - Mapping: Mapping schema definition language layer. Information about how the conceptual model is mapped to the storage model. Enables business objects and relationships defined at the conceptual layer to be mapped to tables and relationships defined at a logical layer.
        - Storage: Database design model that is composed of tables, keys, SPs, views and related relationships.
      - **LINQ to Entities:** query language used to write queries against the object model. 
    - **Entity SQL:** Another query language (more difficult than L2E).
    - **Entity Client Data Provider:** Convert E-SQL / L2E queries into SQL queries that the database understands. 
    - **Net Data Provider:** Uses standard ADO.NET to enable interaction with the database.
    - **Object Service:** Facilitates access to a database, and returns data for analysis when necessary. 
5. Explain what the .edmx file contains:
    - Represents conceptual models, storage models and their mappings. Mapping information between SQL tables and objects. EF designer is used to edit models stored and created in EDMX files. Using EDMX file you automatically generate classes that can interact with your application.
6. What are migrations? Tool to update the database schema automatically when a model is modified without losing any data. There are two types:
    1. Automated: Simply run a command through the package manager console.
    2. Code-based: You can configure additional aspects like setting the default value of a column, computed columns, etc.
7. Three different approaches?
    - Database first: Used to build entity models based on existing databases and reduce the amount of code required. 
    - Code first:Uses classes to create the model and its relations, which are then used to create a database. It is used by most devs using Domain-driven design.
    - Model first: uses ORM to build model classes and their relationships. Following the successful creation of the model classes and relationships, the physical db is created using these models. 
  8. What is a navigation property? A foreign key relationship in the database is represented by the navigation property. It is possible to specify relationships between entities using this property type. 
  9. What are the different entity states?
    - **Added:** entity exists within the context but does not exist within the database (SaveChanges generates INSERT SQL query).
    - **Deleted:** entity is marked for deletion but not removed from the database (SaveChanges generates DELETE SQL). 
    - **Modified:** Entity is modified. Also indicates the existence of the entity in the database. (SaveChanges generates UPDATE SQL).
    - **Unchanged:** Entity is ignored by SaveChanges.
    - **Detached:** Entity is not tracked by the DbContext. 
10. Write the importance of T4 entity in EF. EDMX xml files are read by T4 code templates which generate C# behind code. The generated code consists only of your entity and context classes. (T4 generates entity classes).
11. Explain the ways to increase performance in EF:
    - Choose the right collection for data manipulation.
    - Do not pull all DB objects into one entity model.
    - When entity is not required tracking should be disabled or altered.
    - Don't fetch all fields unless needed.
    - Avoid using Views and Contains. 
    - Bind data to a grid only by retrieving the number of records needed.
    - Optimize and debug LINQ queries.
    - Use compiled queries.
12. What is the migration history table? A database table used to store data about migrations applied to a database by Code first migrations. It contains metadata describing model's schema versions. 
13. How EF supports transactions? The SaveChanges method always wraps any operation involing insert, delete or updating data into a transaction. You don't have to explicitly open the transaction scope. 
14. What is deffered execution? Delaying the evaluation of an expression until its realized value is actually required. Performance is improved since unnecesary execution is avoided. Queries are deferred until the query variable is iterated over a loop. 
15. DbContext vs DbSet. 
    - DbSet is represented by DbSet class that can be used for creating, reading, updating and deleting operations on it. Those DbSet type properties map to db tables and views must be included in the context class.
    - DbContext: bridges the gap between entity or domain class and the database. Communication with db is its primary responsability.
16. ADO.Net vs EF:
    - ADO.Net is more efficient and faster.
    - EF generates code for intermediate layers, data access layers and mappings. 
17. Pluralize vs Singularize in EF:
    - Singular or Plural coding conventions. In convention names an additional 's' is added if there is more than one record in the object. 
18. Dapper vs EF:
    - Dapper is a micro ORM that allows you to perform database operations. It is faster than EF (almost as fast as ADO.NET data readers).
    - A set of .NET APIs. It is Microsoft's official tool for accessing data.
    - Dapper is more difficult to code, especially when multiple relashionships are involved.
19. Explain POCO classes in EF. A class that contains no reference to the EF fw. Poco entities are known as available domain objects. A normal C# class that support LINQ queries, which are supported by entities derived from the entity object itself. 

--Question 26: https://www.interviewbit.com/entity-framework-interview-questions/