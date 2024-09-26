using TextRPG02.Character.Interface;
using TextRPG02.Item.Interface;

namespace TextRPG02.Item
{
    public class Potion : IConsumable
    {
        public int ID { get; }
        public int Potiontype { get; }
        public string Name { get; }
        public string Description { get; }
        public float Effect { get; }
        public int Value { get; }

        public Potion(string name, string description, float effect, int id, int value, int type)
        {
            ID = id;
            Potiontype = type;
            Name = name;
            Description = description;
            Effect = effect;
            Value = value;
        }

        public void Consume(ICharacter character)
        {
            if (Potiontype == 1)
            {
                character.Health += Effect;
                Console.WriteLine($"{Name}을(를) 사용하여 체력이 {Effect}만큼 회복되었습니다.");
            }
            else if (Potiontype == 2)
            {
                character.Strength += (int)Effect;
                Console.WriteLine($"{Name}을(를) 사용하여 힘이 {Effect}만큼 회복되었습니다!");
            }
            
        }
    }

}
