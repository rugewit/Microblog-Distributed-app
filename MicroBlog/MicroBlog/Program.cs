using MicroBlog.WebAppConfigs;

var builder = WebApplication.CreateBuilder(args);

BuilderSetUp.SetUp(builder);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/",  () => "Hello, MicroBlog!");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
