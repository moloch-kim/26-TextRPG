using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character.Interface;
using TextRPG02.Race.Interface;

namespace TextRPG02.Race
{
    public class Elf : IRace
    {
        public string RaceName => "엘프";

        public void ApplyRaceStats(ICharacter character)
        {
            character.Health = 10f;
            character.Magicka = 20f;
            character.Strength = 6;
            character.Agility = 10;
            character.Intelligence = 14;
        }
    }
}
