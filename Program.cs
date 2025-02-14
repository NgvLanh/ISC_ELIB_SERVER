using DotNetEnv;
using ISC_ELIB_SERVER.Configurations;
using ISC_ELIB_SERVER.Mappers;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var databaseUrl = Env.GetString("DATABASE_URL");

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); 

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<isc_elibContext>(options =>
{
    options.UseNpgsql(databaseUrl);
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = CustomValidationResponse.GenerateResponse;
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<UserStatusRepo>();
builder.Services.AddScoped<IUserStatusService, UserStatusService>();
//
builder.Services.AddScoped<AcademicYearRepo>();
builder.Services.AddScoped<IAcademicYearService, AcademicYearService>();
//
builder.Services.AddScoped<CampusRepo>();
builder.Services.AddScoped<ICampusService, CampusService>();
//
builder.Services.AddScoped<SchoolRepo>();
builder.Services.AddScoped<ISchoolService, SchoolService>();


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.Register(c =>
    {
        var optionsBuilder = new DbContextOptionsBuilder<isc_elibContext>();
        optionsBuilder.UseNpgsql(databaseUrl);

        return new isc_elibContext(optionsBuilder.Options);
    })
    .AsSelf()
    .InstancePerLifetimeScope();

    containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        .Where(t => t.Name.EndsWith("Service"))
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

    containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        .Where(t => t.Name.EndsWith("Repo"))
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
