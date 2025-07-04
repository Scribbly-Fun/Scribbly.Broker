namespace Scribbly.Broker;


/// <summary>
/// 
/// </summary>
public abstract record TraceableRecord : ITraceableNotification
{
    /// <inheritdoc />
    public Dictionary<string, string[]> RequestHeaders { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    protected TraceableRecord()
    {
        this.AddCurrentTraceContext();
    }
}