using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;


namespace TextRPG02.Character
{
    public class Player : ICharacter // 플레이어 클래스
    {
        public string Name { get; set; }
        public IRace Race { get; set; }
        public IClass Class { get; set; }
        public float Health { get; set; }
        public float Magicka { get; set; }
        //
        public int Level { get; set; }
        public int Exp {  get; set; }
        //
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        //
        public int StrengthModifier => (Strength - 10) / 2;
        public int AgilityModifier => (Agility - 10) / 2;
        public int IntelligenceModifier => (Intelligence - 10) / 2;
        //
        public int ATK { get; set; }
        public int AC { get; set; }

        public IEquippable EquippedWeapon { get; set; }
        public IEquippable EquippedArmor { get; set; }

        public int Gold { get; set; }

        public List<IItem> Inventory { get; set; }

        

        public Player(string name, IRace race, IClass classtype, int level)
        {
            Name = name;
            Race = race;
            Class = classtype;
            Level = level;

            ATK = 0; // 초기화
            AC = 0;  // 초기화

            Gold = 100;

            Inventory = new List<IItem>();

            Race.ApplyRaceStats(this);
            Class.ApplyClassStats(this);

            foreach (var item in Class.DefaultItems)
            {
                if (item != null)
                {
                    Inventory.Add(item);
                    if (item is IEquippable equippable)
                    {
                        equippable.Equip(this);
                    }
                }
                else
                {
                    Console.WriteLine("기본 아이템 중 일부를 찾을 수 없습니다.");
                }
            }

            DisplayCharacterInfo();
        }

        public void DisplayCharacterInfo()
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"이름: {Name}");
            Console.WriteLine($"종족: {Race.RaceName}, 클래스: {Class.ClassName}");
            Console.WriteLine($"레벨: {Level}");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"체력: {Health}, 마력: {Magicka}");
            Console.WriteLine($"공격력: {ATK}, 방어력: {AC}");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"힘: {Strength}, 민첩: {Agility}, 지능: {Intelligence}");
            Console.WriteLine("---------------------------------------------");
            if (EquippedWeapon != null)
                Console.WriteLine($"장착된 무기: {EquippedWeapon.Name}");
            if (EquippedArmor != null)
                Console.WriteLine($"장착된 방어구: {EquippedArmor.Name}");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
        }
    }
}
