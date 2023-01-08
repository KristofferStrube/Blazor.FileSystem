using Microsoft.AspNetCore.Components;

namespace KristofferStrube.Blazor.FileSystem.WasmExample.Benchmarks;

internal class RenderedComponent<TComponent> where TComponent : IComponent
{
    private readonly int componentId;
    private readonly BenchmarkRenderer renderer;

    public TComponent Instance { get; }

    internal RenderedComponent(TComponent instance, int componentId, BenchmarkRenderer renderer)
    {
        Instance = instance;
        this.componentId = componentId;
        this.renderer = renderer;
    }

    /// <summary>
    /// Pass the provided <paramref name="parameters"/> to the component.
    /// For normal components that inherit from <see cref="ComponentBase"/> this causes
    /// the component to go through all its life cycle methods.
    /// </summary>
    /// <param name="parameters">The parameters to pass to the component. Use <see cref="ParameterView.Empty"/> to pass no parameters and just trigger a re-render.</param>
    /// <returns>A task that completes when the <see cref="IComponent.SetParametersAsync(ParameterView)"/>
    /// method completes.</returns>
    public Task SetParametersAsync(ParameterView parameters)
        => renderer.Dispatcher.InvokeAsync(() =>
        {
            return Instance.SetParametersAsync(parameters).ConfigureAwait(false);
        });

    /// <summary>
    /// Remove the component from the render tree and clean up resources 
    /// allocated to it in the <see cref="BenchmarkRenderer"/>. This 
    /// also calls the <see cref="IDisposable.Dispose"/> and/or 
    /// <see cref="IAsyncDisposable.DisposeAsync"/> methods, if it 
    /// <see cref="TComponent"/> implements either of those interfaces.
    /// </summary>
    public void RemoveComponent()
    {
        renderer.RemoveComponent(componentId);
    }
}