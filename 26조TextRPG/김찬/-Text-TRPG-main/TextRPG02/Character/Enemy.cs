using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;

namespace TextRPG02.Character
{
    public class Enemy : ICharacter // 적 클래스
    {
        public string Name { get; set; }
        public IRace Race { get; set; }
        public IClass Class { get; set; }
        public float Health { get; set; }
        public float Magicka { get; set; }
        public int Level { get; set; }
        //
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        //
        public int StrengthModifier { get; }
        public int AgilityModifier { get; }
        public int IntelligenceModifier { get; }

        //

        public int ATK { get; set; }
        public int AC { get; set; }

        public IEquippable EquippedWeapon { get; set; }
        public IEquippable EquippedArmor { get; set; }

        public int Gold { get; set; }

        public Enemy(string name, IRace race, IClass classtype, int level)
        {
            Name = $"{race.RaceName} {classtype.ClassName}";
            Race = race;
            Class = classtype;
            Level = level;

            Gold = 0;

            ATK = 0; // 초기화
            AC = 0;  // 초기화

            Race.ApplyRaceStats(this);
            Class.ApplyClassStats(this);

            IncountertText();

            // Enemy만의 추가 설정
        }

        public void IncountertText()
        {
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine($"{Name} 을(를) 마주쳤습니다!");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine();
        }

        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"이름 : {Name} ");
            Console.WriteLine($"종족: {Race.RaceName}, 클래스: {Class.ClassName}");
            Console.WriteLine($"레벨: {Level}");
            Console.WriteLine($"체력: {Health}, 마력: {Magicka}");
            Console.WriteLine($"공격력: {ATK}, 방어력: {AC}");
            Console.WriteLine($"힘: {Strength}, 민첩: {Agility}, 지능: {Intelligence}");
        }
    }
}
