using BenchmarkDotNet.Attributes;
using KristofferStrube.ActivityStreams;
using KristofferStrube.Blazor.FileSystem.WasmExample.Search;
using KristofferStrube.Blazor.FileSystem.WasmExample.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using static System.Text.Json.JsonSerializer;

namespace KristofferStrube.Blazor.FileSystem.WasmExample.Benchmarks.Benchmarks;

public class NodePresenterBenchmark
{
    private BenchmarkRenderer renderer = default!;

    private Note videoNote;
    private Note imageNote;
    private Note onlyContentNote;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var services = new ServiceCollection();

        // Add services to inject into components
        // rendered with the renderer here, e.g.:
        // services.AddSingleton<IFoo, Foo>();

        renderer = new BenchmarkRenderer(services.BuildServiceProvider());

        imageNote = Deserialize<Note>(JsonConstants.imageNote)!;
        videoNote = Deserialize<Note>(JsonConstants.videoNote)!;
        onlyContentNote = Deserialize<Note>(JsonConstants.onlyContentNote)!;
    }

    [GlobalCleanup]
#pragma warning disable BL0006 // Do not use RenderTree types
    public void GlobalCleanup() => renderer.Dispose();
#pragma warning restore BL0006 // Do not use RenderTree types

    [Benchmark]
    public async Task<int> RenderImageNote()
    {
        // Render a component
        var parameters = new Dictionary<string, object?>() { { "Note", imageNote } };
        var component = await renderer.RenderAsync<NotePresenter>(ParameterView.FromDictionary(parameters));

        // Remove the component again from the render tree. Stops the
        // renderer from tracking the component.
        component.RemoveComponent();

        return renderer.RenderCount;
    }

    [Benchmark]
    public async Task<int> RenderVideoNote()
    {
        // Render a component
        var parameters = new Dictionary<string, object?>() { { "Note", videoNote } };
        var component = await renderer.RenderAsync<NotePresenter>(ParameterView.FromDictionary(parameters));

        // Remove the component again from the render tree. Stops the
        // renderer from tracking the component.
        component.RemoveComponent();

        return renderer.RenderCount;
    }

    [Benchmark]
    public async Task<int> RenderOnlyContentNote()
    {
        // Render a component
        var parameters = new Dictionary<string, object?>() { { "Note", onlyContentNote } };
        var component = await renderer.RenderAsync<NotePresenter>(ParameterView.FromDictionary(parameters));

        // Remove the component again from the render tree. Stops the
        // renderer from tracking the component.
        component.RemoveComponent();

        return renderer.RenderCount;
    }
}

