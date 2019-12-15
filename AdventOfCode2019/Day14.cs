using System;
using System.Collections.Generic;
using System.Linq;
using ReStructure;

namespace AdventOfCode2019
{
    using Reactions = Dictionary<string, (List<(string name, int amount)> ingredients, int amount_produced)>;
    
    class Day14
    {
        private static readonly Reactions _reactions = new Reactions();

        public static void Part1(string[] input)
        {
            ParseInput(input);
            Console.WriteLine(CountOre("FUEL", 1));
        }

        public static void Part2(string[] input)
        {
            const long ORE = 1000000000000;

            long min = ORE / CountOre("FUEL", 1);
            long max = min * 2;

            while (max != min + 1)
            {
                long fuel = (min + max + 1) / 2;
                if (CountOre("FUEL", fuel) > ORE)
                {
                    max = fuel;
                }
                else
                {
                    min = fuel;
                }
            }

            Console.WriteLine(min);
        }

        private static long CountOre(string product, long quantity)
        {
            var need = new DefaultDictionary<string, long> {{product, quantity}};
            var work = new Stack<string>();
            work.Push(product);

            while (work.Any())
            {
                string next = work.Pop();
                if (next == "ORE") continue;

                long needed   = need[next];
                var  reaction = _reactions[next];

                long factor = (needed - 1) / reaction.amount_produced + 1;
                need[next] -= factor * reaction.amount_produced;

                foreach ((string name, long amount) in reaction.ingredients)
                {
                    need[name] += amount * factor;
                    if (need[name] > 0)
                    {
                        work.Push(name);
                    }
                }
            }

            return need["ORE"];
        }

        private static void ParseInput(string[] input)
        {
            var parse = input.Select(s => s.Split(" => "))
                             .Select(r => (ingredients: r[0], product: r[1]));

            foreach ((string ingredients, string product) in parse)
            {
                var list_of_ingredients = new List<(string, int)>();

                string product_name   = product.Split(' ')[1];
                int    product_amount = int.Parse(product.Split(' ')[0]);

                foreach (string ingredient in ingredients.Split(", "))
                {
                    string ingredient_name   = ingredient.Split(' ')[1];
                    int    ingredient_amount = int.Parse(ingredient.Split(' ')[0]);

                    list_of_ingredients.Add((ingredient_name, ingredient_amount));
                }

                _reactions[product_name] = (list_of_ingredients, product_amount);
            }
        }
    }
}
