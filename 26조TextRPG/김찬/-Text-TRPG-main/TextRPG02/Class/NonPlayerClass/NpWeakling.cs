using TextRPG02.Character.Interface;
using TextRPG02.Class.Interface;
using TextRPG02.Item.Interface;
using TextRPG02.Item;

namespace TextRPG02.Class.NonPlayerClass
{
    public class NpWeakling : IClass
    {
        public string ClassName => "약골";

        public List<IItem> DefaultItems { get; }
        public NpWeakling()
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
            int num = random.Next(0, 10);

            character.Health -= 5f;
            character.Magicka += 0f;
            character.Strength -= 2;
            character.Agility -= 2;
            character.Intelligence += 0;
            character.Gold += num;
        }
    }

}
