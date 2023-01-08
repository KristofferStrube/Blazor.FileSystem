using BenchmarkDotNet.Attributes;
using KristofferStrube.ActivityStreams;
using KristofferStrube.Blazor.FileSystem.WasmExample.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using static System.Text.Json.JsonSerializer;

namespace KristofferStrube.Blazor.FileSystem.WasmExample.Benchmarks.Benchmarks;

public class NodePresenterBenchmark
{
    private BenchmarkRenderer renderer = default!;

    private Note note;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var services = new ServiceCollection();

        // Add services to inject into components
        // rendered with the renderer here, e.g.:
        // services.AddSingleton<IFoo, Foo>();

        renderer = new BenchmarkRenderer(services.BuildServiceProvider());

        var jsonNote = """
                    {
                        "attachment": {
                        "url": "https://media.hachyderm.io/media_attachments/files/109/654/626/071/694/548/original/29d6bd160e3e07bf.mp4",
                        "type": "Document",
                        "name": "Searching through Mastodon posts in a Blazor application.",
                        "mediaType": "video/mp4",
                        "blurhash": "UnQT4N004n9FRjkCkBofWBj[fPj[offQWBju",
                        "focalPoint": [
                            0.0,
                            0.0
                        ],
                        "width": 1702,
                        "height": 958
                        },
                        "attributedTo": {
                        "outbox": "https://hachyderm.io/users/KristofferStrube/outbox",
                        "inbox": "https://hachyderm.io/users/KristofferStrube/inbox",
                        "followers": "https://hachyderm.io/users/KristofferStrube/followers",
                        "following": "https://hachyderm.io/users/KristofferStrube/following",
                        "preferredUsername": "KristofferStrube",
                        "endpoints": {
                            "sharedInbox": "https://hachyderm.io/inbox"
                        },
                        "attachment": [
                            {
                            "type": [
                                "PropertyValue",
                                "Object"
                            ],
                            "name": "Website",
                            "value": "<a href=\"https://kristoffer-strube.dk\" target=\"_blank\" rel=\"nofollow noopener noreferrer me\"><span class=\"invisible\">https://</span><span class=\"\">kristoffer-strube.dk</span><span class=\"invisible\"></span></a>"
                            },
                            {
                            "type": [
                                "PropertyValue",
                                "Object"
                            ],
                            "name": "GitHub",
                            "value": "<a href=\"https://github.com/KristofferStrube\" target=\"_blank\" rel=\"nofollow noopener noreferrer me\"><span class=\"invisible\">https://</span><span class=\"\">github.com/KristofferStrube</span><span class=\"invisible\"></span></a>"
                            }
                        ],
                        "icon": {
                            "url": "https://media.hachyderm.io/accounts/avatars/109/364/675/065/868/952/original/8b34316439e5e2ef.png",
                            "type": "Image",
                            "mediaType": "image/png"
                        },
                        "image": {
                            "url": "https://media.hachyderm.io/accounts/headers/109/364/675/065/868/952/original/8875a598d5103901.jpg",
                            "type": "Image",
                            "mediaType": "image/jpeg"
                        },
                        "tag": [

                        ],
                        "url": "https://hachyderm.io/@KristofferStrube",
                        "published": "2022-11-18T00:00:00Z",
                        "summary": "<p>.NET developer, Comp.-Sci. MSc., and Blazor enthusiast.</p>",
                        "@context": [
                            "https://www.w3.org/ns/activitystreams",
                            "https://w3id.org/security/v1",
                            {

                            }
                        ],
                        "id": "https://hachyderm.io/users/KristofferStrube",
                        "type": "Person",
                        "name": "Kristoffer Strube",
                        "featured": "https://hachyderm.io/users/KristofferStrube/collections/featured",
                        "featuredTags": "https://hachyderm.io/users/KristofferStrube/collections/tags",
                        "manuallyApprovesFollowers": false,
                        "discoverable": true,
                        "devices": "https://hachyderm.io/users/KristofferStrube/collections/devices",
                        "publicKey": {
                            "id": "https://hachyderm.io/users/KristofferStrube#main-key",
                            "owner": "https://hachyderm.io/users/KristofferStrube",
                            "publicKeyPem": "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyhLtxhQyI3tohkFJfdZS\n2T3fgfdVKhmZr3wMeYtmJLQZ01RZMMEBulKXpXiqNfvGPpiOnSFQEMJplqt8albx\nN8o529on9T5V7TKe23HDib0w0vVumw5R6Jv2JZaR6pMRnm93keAUR8K1+zcsAeYr\n/TwzWLNwXMB+e8vwMp5yvW4h3S1rauXQZeStOEiQ4joBEB/X3EKhjW+doz/KgcIl\n8cXN/J6BN8EGY6GFUESEZeMOrxayOE13MHKeauQ8EL/94n9n9gbSZBNym4i6gW8w\nMDfBtChokcV6JxLkbDE769FmleCbibCtYiYGTplhyDu4IVOr7zU6nGBhIw6FkFDv\n4QIDAQAB\n-----END PUBLIC KEY-----\n"
                        }
                        },
                        "cc": "https://hachyderm.io/users/KristofferStrube/followers",
                        "replies": {
                        "first": {
                            "partOf": "https://hachyderm.io/users/KristofferStrube/statuses/109654643609724194/replies",
                            "next": "https://hachyderm.io/users/KristofferStrube/statuses/109654643609724194/replies?only_other_accounts=true&page=true",
                            "items": [

                            ],
                            "type": "CollectionPage"
                        },
                        "id": "https://hachyderm.io/users/KristofferStrube/statuses/109654643609724194/replies",
                        "type": "Collection"
                        },
                        "tag": [
                        {
                            "type": [
                            "Hashtag",
                            "Object"
                            ],
                            "name": "#blazor",
                            "href": "https://hachyderm.io/tags/blazor"
                        },
                        {
                            "type": [
                            "Hashtag",
                            "Object"
                            ],
                            "name": "#dotnet",
                            "href": "https://hachyderm.io/tags/dotnet"
                        },
                        {
                            "type": [
                            "Hashtag",
                            "Object"
                            ],
                            "name": "#csharp",
                            "href": "https://hachyderm.io/tags/csharp"
                        },
                        {
                            "type": [
                            "Hashtag",
                            "Object"
                            ],
                            "name": "#search",
                            "href": "https://hachyderm.io/tags/search"
                        },
                        {
                            "type": [
                            "Hashtag",
                            "Object"
                            ],
                            "name": "#filesystemapi",
                            "href": "https://hachyderm.io/tags/filesystemapi"
                        },
                        {
                            "type": [
                            "Hashtag",
                            "Object"
                            ],
                            "name": "#activitystreams",
                            "href": "https://hachyderm.io/tags/activitystreams"
                        },
                        {
                            "type": [
                            "Hashtag",
                            "Object"
                            ],
                            "name": "#activitypub",
                            "href": "https://hachyderm.io/tags/activitypub"
                        }
                        ],
                        "to": "https://www.w3.org/ns/activitystreams#Public",
                        "url": "https://hachyderm.io/@KristofferStrube/109654643609724194",
                        "content": "<p>My weekend project has been another demo for my Blazor File System wrapper. With this demo I can search in posts from Mastodon and save the search trees used for searching in the Origin Private File System to spare rebuilding them on reloads.<br />Also got to use an old project which was the naive implementation of the search tree itself.<br /><a href=\"https://hachyderm.io/tags/blazor\" class=\"mention hashtag\" rel=\"tag\">#<span>blazor</span></a> <a href=\"https://hachyderm.io/tags/dotnet\" class=\"mention hashtag\" rel=\"tag\">#<span>dotnet</span></a> <a href=\"https://hachyderm.io/tags/csharp\" class=\"mention hashtag\" rel=\"tag\">#<span>csharp</span></a> <a href=\"https://hachyderm.io/tags/search\" class=\"mention hashtag\" rel=\"tag\">#<span>search</span></a> <a href=\"https://hachyderm.io/tags/filesystemapi\" class=\"mention hashtag\" rel=\"tag\">#<span>filesystemapi</span></a> <a href=\"https://hachyderm.io/tags/activitystreams\" class=\"mention hashtag\" rel=\"tag\">#<span>activitystreams</span></a> <a href=\"https://hachyderm.io/tags/activitypub\" class=\"mention hashtag\" rel=\"tag\">#<span>activitypub</span></a><br />Project: <a href=\"https://github.com/KristofferStrube/Blazor.FileSystem\" target=\"_blank\" rel=\"nofollow noopener noreferrer\"><span class=\"invisible\">https://</span><span class=\"ellipsis\">github.com/KristofferStrube/Bl</span><span class=\"invisible\">azor.FileSystem</span></a><br />Demo: <a href=\"https://kristofferstrube.github.io/Blazor.FileSystem/SearchMastodon\" target=\"_blank\" rel=\"nofollow noopener noreferrer\"><span class=\"invisible\">https://</span><span class=\"ellipsis\">kristofferstrube.github.io/Bla</span><span class=\"invisible\">zor.FileSystem/SearchMastodon</span></a></p>",
                        "contentMap": {
                        "en": "<p>My weekend project has been another demo for my Blazor File System wrapper. With this demo I can search in posts from Mastodon and save the search trees used for searching in the Origin Private File System to spare rebuilding them on reloads.<br />Also got to use an old project which was the naive implementation of the search tree itself.<br /><a href=\"https://hachyderm.io/tags/blazor\" class=\"mention hashtag\" rel=\"tag\">#<span>blazor</span></a> <a href=\"https://hachyderm.io/tags/dotnet\" class=\"mention hashtag\" rel=\"tag\">#<span>dotnet</span></a> <a href=\"https://hachyderm.io/tags/csharp\" class=\"mention hashtag\" rel=\"tag\">#<span>csharp</span></a> <a href=\"https://hachyderm.io/tags/search\" class=\"mention hashtag\" rel=\"tag\">#<span>search</span></a> <a href=\"https://hachyderm.io/tags/filesystemapi\" class=\"mention hashtag\" rel=\"tag\">#<span>filesystemapi</span></a> <a href=\"https://hachyderm.io/tags/activitystreams\" class=\"mention hashtag\" rel=\"tag\">#<span>activitystreams</span></a> <a href=\"https://hachyderm.io/tags/activitypub\" class=\"mention hashtag\" rel=\"tag\">#<span>activitypub</span></a><br />Project: <a href=\"https://github.com/KristofferStrube/Blazor.FileSystem\" target=\"_blank\" rel=\"nofollow noopener noreferrer\"><span class=\"invisible\">https://</span><span class=\"ellipsis\">github.com/KristofferStrube/Bl</span><span class=\"invisible\">azor.FileSystem</span></a><br />Demo: <a href=\"https://kristofferstrube.github.io/Blazor.FileSystem/SearchMastodon\" target=\"_blank\" rel=\"nofollow noopener noreferrer\"><span class=\"invisible\">https://</span><span class=\"ellipsis\">kristofferstrube.github.io/Bla</span><span class=\"invisible\">zor.FileSystem/SearchMastodon</span></a></p>"
                        },
                        "published": "2023-01-08T16:57:56Z",
                        "id": "https://hachyderm.io/users/KristofferStrube/statuses/109654643609724194",
                        "type": "Note",
                        "sensitive": false,
                        "atomUri": "https://hachyderm.io/users/KristofferStrube/statuses/109654643609724194",
                        "inReplyToAtomUri": null,
                        "conversation": "tag:hachyderm.io,2023-01-08:objectId=17150450:objectType=Conversation"
                    }
            """;

        note = Deserialize<Note>(jsonNote)!;
    }

    [GlobalCleanup]
#pragma warning disable BL0006 // Do not use RenderTree types
    public void GlobalCleanup() => renderer.Dispose();
#pragma warning restore BL0006 // Do not use RenderTree types

    [Benchmark]
    public async Task<int> InitialRender()
    {
        // Render a component
        var parameters = new Dictionary<string, object?>() { { "Note", note } };
        var component = await renderer.RenderAsync<NotePresenter>(ParameterView.FromDictionary(parameters));

        // Remove the component again from the render tree. Stops the
        // renderer from tracking the component.
        component.RemoveComponent();

        return renderer.RenderCount;
    }
}

