namespace Scribbly.Broker;

/// <summary>
/// 
/// </summary>
public interface ITraceableNotification : INotification
{
    /// <summary>
    /// 
    /// </summary>
    Dictionary<string, string[]> RequestHeaders { get; set; }
}