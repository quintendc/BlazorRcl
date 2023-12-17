using BlazorAppWithRcl.Client.Pages;
using BlazorAppWithRcl.Components;
using BlazorAppWithRcl.Helpers;
using Core;
using Microsoft.AspNetCore.HttpLogging;
using Module1;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
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

// check https://github.com/dotnet/aspnetcore/issues/48767, to fix issue
// https://github.com/treefishuk/ModularBlazor/blob/master/src/ModularBlazor.App/Startup.cs

AssemblyScanner assemblyScanner = new AssemblyScanner();
List<Assembly> relevantAssemblies = assemblyScanner.GetAssembliesWithModule();

//app.MapRazorComponents<App>()
//    .AddInteractiveServerRenderMode()
//    .AddInteractiveWebAssemblyRenderMode()
//    .AddAdditionalAssemblies(typeof(Counter).Assembly)
//    .AddAdditionalAssemblies(typeof(Test).Assembly);


// probably possible to add Module to .client project too... for auto discover
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly)
    .AddAdditionalAssemblies(relevantAssemblies.ToArray());


// og code:
//app.MapRazorComponents<App>()
//    .AddInteractiveServerRenderMode()
//    .AddInteractiveWebAssemblyRenderMode()
//    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();
