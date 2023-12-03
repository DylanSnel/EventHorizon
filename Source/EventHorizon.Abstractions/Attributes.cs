namespace EventHorizon;

#pragma warning disable CS9113 // Parameter is unread.
#pragma warning disable IDE1006 // Naming Styles

public abstract class EventHorizonAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HandlerAttribute(string Name = "", string Description = "", string[]? Tags = null, bool Deprecated = false, bool Temporary = false) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class HandlerAttribute<TEvent>(string Name = "", string Description = "", string[]? Tags = null, bool Deprecated = false, bool Temporary = false) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ProducesAttribute<TEvent>(string? Condition = null, string? Remarks = null) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ProducesErrorAttribute<TEvent>(string? Condition = null, string? Remarks = null) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class StartFlowAttribute(string FlowName, string[]? Tags = null) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class MapsAttribute<TIncoming, TOutGoing>(bool Deprecated = false, bool Temporary = false) : EventHorizonAttribute;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TriggersAttribute<TEvent>(string Description = "") : EventHorizonAttribute;


#pragma warning restore CS9113 // Parameter is unread.
#pragma warning restore IDE1006 // Naming Styles
