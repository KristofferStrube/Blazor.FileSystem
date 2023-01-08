using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem.WasmExample.Search;

public class Node
{
    [JsonConstructor]
    public Node(Node[] children)
    {
        foreach(Node node in children)
        {
            if (node is null) continue;
            node.Parent = this;
        }
        Children = children;
    }

    public Node(int start, int end, int alphabetSize, Node parent, int? label = null)
    {
        Start = start;
        End = end;
        Label = label;
        Parent = parent;
        Children = new Node[alphabetSize];
    }

    public int Start { get; set; }
    public int End { get; set; }

    public Node?[] Children { get; set; }

    [JsonIgnore]
    public Node? Parent { get; set; }

    public int? Label { get; set; }

    public void SetChild(int startMappedCharacter, Node child)
    {
        Children[startMappedCharacter] = child;
        child.Parent = this;
    }
}