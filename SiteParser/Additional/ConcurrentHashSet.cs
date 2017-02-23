using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SiteParser
{
    public class ConcurrentHashSet<T> : IEnumerable<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ConcurrentDictionary<T, object> _dic;

        public ConcurrentHashSet()
        {
            _dic = new ConcurrentDictionary<T, object>();
        }

        public ConcurrentHashSet(IEnumerable<T> initialItems)
        {
            if (initialItems == null)
                throw new ArgumentNullException(nameof(initialItems));

            _dic = new ConcurrentDictionary<T, object>(initialItems.Select(x => new KeyValuePair<T, object>(x, null)));
        }

        public int Count => _dic.Count;

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var kvp in _dic)
                yield return kvp.Key;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool TryAdd(T obj) => _dic.TryAdd(obj, null);

        public bool TryRemove(T obj)
        {
            object _;
            return _dic.TryRemove(obj, out _);
        }

        public bool Contains(T obj) => _dic.ContainsKey(obj);
    }
}