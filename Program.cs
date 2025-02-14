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

//Đăng ký Repository và Service

builder.Services.AddScoped<UserStatusRepo>();
builder.Services.AddScoped<IUserStatusService, UserStatusService>();

//User
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();

//TeacherInfo
builder.Services.AddScoped<TeacherInfoRepo>();
builder.Services.AddScoped<ITeacherInfoService, TeacherInfoService>();

//Role
builder.Services.AddScoped<RoleRepo>();
builder.Services.AddScoped<IRoleService, RoleService>();

//Permisson
builder.Services.AddScoped<PermissionRepo>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

//Role_Permission
builder.Services.AddScoped<RolePermissionRepo>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();

//Temporary
builder.Services.AddScoped<TemporaryLeaveRepo>();
builder.Services.AddScoped<ITemporaryLeaveService, TemporaryLeaveService>();

//Change_Class
builder.Services.AddScoped<ChangeClassRepo>();
builder.Services.AddScoped<IChangeClassService, ChangeClassService>();

//Exemption
builder.Services.AddScoped<ExemptionRepo>();
builder.Services.AddScoped<IExemptionService, ExemptionService>();

//Transfer_School
builder.Services.AddScoped<TransferSchoolRepo>();
builder.Services.AddScoped<ITransferSchoolService, TransferSchoolService>();


// Student_Info
builder.Services.AddScoped<StudentInfoRepo>();
builder.Services.AddScoped<IStudentInfoService, StudentInfoService>();

//WorkProcessRepo
builder.Services.AddScoped<WorkProcessRepo>();
builder.Services.AddScoped<IWorkProcessService, WorkProcessService>();

//RetirementReppo
builder.Services.AddScoped<RetirementRepo>();
builder.Services.AddScoped<IRetirementService, RetirementService>();

//Resignation
builder.Services.AddScoped<ResignationRepo>();
builder.Services.AddScoped<IResignationService, ResignationService>();

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
