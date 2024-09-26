using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;

namespace TextRPG02.Race.NonPlayerRace
{
    public class NpLizardman : IRace
    {
        public string RaceName => "리자드맨";

        public void ApplyRaceStats(ICharacter character)
        {
            character.Health = 20f;
            character.Magicka = 10f;
            character.Strength = 10;
            character.Agility = 12;
            character.Intelligence = 5;
        }
    }
}
