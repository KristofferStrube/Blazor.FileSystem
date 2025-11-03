using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KristofferStrube.Blazor.FileSystem;
using KristofferStrube.Blazor.FileSystem.WasmExample;
using KristofferStrube.Blazor.FileAPI;
using TG.Blazor.IndexedDB;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddStorageManagerServiceInProcess();

builder.Services.AddURLServiceInProcess();

// Adding and configuring IndexedDB used for the IndexedDB sample.
builder.Services.AddIndexedDB(dbStore =>
{
    dbStore.DbName = "FileSystem";
    dbStore.Version = 1;

    dbStore.Stores.Add(new StoreSchema
    {
        Name = "FileReferences"
    });
});

await builder.Build().RunAsync();
