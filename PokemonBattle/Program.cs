using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gen1BattleEngine;
using BattleEngine;
using System.Media;

namespace PokemonBattle
{
    
class Program
    {

        static void Main(string[] args)
        {

            var typematrix = FileImporter.ImportTypeMatrix("Gen2MoreMatrix.csv");
            //ShowTypeMatrix(typematrix);
            Battle();
            
        }

        private static void ShowTypeMatrix(float [,] matrix)
        {
            for(int i=0; i<18; i++)
            {
                for(int j=0; j<18; j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        private static void Music()
        {
            SoundPlayer sp = new SoundPlayer("Gen1Battle.wav");
            sp.PlayLooping();
        }


        private static void Battle()
        {
            var AllPokes = FileImporter.GetPokesFromCSV(@"Gen1\Pokedex\updated_csv_pokedex.csv");
            var AllMoves = FileImporter.GetPokemonMovesFromCSV(@"Gen1\Gen1MoveList.csv");

            var p = FileImporter.Deserialize("user.rby");

            foreach (var poke in p)
                poke.PrepareForBattle(AllMoves, 100, AllPokes.First(k => k.PokemonName == poke.Name));

            var player = new List<RBYPokemon>();
            var cpu = new List<RBYPokemon>();


            foreach (var poke in p)
                player.Add(new RBYPokemon(poke, 100));

            foreach (var poke in p)
                cpu.Add(new RBYPokemon(poke, 100));

            var Gen1 = new Engine(player, cpu, FileImporter.ImportTypeMatrix("Gen2MoreMatrix.csv"),AllMoves);
            
            Gen1.StartBattle();
        }
    }
}
