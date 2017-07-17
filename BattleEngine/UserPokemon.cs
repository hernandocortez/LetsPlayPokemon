using BattleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BattleEngine
{

    public class UserPokemon
    {
        int _HealthDV;
        int _AtkDV;
        int _DefDV;
        int _SpAtkDV;
        int _SpDefDV;
        int _SpeedDV;

        int _HealthEV;
        int _AtkEV;
        int _DefEV;
        int _SpAtkEV;
        int _SpDefEV;
        int _SpeedEV;
        int _Level;

        int _hP;
        int _atk;
        int _def;
        int _spatk;
        int _spdef;
        int _speed;
        List<string> _moveNames;
        List<Attacks> _moves;
        private string _name;
        private ElementTypes _type1;
        private ElementTypes _type2;

        public int HealthDV
        {
            get
            {
                return _HealthDV;
            }

            set
            {
                _HealthDV = value;
            }
        }

        public int AtkDV
        {
            get
            {
                return _AtkDV;
            }

            set
            {
                _AtkDV = value;
            }
        }

        public int DefDV
        {
            get
            {
                return _DefDV;
            }

            set
            {
                _DefDV = value;
            }
        }

        public int SpAtkDV
        {
            get
            {
                return _SpAtkDV;
            }

            set
            {
                _SpAtkDV = value;
            }
        }

        public int SpDefDV
        {
            get
            {
                return _SpDefDV;
            }

            set
            {
                _SpDefDV = value;
            }
        }

        public int SpeedDV
        {
            get
            {
                return _SpeedDV;
            }

            set
            {
                _SpeedDV = value;
            }
        }

        public int HealthEV
        {
            get
            {
                return _HealthEV;
            }

            set
            {
                _HealthEV = value;
            }
        }

        public int AtkEV
        {
            get
            {
                return _AtkEV;
            }

            set
            {
                _AtkEV = value;
            }
        }

        public int DefEV
        {
            get
            {
                return _DefEV;
            }

            set
            {
                _DefEV = value;
            }
        }

        public int SpAtkEV
        {
            get
            {
                return _SpAtkEV;
            }

            set
            {
                _SpAtkEV = value;
            }
        }

        public int SpDefEV
        {
            get
            {
                return _SpDefEV;
            }

            set
            {
                _SpDefEV = value;
            }
        }

        public int SpeedEV
        {
            get
            {
                return _SpeedEV;
            }

            set
            {
                _SpeedEV = value;
            }
        }

        public int Level
        {
            get
            {
                return _Level;
            }

            set
            {
                _Level = value;
            }
        }

        public int HP
        {
            get
            {
                return _hP;
            }

            set
            {
                _hP = value;
            }
        }

        public int Atk
        {
            get
            {
                return _atk;
            }

            set
            {
                _atk = value;
            }
        }

        public int Def
        {
            get
            {
                return _def;
            }

            set
            {
                _def = value;
            }
        }

        public int Spatk
        {
            get
            {
                return _spatk;
            }

            set
            {
                _spatk = value;
            }
        }

        public int Spdef
        {
            get
            {
                return _spdef;
            }

            set
            {
                _spdef = value;
            }
        }

        public int Speed
        {
            get
            {
                return _speed;
            }

            set
            {
                _speed = value;
            }
        }

        public List<string> MoveNames
        {
            get
            {
                return _moveNames;
            }

            set
            {
                _moveNames = value;
            }
        }

        [XmlIgnore]
        public List<Attacks> Moves
        {
            get
            {
                return _moves;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        
        public ElementTypes Type1
        {
            get
            {
                return _type1;
            }

            set
            {
                _type1 = value;
            }
        }

        
        public ElementTypes Type2
        {
            get
            {
                return _type2;
            }

            set
            {
                _type2 = value;
            }
        }

        public UserPokemon()
        { }

        public UserPokemon(int lvl, PokemonDefinition def,List<Attacks> MoveDex, List<int> MoveIndeces)
        {
            _moveNames = new List<string>();
            _moves = new List<Attacks>();
            _Level = lvl;

            _HealthEV = 63;
            _AtkEV = 63;
            _DefEV = 63;
            _SpAtkEV = 63;
            _SpDefEV = 63;
            _SpeedEV = 63;
            _Level = 63;
            _name = def.PokemonName;
            _type1 = def.FirstType;
            _type2 = def.SecondType;

            _HealthDV = 13;
            _AtkDV = 13;
            _DefDV = 13;
            _SpAtkDV = 13;
            _SpDefDV = 13;
            _SpeedDV = 13;

            _atk = CalculateStat(def.Attack, _AtkDV, _AtkEV, lvl, 1);
            _def = CalculateStat(def.Defense, _DefDV, _DefEV, lvl, 1);
            _spatk = CalculateStat(def.SpAtk, _SpAtkDV, _SpAtkEV, lvl, 1);
            _spdef = CalculateStat(def.SpDef, _SpDefDV, _SpDefEV, lvl, 1);
            _speed = CalculateStat(def.Speed, _SpeedDV, _SpeedEV, lvl, 1);
            _hP = CalculateHealth(def.HP, _HealthDV, _HealthEV, lvl);

            foreach(var i in MoveIndeces)
            {
                _moveNames.Add(MoveDex[i].Name.Replace(' ','_'));
                _moves.Add(MoveDex[i]);
            }

        }

        public void PrepareForBattle(List<Attacks> atks,int lvl,PokemonDefinition def)
        {
            _moves = new List<Attacks>();
            foreach(var m in _moveNames)
            {
                var move = m.Replace('_', ' ');
                _moves.Add(atks.First(p => p.Name == move));
            }


            _atk = CalculateStat(def.Attack, _AtkDV, _AtkEV, lvl, 1);
            _def = CalculateStat(def.Defense, _DefDV, _DefEV, lvl, 1);
            _spatk = CalculateStat(def.SpAtk, _SpAtkDV, _SpAtkEV, lvl, 1);
            _spdef = CalculateStat(def.SpDef, _SpDefDV, _SpDefEV, lvl, 1);
            _speed = CalculateStat(def.Speed, _SpeedDV, _SpeedEV, lvl, 1);
            _hP = CalculateHealth(def.HP, _HealthDV, _HealthEV, lvl);
        }

        public int CalculateStat(int basestat, int iv, int ev, int lvl, int nature)
        { return ((2 * basestat + iv + ev) * lvl / 100 + 5) * nature; }

        public int CalculateHealth(int basestat, int iv, int ev, int lvl)
        { return ((2 * basestat + iv + ev) * lvl / 100 + lvl + 10); }


        
    }
}
