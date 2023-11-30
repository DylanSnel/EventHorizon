namespace EventHorizon;

#pragma warning disable CS9113 // Parameter is unread.
#pragma warning disable IDE1006 // Naming Styles

public abstract class EventHorizonAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HandlerAttribute(string Description = "", string Tag = "", bool Deprecated = false) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HandlerAttribute<TEvent>(string Description = "", string Tag = "", bool Deprecated = false) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ProducesAttribute<TEvent>(string? Condition = null, string? Remarks = null) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ProducesErrorAttribute<TEvent>(string? Condition = null, string? Remarks = null) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class StartFlowAttribute(string FlowName, string[]? Tag = null) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class Maps<TIncoming, TOutGoing>(bool Temporary = false) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class Triggers<TEvent>(string Description = "") : EventHorizonAttribute;


#pragma warning restore CS9113 // Parameter is unread.
#pragma warning restore IDE1006 // Naming Styles
