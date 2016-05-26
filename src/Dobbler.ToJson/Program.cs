using Dobbler.Core;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Dobbler.ToJson
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dobbler = new Core.Dobbler();

            foreach (var size in Core.Dobbler.AuthorizedPlanarSizes)
            {
                var deck = dobbler.CreateDobbleDeck(size);

                if (!dobbler.CheckDobbleDeck(deck))
                    continue;

                foreach (var ints in deck)
                {
                    ints.Shuffle();
                }

                deck.Shuffle();

                var path = $@"dobble_planar_{size}.json";
                using (var file = File.CreateText(path))
                {
                    Console.WriteLine($"Write {path} to disk.");
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, deck);
                }
            }

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}