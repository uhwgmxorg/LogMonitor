using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WpfFileDialogs
{
    class ItemWithChildren : Item
    {
        protected static readonly Item DummyItem;

        static ItemWithChildren()
        {
            DummyItem = new ItemWithNoChildren(null, "Loading, please wait ....","");
        }

        public ItemWithChildren(ItemFactory dirFactory, string path, string sPattern)
          : base(dirFactory, path, sPattern)
        {
            _subItems.Add(DummyItem);
        }

        public override void IgnoreSubItems()
        {
            _subItems.Clear();
            _subItems.Add(DummyItem);
        }

        public async override void ExpandSubItems()
        {
            _subItems.Clear();
            var dirs = await GetDirs();
            FillSubItems(dirs);
            var files = await GetFiles();
            FillSubItems(files);
        }

        private async Task<IEnumerable<Item>> GetDirs()
        {
            return await Task<IEnumerable<Item>>.Factory.StartNew(() =>
            {
                Thread.Sleep(5);
                IEnumerable<Item> subItems;
                try
                {
                    subItems = System.IO.Directory.GetDirectories(PathName).Select(ItemFactory.CreateItem);
                }
                catch (IOException)
                {
                    subItems = new List<Item>(0);
                }
                return subItems;
            });
        }

        private async Task<IEnumerable<Item>> GetFiles()
        {
            return await Task<IEnumerable<Item>>.Factory.StartNew(() =>
            {
                Thread.Sleep(5);
                IEnumerable<Item> items;
                try
                {
                    items = GetFilesWithFilter(PathName, SPattern, SearchOption.TopDirectoryOnly).Select(ItemFactory.CreateItem);
                }
                catch (IOException)
                {
                    items = new List<Item>(0);
                }
                return items;
            });
        }


        private void FillSubItems(IEnumerable<Item> content)
        {
            foreach (var item in content)
                _subItems.Add(item);
        }

        private static string[] GetFilesWithFilter(string sourceFolder, string filters, System.IO.SearchOption searchOption)
        {
            return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption)).ToArray();
        }

    }
}