using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;

namespace TextRPG02.Race
{
    public class Human : IRace
    {
        public string RaceName => "인간";

        public void ApplyRaceStats(ICharacter character)
        {
            character.Health = 20f;
            character.Magicka = 10f;
            character.Strength = 10;
            character.Agility = 10;
            character.Intelligence = 10;
        }
    }
}
