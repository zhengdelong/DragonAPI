using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public static class LockExtensions
    {
        /// <summary>
        /// Executes given <paramref name="action"/> by locking given <paramref name="source"/> object.
        /// </summary>
        /// <typeparam name="T">Type of the object (to be locked)</typeparam>
        /// <param name="source">Source object (to be locked)</param>
        /// <param name="action">Action (to be executed)</param>
        public static void Locking<T>(this T source, Action<T> action) where T : class
        {
            lock (source)
            {
                action(source);
            }
        }
    }
}
