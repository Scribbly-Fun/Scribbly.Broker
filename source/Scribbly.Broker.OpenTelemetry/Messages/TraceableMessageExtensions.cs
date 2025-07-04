namespace Scribbly.Broker;

/// <summary>
/// 
/// </summary>
public static class TraceableMessageExtensions
{
    private const string MessageErrorKey = "notification.error";
    private const string MessageSuccessKey = "notification.success";
    private const string AsyncTraceableMessage = "Async.Traceable.Message";

    /// <summary>
    /// 
    /// </summary>
    public static readonly ActivitySource Source = new(AsyncTraceableMessage);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notification"></param>
    public static void AddCurrentTraceContext(this ITraceableNotification notification)
    {
        Propagators.DefaultTextMapPropagator.Inject(
            new PropagationContext(Activity.Current?.Context ?? new ActivityContext(), Baggage.Current),
            notification.RequestHeaders, (headers, key, value) => headers[key] = [value]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    public static Activity? StartNewSpanFromRequest(this ITraceableNotification notification)
    {
#if NETSTANDARD2_0
        var context = Propagators.DefaultTextMapPropagator.Extract(
            new PropagationContext(Activity.Current?.Context ?? new ActivityContext(), Baggage.Current),
            notification.RequestHeaders, (headers, key) => headers.TryGetValue(key, out var value) ? value : null);
#else
        var context = Propagators.DefaultTextMapPropagator.Extract(
            new PropagationContext(Activity.Current?.Context ?? new ActivityContext(), Baggage.Current), 
            notification.RequestHeaders, (headers, key) => headers.GetValueOrDefault(key));
#endif
        Baggage.Current = context.Baggage;

        return Source.StartActivity($"Process {notification.GetType().Name}",
            ActivityKind.Internal,
            context.ActivityContext);
    }

    /// <summary>
    /// Starts a new root span from the notification
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    public static Activity? StartNewRootSpanFromRequest(this ITraceableNotification notification)
    {
#if NETSTANDARD2_0
        var context = Propagators.DefaultTextMapPropagator.Extract(
            new PropagationContext(Activity.Current?.Context ?? new ActivityContext(), Baggage.Current),
            notification.RequestHeaders, (headers, key) => headers.TryGetValue(key, out var value) ? value : null);
#else
        var context = Propagators.DefaultTextMapPropagator.Extract(
            new PropagationContext(Activity.Current?.Context ?? new ActivityContext(), Baggage.Current), 
            notification.RequestHeaders, (headers, key) => headers.GetValueOrDefault(key));
#endif

        Baggage.Current = context.Baggage;
        Activity.Current = null;

        return Source.StartActivity($"Process {notification.GetType().Name}",
            ActivityKind.Server,
            new ActivityContext(),
            links: [new(context.ActivityContext)]);
    }

    /// <summary>
    /// Records an exception on the activity.
    /// </summary>
    /// <param name="activity">the activity</param>
    /// <param name="exception">An optional exception to record</param>
    /// <param name="message">An optional notification to record the error</param>
    public static void RecordError(this Activity? activity, Exception? exception = null, string? message = null)
    {
        if (activity is null)
        {
            return;
        }

        activity.SetStatus(Status.Error);

        if (exception is not null)
        {
            activity.RecordException(exception);
        }

        if (!string.IsNullOrEmpty(message))
        {
            activity.SetTag(MessageErrorKey, message);
        }
    }

    /// <summary>
    /// Records a status event on the trace reporting success
    /// </summary>
    /// <param name="activity">This nullable activity</param>
    /// <param name="message">The notification to record</param>
    public static void RecordSuccess(this Activity? activity, string? message = null)
    {
        if (activity is null)
        {
            return;
        }

        activity.SetStatus(Status.Ok, message);

        if (!string.IsNullOrEmpty(message))
        {
            activity.SetTag(MessageSuccessKey, message);
        }
    }
}