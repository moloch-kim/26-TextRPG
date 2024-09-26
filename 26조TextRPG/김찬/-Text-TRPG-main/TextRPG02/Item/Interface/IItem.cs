using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG02.Item.Interface
{
    public interface IItem
    {
        int ID { get; }
        string Name { get; }
        string Description { get; }
        int Value { get; }
    }
}
