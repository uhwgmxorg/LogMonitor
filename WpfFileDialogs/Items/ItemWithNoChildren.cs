namespace WpfFileDialogs
{
    class ItemWithNoChildren : Item
    {
        public ItemWithNoChildren(ItemFactory itemFactory, string path, string sPattern)
          : base(itemFactory, path, sPattern)
        {
        }

        public override void ExpandSubItems()
        {
        }

        public override void IgnoreSubItems()
        {
        }
    }
}