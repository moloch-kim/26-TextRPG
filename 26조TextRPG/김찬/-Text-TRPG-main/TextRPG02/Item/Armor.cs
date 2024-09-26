using TextRPG02.Character.Interface;
using TextRPG02.Character;
using TextRPG02.Item.Interface;

namespace TextRPG02.Item
{
    public class Armor : IEquippable
    {
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }
        public int ArmorClass { get; }
        public bool isEquip { get; set; }
        public int Value { get; }
        public int Obstruct { get; }

        public Armor(string name, string description, int defense, int id, int value, int obstruct)
        {
            ID = id;
            Name = name;
            Description = description;
            ArmorClass = defense;
            Value = value;
            Obstruct = obstruct;
        }

        public void Equip(ICharacter character)
        {
            if (character is Player player)
            {
                if (player.EquippedArmor != null)
                {
                    player.EquippedArmor.Unequip(player);
                }

                player.AC += ArmorClass;
                player.EquippedArmor = this;
                Console.WriteLine($"{Name}을(를) 장착했습니다.");
            }
        }
        public void Unequip(ICharacter character)
        {
            if (character is Player player)
            {
                player.AC -= ArmorClass;
                player.EquippedArmor = null;
                Console.WriteLine($"{Name}을(를) 해제했습니다.");
            }
        }

    }

    

}
