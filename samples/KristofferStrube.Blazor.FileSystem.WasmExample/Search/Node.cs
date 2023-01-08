using MessagePack;

namespace KristofferStrube.Blazor.FileSystem.WasmExample.Search;

[MessagePackObject]
public class Node
{
    [SerializationConstructor]
    public Node(int start, int end, Node?[] children, int? label)
    {
        Start = start;
        End = end;
        foreach (Node? node in children)
        {
            if (node is null) continue;
            node.Parent = this;
        }
        Children = children;
        Label = label;
    }

    public Node(int start, int end, int alphabetSize, Node parent, int? label = null)
    {
        Start = start;
        End = end;
        Label = label;
        Parent = parent;
        Children = new Node[alphabetSize];
    }

    [Key(0)]
    public int Start { get; set; }
    [Key(1)]
    public int End { get; set; }

    [Key(2)]
    public Node?[] Children { get; set; }

    [IgnoreMember]
    public Node? Parent { get; set; }

    [Key(3)]
    public int? Label { get; set; }

    public void SetChild(int startMappedCharacter, Node child)
    {
        Children[startMappedCharacter] = child;
        child.Parent = this;
    }
}