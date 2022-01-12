
//builder
using Nacos.AspNetCore.V2;


var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMvc(option => { option.EnableEndpointRouting = false; });

    // nacos server v1.x or v2.x
    builder.Services.AddNacosAspNet(builder.Configuration);

}



//app
var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseCors(builder =>
    //{
    //    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetPreflightMaxAge(TimeSpan.FromSeconds(60));
    //});

    app.UseMvcWithDefaultRoute();
    app.Run();
}