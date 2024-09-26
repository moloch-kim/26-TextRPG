using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character.Interface;
using TextRPG02.Item.Interface;

namespace TextRPG02.Class.Interface
{
    public interface IClass
    {
        string ClassName { get; }
        void ApplyClassStats(ICharacter character);
        List<IItem> DefaultItems { get; }
    }

}
