using System;
using System.Collections.Generic;

namespace Toolbox
{
    /// <summary>
    /// This doesn't seem to actually work for some reason?
    /// </summary>
    public class GenericComparer<TComparable> : IEqualityComparer<TComparable>
    {
        private readonly Func<TComparable, object> compare;

        public GenericComparer(Func<TComparable, object> compare)
        {
            this.compare = compare;
        }

        public bool Equals(TComparable x, TComparable y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else
            {
                return compare(x) == compare(y);
            }
        }

        public int GetHashCode(TComparable obj)
        {
            var hashCode = compare(obj).GetHashCode();
            return hashCode;
        }
    }
}
