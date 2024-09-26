using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;

namespace TextRPG02.Race
{
    public class Ogre : IRace
    {
        public string RaceName => "오거";

        public void ApplyRaceStats(ICharacter character)
        {
            character.Health = 30f;
            character.Magicka = 5f;
            character.Strength = 14;
            character.Agility = 12;
            character.Intelligence = 6;
        }
    }
}
