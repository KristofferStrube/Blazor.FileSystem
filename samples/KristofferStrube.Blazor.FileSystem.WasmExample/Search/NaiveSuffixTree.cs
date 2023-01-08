using MessagePack;
using System.Text;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileSystem.WasmExample.Search;

/// <remarks>
/// This is taken from my groups project in the course Genome Scale Algorithms at Department of Bioinformatics at Aarhus University
/// </remarks>
[MessagePackObject]
public class NaiveSuffixTree
{
    [Key(0)]
    public Node Root { get; set; }
    [Key(1)]
    public int AlphabetSize { get; set; }

    // These are the alphabet.
    [Key(2)]
    public Dictionary<char, int> CharToIndex { get; set; }
    [Key(3)]
    public char[] IndexToChar { get; set; }

    [Key(4)]
    // Mapped Content
    public int[] MappedSequence { get; set; }

    public NaiveSuffixTree()
    {

    }

    public NaiveSuffixTree(string input)
    {
        (CharToIndex, IndexToChar) = input.CreateAlphabet();
        MappedSequence = input.MapWithAlphabet(CharToIndex);
        AlphabetSize = IndexToChar.Length;

        Root = new Node(0, 0, AlphabetSize, null);
        int n = MappedSequence.Length;

        for (int i = 0; i < n; i++)
        {
            int from = i;
            Node node = Root;
            while (from < n)
            {
                Node newNode = null;
                if (node.Children[MappedSequence[from]] == null)
                {
                    //Create new child node
                    newNode = new Node(from, n, AlphabetSize, node, i);
                    node.SetChild(MappedSequence[from], newNode);
                    from = newNode.End;
                }
                else
                {
                    //Search for edge match length, if completely matching go to next node, otherwise create new split node
                    Node child = node.Children[MappedSequence[from]];
                    for (int j = 1; j < child.End - child.Start; j++)
                    {
                        if (MappedSequence[child.Start + j] != MappedSequence[from + j])
                        {
                            newNode = InsertNodeOnEdge(node, child, j, from, MappedSequence, i);
                            from = newNode.End;
                            break;
                        }
                    }
                    if (newNode == null)
                    {
                        newNode = child;
                        from += newNode.End - newNode.Start;
                    }
                }

                node = newNode;

            }
        }
    }

    public NaiveSuffixTree(int[] sa, int[] lcp, string input)
    {
        (CharToIndex, IndexToChar) = input.CreateAlphabet();
        MappedSequence = input.MapWithAlphabet(CharToIndex);
        AlphabetSize = IndexToChar.Length;
        Root = new Node(0, 0, AlphabetSize, null);

        Node n = new Node(MappedSequence.Length - 1, MappedSequence.Length, AlphabetSize, Root, sa[0]);
        Root.Children[0] = n;

        int depth = 1;

        Stack<Node> stack = new();
        stack.Push(Root);
        stack.Push(n);

        Node curr = n;

        for (int i = 1; i < MappedSequence.Length; i++)
        {

            while (depth - (curr.End - curr.Start) >= lcp[i] && (curr.End - curr.Start) != 0)
            {
                depth -= (curr.End - curr.Start);
                stack.Pop();
                curr = stack.Peek();
            }
            if (depth == lcp[i])
            {
                Node newNode = new Node(sa[i] + lcp[i], MappedSequence.Length, AlphabetSize, curr, sa[i]);
                curr.Children[MappedSequence[newNode.Start]] = newNode;
                curr = newNode;
                depth += (curr.End - curr.Start);
                stack.Push(curr);
            }
            else
            {
                int splitnodeEnd = curr.Start + lcp[i] - (depth - (curr.End - curr.Start));
                Node splitNode = new Node(curr.Start, splitnodeEnd, AlphabetSize, curr.Parent);
                curr.Parent.Children[MappedSequence[splitNode.Start]] = splitNode;
                curr.Start = splitnodeEnd;
                curr.Parent = splitNode;
                splitNode.Children[MappedSequence[curr.Start]] = curr;

                depth -= (curr.End - curr.Start);

                Node newNode = new Node(sa[i] + lcp[i], MappedSequence.Length, AlphabetSize, splitNode, sa[i]);
                splitNode.Children[MappedSequence[newNode.Start]] = newNode;

                depth += (newNode.End - newNode.Start);

                stack.Pop();
                stack.Push(splitNode);
                stack.Push(newNode);

                curr = newNode;
            }
        }
    }

