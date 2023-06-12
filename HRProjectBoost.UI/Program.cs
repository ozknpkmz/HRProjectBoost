using HRProjectBoost.Business.DependencyResolvers;
using HRProjectBoost.DataAccess.Context;
using HRProjectBoost.DataAccess.Extensions;
using HRProjectBoost.Entities.Domains;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//DataAccess katmanı için yapılan extensionlar.
builder.Services.LoadDataLayerExtensions(builder.Configuration);
//Bussiness katmanı için yapılan extensionlar
builder.Services.AddDependencies();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStatusCodePagesWithReExecute("/Home/ErrorPage", "?code={0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapAreaControllerRoute(
  name: "Admin",
  areaName: "Admin",
  pattern: "Admin/{controller=Admin}/{action=Index}/{id?}"
);

app.MapAreaControllerRoute(
  name: "Manager",
  areaName: "Manager",
  pattern: "Manager/{controller=Manager}/{action=Index}/{id?}"
);

app.MapAreaControllerRoute(
  name: "Personnel",
  areaName: "Personnel",
  pattern: "Personnel/{controller=Personnel}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}");

app.Run();
