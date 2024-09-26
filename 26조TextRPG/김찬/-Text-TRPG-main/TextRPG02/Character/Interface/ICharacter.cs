using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Race.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;

namespace TextRPG02.Character.Interface
{
    public interface ICharacter // 캐릭터 인터패이스 
    {
        string Name { get; set; }
        IRace Race { get; set; }
        IClass Class { get; set; }
        float Health { get; set; }
        float Magicka { get; set; }
        int Level { get; set; }
        //
        int Strength { get; set; }
        int Agility { get; set; }
        int Intelligence { get; set; }
        //
        int StrengthModifier { get; }
        int AgilityModifier { get; }
        int IntelligenceModifier { get; }

        //
        int ATK { get; set; }
        int AC { get; set; }
        //


        IEquippable EquippedWeapon { get; set; }
        IEquippable EquippedArmor { get; set; }

        int Gold { get; set; }

    }
}
