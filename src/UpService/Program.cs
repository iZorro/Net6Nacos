
//builder
using Nacos.AspNetCore.V2;
using Nacos.V2.Config;
using Nacos.V2.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMvc(option => { option.EnableEndpointRouting = false; });

    builder.Services.AddNacosAspNet(builder.Configuration);

    //用于管理Nacos的配置的信息
    builder.Services.AddNacosV2Config(x =>
    {
        x.ServerAddresses = new System.Collections.Generic.List<string> { "http://localhost:8848/" };
        x.EndPoint = "";
        x.Namespace = "public";
        x.UserName = "nacos";
        x.Password = "nacos";

        // this sample will add the filter to encrypt the config with AES.
        x.ConfigFilterAssemblies = new System.Collections.Generic.List<string> { "BaseApi" };

        // swich to use http or rpc
        x.ConfigUseRpc = false;
    });

    //用于添加临时服务
    builder.Services.AddNacosV2Naming(x =>
    {
        x.ServerAddresses = new System.Collections.Generic.List<string> { "http://localhost:8848/" };
        x.EndPoint = "";
        x.Namespace = "public";

        // swich to use http or rpc
        x.NamingUseRpc = false;
    });
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

    app.UseMvcWithDefaultRoute();
    app.Run();
}