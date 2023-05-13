## Questions

1. What is Entity framework? Is an open-source Object-relational mapper for .Net applications. It is a set of utilities and mechanisms that work towards optimizing data-driven applications. 
    - Little coding and high level of absorption when dealing with data (unlike ADO.Net).
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

20. How do you use SP in EF? YOu can use DbSet<TEntity>.FromSql() or DbContext.Database.ExecuteSqlCommand() methods. Some limitations:
    - Result must be an entity type (must return all the columns).
    - Results cannot contain related data (joined tables). 
    ```c#
    var context = new SchoolContext(); 
    var param = new SqlParameter("@FirstName", "Bill");
    //or
    /*var param = new SqlParameter() {
                        ParameterName = "@FirstName",
                        SqlDbType =  System.Data.SqlDbType.VarChar,
                        Direction = System.Data.ParameterDirection.Input,
                        Size = 50,
                        Value = "Bill"
    };*/

    var students = context.Students.FromSql("GetStudents @FirstName", param).ToList();
    ```
21. Database concurrency and the way to handle it? Multiple users can simultaneously modify the same data in one database. Optimistic locking is usually used to handle database concurrency. Right-click the EDMX designer and then change the concurrency mode to Fixed to implement locking. If there is a concurrency issue we will receive a positive concurrency exception error. 
22. Types of loading:
    - Eager loading.
        - A query for one type of entity also loads related entities as part of the query. Use of the **Include method**:
        ```c#
        using (var context = new BloggingContext())
        {
            // Load all blogs and related posts.
            var blogs1 = context.Blogs
                                .Include(b => b.Posts)
                                .ToList();

            // Load one blog and its related posts.
            var blog1 = context.Blogs
                            .Where(b => b.Name == "ADO.NET Blog")
                            .Include(b => b.Posts)
                            .FirstOrDefault();

            // Load all blogs and related posts
            // using a string to specify the relationship.
            var blogs2 = context.Blogs
                                .Include("Posts")
                                .ToList();

            // Load one blog and its related posts
            // using a string to specify the relationship.
            var blog2 = context.Blogs
                            .Where(b => b.Name == "ADO.NET Blog")
                            .Include("Posts")
                            .FirstOrDefault();
        }
        ```
        - It is also possible to eagerly load multiple levels of related entities:
        ```c#
        using (var context = new BloggingContext())
        {
            // Load all blogs, all related posts, and all related comments.
            var blogs1 = context.Blogs
                                .Include(b => b.Posts.Select(p => p.Comments))
                                .ToList();
        }
        ```

    - Lazy loading
        - Process whereby an entity or collection of entities is automatically loaded from the database the first time. Using POCO entity types, LL is achieved by creating instances of derived proxy types and then overriding virtual properties to add a loading hook. 
        - When using the Blog entity class defined below, the related Posts will be loaded the first time the Posts navigation property is accessed
        ```c#
        public class Blog
        {
            public int BlogId { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            public string Tags { get; set; }

            public virtual ICollection<Post> Posts { get; set; }
        }
        ```
        - Good practice to turn lazy loading off before you serialize an entity (Lazy loading of the Posts collection can be turned off by making the Posts property non-virtual). or turned all for all entities:
        ```c#
        public class BloggingContext : DbContext
        {
            public BloggingContext()
            {
                this.Configuration.LazyLoadingEnabled = false;
            }
        }
        ```
    - Explicit loading: it is possible to lazily load related entities by explicitly loading. To do so you use the **Load method** on the related entity's entry:
        ```c#
        using (var context = new BloggingContext())
        {
            var post = context.Posts.Find(2);

            // Load the blog related to a given post.
            context.Entry(post).Reference(p => p.Blog).Load();

            // Load the blog related to a given post using a string.
            context.Entry(post).Reference("Blog").Load();

            var blog = context.Blogs.Find(1);

            // Load the posts related to a given blog.
            context.Entry(blog).Collection(p => p.Posts).Load();

            // Load the posts related to a given blog
            // using a string to specify the relationship.
            context.Entry(blog).Collection("Posts").Load();
        }
        ```
23. Different types of inheritance supported by EF?
    - Table per Hierarchy (TPH): shows one table per inheritance hierarchy class. 
    - Table per type: Each domain class has its own table. 
    - Table per Concrete class: a single table per concrete class but does not include the abstract class.
