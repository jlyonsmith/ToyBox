using System;
using System.Collections.Generic;
using System.Text;

namespace ToyBox
{
    public static class CollectionHelper
    {
        public static T GetIfExists<T>(IList<T> list, int index)
        {
            if (list.Count > index)
            {
                return list[index];
            }
            else
            {
                return default(T);
            }
        }

        public static void DisposeItems<T>(IList<T> list)
        {
            for (int index = list.Count - 1; index >= 0; --index)
            {
                IDisposable disposable = list[index] as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
