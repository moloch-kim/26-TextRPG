using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG02
{
    public class Dice
    {
        private static Random random = new Random();

        public static int Roll(int side)
        {
            return random.Next(1, side + 1);
        }

        public static int RollMultiple(int dicenum, int sides)
        {
            int total = 0;
            for (int i = 0; i < dicenum; i++)
            {
                total += Roll(sides);
            }
            return total;
        }
        
        


    }

    

}
