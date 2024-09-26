using TextRPG02.Character.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;
using TextRPG02.Item;

namespace TextRPG02.Class.NonPlayerClass
{
    public class NpScoundrel : IClass
    {
        public string ClassName => "불한당";

        public List<IItem> DefaultItems { get; }
        public NpScoundrel()
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
            int num = random.Next(0, 50);

            character.Health += 5f;
            character.Magicka += 0f;
            character.Strength += 5;
            character.Agility += 5;
            character.Intelligence += 0;
            character.Gold += num;
        }
    }

}
