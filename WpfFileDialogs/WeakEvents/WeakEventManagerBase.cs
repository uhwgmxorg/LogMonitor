using System;
using System.Windows;

namespace WpfFileDialogs.WeakEvents
{
    /// <summary>
    /// Base class for weak event managers.
    /// </summary>
    /// <typeparam name="TManager">The type of the weak event manager.</typeparam>
    /// <typeparam name="TSource">The type of the source of the event.</typeparam>
    /// <remarks>
    /// Based on the idea presented by <a href="http://wekempf.spaces.live.com/">William Kempf</a>
    /// in his article <a href="http://wekempf.spaces.live.com/blog/cns!D18C3EC06EA971CF!373.entry">WeakEventManager</a>.
    /// </remarks>
    public abstract class WeakEventManagerBase<TManager, TSource> : WeakEventManager
      where TManager : WeakEventManagerBase<TManager, TSource>, new()
      where TSource : class
    {
        public static TManager Current
        {
            get
            {
                Type managerType = typeof(TManager);
                var manager = GetCurrentManager(managerType) as TManager;
                if (manager == null)
                {
                    manager = new TManager();
                    SetCurrentManager(managerType, manager);
                }
                return manager;
            }
        }

        public static void AddListener(TSource source, IWeakEventListener listener)
        {
            Current.ProtectedAddListener(source, listener);
        }

        public static void RemoveListener(TSource source, IWeakEventListener listener)
        {
            Current.ProtectedRemoveListener(source, listener);
        }

        protected override sealed void StartListening(object source)
        {
            StartListeningTo(source as TSource);
        }

        protected abstract void StartListeningTo(TSource source);

        protected override sealed void StopListening(object source)
        {
            StopListeningTo(source as TSource);
        }

        protected abstract void StopListeningTo(TSource source);
    }
}
