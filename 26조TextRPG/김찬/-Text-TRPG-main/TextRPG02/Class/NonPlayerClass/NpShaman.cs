using TextRPG02.Character.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;
using TextRPG02.Item;

namespace TextRPG02.Class.NonPlayerClass
{
    public class NpShaman : IClass
    {
        public string ClassName => "주술사";

        public List<IItem> DefaultItems { get; }
        public NpShaman()
        {
            DefaultItems = new List<IItem>
            {
                ItemRepository.GetItemByName("롱소드"),
                ItemRepository.GetItemByName("사슬갑옷")
            };
        }
        public void ApplyClassStats(ICharacter character)
        {
            Random random = new Random();
            int num = random.Next(0, 100);

            character.Health += 0f;
            character.Magicka += 10f;
            character.Strength += 0;
            character.Agility += 5;
            character.Intelligence += 10;
            character.Gold += num;
        }
    }

}
