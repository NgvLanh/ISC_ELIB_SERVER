﻿using DotNetEnv;
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
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var databaseUrl = Env.GetString("DATABASE_URL");

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
  

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = 64; // Đặt MaxDepth
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

//Khai bao task cua Nam :>>
builder.Services.AddScoped<IUserStatusService, UserStatusService>();
builder.Services.AddScoped<AnswersQaRepo>();  
builder.Services.AddScoped<IAnswersQaService>();
builder.Services.AddScoped<QuestionImagesQaRepo>();
builder.Services.AddScoped<IQuestionImagesQaService>();
builder.Services.AddScoped<AnswerImagesQaRepo>();
builder.Services.AddScoped<IAnswerImagesQaService>();


// Add services and repositories Test attachment
builder.Services.AddScoped<TestsAttachmentRepo>();
builder.Services.AddScoped<ITestsAttachmentService, TestsAttachmentService>();

// Add services and repositories Test Submission Answer
builder.Services.AddScoped<TestSubmissionAnswerRepo>();
builder.Services.AddScoped<ITestSubmissionAnswerService, TestSubmissionAnswerService>();

// Add services and repositories Test attachment
builder.Services.AddScoped<ExamRepo>();
builder.Services.AddScoped<IExamService, ExamService>();

// Add services and repositories Test Answer
builder.Services.AddScoped<TestAnswerRepo>();

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
    containerBuilder.RegisterType<QuestionQaRepo>().As<QuestionQaRepo>().InstancePerLifetimeScope();
    containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
    .Where(t => t.Name.EndsWith("Service"))
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
