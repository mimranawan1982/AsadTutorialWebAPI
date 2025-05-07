using System.Net;
using System.Text;
using AsadTutorialWebAPI.Data;
using AsadTutorialWebAPI.Data.Repo;
using AsadTutorialWebAPI.Helpers;
using AsadTutorialWebAPI.Interfaces;
using AsadTutorialWebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register DbContext with connection string
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Add the unit of work Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkRepository>();

// Add Auto Mapper 
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Add NewtonSoftJson
//builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
//                        .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
//                        .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddControllers().AddNewtonsoftJson();

// Add JWT Bearer Authenticate

// Change the prefix of environment variable e-g HSPA_AppSettings__Keys
builder.Configuration.AddEnvironmentVariables(prefix: "HSPA_");

var secretKey1 = builder.Configuration["JWT:Key1"].ToString();
var secretKey2 = builder.Configuration.GetSection("JWT:Key1").Value;
var secretKey3 = builder.Configuration.GetSection("AppSettings:Key1").Value;   // Get value from secrets.json
var secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;    //Get value from environment variable
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = key
        };
    });
        

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
  //  app.UseDeveloperExceptionPage();
}

//else
//{
//app.UseExceptionHandler(options =>
//{
//    options.Run(
//        async context =>
//        {
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            var ex = context.Features.Get<IExceptionHandlerFeature>();
//            if (ex != null)
//            {
//                await context.Response.WriteAsync(ex.Error.Message);
//            }
//        }
//        );

//});
//}
//app.UseExceptionHandler( _ => { });

app.UseMiddleware<ExceptionsMiddleware>();

// these 2 below methods tell us that only https command will be executed and convert it from http to https
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
