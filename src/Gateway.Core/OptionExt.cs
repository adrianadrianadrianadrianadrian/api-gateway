namespace Gateway.Core;

using Bearded.Monads;
using System.Threading.Tasks;
using System;

public static class OptionExt
{
    public static Task<Option<B>> SelectMany<A, B>(this Option<A> mA, Func<A, Task<Option<B>>> mapB)
            => mA.Select(mapB).Else(() => Task.FromResult(Option<B>.None));
}