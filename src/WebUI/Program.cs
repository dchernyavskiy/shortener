using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shortener.Application.Common.Interfaces;
using Shortener.Application.Common.Options;
using Shortener.Infrastructure.Identity;
using Shortener.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions<ApplicationOptions>().BindConfiguration(nameof(ApplicationOptions));
builder.Services.AddSingleton<ApplicationOptions>(sp => sp.GetRequiredService<IOptions<ApplicationOptions>>().Value);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=About}/{action=Index}/{id?}");
app.MapGet("/{code}", async (
    [FromRoute] string code,
    IApplicationDbContext context
) =>
{
    var url = await context.Urls.FirstOrDefaultAsync(x => x.Hash == code);
    return Results.Redirect(url!.BaseUrl);
});

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();