    public IEnumerable<SamLine> SearchForAllApproximateOccurences(string fastaName, string fastqName, string pattern, int edits)
    {
        List<SamLine> samlines = new List<SamLine>();

        int[] mappedPattern = pattern.MapWithAlphabetWithoutSentinelAllowNewChars(CharToIndex);

        //(currNode, currPos on edge, edits left, position in pattern, lastEditOp)
        Stack<(Node, int, int, int, EditOperation)> stack = new();
        stack.Push((Root, 0, edits, 0, EditOperation.None));

        //Maybe this should be array
        Stack<EditOperation> editOps = new();

        while (stack.Count > 0)
        {
            (Node currNode, int currPosOnEdge, int editsLeft, int posInPattern, EditOperation lastEditOp) = stack.Pop();

            //Check if we return from "recursive call" and need to pop from editOps
            if (currNode == null)
            {
                editOps.Pop();
                continue;
            }

            //Push latest edit operation to stack
            if (lastEditOp != EditOperation.None)
                editOps.Push(lastEditOp);

            //checked if end of pattern
            if (posInPattern == mappedPattern.Length)
            {
                AddSubtreeToSamlines(samlines, editOps, currNode, fastqName, fastaName, pattern);

                stack.Push((null, 0, 0, 0, EditOperation.None));
                continue;
            }

            //Add event that we have to pop Edit Operation  from stack when "return"
            if (lastEditOp != EditOperation.None)
                stack.Push((null, 0, 0, 0, EditOperation.None));


            if (currPosOnEdge == currNode.End - currNode.Start)
            {
                //Handles node as node (Start of next edge)

                for (int i = 0; i < currNode.Children.Length; i++)
                {
                    if (currNode.Children[i] == null) continue;
                    stack.Push((currNode.Children[i], 0, editsLeft, posInPattern, EditOperation.None));
                }
            }
            else
            {
                //Handles node as an edge


                //Check for sentinel
                if (MappedSequence[currNode.Start + currPosOnEdge] == 0)
                {
                    if (editsLeft >= mappedPattern.Length - posInPattern)
                    {
                        for (int i = 0; i < mappedPattern.Length - posInPattern; i++)
                        {
                            editOps.Push(EditOperation.I);
                        }

                        AddSubtreeToSamlines(samlines, editOps, currNode, fastqName, fastaName, pattern);

                        for (int i = 0; i < mappedPattern.Length - posInPattern; i++)
                        {
                            editOps.Pop();
                        }
                    }
                    continue;
                }

                //Match/mismatch
                if (mappedPattern[posInPattern] == MappedSequence[currNode.Start + currPosOnEdge])
                {
                    //If match
                    stack.Push((currNode, currPosOnEdge + 1, editsLeft, posInPattern + 1, EditOperation.M));
                }
                else
                {
                    //if mismatch
                    if (editsLeft > 0)
                        stack.Push((currNode, currPosOnEdge + 1, editsLeft - 1, posInPattern + 1, EditOperation.Q));
                }

                if (editsLeft > 0)
                {
                    //Delete
                    stack.Push((currNode, currPosOnEdge + 1, editsLeft - 1, posInPattern, EditOperation.D));

                    //Insert
                    stack.Push((currNode, currPosOnEdge, editsLeft - 1, posInPattern + 1, EditOperation.I));
                }

            }
        }

        return samlines;
    }

    private void AddSubtreeToSamlines(List<SamLine> samlines, Stack<EditOperation> editOps, Node currNode, string fastqName, string fastaName, string pattern)
    {
        //traverse all children in the subtree
        string cigar = CreateCigar(editOps);
        if (cigar[1] == 'D')
            return;


        Queue<Node> toTraverse = new Queue<Node>();
        toTraverse.Enqueue(currNode);
        while (toTraverse.Count > 0)
        {
            var node = toTraverse.Dequeue();
            if (node.Label.HasValue)
            {
                samlines.Add(new SamLine(fastqName, 0, fastaName, node.Label.Value, 0, cigar, "*", 0, 0, pattern));
            }
            else
            {
                foreach (var c in node.Children)
                {
                    if (c != null)
                    {
                        toTraverse.Enqueue(c);
                    }
                }
            }
        }
    }

    private string CreateCigar(Stack<EditOperation> editOps)
    {
        StringBuilder sb = new StringBuilder();

        var enumerator = editOps.GetEnumerator();
        Stack<string> stringStack = new();
        //Assume cigar not empty
        enumerator.MoveNext();
        EditOperation currEditOp = enumerator.Current;
        int c = 1;

        while (enumerator.MoveNext())
        {
            if (enumerator.Current == EditOperation.None) continue;
            if (enumerator.Current != currEditOp)
            {
                stringStack.Push(currEditOp.ToString());
                stringStack.Push(c.ToString());
                c = 1;
                currEditOp = enumerator.Current;
            }
            else
            {
                c++;
            }
        }

        stringStack.Push(currEditOp.ToString());
        stringStack.Push(c.ToString());

        foreach (string s in stringStack)
            sb.Append(s);

        return sb.ToString();
    }

