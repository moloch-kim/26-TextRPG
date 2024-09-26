using TextRPG02.Character.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;
using TextRPG02.Item;

namespace TextRPG02.Class
{
    public class Wizard : IClass
    {
        public string ClassName => "위저드";

        public List<IItem> DefaultItems { get; }
        public Wizard()
        {
            DefaultItems = new List<IItem>
            {
                ItemRepository.GetItemByName("쿼터스태프"),
                ItemRepository.GetItemByName("로브")
            };
        }
        public void ApplyClassStats(ICharacter character)
        {
            character.Health += 0f;
            character.Magicka += 10f;
            character.Strength += 0;
            character.Agility += 0;
            character.Intelligence += 15;
        }
    }

}
