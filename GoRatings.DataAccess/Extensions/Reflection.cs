using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoRatings.DataAccess;

public static partial class Extensions
{
    public static T GetNone<T>(this Type t)
    {
        var pi = t.GetProperty("None", BindingFlags.Public | BindingFlags.Static);

        if (pi == null)
            throw new ApplicationException($"Type {t.Name} does not define a static property named None.");

        object? v = pi.GetValue(null);

        if (v == null)
            throw new ApplicationException($"None property of type {t.Name} has null value.");

        if (v is not T)
            throw new ApplicationException(string.Format("None property of type {0} is not of type {0}.", t.Name));

        return (T)v;
    }
}
