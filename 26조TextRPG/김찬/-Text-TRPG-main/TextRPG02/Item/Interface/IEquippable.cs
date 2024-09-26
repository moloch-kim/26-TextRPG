using TextRPG02.Character.Interface;

namespace TextRPG02.Item.Interface
{
    public interface IEquippable : IItem
    {
        void Equip(ICharacter character);
        void Unequip(ICharacter character);
    }
}
