1. What are the modes of transactions?
    - Autocommit transactions.
    - Explicit transactions.
    - Implicit transactions.
    - Batch-scoped transactions.
- Transactions are recorded in the transaction log. It is used to retrieve the db to a consistent state.
2. ACID properties:
    - Atomicity: a complete transaction must take place in a single execution.
    - Consistency: it ensures that a transaction takes place with absolute data consistency before and after the transaction.
    - Isolation: Each transaction takes place in complete isolation from other transactions.
    - Durability: Every transaction must be recoverable when required. 
3. What are extended stored procedures? Expand functionality of SQL server through external functions written in C or C++.
4. Normalisation vs denormalization: 
    - Normaliaation: restructuring a relational database to reduce data redundancy and improve integrity.
    - Denormalization: reverse engineering that helps increase the read performance of a database. Add copies of data or grouping data so it can be read in shorter period.
5. What is a JOIN? Retrieve data from two or more two tables. JOIN types:
    - INNER JOIN
    - LEFT (OUTER) JOIN
    - RIGHT (OUTER) JOIN
    - FULL (OUTER) JOIN
    - CROSS JOIN
6. Subquery? A query nested inside the statements such as SELECT, INSERT, UPDATE or DELETE. 
7. Primary key vs Unique key: PK identifies each record in a table. It should have unique values but not NULL values. Unique key ensures that all the values are different. A table can have multiple unique keys but only one PK.
8. What are cursors? Extension to result in sets that are the group of rows returned for a statement. They can support data modifications for the rows in the current position in the result set.
9. What are triggers? Special Stored Procedures executed automatically when there is an event in SQL Server.
    - LOGON triggers.
    - DDL triggers (data definition language event).
    - DML triggers (data manipulation language event).
10. Referential integrity? Keep SQL databases consistent. Enforced through FK constraints or check constraints or user-defined functions and triggers.
11. CTE: common table expression. Specifies a temporary named result set (obtained by executing queries). CTE can be referred to in SELECT, INSERT, UPDATE, DELETE and Merge statements. They can even be used in VIEW statements. 
12. Type of locks?
    - Shared: page or rows only for reading. Restricts modifications of data by concurrent transactions.
    - Exclusive: exclusive transactions to modify a page or row using DML statements.
    - Updated: To avoid deadlocks. 
13. SQL server profiler? Graphical user interface for monitoring an instance of the database engine. Creates and manages traces and analizes results. Used to diagnosed issues such as slow-running queries.
14. COALESCE? Evaluates arguments in a list and returns only the first value that is not NULL (data type must be the same).
15. What is Bulk copy in SQL server? Allows larga amount of data transfer in and out of SQL tables or views. Four modes:
    1. Native mode data file: from table or view into table or view in the same format.
    2. Character mode data file: Bulk copy from table or view into another table in different format.
    3. From data file into a table or view.
    4. Loading into program variables initially and then bulk copying into a table/view.
16. Collation? Rules to sort and compare data, they determine how the data should be stored, accessed and compared. SQL server can store objects that have different collations in a single db.
17. UPDATE_STATISTICS command? Update query optimization statistics regularly in a database table or indexed view. 
18. What is a filtered index? Nun-clustered index with an optimized disk-based restore. It uses a filter predicate to select a portion of rows in a table (useful when a column has a fewer relevant values for queries). It helps to improve query performance and reduce storage costs.
19. Table variable vs temporary table? Table variable functions are faster than temporary table because they are stored in memory (temp tables are stored in disk). If table's variable size exceeds the size of memory then its speed decreases.
20. Scheduled tasks? Update the backups and statistics through schedule jobs. 
21. SQL injection? Malicious attack targeting an SQL Server instance. 
22. DB mirroring? two copies of a single db in two different locations. Db which adapt the full recovery model. 
23. Difference between SP and function? 
    - In SP codes are usually compiled and executed when the program calls them. Functions are compiled and executed at call time.
    - Functions must have a return whereas it is optional in stored proc. 
    - Functions can be called from SP but not the other way around.
24. Types of queries?
    - SELECT query
    - Insert results (INSERT INTO ... SELECT ...).
    - Insert values.
    - Update query
    - Delete query
    - Make table query (SELECT INTO creates a new table and rows in it).
25. UNION vs UNION ALL? Union combines two queries into a single result set using select statements (query1 UNION query2) based on specified conditions. Union all does the same thing extracting all the rows from tables without any conditions. (query1 ALL query2)
26. Types of JOINS?
    INNER JOIN: return records that are common to both tables.
    LEFT JOIN: return values that are common to each other along the the complete records from the left table.
    RIGHT JOIN: return values that are common to each other along with the complete records of the right table.
    FULL JOIN: return all table's records where there is a match between the two. 
    CROSS JOIN: cartesian join, returns all combinations of each join from the tables.
