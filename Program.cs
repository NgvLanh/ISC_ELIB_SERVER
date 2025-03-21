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
using Autofac.Core;
using CloudinaryDotNet;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BTBackendOnline2.Models;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var databaseUrl = Env.GetString("DATABASE_URL");

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000") // Cho phép React truy cập API
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var jwtSettings = new TokenRequiment
{
    SecretKey = Env.GetString("JWT_SECRET_KEY"),
    Issuer = Env.GetString("JWT_ISSUER"),
    Audience = Env.GetString("JWT_AUDIENCE"),
    Subject = Env.GetString("JWT_SUBJECT")
};


var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidAudience = jwtSettings.Audience,
                        ValidIssuer = jwtSettings.Issuer,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.Headers.Add("Token-Validation-Error", context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            if (!context.Response.Headers.ContainsKey("Token-Validation-Error"))
                            {
                                context.Response.Headers.Add("Token-Validation-Error", context.ErrorDescription);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter AccessToken",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization();

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

//Tests
builder.Services.AddScoped<TestRepo>();
builder.Services.AddScoped<ITestService, TestService>();

//Test-Question
builder.Services.AddScoped<TestQuestionRepo>();
builder.Services.AddScoped<ITestQuestionService, TestQuestionService>();
//Test-submisstion
builder.Services.AddScoped<TestsSubmissionRepo>();
builder.Services.AddScoped<ITestsSubmissionService, TestsSubmissionService>();

//Test-Answer
builder.Services.AddScoped<TestAnswerRepo>(); 
builder.Services.AddScoped<TestAnswerService>(); 

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
//builder.Services.AddScoped<IQuestionImagesQaService>();
builder.Services.AddScoped<AnswerImagesQaRepo>();
//builder.Services.AddScoped<IAnswerImagesQaService>();

builder.Services.AddScoped<IQuestionImagesQaService, QuestionImagesQaService>();
builder.Services.AddScoped<AnswerImagesQaRepo>();
builder.Services.AddScoped<IAnswerImagesQaService, AnswerImagesQaService>();
builder.Services.AddScoped<IQuestionQaService, QuestionQaService>();
builder.Services.AddScoped<QuestionViewRepo>();
// Đăng ký QuestionView Repository và Service
builder.Services.AddScoped<QuestionViewRepo>();
builder.Services.AddScoped<IQuestionViewService, QuestionViewService>();




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
builder.Services.AddScoped<IThemesService, ThemesService>();
builder.Services.AddScoped<NotificationRepo>();
builder.Services.AddScoped<INotificationService, INotificationService>();
builder.Services.AddScoped<MajorRepo>();
builder.Services.AddScoped<IMajorService, MajorService>();
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

//WorkProcess
builder.Services.AddScoped<WorkProcessRepo>();
builder.Services.AddScoped<IWorkProcessService, WorkProcessService>();

//Resignation
builder.Services.AddScoped<ResignationRepo>();
builder.Services.AddScoped<IResignationService, ResignationService>();

//Authentication
builder.Services.AddScoped<ILoginService, AuthService>();

builder.Services.AddScoped<TopicRepo>();
builder.Services.AddScoped<TopicsFileRepo>();
builder.Services.AddAutoMapper(typeof(SessionMapper));
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<SessionRepo>();

//class
builder.Services.AddScoped<ClassRepo>();
builder.Services.AddScoped<IClassesService, ClassesService>();

//Retirement
builder.Services.AddScoped<RetirementRepo>();
builder.Services.AddScoped<IRetirementService, RetirementService>();

//TeacherList
builder.Services.AddScoped<TeacherListRepo>();
builder.Services.AddScoped<ITeacherListService, TeacherListService>();

//EntryType
builder.Services.AddScoped<EntryTypeRepo>();
builder.Services.AddScoped<IEntryTypeService, EntryTypeService>();

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
builder.Services.AddSwaggerGen(c =>
{
    // Map RetirementStatus enum thành kiểu int
    c.MapType<RetirementStatus>(() => new OpenApiSchema
    {
        Type = "integer",
        Format = "int32"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
