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
27. Difference between JOIN and UNION? Join is the operator that combines data from many tables based on specific conditions and it creates new columns. In contrast, UNION combines data from many tables using SELECT statements creating new rows. 
    - SET operators (UNION, UNION ALL, INTERSECT and EXCEPT).
        - Two input queries must produce results with the same number of columns, and corresponding columns must have compatible data types. 
        - The names of the columns in the result are determined by the first query. 
        
28. Clustered indexes? 