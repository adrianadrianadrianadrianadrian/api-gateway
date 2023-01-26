namespace Gateway.IO;

using Bearded.Monads;

public interface ICache<T>
{
    Option<T> Get(string id);
    void Set(string id, T t);
}