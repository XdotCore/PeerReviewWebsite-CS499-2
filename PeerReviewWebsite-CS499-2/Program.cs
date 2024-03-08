using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeerReviewWebsite.Classes.Data;
using PeerReviewWebsite.Classes.Data.Login;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<MyStateContainer>();

// Add connections
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<WebsiteDbContext>(options => options.UseSqlServer(connectionString));

// Add services
builder.Services.AddScoped<UserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Ensure database is created
// TODO: Possibly move to migrations when coming to release
using IServiceScope scope = app.Services.CreateScope();
WebsiteDbContext dbContext = scope.ServiceProvider.GetRequiredService<WebsiteDbContext>();
#if DEBUG
dbContext.Database.EnsureDeleted();
#endif
dbContext.Database.EnsureCreated();

app.Run();
