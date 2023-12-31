using System.Data.SqlClient;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin());
app.MapGet("/podcasts", async ()=> {

    var db = new SqlConnection("Server=tcp:localhost; Initial Catalog=podcasts;Persist Security Info=False;User ID=sa;Password=DiegoGH#123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;");
    
    return (await db.QueryAsync<Podcast>("SELECT * FROM Podcasts")).Select(x => x.Title);  
    // return new List<string>{
    //     "Docker Deep Dive Podcast",
    //     "Unhandled Exception Podcast",
    //     "The .Net Rocks Podcast",
    //     "The Stack Overflow Podcast"
    // }
});

app.Run();

record Podcast(Guid Id, string Title);
