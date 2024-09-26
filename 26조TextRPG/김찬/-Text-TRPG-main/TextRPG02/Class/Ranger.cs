using TextRPG02.Character.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;
using TextRPG02.Item;

namespace TextRPG02.Class
{
    public class Ranger : IClass
    {
        public string ClassName => "레인저";

        public List<IItem> DefaultItems { get; }
        public Ranger()
        {
            DefaultItems = new List<IItem>
            {
                ItemRepository.GetItemByName("롱소드"),
                ItemRepository.GetItemByName("가죽 갑옷")
            };
        }
        public void ApplyClassStats(ICharacter character)
        {
            character.Health += 10f;
            character.Magicka += 5f;
            character.Strength += 5;
            character.Agility += 10;
            character.Intelligence += 0;
        }
    }

}
