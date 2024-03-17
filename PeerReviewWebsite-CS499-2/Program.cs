using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeerReviewWebsite.Classes.Data;
using PeerReviewWebsite.Classes.Data.Account;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<MyStateContainer>();

// Add connections
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<WebsiteDbContext>(options => options.UseSqlServer(connectionString));

// Add services
builder.Services.AddScoped<AccountService>();

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

AccountService accountService = new(dbContext);

// Add admin account
string adminEmail = "admin";
if (await accountService.GetUserAsync(adminEmail) is null) {
    // Add admin role
    string adminRoleName = "Administrator";
    Role adminRole = await accountService.GetRoleAsync(adminRoleName);
    adminRole ??= await accountService.CreateRoleAsync(new Role {
        Name = adminRoleName,
        Permissions = Permission.All
    });

    User adminAccount = await accountService.CreateUserAsync(new User {
        FirstName = "Admin",
        LastName = "User",
        Email = adminEmail,
        Password = "admin123"
    });

    // Add admin role to admin account
    await accountService.AddRoleToUserAsync(adminAccount, adminRole);
}

app.Run();
