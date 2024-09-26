using TextRPG02.Character.Interface;

namespace TextRPG02.Item.Interface
{
    public interface IConsumable : IItem
    {
        void Consume(ICharacter character);
    }
}
