using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfFileDialogs.WeakEvents;

namespace WpfFileDialogs
{
    public sealed class LazyTreeViewExtension : DependencyObject, IWeakEventListener
    {
        private static readonly LazyTreeViewExtension _lazyTreeViewExtension;
        private readonly Dictionary<Type, Action<ILazyCollection>> _actionByEventType;

        public bool IsLazyLoading
        {
            get { return (bool)GetValue(IsLazyLoadingProperty); }
            set { SetValue(IsLazyLoadingProperty, value); }
        }
        public static readonly DependencyProperty IsLazyLoadingProperty = DependencyProperty.RegisterAttached("IsLazyLoading", typeof(bool), typeof(LazyTreeViewExtension),
        new PropertyMetadata(false, IsLazyLoadingChanged));

        static LazyTreeViewExtension()
        {
            _lazyTreeViewExtension = new LazyTreeViewExtension();
        }

        private LazyTreeViewExtension()
        {
            _actionByEventType = new Dictionary<Type, Action<ILazyCollection>>
            {
                { typeof(TreeViewCollapsedRoutedEventManager), ItemCollapsed },
                { typeof(TreeViewExpandedRoutedEventManager), ItemExpanded }
            };
        }

        public static bool GetIsLazyLoading(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsLazyLoadingProperty);
        }
        public static void SetIsLazyLoading(DependencyObject obj, bool value)
        {
            obj.SetValue(IsLazyLoadingProperty, value);
        }

        static private void IsLazyLoadingChanged(DependencyObject s, DependencyPropertyChangedEventArgs args)
        {
            var treeView = s as TreeView;
            if (treeView == null)
                return;

            var wasEnable = (bool)args.OldValue;
            var isEnabled = (bool)args.NewValue;

            if (wasEnable)
            {
                Disable(treeView);
            }
            if (isEnabled)
            {
                Enable(treeView);
            }
        }
        private static void Enable(TreeView treeView)
        {
            TreeViewExpandedRoutedEventManager.AddListener(treeView, _lazyTreeViewExtension);
            TreeViewCollapsedRoutedEventManager.AddListener(treeView, _lazyTreeViewExtension);
        }
        private static void Disable(TreeView treeView)
        {
            TreeViewExpandedRoutedEventManager.RemoveListener(treeView, _lazyTreeViewExtension);
            TreeViewCollapsedRoutedEventManager.RemoveListener(treeView, _lazyTreeViewExtension);
        }

        public bool ReceiveWeakEvent(Type eventType, object sender, EventArgs e)
        {
            Action<ILazyCollection> handleAction;
            if (!_actionByEventType.TryGetValue(eventType, out handleAction))
            {
                return false;
            }

            var lazyCollectionComponent = GetLazyCollectionComponent(e);
            if (lazyCollectionComponent == null)
                return false;

            handleAction(lazyCollectionComponent);

            return true;
        }
        private ILazyCollection GetLazyCollectionComponent(EventArgs e)
        {
            var routedEventArgs = e as RoutedEventArgs;

            var treeViewItem = routedEventArgs?.OriginalSource as TreeViewItem;
            var lazyCollectionComponent = treeViewItem?.Header as ILazyCollection;

            return lazyCollectionComponent;
        }
        private void ItemExpanded(ILazyCollection lazyCollectionComponent)
        {
            lazyCollectionComponent.ExpandSubItems();
        }
        private void ItemCollapsed(ILazyCollection lazyCollectionComponent)
        {
            lazyCollectionComponent.IgnoreSubItems();
        }
    }
}
