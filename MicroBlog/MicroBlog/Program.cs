using MicroBlog.DataHandling;
using MicroBlog.Models;
using MicroBlog.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// add mongo db
builder.Services.Configure<MicroBlogDatabaseSettings>(
    builder.Configuration.GetSection("MicroBlogDatabase"));
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<UserAccountsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/load-users", async (UserAccountsService userAccountsService) =>
{
    await DatasetLoader.LoadUsers(userAccountsService);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();