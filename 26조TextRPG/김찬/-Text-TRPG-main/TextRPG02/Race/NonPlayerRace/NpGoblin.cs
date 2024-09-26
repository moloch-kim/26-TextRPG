using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;

namespace TextRPG02.Race.NonPlayerRace
{
    public class NpGoblin : IRace
    {
        public string RaceName => "고블린";

        public void ApplyRaceStats(ICharacter character)
        {
            character.Health = 10f;
            character.Magicka = 5f;
            character.Strength = 5;
            character.Agility = 5;
            character.Intelligence = 5;
        }
    }
}