27. Difference between JOIN and UNION? Join is the operator that combines data from many tables based on specific conditions and it creates new columns. In contrast, UNION combines data from many tables using SELECT statements creating new rows. The join such as INNER JOIN or LEFT JOIN combines columns from two tables while the UNION combines rows from two queries. In other words, join appends the result sets horizontally while union appends the result set vertically.
    - SET operators (UNION, UNION ALL, INTERSECT and EXCEPT).
        - Two input queries must produce results with the same number of columns, and corresponding columns must have compatible data types. 
        - The names of the columns in the result are determined by the first query. 
        - Two flavors: DISTINCT and ALL.
            - DISTINCT aliminates duplicates and returns a set.
            - ALL doesn't remove duplicates and returns a multiset.
        - **UNION:** unifies the result of two input queries. If a row apears in any of the input sets it will appear in the result of the UNION Operator. 
        - **UNION ALL:** Unifies two input query results without attemptin to remove duplicates. 
        - **INTERSECT:** Returns only distinct rows that appear in both input query results. As long as a roew appears at least once on BOTH query results it's returnes only once in the operator's result.
        - **EXCEPT:** Implements set differences. It operates on the roew results of two input queries and returns rows that appear in the first input but not in the second. 

        
28. Clustered indexes? 
    - Describes how data is stored in a table, and the table should have a key value. There can be only one clustered index for a table. When there is no clustered index in a table the data is stored in tables unstructured. 
29. How can you use SCOPE_IDENTITY function? Returns the last identity value inserted into an identity column within the same scope (if two statements exist in the same sp or batch or function they are in the same scope).
30. What is the use of WITH TIES? Adding one or more rows along with the rows limited by the TOP or similar statements
    ```sql
    [
        TOP(expression)[PERCENT]
        [WITH TIES]
    ]
    ```
    - We have a table with 6 entires 1 to 4 and 5 twice. Running: 
    ```sql
    SELECT TOP 5 WITH TIES *
    FROM MyTable 
    ORDER BY ID;
    ```
    returns 6 rows, as the last row is tied (exists more than once.)
31. How can Deadlocks in the sql server be resolved?
    - It happens when two processes lock a single resource simultaneously and wait for the other to unlock the resource. Generaly the SQL engine notices this type of incident and ends one of the processes, it allows one process to complete successfully while stopping another process simultaneously.
32. Local vs global temporary tables? Local are visible only to the table creators when connected to a sql instance. They are deleted once the user disconnects the sql instance.
    - Global temporary tables are visible to any user. These are deleted only when any user referencing these tables gets disconnected from the sql instance. 
33. SUBSTR vs CHARINDEX: substring extracts a substring from the specified string, CHARINDEX helps indentify a substring's position from the specified string.
34. When to execute COMMIT AND ROLLBACK:
    - COMMIT: executed to save the changes made on the current transaction, after that it becomes permanent.
    - ROLLBACK: executed to delete the changes made on the current transaction after the last commit.
35. GETDATE vs SYSDATETIME? Getdate returns date and time of a location, SYSDATETIME returns the date and time with the precision of 7 digits after the decimal point.
36. What is SSMA in SQL Server? Microsoft SQL Server Migration Assistant. Automation tool that helps migrate from Access DB to SQL Server or Azure. It also supports DB2, MySQL, Oracle, etc.
37. What are Data Quality Services? knowledge-driven data quality platform that supports carrying oout data quality tasks such as correction, enrichment, standarization and de-duplication. Analysis tool. 
38. SQL Server integration services? platform for enterprise-level data integration and data transformation services. Copying and downloading files, loading data warehouses, cleaning and mining data to solve complex business problems. Built-in tasks, graphical tools. 
39. Clustered index vs non-clustered index?
    | Clustered      | Non-clustered |
    | ----------- | ----------- |
    | It describes the order in which data is stored in tables physically      | Doesn't sort tables physically inside a table but it creates a logical order for stored data       |
    | Each table can have only one   | There could be many for a table        |
    | Less storage is required|Non-clustered is stored in one location and data is stored in another. Large storage is required|
    |Support faster operations|Decreaces the speed of performance due to extra lookup setup|
40. Different levels of normalization:
    1. First normal form: avoids data duplication in a table. It creates a specific table for the related data and uses PK to identify data.
     - 
    2. Second form: It creates separate tables for the group of data that belongs to multiple records. Tables are linked with FK.
        - For every candidate key every nonkey has to be fully functionally dependent on the candidate key. 
        - Example of violation:

        | Orders      |    
        | ----------- |    
        | orderId PK      |   
        | productId PK   |    
        |orderDate|
        |customerId|
        |companyName|
        |qty|
        - Here you can find the orderDate and customerId just using the orderId.
        - To fix it split your original relation into two relations:

        | Orders|    
        | ----------- |    
        | orderId PK|    
        |orderDate|
        |customerId|
        |companyName|

        | OrderDetails|    
        | ----------- |    
        | orderId PK FK1|   
        | productId PK   |   
        |qty|

    3. Third form: it eliminates the fields that are not related to keys.
        - All nonkey attributes must be mutually independent. In other words, one nonkey attribute cannot be dependent on another nonkey attribute (like customerId and companyName).
        - To fix it add a Customers relations with the attributes customerId (as PK) and companyName:

        | Orders|    
        | ----------- |    
        | orderId PK|    
        |orderDate|
        |customerId FK|


        | OrderDetails|    
        | ----------- |    
        | orderId PK FK1|   
        | productId PK   |   
        |qty|

        |Customers|
        |-----------|
        |customerId PK|
        |companyName|


    4. Fourth normal form: it should be in the form 3 and there shouldn't be any multi-valued dependencies.
        ---------
