using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.Classes
{
    public class Card
    {
        public string Suit { get; set; }
        public string Face { get; set; }
        public int  Value { get; set; }
        public bool Hidden { get; set; }

        public Card(int suit, int face, bool hide = false)
        {
            string[] suits = { "C", "D", "H", "S" };
            string[] faces = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            Suit = suits[suit];
            Face = faces[face];
            if (face < 10) {
                Value = face + 1;
            } else
            {
                Value = 10;
            }
            Hidden = hide;
        }
    }
}
