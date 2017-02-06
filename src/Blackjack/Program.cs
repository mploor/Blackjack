using Blackjack.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.UTF8Encoding.UTF8;

            string userChoice;
            int playerMoney = 1000;
            int currentWager = 0;
            bool quit = false;
            bool done = false;
            bool splitFlag = false;

            var rnd = new Random();

            while (!quit)
            {
                // Initialize player and dealer hands
                Hand playerHand = new Hand();
                playerHand.Cards = new List<Card>();
                Hand dealerHand = new Hand();
                dealerHand.Cards = new List<Card>();
                Hand splitHand = new Hand();
                splitHand.Cards = new List<Card>();
                // Track cards already dealt
                Hand usedCards = new Hand();
                usedCards.Cards = new List<Card>();

                // Get wager
                Utility.DisplayHeader();
                Console.WriteLine("You have {0} chips", playerMoney);
                Console.WriteLine("How many chips would you like to wager?");
                while (!int.TryParse(Console.ReadLine(), out currentWager))
                {
                    Console.WriteLine("Invalid entry. Please enter a whole number of chips to wager");
                }
                if (currentWager > playerMoney)
                {
                    Console.WriteLine("Your wager exceeds your chip total.  However, we are happy to extend you credit!");
                    Thread.Sleep(3000);
                }

                // Deal cards
                playerHand.DealCard(rnd, usedCards);
                dealerHand.DealCard(rnd, usedCards);
                playerHand.DealCard(rnd, usedCards);
                dealerHand.DealCard(rnd, usedCards, true);

                // Player plays
                done = false;
                splitFlag = false;
                // Check if can be split
                if (playerHand.Cards[0].Face == playerHand.Cards[1].Face)
                {
                    Utility.DisplayTable(playerHand, dealerHand, currentWager);
                    Console.Write("Do you want to (h)it, (s)tand, (d)ouble down, or split? ");
                    userChoice = Console.ReadLine();
                    // Play split hand if chosen
                    if (userChoice.ToLower() == "split")
                    {
                        splitFlag = true;
                        splitHand.Cards.Add(playerHand.Cards[0]);
                        playerHand.Cards.RemoveAt(0);
                        playerHand.DealCard(rnd, usedCards);
                        splitHand.DealCard(rnd, usedCards);
                        bool done1 = false;
                        while (playerHand.HandTotal() < 21 && !done1)
                        {
                            Utility.DisplayTable(playerHand, dealerHand, currentWager, splitHand);
                            Console.Write("Hand 1: Do you want to (h)it or (s)tand? ");
                            done1 = Utility.PlayerMove(rnd, playerHand, usedCards);
                        }
                        while (splitHand.HandTotal() < 21 && !done)
                        {
                            Utility.DisplayTable(playerHand, dealerHand, currentWager, splitHand);
                            Console.Write("             Hand 2: Do you want to (h)it or (s)tand? ");
                            done = Utility.PlayerMove(rnd, splitHand, usedCards);
                        }
                    }
                    else if (userChoice == "d" || userChoice == "dd")
                    {
                        currentWager *= 2;
                        Utility.PlayerMove(rnd, playerHand, usedCards, "h");
                        done = true;
                    }
                    else
                    {
                        done = Utility.PlayerMove(rnd, playerHand, usedCards, userChoice);
                    }
                }
                else if (playerHand.HandTotal() < 21)
                {
                    // Play regular, non-split hand
                    Utility.DisplayTable(playerHand, dealerHand, currentWager);
                    Console.Write("Do you want to (h)it, (s)tand, or (d)ouble down? ");
                    userChoice = Console.ReadLine();
                    if (userChoice == "d" || userChoice == "dd")
                    {
                        currentWager *= 2;
                        Utility.PlayerMove(rnd, playerHand, usedCards, "h");
                        done = true;
                    }
                    else
                    {
                        done = Utility.PlayerMove(rnd, playerHand, usedCards, userChoice);
                    }
                }

                // Continue regular hand until stand or bust
                while (playerHand.HandTotal() < 21 && !done)
                {
                    Utility.DisplayTable(playerHand, dealerHand, currentWager);
                    Console.Write("Do you want to (h)it or (s)tand? ");
                    done = Utility.PlayerMove(rnd, playerHand, usedCards);
                }

                // Dealer plays
                while ((dealerHand.HandTotal() < 17 && playerHand.HandTotal() < 22) && !(playerHand.HandTotal() == 21 && playerHand.Cards.Count == 2))
                {
                    dealerHand.DealCard(rnd, usedCards);
                }

                // Determine winner and display final hands
                dealerHand.UnHideAll();
                if (splitFlag)
                {
                    Utility.DisplayTable(playerHand, dealerHand, currentWager, splitHand);
                    Console.Write("Hand 1: ");
                    playerMoney = Utility.FindWinner(playerHand, dealerHand, currentWager, playerMoney);
                    Console.Write("Hand 2: ");
                    playerMoney = Utility.FindWinner(splitHand, dealerHand, currentWager, playerMoney);
                } else
                {
                    Utility.DisplayTable(playerHand, dealerHand, currentWager);
                    playerMoney = Utility.FindWinner(playerHand, dealerHand, currentWager, playerMoney);
                }

                // Ask whether to continue
                if (playerMoney > 0)
                {
                    Console.WriteLine("You have {0} chips", playerMoney);
                    Console.WriteLine();
                    Console.WriteLine("Enter to deal again, or q to quit");
                } else if (playerMoney == 0)
                {
                    Console.WriteLine("You're out of money!  Thanks for playing.  Please come back when you have more money.");
                    quit = true;
                } else
                {
                    Console.WriteLine("You owe us {0} chips. A collector will be by shortly to break your legs.", -1 * playerMoney);
                    quit = true;
                }

                userChoice = Console.ReadLine();
                if (userChoice.ToLower() == "q" || userChoice.ToLower() == "quit")
                {
                    quit = true;
                }
            }
        }
    }
}
