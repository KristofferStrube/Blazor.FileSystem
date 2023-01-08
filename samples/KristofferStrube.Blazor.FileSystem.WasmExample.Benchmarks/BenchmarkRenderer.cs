using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;

namespace KristofferStrube.Blazor.FileSystem.WasmExample.Benchmarks;

/// <remarks>
/// Taken from: https://github.com/egil/Benchmark.Blazor/blob/main/Rendering/BenchmarkRenderer.cs
/// </remarks>
#pragma warning disable BL0006 // Do not use RenderTree types
internal class BenchmarkRenderer : Renderer
{
    private readonly List<int> rootComponentIds = new();

    /// <inheritdoc/>
    public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

    /// <summary>
    /// Any unhandled exceptions thrown by the Blazor Renderer after 
    /// a call to <see cref="RenderAsync{TComponent}(ParameterView)"/> or 
    /// <see cref="SetParametersAsync{TComponent}(TComponent, ParameterView)"/>.    
    /// </summary>
    /// <remarks>
    /// This is reset after each call to <see cref="RenderAsync{TComponent}(ParameterView)"/> or
    /// <see cref="SetParametersAsync{TComponent}(TComponent, ParameterView)"/>.
    /// </remarks>
    public Exception? UnhandledException { get; private set; }

    /// <summary>
    /// The number of times the render tree has been undated, e.g. 
    /// when a render cycle has happened (the <see cref="UpdateDisplayAsync(in RenderBatch)"/>)
    /// has been called.
    /// </summary>
    public int RenderCount { get; private set; }

    /// <summary>
    /// Create an instance of the <see cref="BenchmarkRenderer"/>. This is a minimal 
    /// <see cref="Renderer"/> that just allows the rendering of a component.
    /// </summary>
    /// <param name="serviceProvider">
    /// An optional <see cref="IServiceProvider"/> that can be used to inject 
    /// services into the component being rendered. 
    /// Default is an empty <see cref="ServiceProvider"/>.
    /// </param>
    /// <param name="loggerFactory">
    /// An optional <see cref="ILoggerFactory"/> that will collect logs from the <see cref="Renderer"/>.
    /// Default is <see cref="NullLoggerFactory"/>.
    /// </param>
    public BenchmarkRenderer(IServiceProvider? serviceProvider = null, ILoggerFactory? loggerFactory = null)
        : base(serviceProvider ?? new ServiceCollection().BuildServiceProvider(), loggerFactory ?? NullLoggerFactory.Instance)
    {
    }

    /// <summary>
    /// Renders a component of type <typeparamref name="TComponent"/> with the
    /// provided <paramref name="parameters"/>.    
    /// </summary>
    /// <remarks>
    /// Any unhandled exceptions during the first render is captured in the
    /// <see cref="UnhandledException"/> property.
    /// </remarks>
    /// <typeparam name="TComponent">The type of the component to render.</typeparam>
    /// <param name="parameters">
    /// The parameters to pass to the <typeparamref name="TComponent"/> during 
    /// first render. Use <see cref="ParameterView.Empty"/> to pass no parameters.</param>
    /// <returns>The instance of the <typeparamref name="TComponent"/>.</returns>
    public Task<RenderedComponent<TComponent>> RenderAsync<TComponent>(ParameterView parameters)
        where TComponent : IComponent
    {
        UnhandledException = null;

        var result = Dispatcher.InvokeAsync(async () =>
        {
            var component = (TComponent)InstantiateComponent(typeof(TComponent));
            var componentId = AssignRootComponentId(component);
            rootComponentIds.Add(componentId);
            await RenderRootComponentAsync(componentId, parameters).ConfigureAwait(false);
            return new RenderedComponent<TComponent>(component, componentId, this);
        });

        return result;
    }

    /// <summary>
    /// Remove all component from the render tree(s) and clean up resources 
    /// allocated to them in the <see cref="BenchmarkRenderer"/>. 
    /// 
    /// This also calls the <see cref="IDisposable.Dispose"/> and/or 
    /// <see cref="IAsyncDisposable.DisposeAsync"/> methods, if it 
    /// components implements either of those interfaces.
    /// </summary>
    public void RemoveComponents()
    {
        Dispatcher.InvokeAsync(() =>
        {
            foreach (var componentId in rootComponentIds)
            {
                RemoveRootComponent(componentId);
            }
        });

        rootComponentIds.Clear();
    }

    internal void RemoveComponent(int componentId)
    {
        Dispatcher.InvokeAsync(() => RemoveRootComponent(componentId));
    }

    protected override void HandleException(Exception exception)
        => UnhandledException = exception;

    protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
    {
        // This is called after every render and
        // contains the changes/updates to the render tree.
        RenderCount++;
        return Task.CompletedTask;
    }
}
#pragma warning restore BL0006 // Do not use RenderTree types