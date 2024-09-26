using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Item.Interface;

namespace TextRPG02.Item
{
    public static class ItemRepository
    {
        public static  List<IItem> Allitems { get; private set; }

        public static List<Potion> AllPotions { get; private set; }

        public static List<Armor> AllArmors { get; private set; }

        public static List<Weapon> AllWeapons { get; private set; }


        static ItemRepository()
        {

            AllArmors = new List<Armor>()
            {
            new("로브", "매우 편하지만 방어력은 없습니다", 0 , 101, 10, 0),
            new("가죽 갑옷", "가볍고 유연한 가죽 갑옷 입니다.", 102 , 2, 20, 2),
            new("사슬 갑옷", "균형잡힌 사슬 갑옷 입니다.", 6 , 103, 60, 6),
            new("판금 갑옷", "매우 튼튼한 갑옷 입니다.", 10, 104, 100, 10),
            };

            AllPotions = new List<Potion>()
            {
            new("체력의 물약", "물약의 효과만큼 건강해집니다.", 5, 201, 15, 1),
            new("체력의 영약", "물약의 효과만큼 건강해집니다.", 20, 202, 60, 1),
            new("힘의 영약", "물약의 효과만큼 힘이 상승합니다.", 2, 203, 100, 2)
            };

            AllWeapons = new List<Weapon>()
            {
            new("롱소드", "유연하고 믿음직한 장검입니다.", 5 , 301, 50, 6),
            new("레이피어", "허를 찌르는대 유용한 장검입니다.", 5, 302, 50, 6),
            new("단검", "자기를 지키기엔 부족함이 없습니다.", 2, 303, 20, 10),
            new("전투도끼", "강력한 위력의 전투도끼입니다.", 8, 304, 50, 4),
            new("쿼터스태프", "얕보이기 쉽지만 유용한 성능을 지녔습니다.", 4, 305, 50, 6),
            };

            Allitems = new List<IItem>();
            Allitems.AddRange(AllArmors);
            Allitems.AddRange(AllPotions);
            Allitems.AddRange(AllWeapons);
        }
        
        public static IItem GetItemByID(int id)
        {
            return Allitems.FirstOrDefault(item => item.ID == id);
        }

        public static IItem GetItemByName(string name)
        {
            return Allitems.FirstOrDefault(item => item.Name == name);
        }


        public static Potion GetRandomPotion()
        {
            Random random = new Random();

            int index = random.Next(0, AllPotions.Count -1 );
            return AllPotions[index];

        }

    }
}
