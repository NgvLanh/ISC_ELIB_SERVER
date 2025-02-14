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

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var databaseUrl = Env.GetString("DATABASE_URL");

builder.Services.AddControllers();
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
builder.Services.AddScoped<TestRepo>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<TestQuestionRepo>();
builder.Services.AddScoped<ITestQuestionService, TestQuestionService>();
builder.Services.AddScoped<SubjectTypeRepo>();
builder.Services.AddScoped<ISubjectTypeService, SubjectTypeService>();
builder.Services.AddScoped<SubjectGroupRepo>();
builder.Services.AddScoped<ISubjectGroupService, SubjectGroupService>();
builder.Services.AddScoped<SubjectRepo>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ExamGraderRepo>();
builder.Services.AddScoped<IExamGraderService, ExamGraderService>();
builder.Services.AddScoped<ExamScheduleRepo>();
builder.Services.AddScoped<IExamScheduleService, ExamScheduleService>();
builder.Services.AddScoped<ExamScheduleClassRepo>();
builder.Services.AddScoped<IExamScheduleClassService, ExamScheduleClassService>();
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