24. Complex type in EF? Non-scalar properties of entity types that assist in organizing scalar properties within entities. Complex types have other complex type properties. 
25. Micro ORM? Focus on working with db tables (not work with db schemas, tracking changes, etc).
26. EF Data access arquitecture:
    - Disconnected data access: possible with data adapter object. Datasets work independently of db and the data can be edited.
        - Disconnected scenario is when an entity is retrieved from the database and modified in the different context. Let's suppose we want to display some data in a Presentation Layer and we are using some n-tier application, so it would be better to open the context, fetch the data and finally close the context. Since here we have fetched the data and closed the context, the entities that we have fetched are no longer tracked and this is the disconnected scenario.
        ```c#
        class Program {
            static void Main(string[] args) {

                var student = new Student {
                    ID = 1001, 
                    FirstMidName = "Wasim", 
                    LastName = "Akram", 
                    EnrollmentDate = DateTime.Parse( DateTime.Today.ToString())
                };

                using (var context = new MyContext()) {

                    context.Students.Add(student);
                    context.SaveChanges();

                    //// Display all Students from the database

                    var students = (from s in context.Students 
                        orderby s.FirstMidName select s).ToList<Student>();

                    Console.WriteLine("Retrieve all Students from the database:");

                    foreach (var stdnt in students) {
                        string name = stdnt.FirstMidName + " " + stdnt.LastName;
                        Console.WriteLine("ID: {0}, Name: {1}", stdnt.ID, name);
                    }

                    Console.ReadKey();
                }
            }
        }
        ```
    - Connected data access: A data reader object of a data provider allows you to access linked dta. Data can be accessed quickly but editing is not permited. 
        - Connected scenario is when an entity is retrieved from the database and modified in the same context. For a connected scenario let us suppose we have a Windows service and we are doing some business operations with that entity so we will open the context, loop through all the entities, do our business operations and then save the changes with the same context that we opened in the beginning.
        ```c#
        class Program {
            static void Main(string[] args) {

                using (var context = new MyContext()) {

                    var studentList = context.Students.ToList();

                    foreach (var stdnt in studentList) {
                        stdnt.FirstMidName = "Edited " + stdnt.FirstMidName;
                    }

                    context.SaveChanges();

                    //// Display all Students from the database

                    var students = (from s in context.Students
                        orderby s.FirstMidName select s).ToList<Student>();

                    Console.WriteLine("Retrieve all Students from the database:");

                    foreach (var stdnt in students) {
                        string name = stdnt.FirstMidName + " " + stdnt.LastName;
                        Console.WriteLine("ID: {0}, Name: {1}", stdnt.ID, name);
                    }

                    Console.ReadKey();
                }
            }
        }
        ```
27. How to handle SQL injection attacks in EF? EF by default generates parameterized SQL commants that help prevent injections. Never combine user inputs with Entity SQL commands text. 
28. What is the ObjectSet in EF? Specific type of data set that is commonly used to read, update, create and remove operations from existing entities. You create it using the ObjectContext. 
29. EDM (Entity Data Model)? Entity-relationship prototype that assigns some basic prototypes fo the data using various modeling procedures. A link or connection created between the db and the prototype. Add -> New item -> ADO.Net Entity Data Model template.
    - In-memory representation of metadata including the conceptual model, the storage model and mapping between them. 
30. DbEntityEntry class? helps retrieve a variety of information about an entity. It is useful to set the EntityState (to set it to modified for example):
```c#
using(var context = new MyContext()){
    context.Entry(student).State = System.Data.Entity.EntityState.Modified;
}
```
    - An existing entity in context is represented by DbEntityEntry
31. Data seeding?
    - Populating a database with an initial set of data. 
    - Three types:
        - Model seed data
            - EF core seeding can be associated with an entity type as part of the model configuration. 
            - Add seed data OnModelCreating method using **HasData**:
            ```c#
            modelBuilder.Entity<Post>().HasData(
            new Post { BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1" });
            ```
        - Manual migration customization
            - When a migration is added the changes to the data specified with HasData are transformed to callss to InsertData(), UpdateData(), DeleteData(). Manually add these calls to the migration:
            ```c#
            migrationBuilder.InsertData(
            table: "Blogs",
            columns: new[] { "Url" },
            values: new object[] { "http://generated.com" });
            ```
        - Custom initialization logic
            - Use DbContextSaveChanges() before the main application begins execution:
            ```c#
            using (var context = new DataSeedingContext())
            {
                context.Database.EnsureCreated();

                var testBlog = context.Blogs.FirstOrDefault(b => b.Url == "http://test.com");
                if (testBlog == null)
                {
                    context.Blogs.Add(new Blog { Url = "http://test.com" });
                }

                context.SaveChanges();
            }
            ```
        - The seeding code should not be part of the normal app execution. 
32. Manage migrations? 
    - First install EF Core command-line tools (package manager console).
    - Add a migration (migration name can be used like a commit message):
    ```batch
    dotnet ef migrations add AddBlogCreatedTimestamp
    ```
    - Three files are added to your project under the Migrations directory:
        1. XXXXXXXXXXXXXX_AddCreatedTimestamp.cs: the main migrations file. Contains the operations necessary to apply the migration (Up) and to revert it (Down)
        2. XXXXXXXXXXXXXX_AddCreatedTimestamp.Designer.cs: migrations metadata file. Information used by EF.
        3. MyContextModelSnapshot.cs: a snapshot of your current model. Used to determine what changed when adding the next migration.
    - You can move the migration files and change their namespace manually. You can specify also the directory at generation time:
      ```batch
        dotnet ef migrations add AddBlogCreatedTimestamp --output-dir My/Directory
        ```
    - Customize migration code: 
        - Column renames (renaming a property for example Name to FullName you need to specify the newName otherwise EF Core will drop a column and add a new one for the FullName)
        ```c#
        migrationBuilder.RenameColumn(
            name: "Name",
            table: "Customers",
            newName: "FullName");
        ```
        - If we want to replace FirstName and LastName with a single FullName property you need to add raw SQL (otherwise it will delete FirstName and LastName and create a new FullName column).
        ```c#
        migrationBuilder.AddColumn<string>(
            name: "FullName",
            table: "Customer",
            nullable: true);

        migrationBuilder.Sql(
        @"
            UPDATE Customer
            SET FullName = FirstName + ' ' + LastName;
        ");

        migrationBuilder.DropColumn(
            name: "FirstName",
            table: "Customer");

        migrationBuilder.DropColumn(
            name: "LastName",
            table: "Customer");
        ```
        - Arbitrary changes via raw sql use migrationBuilder.Sql (SPs, Full-Text Search, functions, triggers, views, etc)
        ```c#
        migrationBuilder.Sql(
        @"
            EXEC ('CREATE PROCEDURE getFullName
                @LastName nvarchar(50),
                @FirstName nvarchar(50)
            AS
                RETURN @LastName + @FirstName;')");
        ```