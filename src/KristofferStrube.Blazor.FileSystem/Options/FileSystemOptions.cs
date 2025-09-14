using System.Reflection;

namespace KristofferStrube.Blazor.FileSystem;

/// <summary>
/// Options for what path the JS Helper module will be found at.
/// </summary>
public class FileSystemOptions
{
    /// <summary>
    /// The base path that is used by default.
    /// </summary>
    public const string DefaultBasePath = "./_content/";

    /// <summary>
    /// The namespace that the JS module will be found in by default.
    /// </summary>
    public static readonly string DefaultNamespace = Assembly.GetExecutingAssembly().GetName().Name ?? "KristofferStrube.Blazor.FileSystem";

    /// <summary>
    /// The standard fill path for the JS module.
    /// </summary>
    public static readonly string DefaultScriptPath = $"{DefaultNamespace}/{DefaultNamespace}.js";

    /// <summary>
    /// Option for changing the base path.
    /// </summary>
    public string BasePath { get; set; } = DefaultBasePath;

    /// <summary>
    /// Option for changing the relative path for the JS module.
    /// </summary>
    public string ScriptPath { get; set; } = DefaultScriptPath;

    /// <summary>
    /// The calculated full script path that locates the JS module.
    /// </summary>
    public string FullScriptPath => Path.Combine(BasePath, ScriptPath);

    /// <summary>
    /// A static instance that can be reused in places where no other options are defined.
    /// </summary>
    internal static FileSystemOptions DefaultInstance = new();
}