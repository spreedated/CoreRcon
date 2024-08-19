using System;
using System.Linq;
using System.Reflection;

namespace CoreRCON.Parsers
{
    internal static class ParserHelpers
    {
        internal static IParser<T> CreateParser<T>()
            where T : class, IParseable, new()
        {
            Type implementor = Array.Find<Type>(new T().GetType().GetTypeInfo().Assembly.GetTypes(), t => t.GetTypeInfo().GetInterfaces().Contains(typeof(IParser<T>)));

            return implementor == null
                ? throw new ArgumentException($"A class implementing {nameof(IParser)}<{typeof(T).FullName}> was not found in the assembly.")
                : (IParser<T>)Activator.CreateInstance(implementor);
        }
    }
}
