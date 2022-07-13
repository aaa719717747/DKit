using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.ThreadHelper.Runtime
{
     public class HTCache<TKey, TValue>
    {
        const int DefaultCapacity = 255;

        int _capacity;
        ReaderWriterLockSlim _locker;
        IDictionary<TKey, TValue> _dictionary;
        LinkedList<TKey> _linkedList;

        public HTCache() : this(DefaultCapacity)
        {
        }

        public HTCache(int capacity)
        {
            _locker = new ReaderWriterLockSlim();
            _capacity = capacity > 0 ? capacity : DefaultCapacity;
            _dictionary = new Dictionary<TKey, TValue>();
            _linkedList = new LinkedList<TKey>();
        }

        public void Set(TKey key, TValue value)
        {
            _locker.EnterWriteLock();
            try
            {
                _dictionary[key] = value;
                _linkedList.Remove(key);
                _linkedList.AddFirst(key);
                if (_linkedList.Count > _capacity)
                {
                    _dictionary.Remove(_linkedList.Last.Value);
                    _linkedList.RemoveLast();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public void SetRange(IDictionary<TKey, TValue> items)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            _locker.EnterWriteLock();

            try
            {
                foreach (KeyValuePair<TKey, TValue> keyValuePair in items)
                {
                    _dictionary[keyValuePair.Key] = keyValuePair.Value;
                    _linkedList.Remove(keyValuePair.Key);
                    _linkedList.AddFirst(keyValuePair.Key);
                    if (_linkedList.Count > _capacity)
                    {
                        _dictionary.Remove(_linkedList.Last.Value);
                        _linkedList.RemoveLast();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public bool TryGet(TKey key, out TValue value)
        {
            _locker.EnterUpgradeableReadLock();
            try
            {
                bool b = _dictionary.TryGetValue(key, out value);
                if (b)
                {
                    _locker.EnterWriteLock();
                    try
                    {
                        _linkedList.Remove(key);
                        _linkedList.AddFirst(key);
                    }
                    finally
                    {
                        _locker.ExitWriteLock();
                    }
                }

                return b;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            finally
            {
                _locker.ExitUpgradeableReadLock();
            }

            value = default;
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            _locker.EnterReadLock();
            try
            {
                return _dictionary.ContainsKey(key);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            finally
            {
                _locker.ExitReadLock();
            }

            return false;
        }

        public int Count
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _dictionary.Count;
                }
                finally
                {
                    _locker.ExitReadLock();
                }
            }
        }

        public int Capacity
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _capacity;
                }
                finally
                {
                    _locker.ExitReadLock();
                }
            }
            set
            {
                _locker.EnterUpgradeableReadLock();
                try
                {
                    if (value > 0 && _capacity != value)
                    {
                        _locker.EnterWriteLock();
                        try
                        {
                            _capacity = value;
                            while (_linkedList.Count > _capacity)
                            {
                                _linkedList.RemoveLast();
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning(e);
                        }
                        finally
                        {
                            _locker.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    _locker.ExitUpgradeableReadLock();
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _dictionary.Keys;
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
                finally
                {
                    _locker.ExitReadLock();
                }

                return null;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                _locker.EnterReadLock();
                try
                {
                    return _dictionary.Values;
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
                finally
                {
                    _locker.ExitReadLock();
                }

                return null;
            }
        }

        public void Clean()
        {
            _locker.EnterWriteLock();
            try
            {
                _dictionary.Clear();
                _linkedList.Clear();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }
    }
}