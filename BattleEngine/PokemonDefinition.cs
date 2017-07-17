using BattleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
   

    public class PokemonDefinition
    {
        private string PokeName;
        private ElementTypes Type1;
        private ElementTypes Type2;
        private int BaseHealth;
        private int BaseAttack;
        private int BaseDefense;
        private int BaseSpAtk;
        private int BaseSpDef;
        private int BaseSpeed;
        private int Index;
        private List<int> AvailableMoves;

        public PokemonDefinition(string name, string type1, string type2, int hp, int atk, int def, int spatk, int spdef, int speed, int number, List<int> moves)
        {
            AvailableMoves = new List<int>();
            AvailableMoves = moves;
            PokeName = name;
            Type1 = (ElementTypes)Enum.Parse(typeof(ElementTypes), type1);
            Type2 = (ElementTypes)Enum.Parse(typeof(ElementTypes), type2);
            BaseHealth = hp;
            BaseAttack = atk;
            BaseDefense = def;
            BaseSpAtk = spatk;
            BaseSpDef = spdef;
            BaseSpeed = speed;
            Index = number;
        }

        public int Number
        { get { return Index; } set { Index = value; } }

        public string PokemonName
        { get { return PokeName; } }

        public ElementTypes FirstType
        { get { return Type1; } }

        public ElementTypes SecondType
        { get { return Type2; } }

        public int HP
        { get { return BaseHealth; } }

        public int Attack
        { get { return BaseAttack; } }

        public int Defense
        { get { return BaseDefense; } }

        public int SpAtk
        { get { return BaseSpAtk; } }

        public int SpDef
        { get { return BaseSpDef; } }

        public int Speed
        { get { return BaseSpeed; } }

        public List<int> Moves
        { get { return AvailableMoves; } }

        
    }
}
