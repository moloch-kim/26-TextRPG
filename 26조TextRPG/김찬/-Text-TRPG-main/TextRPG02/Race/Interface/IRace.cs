using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG02.Character.Interface;

namespace TextRPG02.Race.Interface
{
    public interface IRace
    {
        string RaceName { get; }
        void ApplyRaceStats(ICharacter character);
    }
}
