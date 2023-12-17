using BlazorAppWithRcl.Client.Pages;
using BlazorAppWithRcl.Components;
using Core;
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

var assemblies = GetAssembliesWithModule<IModule>();

//app.MapRazorComponents<App>()
//    .AddInteractiveServerRenderMode()
//    .AddInteractiveWebAssemblyRenderMode()
//    .AddAdditionalAssemblies(typeof(Counter).Assembly)
//    .AddAdditionalAssemblies(typeof(Test).Assembly);


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(assemblies);

app.Run();


static Assembly[] GetAssembliesWithModule<T>() where T : IModule
{
    var assembliesWithModule = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass)
        .Select(type =>
        {
            // Check of de klasse een default constructor heeft
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                var moduleInstance = (T)Activator.CreateInstance(type);
                return moduleInstance.GetModuleAssembly();
            }
            return null;
        })
        .Where(assembly => assembly != null)
        .ToArray();

    return assembliesWithModule;
}
