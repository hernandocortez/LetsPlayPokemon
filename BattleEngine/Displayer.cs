using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public static class Displayer
    {
        public static void DisplayBattleChoice()
        {
            Console.WriteLine();
            Console.WriteLine("Select Action");
            Console.WriteLine("0..Attack");
            Console.WriteLine("1..Switch");
        }

        public static void DisplayMovesToSelect(Pokemon ThePoke)
        {
            Console.WriteLine("Pick a move");
            for (int i = 0; i < ThePoke.Moveset.Count; i++)
                Console.WriteLine($"[{i}] {ThePoke.Moveset[i].Name}   Type: {SentenceCase(ThePoke.Moveset[i].MoveType.ToString())}");
        }

        public static void DisplayPokesToSwitch(List<Pokemon> AliveOnes)
        {
            Console.WriteLine("Pick poke to switch");
            for (int i = 0; i < AliveOnes.Count; i++)
                Console.WriteLine($"[{i}] {AliveOnes[i].Name} {AliveOnes[i].SubHPRemaining}/{AliveOnes[i].HealthPoints}");
        }
        
        public static void DisplayCurrentPokemonInfo(Pokemon user, Pokemon cpu)
        {
            Console.WriteLine();
            Console.WriteLine("{0,-50}{1,-20}", "Trainer", "Opponent");
            Console.WriteLine("{0,-50}{1,-20}", $"Your pokemon {user.Name}", $"His pokemon {cpu.Name}");
            Console.WriteLine("{0,-50}{1,-20}", $"{user.SubHPRemaining}/{user.HealthPoints}", $"{cpu.SubHPRemaining}/{cpu.HealthPoints}");
            Console.WriteLine("{0,-50}{1,-20}", $"Current Status: {user.CurrentStatus.ToString()}", $"Current Status: {cpu.CurrentStatus.ToString()}");
            Console.WriteLine("{0,-50}{1,-20}", $"Substitute:", $"Substitute");

        }

        public static void ClearScreen()
        { Console.Clear(); }

        public static void ReceiveDamage(string caller, string name,int dmg)
        { Console.Write($"{caller} {name} took {dmg} damage");
            Console.ReadLine();

        }

        public static void HealDamage(string calling, string name, int dmg)
        { Console.Write($"{calling} {name} healed {dmg} damage"); Console.ReadLine(); }

        public static void Fainted(string calling, string name)
        { Console.Write($"{calling} {name} fainted"); Console.ReadLine(); }

        public static void FullHealth(string calling, string name)
        { Console.Write($"{calling} {name} is already at full health"); Console.ReadLine(); }
         
        public static void FlinchMsg(Pokemon poke)
        { Console.Write($"{poke.OwnerCall} {poke.Name} flinched"); Console.ReadLine(); }

        public static void ParalyzedMsg(Pokemon poke)
        { Console.Write($" {poke.OwnerCall} {poke.Name} was fully paralyzed"); Console.ReadLine(); }

        public static void AsleepMsg(Pokemon poke)
        { Console.Write($"{poke.OwnerCall} {poke.Name} is sleeping"); Console.ReadLine(); }

        public static void WokeUpMsg(Pokemon poke)
        { Console.Write($"{poke.OwnerCall} {poke.Name} wokeup"); Console.ReadLine(); }

        public static void Enter()
        { Console.ReadLine(); }


        public static string SentenceCase(string input)
        {
            if (input.Length < 1)
                return input;

            string sentence = input.ToLower();
            return sentence[0].ToString().ToUpper() +
               sentence.Substring(1);
        }

        public static void Recharging()
        { Console.Write("Recharging"); Console.ReadLine(); }

        public static void StillWrapped()
        { Console.Write("The foe is still going"); Console.ReadLine(); }

        public static void WrappedUp()
        { Console.Write("He is free!"); Console.ReadLine(); }

        public static void ConfusedHitMsg()
        { Console.Write("The pokemon hit himself in confusion"); Console.ReadLine(); }

        public static void OutOfConfusionMsg()
        { Console.Write("He snapped out of confusion"); Console.ReadLine(); }

        public static void TwoTurn()
        { Console.Write("He is getting ready for a vigorous attack"); Console.ReadLine(); }

        public static void ShoutAttack(string calling, string name, string poke)
        { Console.Write($"{calling} {poke} used {name}!"); Console.ReadLine(); }

        public static void SuperEffective(double multipler)
        {


            if (multipler > 2)
                Console.Write("Super Effective!!");
            else if (multipler == 0)
                Console.Write("It doesnt affect");
            else if (multipler < 1)
                Console.Write("Not very effective..");
            else if (multipler == 1)
                return;
            Console.ReadLine();
        }

        
        public static void RecoilHappened(string calling, string name)
        { Console.Write($"{calling} {name} took recoil damage"); Console.ReadLine(); }

        public static void AttackContiunes()
        { Console.Write("The attack continues"); Console.ReadLine(); }

        public static void Seeded(string calling, string name)
        { Console.Write($"{calling} {name} was seeded"); Console.ReadLine(); }

        public static void Poisoned(bool badpoison, Pokemon poke)
        {
            if (badpoison)
                Console.Write($" {poke.OwnerCall} {poke.Name} was badly poisoned");
            else
                Console.Write("{poke.OwnerCall} {poke.Name} was poisoned");
            Console.ReadLine();
        }

        public static void Burned(string calling,string name)
        { Console.Write($"{calling} {name} was burned"); Console.ReadLine(); }

        public static void Confused(string calling ,string name)
        { Console.Write($"{calling}  {name} was confused"); Console.ReadLine(); }

        public static void PutAsleep(string calling,string name)
        { Console.Write($"{calling}  {name} was put to sleep"); Console.ReadLine(); }

        public static void Paralyzed(string calling,string name)
        { Console.Write($"{calling}  {name} was paralyzed and may not attack"); Console.ReadLine(); }

        public static void Freezed(string calling, string name)
        {
            Console.Write($"{calling} {name} was forzen. Sorry");
            Console.ReadLine();
        }

        public static void Statup(int offset, string attack)
        {
            bool down = false;
            bool sharp = false;

            if (offset < 0)
                down = true;
            if (Math.Abs(offset) > 1)
                sharp = true;

            string msg = attack + (sharp ? " Sharply " : "") + (down ? "was lowered" : "Rose");
            Console.Write(msg);
            Console.ReadLine(); 
        }

        public static void TooFat(int stage)
        {
            if (stage > 0)
                Console.Write("Cannot go any higher..");
            else
                Console.Write("Cannot go any lower..");
            Console.ReadLine();
        }

        public static void AtkMissed()
        {
            Console.Write("Attack Missed!");
            Console.ReadLine();
        }

        public static void TrainerSwitchedPokemon(string trainer_name, string pokemon_name)
        { Console.Write($"{trainer_name} withdrew pokemon and switched to {pokemon_name}");
            Console.ReadLine();
        }

        public static void RelayMessage(string msg)
        {
            Console.Write(msg);
            Console.ReadLine();
        }

    }
}
