using TextRPG02.Character.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;
using TextRPG02.Item;

namespace TextRPG02.Class
{
    public class Rogue : IClass
    {
        public string ClassName => "로그";

        public List<IItem> DefaultItems { get; }
        public Rogue()
        {
            DefaultItems = new List<IItem>
            {
                ItemRepository.GetItemByName("단검"),
                ItemRepository.GetItemByName("가죽 갑옷")
            };
        }
        public void ApplyClassStats(ICharacter character)
        {
            character.Health += 0f;
            character.Magicka += 0f;
            character.Strength += 0;
            character.Agility += 15;
            character.Intelligence += 5;
        }
    }

}
