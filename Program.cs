using System.Runtime.CompilerServices;
using TodoApi;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString=builder.Configuration.GetConnectionString("ToDoDB");

//DI
builder.Services.AddDbContext<ToDoDbContext>(x =>
x.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql")));

//CROS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:3000");
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();

                      });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);


app.MapGet("/", () => "Hello World!");

app.MapGet("/toDo", (ToDoDbContext toDoDbContext) => toDoDbContext.getAllDoings());
app.MapPost("/toDo",(Item item,ToDoDbContext toDoDbContext) =>toDoDbContext.postDoing(item) );
app.MapDelete("/toDo/{id}",(int id,ToDoDbContext toDoDbContext) => toDoDbContext.deleteDoing(id));
app.MapPut("toDo/{id}",(int id,Item item,ToDoDbContext toDoDbContext) => toDoDbContext.putDoing(id,item));
app.Run();
