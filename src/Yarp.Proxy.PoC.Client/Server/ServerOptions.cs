namespace Yarp.Proxy.PoC.Client.Server;

internal sealed record ServerOptions
{
    public static readonly string SectionName = "Server";
    public Uri BaseAddress { get; init; } = new("about:blank");
}
