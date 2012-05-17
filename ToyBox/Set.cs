using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ToyBox
{
    public class Set<T> : IEnumerable<T>
    {
        private List<T> list = new List<T>();

        public Set()
        {
        }
   
        public void Add(T t)
        {
            if (!list.Contains(t))
                list.Add(t);
        }

        public void Remove(T t)
        {
            list.Remove(t);
        }

        public void Clear()
        {
            list.Clear();
        }

        public int Count 
        {
            get
            {
                return list.Count;
            }
        }

        public void Union(Set<T> other)
        {
            foreach (var t in other)
            {
                this.Add(t);
            }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in list)
                yield return item;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
