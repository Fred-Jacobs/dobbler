using System;
using System.Collections.Generic;
using System.Linq;

namespace Dobbler.Core
{
    public class Dobbler
    {
        public static int[] AuthorizedPlanarSizes => authorizedPlanarSizes;

        // must be a prime number
        private static readonly int[] authorizedPlanarSizes = new int[] { 2, 3, 5, 7, 11/*, 13, 17 ...*/ };

        public Dobbler()
        {
        }

        /// <summary>
        /// Creates the dobble deck.
        /// </summary>
        /// <param name="planarSize">Size of the planar.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public List<List<int>> CreateDobbleDeck(int planarSize)
        {
            /* http://www.madore.org/~david/weblog/d.2015-07-15.2307.html#d.2015-07-15.2307
             * http://www.madore.org/~david/images/dobble-cards.jpg
             * http://www.bibmath.net/forums/viewtopic.php?id=5134
             * http://math.stackexchange.com/questions/464932/dobble-card-game-mathematical-background
             *****************************************************************************************/

            if (!authorizedPlanarSizes.Any(ps => ps == planarSize))
            {
                throw new ArgumentException($"{nameof(planarSize)} argument must be in [{string.Join(", ", authorizedPlanarSizes)}]");
            }

            var deck = new List<List<int>>();

            // basic ones : lines and columns
            /* http://www.bibmath.net/forums/viewtopic.php?id=5134
             * x = b
             * y = c
             */
            for (var x = 0; x < planarSize; x++)
            {
                var card = new List<int>();

                for (var y = 0; y < planarSize; y++)
                {
                    card.Add(x * planarSize + y);
                }

                // leak
                card.Add(planarSize * planarSize);

                deck.Add(card);
            }

            // diagonals
            /* http://www.bibmath.net/forums/viewtopic.php?id=5134
             * y = x + b
             * y = 2x + b
             * ...
             * y = (planarSize - 1) * x + b
             */
            for (var x = 0; x < planarSize; x++)
            {
                for (var y = 0; y < planarSize; y++)
                {
                    var card = new List<int>();

                    for (var fn = 0; fn < planarSize; fn++)
                    {
                        card.Add(fn * planarSize + (y + x * fn) % planarSize);
                    }

                    // leak
                    card.Add(planarSize * planarSize + 1 + x);

                    deck.Add(card);
                }
            }

            // add a last for traversal on leaks
            var _card = new List<int>();

            // leaks
            for (var i = 0; i < planarSize + 1; i++)
            {
                _card.Add(planarSize * planarSize + i);
            }

            deck.Add(_card);

            return deck;
        }

        /// <summary>
        /// Checks the dobble deck.
        /// </summary>
        /// <param name="deck">The deck.</param>
        /// <param name="output">if set to <c>true</c> [output].</param>
        /// <returns></returns>
        public bool CheckDobbleDeck(IReadOnlyList<List<int>> deck, bool output = true)
        {
            if (output)
            {
                Console.WriteLine(new string('-', 25));

                for (var i = 0; i < deck.Count(); i++)
                {
                    Console.WriteLine($"{i + 1,3}: {string.Join(" ", deck[i].Select(x => $"{x,3}"))}");
                }

                Console.WriteLine(new string('-', 25));
            }

            var valid = true;

            // iterate all cards
            for (var i = 0; i < deck.Count(); i++)
            {
                // compare with all other cards (not yet tested)
                for (var j = i + 1; j < deck.Count(); j++)
                {
                    var similarSymbol = 0;

                    // compare each symbols
                    for (var k = 0; k < deck[i].Count(); k++)
                    {
                        for (var l = 0; l < deck[j].Count(); l++)
                            if (deck[i][k] == deck[j][l])
                            {
                                similarSymbol++;
                            }
                    }

                    if (similarSymbol == 1)
                        continue;

                    if (output)
                        Console.WriteLine("Error on cards : {0} {1}", i + 1, j + 1);

                    valid = false;
                }
            }

            return valid;
        }
    }
}