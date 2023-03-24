using AdministrationSystem.Eamv.Models;
using AdministrationSystem.Eamv.Models.EntityFramework;
using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

string DatabaseLocation = "UserDB";
string MainDBLocation = "MainDB";
string FeedbackDBLocation = "FeedbackDB";


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<UserDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration[$"ConnectionStrings:{DatabaseLocation}"]);
});

builder.Services.AddDbContext<MainDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration[$"ConnectionStrings:{MainDBLocation}"]);
});

builder.Services.AddDbContext<FeedbackDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration[$"ConnectionStrings:{FeedbackDBLocation}"]);
});

builder.Services.AddAuthentication("EamvCookie").AddCookie("EamvCookie", options =>
{
    options.Cookie.Name = "EamvCookie";
    options.LoginPath = "/Login/Index";
    options.AccessDeniedPath = "/Home/Accessdenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StandardUser", policy => policy.RequireClaim("StandardUser"));
    options.AddPolicy("AdminUser", policy => policy.RequireClaim("AdminUser"));
});

builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IRepositoryCrud<Department>, EFDepartmentRepository>();
builder.Services.AddScoped<IRepositoryCrud<Room>, EFRoomRepository>();
builder.Services.AddScoped<IFeedbackRepository, EFFeedbackRepository>();
builder.Services.AddScoped<IActiveRepository, EFActivityRepository>();
builder.Services.AddScoped<IBannerRepository, EFBannerRepository>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Activity}/{action=CreateActivity}/{id?}");

app.Run();
