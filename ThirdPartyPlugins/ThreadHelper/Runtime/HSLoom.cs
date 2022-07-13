using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DKit.ThirdPartyPlugins.ThreadHelper.Runtime;
using UnityEngine;

public class HSLoom : MonoBehaviour
{
    private static HSLoom _current;
    public static HSLoom Instance => _current;
    private static TaskFactory _factory;
    private int _count;

    /// <summary>
    /// Return the current instance
    /// </summary>
    /// <value>
    /// 
    /// </value>
    public static HSLoom Current
    {
        get
        {
            if (!_initialized) Initialize();
            return _current;
        }
    }

    static bool _initialized;
    static int _threadId = -1;


    public static void Initialize()
    {
        var go = !_initialized;

        if (go && _threadId != -1 && _threadId != Thread.CurrentThread.ManagedThreadId)
            return;

        if (go)
        {
            var g = new GameObject(nameof(HSLoom))
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            GameObject.DontDestroyOnLoad(g);
            _current = g.AddComponent<HSLoom>();
            Component.DontDestroyOnLoad(_current);
            _initialized = true;
            _threadId = Thread.CurrentThread.ManagedThreadId;
            _factory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(8));
        }
    }

    private List<Action> _actions = new List<Action>();

    public class DelayedQueueItem
    {
        public float time;
        public Action action;
    }

    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    private const int MaxLossyCapacity = 7;

    private static readonly HTCache<int, Action> lossyQueue = new HTCache<int, Action>(MaxLossyCapacity);

    /// <summary>
    ///  有损更新，按每秒任务更新数量来通知，部分更新被舍弃
    /// </summary>
    /// <param name="action"></param>
    public static void LossyQueueOnMainThread(Action action)
    {
        lock (lossyQueue)
        {
            lossyQueue.Set(action.GetHashCode(), action);
        }
    }

    public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

    public static long TimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - UnixEpoch;
        return (long) ts.TotalMilliseconds;
    }


    /// <summary>
    /// 延迟后在主线程上对动作进行排队 
    /// </summary>
    /// <param name='action'>
    /// The action to run
    /// </param>
    /// <param name='time'>
    /// The amount of time to delay
    /// </param>
    public static void QueueOnMainThread(Action action, float time = 0f)
    {
        if (time != 0)
        {
            lock (Current._delayed)
            {
                Current._delayed.Add(new DelayedQueueItem {time = Time.time + time, action = action});
            }
        }
        else
        {
            lock (Current._actions)
            {
                Current._actions.Add(action);
            }
        }
    }

    /// <summary>
    /// 在另一个非主线程上运行一个动作
    /// </summary>
    /// <param name='action'>
    /// The action to execute on another thread
    /// </param>
    public static void RunAsync(Action action)
    {
        _factory?.StartNew(
            action,
            TaskCreationOptions.LongRunning
        );
    }


    // Update is called once per frame
    void Update()
    {
        lock (lossyQueue)
        {
            if (lossyQueue.Count > 0)
            {
                ICollection<Action> values = lossyQueue.Values;
                values.First()();
                lossyQueue.Clean();
            }
        }

        lock (_actions)
        {
            if (_actions.Count > 0)
            {
                var actions = new List<Action>();
                actions.AddRange(_actions);
                _actions.Clear();
                foreach (var a in actions)
                {
                    a();
                }
            }
        }

        lock (_delayed)
        {
            if (_delayed.Count > 0)
            {
                List<DelayedQueueItem> delayed = _delayed.Where(d => d.time <= Time.time).ToList();
                if (delayed.Count > 0)
                {
                    foreach (var dc in delayed)
                    {
                        dc.action();
                        _delayed.Remove(dc);
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        _initialized = false;
    }

    public void Clean()
    {
        lock (_actions)
        {
            _actions.Clear();
        }

        lock (_delayed)
        {
            _delayed.Clear();
        }
    }

    public void Dispose()
    {
        Destroy(gameObject);
        _initialized = false;
    }
}