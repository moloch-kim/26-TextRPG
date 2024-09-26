using TextRPG02.Character;
using TextRPG02.Character.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item;
using TextRPG02.Item.Interface;

namespace TextRPG02.Class
{
    public class Fighter : IClass
    {
        public string ClassName => "파이터";

        public List<IItem> DefaultItems { get; }
        public Fighter()
        {
            DefaultItems = new List<IItem>
            {
                ItemRepository.GetItemByName("롱소드"),
                ItemRepository.GetItemByName("사슬 갑옷")
            };
        }

        public void ApplyClassStats(ICharacter character)
        {
            character.Health += 10f;
            character.Magicka += 0f;
            character.Strength += 10;
            character.Agility += 5;
            character.Intelligence += 0;

        }
    }

}
