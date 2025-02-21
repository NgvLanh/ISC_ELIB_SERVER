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
using ISC_ELIB_SERVER.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var databaseUrl = Env.GetString("DATABASE_URL");


builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = 64; // Đặt MaxDepth
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<isc_dbContext>(options =>
{
    options.UseNpgsql(databaseUrl);
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = CustomValidationResponse.GenerateResponse;
});

// builder.Services.Configure<JsonOptions>(options =>
// {
//     options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
// });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Đăng ký Repository và Service

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

builder.Services.AddScoped<AnswersQaRepo>();
builder.Services.AddScoped<IAnswersQaService, AnswersQaService>();
builder.Services.AddScoped<QuestionImagesQaRepo>();
builder.Services.AddScoped<IQuestionImagesQaService, QuestionImagesQaService>();
builder.Services.AddScoped<AnswerImagesQaRepo>();
builder.Services.AddScoped<IAnswerImagesQaService, AnswerImagesQaService>();
builder.Services.AddScoped<IQuestionQaService, QuestionQaService>();


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

builder.Services.AddScoped<ThemesRepo>();
builder.Services.AddScoped<IThemesService, IThemesService>();
builder.Services.AddScoped<MajorRepo>();
builder.Services.AddScoped<IMajorService, IMajorService>();
builder.Services.AddScoped<TrainingProgramsRepo>();
builder.Services.AddScoped<ITrainingProgramService, ITrainingProgramService>();

//
builder.Services.AddScoped<AcademicYearRepo>();
builder.Services.AddScoped<IAcademicYearService, AcademicYearService>();
//
builder.Services.AddScoped<CampusRepo>();
builder.Services.AddScoped<ICampusService, CampusService>();
//
builder.Services.AddScoped<SchoolRepo>();
builder.Services.AddScoped<ISchoolService, SchoolService>();


//User
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();

//TeacherInfo
builder.Services.AddScoped<TeacherInfoRepo>();
builder.Services.AddScoped<ITeacherInfoService, TeacherInfoService>();


//StudentInfo
builder.Services.AddScoped<StudentInfoRepo>();
builder.Services.AddScoped<IStudentInfoService, StudentInfoService>();

=======
//Role
builder.Services.AddScoped<RoleRepo>();
builder.Services.AddScoped<IRoleService, RoleService>();

//Permisson
builder.Services.AddScoped<PermissionRepo>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

//Role_Permission
builder.Services.AddScoped<RolePermissionRepo>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();




//Semester
builder.Services.AddScoped<SemesterRepo>();
builder.Services.AddScoped<ISemesterService, SemesterService>();

//GradeLevel
builder.Services.AddScoped<GradeLevelRepo>();
builder.Services.AddScoped<IGradeLevelService, GradeLevelService>();

//EducationLevel
builder.Services.AddScoped<EducationLevelRepo>();
builder.Services.AddScoped<IEducationLevelService, EducationLevelService>();






>>>>>>> dev
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.Register(c =>
    {
        var optionsBuilder = new DbContextOptionsBuilder<isc_dbContext>();
        optionsBuilder.UseNpgsql(databaseUrl);

        return new isc_dbContext(optionsBuilder.Options);
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
