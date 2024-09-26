using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;

namespace TextRPG02.Race
{
    public class Spriggan : IRace
    {
        public string RaceName => "스프리건";

        public void ApplyRaceStats(ICharacter character)
        {
            character.Health = 20f;
            character.Magicka = 10f;
            character.Strength = 6;
            character.Agility = 18;
            character.Intelligence = 6;
        }
    }
}