    public (int[] sa, int[] lcp) ComputeSaAndLcp()
    {
        Stack<Node> nodeStack = new();
        Stack<int> lcpCounterStack = new();
        Stack<int> pathDepthStack = new();
        nodeStack.Push(Root);
        lcpCounterStack.Push(0);
        pathDepthStack.Push(0);
        var sa = new int[MappedSequence.Length];
        int[] lcp = new int[MappedSequence.Length];
        var progress = 0;
        while (nodeStack.Count > 0)
        {
            var node = nodeStack.Pop();
            int lcpCounter = lcpCounterStack.Pop();
            int pathDepth = pathDepthStack.Pop();
            if (node.Label.HasValue)
            {
                lcp[progress] = pathDepth - lcpCounter;
                sa[progress++] = node.Label.Value;
                continue;
            }
            int noOfChildren = 0;
            for (int i = AlphabetSize - 1; i >= 0; i--)
            {
                Node child = node.Children[i];
                if (child is not null)
                {

                    nodeStack.Push(child);
                    pathDepthStack.Push(pathDepth + (child.End - child.Start));
                    noOfChildren++;
                }
            }
            if (noOfChildren != 0)
            {
                for (int i = AlphabetSize - 1; i >= 0; i--)
                {
                    Node child = node.Children[i];
                    if (child != null)
                    {
                        if (noOfChildren > 1)
                        {
                            noOfChildren--;
                            lcpCounterStack.Push(child.End - child.Start);
                        }
                        else
                        {
                            lcpCounterStack.Push(lcpCounter + (child.End - child.Start));
                        }
                    }

                }
            }
        }
        return (sa, lcp);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="child"></param>
    /// <param name="splitIndex">Indexed from 0 at the start of the edge to edgelength</param>
    /// <param name="startindexNewPath">Index of new path at edge start</param>
    /// <param name="mappedInput"></param>
    private Node InsertNodeOnEdge(Node node, Node child, int splitIndex, int startindexNewPath, int[] mappedInput, int newLabel)
    {
        Node splitNode = new Node(child.Start, child.Start + splitIndex, AlphabetSize, node);
        node.SetChild(mappedInput[splitNode.Start], splitNode);
        child.Start = splitNode.End;
        splitNode.SetChild(mappedInput[splitNode.End], child);
        Node newPathNode = new Node(startindexNewPath + splitIndex, mappedInput.Length, AlphabetSize, splitNode, newLabel);
        splitNode.SetChild(mappedInput[startindexNewPath + splitIndex], newPathNode);
        return newPathNode;
    }

    public int SeachForFirstOccurence(string pattern)
    {
        int[] mappedPattern;

        try
        {
            mappedPattern = pattern.MapWithAlphabetWithoutSentinel(CharToIndex);
        }
        catch
        {
            return -1;
        }

        Node curr = Root;
        int indexInPattern = 0;

        while (indexInPattern < mappedPattern.Length)
        {
            if (curr.Children[mappedPattern[indexInPattern]] == null) return -1;
            Node child = curr.Children[mappedPattern[indexInPattern]];
            for (int i = child.Start; i < child.End; i++)
            {
                if (mappedPattern[i - child.Start + indexInPattern] != MappedSequence[i]) return -1;
                if (i == mappedPattern.Length - 1 + child.Start - indexInPattern)
                {
                    return i - mappedPattern.Length + 1;
                }
            }
            indexInPattern += child.End - child.Start;
            curr = child;
        }

        return -1;
    }

    public IEnumerable<SamLine> SearchForAllOccurences(string fastaName, string fastqName, string pattern)
    {
        string cigar = pattern.Length + "M"; //Should add the proper cigar handling in later assignments

        List<SamLine> samlines = new List<SamLine>();

        int[] mappedPattern;
        try
        {
            mappedPattern = pattern.MapWithAlphabetWithoutSentinel(CharToIndex);
        }
        catch
        {
            return samlines;
        }

        Node curr = Root;
        int indexInPattern = 0;

        while (indexInPattern < mappedPattern.Length)
        {
            if (curr.Children[mappedPattern[indexInPattern]] == null) return samlines;
            Node child = curr.Children[mappedPattern[indexInPattern]];
            for (int i = child.Start; i < child.End; i++)
            {
                if (mappedPattern[i - child.Start + indexInPattern] != MappedSequence[i]) return samlines;
                if (i == mappedPattern.Length - 1 + child.Start - indexInPattern)
                {
                    Queue<Node> toTraverse = new Queue<Node>();
                    toTraverse.Enqueue(child);
                    while (toTraverse.Count > 0)
                    {
                        var node = toTraverse.Dequeue();
                        if (node.Label.HasValue)
                        {
                            samlines.Add(new SamLine(fastqName, 0, fastaName, node.Label.Value, 0, cigar, "*", 0, 0, pattern));
                        }
                        else
                        {
                            foreach (var c in node.Children)
                            {
                                if (c != null)
                                {
                                    toTraverse.Enqueue(c);
                                }
                            }
                        }
                    }
                    return samlines;
                }
            }
            indexInPattern += child.End - child.Start;
            curr = child;
        }

        return samlines;
    }
}