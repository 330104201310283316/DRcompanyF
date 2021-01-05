using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDTH.WebApi
{
    public static class Extensions
    {
        public static IEnumerable<Exception> FlattenHierarchy(this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }
    }
}
