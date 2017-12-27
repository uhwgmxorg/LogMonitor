using System.Windows;
using System.Windows.Controls;

namespace WpfFileDialogs.WeakEvents
{
    internal class TreeViewCollapsedRoutedEventManager : WeakEventManagerBase<TreeViewCollapsedRoutedEventManager, TreeView>
    {
        protected override void StartListeningTo(TreeView source)
        {
            source.AddHandler(TreeViewItem.CollapsedEvent, new RoutedEventHandler(DeliverEvent));
        }

        protected override void StopListeningTo(TreeView source)
        {
            source.RemoveHandler(TreeViewItem.CollapsedEvent, new RoutedEventHandler(DeliverEvent));
        }
    }
}
