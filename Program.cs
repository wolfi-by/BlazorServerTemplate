﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using BlazorServerTemplate.Components;
using BlazorServerTemplate.Components.Account;
using BlazorServerTemplate.Data;
using MudExtensions.Services;
using Serilog;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Toolbelt.Blazor.I18nText;
using System.Globalization;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt",
                              rollingInterval: RollingInterval.Day)
                .CreateLogger();

builder.Services.AddSerilog();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddMudExtensions();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddFileSystemAccessService();

builder.Services.AddI18nText(options =>
{
    options.PersistenceLevel = PersistanceLevel.Cookie;
    options.GetInitialLanguageAsync = (_, _) => ValueTask.FromResult(CultureInfo.CurrentUICulture.Name);
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "de-DE" };
    var localizationOptions = new RequestLocalizationOptions()
        .SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "i18ntext";
});
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
    {
        var databaseExisted = context.Database.GetAppliedMigrations().Any();
        //context.Database.EnsureCreated();
        context.Database.Migrate();

        if (!databaseExisted)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new() { Name = "Administrator" });

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            ApplicationUser adminUser = new()
            {
                Email = "admin",
                UserName = "admin",
                EmailConfirmed = true,
                                PasswordHash="admin"
            };

            var result= await userManager.CreateAsync(adminUser, adminUser.PasswordHash);
            await userManager.AddToRoleAsync(adminUser, "Administrators");
        }

    }
}

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (localizationOptions != null)
{
    app.UseRequestLocalization(localizationOptions.Value);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapGet("Culture/Set", (HttpRequest request, [FromQuery] string culture, [FromQuery] string redirectUri) =>
{
    if (culture != null)
    {
        request.HttpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
             new RequestCulture(culture, culture)),
              new CookieOptions
              {
                  Path = "/",
                  Expires = DateTime.Now.AddYears(1)
              });
    }

    return Results.LocalRedirect(redirectUri);
});

await app.RunAsync();
