namespace Gateway.Core.Abstractions;

public interface IsFor<A>
{
    bool IsFor(A a);
}