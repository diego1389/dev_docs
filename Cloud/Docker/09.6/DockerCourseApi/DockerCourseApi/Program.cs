using System.Data.SqlClient;
using Dapper;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>{
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>{
        policy.WithOrigins("http://localhost:1234");
    });
});


var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/podcasts", async () =>
{

    var db = new SqlConnection("Server=tcp:database; Initial Catalog=podcasts;Persist Security Info=False;User ID=sa;Password=Dometrain#123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;");   
    return (await db.QueryAsync<Podcast>("SELECT * FROM Podcasts")).Select(x => x.Title);

    // return new List<string>
    // {
    //     "Unhandled Exception Podcast",
    //     "Developer Weekly Podcast",
    //     "The Stack Overflow Podcast",
    //     "The Hanselminutes Podcast",
    //     "The .NET Rocks Podcast",
    //     "The Azure Podcast",
    //     "The AWS Podcast",
    //     "The Rabbit Hole Podcast",
    //     "The .NET Core Podcast",
    // };
});

app.Run();

record Podcast(Guid Id, string Title);