41. DELETE vs TRUNCATE? DELETE removes a row(s) from a table based on given condition whereas TRUNCATE removes the entire rows from a table. TRUNCATE changes are commited automatically (DELETE are not).
42. Local vs global variables?

    |Global variables|Local variables |
    |-----------|---------|
    |Declared outside of all functions|Declared inside a function and be called only by that function |
    |Stored in fixed memory and not cleaned up automatically|Stored in stack memory and cleaned up automatically|
43. FLOOR function? Returning the largest integer value which is less than or equal to the specified value.
    ```sql
    SELECT FLOOR (22.35) AS FloorValue --22
    ```
44. SQL server locks and what resources can be locked by server locks? Exclusive locks lock a row during a transaction other transaction have to wait to view or modify that row only when the lock is released. Locks reduce concurrency. Application, db, rows, table, keys, etc can be locked.
45. SET NOCOUNT function? Stop the message that indicates how many rows are being affected by a statement. 
46. What are magic tables? Virtual tables that exist in two types (Inserted and deleted). They hold the information of the newly inserted deleted rows. INSERTED table and DELETED table.
47. Prevent sql injection? 
    1. Type-safe sql parameters.
    2. Parameterized input with SP.
    3. Filtering inputs.
    4. Reviewing codes.
48. Recovery model? Property that controls the transaction log maintenance in a db. Tracks logging of transactions and decides about neccesary backups. 
    - Simple recovery model: no log backup.
    - Full recovery model: requires log backups
    - Bulk-logged model: requires log backups. It allows high-performance bulk-copy operations.
49. Using HAVING and WHERE clauses in a single query? Where acts on individual rows whereas the having acts on groups. Where clause acts first and then the having acts on the groups.
50. Types of UDF (User defined functions)?
    1. Scalar.
    2. Table-valued
    3. System functions.
51. Advantages of SP over dynamic SQL?
    - SP is cached in server memory so it is faster.
    - SP keep business logic separate from db logic. 
    - Stored procedures with static SQL can detect errors before they run.
52. SSRS in SQL server? SQL server reporting services. Provides a group of tools and services like: paginated reports, mobile reports and web portal.
53. Isolation level? property of transactions which is used to isolate a SQL transaction from another one. Isolation feature helps lock a row during a transaction so no other transaction can access that row. 
54. Triggers vs event notifications? Triggers respond to DML and DDL whereas events respond only to DDL events
    - Event notifications don't run any codes. 
55. FILESTREAM? Applications need to store unstructured data such as images and documents.
56. Columstore index and why use it? Method of storing, retrieving and managing data using a columnar data format. 10x better performance than row-oriented storage. Used for large data warehouse tables.
57. How to improve query performance in SQL server?
    - Choosing right execution plan (avoid table scans).
    - Avoiding cursors.
    - Using partitioned views.
    - Reducing table sizes and simplifying joins.
    - Not using Select * 
    - Using Exist() instead of Count()
    - Creating indexes.
    - Avoiding running queries in loops.
    - Transaction need to be shorter to avoid deadlocks and blockings.
58. Graph Db? You can create a graph using node or edge tables for a database. Node tables are a colection of similar types of nodes. Edge tables are a collction of similar types of edges. Node tables can be created based on a logical graph under any schema. 
59. What is the use of views? Virtual db table created by selecting a few rows and columns from other tables. Used to simplify compex queries, restricting access to datga and summarising data from many tables. 
    - System defined views
    - User defined views.
60. Extended stored procedures vs CLR integration?

    |Stored procedure|CLR integration|
    |--------|--------------|
    |Support the functionalities that cannot work with T-SQL SP|Managed code with services such as cross-language integration, debugging and profiling |
    |Devs need to write complex server-side logic| Provides an alternative to simplify coding|
    |Codes can be written in C/C++ programming language|Code can be written in .NET programming languages|
61. Two execution modes? 
    1. Row-mode execution: data stored in row format, rows are read ony by one. 
    2. Batch-mode execution: multiple modes are executed together as a batch in this mode. It offers better parallelism and faster performance. 
62. Subquery restrictions?
    1. ntext, image and text data types cannot be used.
    2. If there is a column name in the WHERE clause of an outer query it should be join-compatible with the column in the subquery select list.
    3. DISTINC cannot be used with subqueries that include GROUP By
    4. COMPUTE and INTO clauses cannot be specified. 
63. EXCEPT vs INTERCEPT commandds.
    - Return distinct rows by comparing results of two separate queries. 
    - EXCEPT: operator allows returning distinct rows from the left input query only.
    - INTERCEPT: operator allows returning ditinct rows from both left and right input queries.
64. Pattern matching? LIKE operator to identify wheter a character string matches a specified patternt. It consist of regular character as well as wildcard characters (% for example).
