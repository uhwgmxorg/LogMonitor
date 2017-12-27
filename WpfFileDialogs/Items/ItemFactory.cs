using System;
using System.IO;
using System.Linq;

namespace WpfFileDialogs
{
    public class ItemFactory
    {
        string SPattern { get; set; }

        public ItemFactory(string sPattern)
        {
            SPattern = sPattern;
        }

        public Item CreateItem(string path)
        {
            bool hasChildren;
            bool hasFiles;
            try
            {
                hasChildren = System.IO.Directory.GetDirectories(path).Any();
                hasFiles = System.IO.Directory.GetFiles(path).Any();
            }
            catch (UnauthorizedAccessException)
            {
                hasChildren = false;
                hasFiles = false;
            }
            catch (IOException)
            {
                hasChildren = false;
                hasFiles = false;
            }

            if (hasChildren || hasFiles)
                return new ItemWithChildren(this, path, SPattern);
            else
                return new ItemWithNoChildren(this, path, SPattern);
        }
    }
}
