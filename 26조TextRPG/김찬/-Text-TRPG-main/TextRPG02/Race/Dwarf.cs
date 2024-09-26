using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;

namespace TextRPG02.Race
{
    public class Dwarf : IRace
    {
        public string RaceName => "드워프";

        public void ApplyRaceStats(ICharacter character)
        {
            character.Health = 25f;
            character.Magicka = 15f;
            character.Strength = 10;
            character.Agility = 8;
            character.Intelligence = 10;
        }
    }
}
