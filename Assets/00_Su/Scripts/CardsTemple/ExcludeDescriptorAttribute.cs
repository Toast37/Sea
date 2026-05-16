using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class ExcludeDescriptorAttribute : Attribute
{
    public Type[] Excluded { get; }
    public ExcludeDescriptorAttribute(params Type[] excluded) => Excluded = excluded;
}
