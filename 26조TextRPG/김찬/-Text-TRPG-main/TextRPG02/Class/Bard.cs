using TextRPG02.Character.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;
using TextRPG02.Item;

namespace TextRPG02.Class
{
    public class Bard : IClass
    {
        public string ClassName => "바드";

        public List<IItem> DefaultItems { get; }
        public Bard()
        {
            DefaultItems = new List<IItem>
            {
                ItemRepository.GetItemByName("레이피어"),
                ItemRepository.GetItemByName("가죽 갑옷")
            };
        }

        public void ApplyClassStats(ICharacter character)
        {
            character.Health += 10f;
            character.Magicka += 10f;
            character.Strength += 5;
            character.Agility += 5;
            character.Intelligence += 5;
        }
    }

}
