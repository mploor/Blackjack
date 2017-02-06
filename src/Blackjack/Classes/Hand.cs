using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.Classes
{
    public class Hand
    {
        public List<Card> Cards { get; set; }

        public int HandTotal()
        {
            int total = 0;
            bool aceSeen = false;
            // Calcuate soft total
            foreach (Card card in Cards)
            {
                total += card.Value;
                if (card.Face == "A" && !aceSeen)
                {
                    total += 10;
                    aceSeen = true;
                }
            }

            // if soft total over 21, use hard total
            if (total > 21)
            {
                total = 0;
                foreach (Card card in Cards)
                {
                    total += card.Value;
                }
            }
            return total;
        }

        // Add new random card to hand, checking that it hasn't already been used
        public void DealCard(Random rnd, Hand usedCards, bool hidden = false)
        {
            bool unique = true;
            Card newCard;
            do
            {
                unique = true;
                newCard = new Card(rnd.Next(4), rnd.Next(13), hidden);
                foreach (Card card in usedCards.Cards)
                {
                    if (newCard.Face == card.Face && newCard.Suit == card.Suit)
                    {
                        unique = false;
                    }
                }
            } while (!unique);
            Cards.Add(newCard);
            usedCards.Cards.Add(newCard);
        }

        // Unhide dealer's hidden card
        public void UnHideAll()
        {
            foreach (Card card in Cards)
            {
                card.Hidden = false;
            }
        }


    }
}
