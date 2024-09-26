using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character.Interface;
using TextRPG02.Character;
using TextRPG02.Item.Interface;

namespace TextRPG02.Item
{
    public class Weapon : IEquippable
    {
        public int ID { get; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Power { get; set; }
        public bool isEquip { get; set; }
        public int Value { get; }
        public int Speed { get; }

        public Weapon(string name, string description, int power , int id , int value, int speed)
        {
            ID = id;
            Name = name;
            Description = description;
            Power = power;
            Value = value;
            Speed = speed;
        }

        public void Equip(ICharacter character)
        {
            if (character is Player player)
            {
                if (player.EquippedWeapon != null)
                {
                    player.EquippedWeapon.Unequip(player);
                }

                player.ATK += Power;
                player.EquippedWeapon = this;
                Console.WriteLine($"{Name}을(를) 장착했습니다.");
            }

        }

        public void Unequip(ICharacter character)
        {
            if (character is Player player)
            {
                player.ATK -= Power;
                player.EquippedWeapon = null;
                Console.WriteLine($"{Name}을(를) 해제했습니다.");
            }
        }
    }

}
