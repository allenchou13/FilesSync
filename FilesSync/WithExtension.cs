using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesSync
{
    public static class WithExtension
    {
        public static T With<T>(this T target, Action<T> action)
        {
            action.Invoke(target);
            return target;
        }
    }
}
