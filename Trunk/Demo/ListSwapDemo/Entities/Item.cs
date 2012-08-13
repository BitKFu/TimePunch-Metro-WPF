using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListSwapDemo.Entities
{
    public class Item
    {
        public Item(int order, string name)
        {
            SortOrder = order;
            Name = name;
        }

        public int SortOrder { get; private set; }
        public string Name { get; private set; }
    }
}
