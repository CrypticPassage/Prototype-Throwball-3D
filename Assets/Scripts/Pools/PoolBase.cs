using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public class PoolBase<T>
    {
        private Func<T> _preLoadFunc;
        private Action<T> _getAction;
        private Action<T> _returnAction;
        private Queue<T> _pool;
        private List<T> _active = new List<T>();

        public PoolBase(Func<T> preLoadFunc, Action<T> getAction, Action<T> returnAction, int preLoadCount)
        {
            _pool = new Queue<T>();
            
            _preLoadFunc = preLoadFunc;
            _getAction = getAction;
            _returnAction = returnAction;
            
            if (preLoadFunc == null)
            {
                Debug.Log("Preload Function is Null");
                return;
            }
            
            for (int i = 0; i < preLoadCount; i++)
                Return(preLoadFunc());
        }
        
        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : _preLoadFunc();
            _getAction?.Invoke(item);
            _active.Add(item);

            return item;
        }

        public List<T> GetActiveList()
        {
            return _active;
        }
        
        public void Return(T item)
        {
            _returnAction?.Invoke(item);
            _pool.Enqueue(item);
            _active.Remove(item);
        }

        public void ReturnAll()
        {
            foreach (T item in _active.ToArray())
                Return(item);
        }
    }
}