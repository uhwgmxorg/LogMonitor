using System.Collections.ObjectModel;

namespace WpfFileDialogs
{
    public abstract class Item : ILazyCollection
    {
        protected readonly ItemFactory ItemFactory;

        protected readonly ObservableCollection<Item> _subItems = new ObservableCollection<Item>();
        public string PathName { get; set; }
        public string SPattern { get; set; }

        protected Item(ItemFactory itemFactory, string path, string sPattern)
        {
            ItemFactory = itemFactory;
            PathName = path;
            SPattern = sPattern;
        }

        public ObservableCollection<Item> SubItems => _subItems;

        public string ShortName
        {
            get
            {
                var ret = System.IO.Path.GetFileName(PathName);
                if (string.IsNullOrEmpty(ret))
                    ret = PathName;
                return ret;
            }
        }
        public string LongName
        {
            get
            {
                var ret = PathName;
                if (string.IsNullOrEmpty(ret))
                    ret = PathName;
                return ret;
            }
        }

        public abstract void ExpandSubItems();

        public abstract void IgnoreSubItems();
    }
}
