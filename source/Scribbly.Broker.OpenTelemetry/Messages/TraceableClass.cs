namespace Scribbly.Broker;

/// <summary>
/// 
/// </summary>
public abstract class TraceableClass : ITraceableNotification
{
    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, string[]> RequestHeaders { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    protected TraceableClass()
    {
        this.AddCurrentTraceContext();
    }
}