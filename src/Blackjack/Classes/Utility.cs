using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.Classes
{
    public class Utility
    {
        public static void DisplayCard(Card card)
        {
            var ucodes = new Dictionary<string, string>()
            {
                { "C", "\u2663" },
                { "D", "\u2666" },
                { "H", "\u2665" },
                { "S", "\u2660" }
            };
            Console.BackgroundColor = ConsoleColor.White;
            if (card.Suit == "H" || card.Suit == "D")
            {
                Console.ForegroundColor = ConsoleColor.Red;
            } else
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            if (card.Hidden)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("XX");
            }
            else
            {
                Console.Write(card.Face + ucodes[card.Suit]);
            }
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void DisplayHand(Hand hand)
        {
            foreach (Card card in hand.Cards)
            {
                DisplayCard(card);
                Console.Write(" ");
            }
        }

        public static void DisplayHeader()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            Console.WriteLine("     Matthew's Casino");
            Console.WriteLine("        BLACKJACK    ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void DisplayTable(Hand playerHand, Hand dealerHand, int currentWager)
        {
            DisplayHeader();
            Console.Write("    Dealer Hand: ");
            DisplayHand(dealerHand);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("    Player Hand: ");
            DisplayHand(playerHand);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("     Current bet: {0}", currentWager);
            Console.WriteLine();
        }

        public static void DisplayTable(Hand playerHand, Hand dealerHand, int currentWager, Hand splitHand)
        {
            DisplayHeader();
            Console.Write("               Dealer Hand: ");
            DisplayHand(dealerHand);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("  Player Hand1: ");
            DisplayHand(playerHand);
            Console.Write("    Player Hand2: ");
            DisplayHand(splitHand);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("     Current bet: {0}       Current bet: {0}", currentWager);
            Console.WriteLine();
        }

        public static bool PlayerMove(Random rnd, Hand playerHand, Hand usedCards, string choice = "")
        {
            string userChoice;
            if (choice == "")
            {
                userChoice = Console.ReadLine();
            } else
            {
                userChoice = choice;
            }

            if (userChoice.ToLower() == "h" || userChoice.ToLower() == "hit")
            {
                playerHand.DealCard(rnd, usedCards);
            }
            if (userChoice.ToLower() == "s" || userChoice.ToLower() == "stand")
            {
                return true;
            } else
            {
                return false;
            }
        }

        public static int FindWinner(Hand playerHand, Hand dealerHand, int currentWager, int playerMoney)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (playerHand.HandTotal() == dealerHand.HandTotal())
            {
                Console.WriteLine("PUSH");
            }
            else if (playerHand.HandTotal() == 21)
            {
                if (playerHand.Cards.Count == 2)
                {
                    Console.WriteLine("Blackjack! You win");
                } else
                {
                    Console.WriteLine("21! You win");
                }
                playerMoney += currentWager;
            }
            else if (dealerHand.HandTotal() == 21)
            {
                if (dealerHand.Cards.Count == 2)
                {
                    Console.WriteLine("Blackjack! Dealer wins");
                } else
                {
                    Console.WriteLine("21! Dealer wins");
                }
                playerMoney -= currentWager;
            }
            else if (playerHand.HandTotal() > 21)
            {
                Console.WriteLine("Player BUSTS - you lose");
                playerMoney -= currentWager;
            }
            else if (dealerHand.HandTotal() > 21)
            {
                Console.WriteLine("Dealer BUSTS - you win");
                playerMoney += currentWager;
            }
            else if (playerHand.HandTotal() > dealerHand.HandTotal())
            {
                Console.WriteLine("You win");
                playerMoney += currentWager;
            }
            else
            {
                Console.WriteLine("Dealer wins");
                playerMoney -= currentWager;
            }
            Console.ForegroundColor = ConsoleColor.Black;
            return playerMoney;
        }
    }
}
