using BattleEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PokemonBattle
{
    static class FileImporter
    {
        public static List<PokemonDefinition> GetPokesFromCSV(string filename)
        {
            var sr = new StreamReader(filename);
            List<PokemonDefinition> pokes = new List<PokemonDefinition>();
            int index = 1;

            while(!sr.EndOfStream)
            {
                var entry = sr.ReadLine();

                var data = Parser(entry);
                var hp = int.Parse(data[3]);
                var atk = int.Parse(data[4]);
                var def = int.Parse(data[5]);
                var spatk = int.Parse(data[6]);
                var spdef = int.Parse(data[7]);
                var speed = int.Parse(data[8]);
                var raw_moves = Parser(data[9],'|');

                List<int> moves = new List<int>();

                foreach(var m in raw_moves)
                { moves.Add(int.Parse(m)); }

                var poke = new PokemonDefinition(data[0], data[1].ToUpper(), data[2].ToUpper(), hp, atk, def, spatk, spdef, speed, index,moves);
                pokes.Add(poke);
                index++;
            }

            return pokes;
        }

        public static List<Attacks> GetPokemonMovesFromCSV(string filename)
        {
            var sr = new StreamReader(filename);
            List<Attacks> all_attacks = new List<Attacks>();

            while(!sr.EndOfStream)
            {
                var entry = sr.ReadLine();
                var data = Parser(entry);

                int index = int.Parse(data[0]);
                var name = data[1];
                var type = data[2];
                var pp = int.Parse(data[4]);
                int accuracy = (int)(100 * double.Parse(data[5]));
                var basepower = int.Parse(data[3]);
                var info = Int64.Parse(data[6], System.Globalization.NumberStyles.HexNumber);

                var attack = new Attacks(info, name, index, accuracy, pp, type.ToUpper(),basepower);
                all_attacks.Add(attack);
            }

            return all_attacks;
        }

        public static List<UserPokemon> Deserialize(string filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<UserPokemon>));
            TextReader reader = new StreamReader(filename);
            object obj = deserializer.Deserialize(reader);
            reader.Close();
            return (List<UserPokemon>)obj;
        }

        public static float[,] ImportTypeMatrix(string filename)
        {
            var sr = new StreamReader(filename);
            float[,] matrix = new float[18, 18];
            int i = 0;

            sr.ReadLine();

            while (!sr.EndOfStream)
            {
                var data = Parser(sr.ReadLine());
                for (int j = 1; j < data.Count; j++)
                    matrix[i, j-1] = float.Parse(data[j]);
                i++;
            }
            return matrix;
        }



        public static List<string> Parser(string entry, char delimiter = ',')
        {
            var output = new List<string>();

            if (!entry.Contains(delimiter))
                return new List<string> { entry };

            while (entry.Contains(delimiter))
            {
                int index = entry.IndexOf(delimiter);
                output.Add(entry.Substring(0, index));
                entry = entry.Substring(index + 1);
            }
            output.Add(entry);
            return output;
        }

        public static void ValidateFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.Write($"This file {filename} does not exist. Program will terminate");
                Console.Read();
                Environment.Exit(0);
            }
        }

    }
}
