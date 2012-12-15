using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToyBox
{
    public interface IPropertyListService
    {
        void Load(string propertyListName, EventHandler<SupplyDefaultValueEventArgs> handler); 
        PropertyList Get(string propertyListName);
        void Save(string propertyListName);
    }
}
