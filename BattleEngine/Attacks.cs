using BattleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class Attacks
    {
        long _information;
        string _name;
        int _index;
        int _accuracy;
        int _pp;
        int _basepower;
        long _movetype;


        ElementTypes move_type;
        AttackType _attack;


        public Attacks(long info, string name, int index, int accuracy, int pp,string type,int basepower)
        {
            _information = info;
            _name = name;
            _index = index;
            _accuracy = accuracy;
            _pp = pp;
            _basepower = basepower;
            move_type = (ElementTypes)Enum.Parse(typeof(ElementTypes), type);

            _movetype = (info >> 60) & 0xF;
            _attack = GetAttackType(_movetype, info);
        }


        private AttackType GetAttackType(long atk_bits,long info)
        {
            switch(atk_bits)
            {
                case 0: return new DamageMove(info);
                case 1: return new StatusMove(info);
                case 2: return new StatBoost(info); 
                case 3: return new Recover();
                default: return new DamageMove(info);              
            }            
        }

        public ElementTypes MoveType
        { get { return move_type; } }

        public long Information
        { get { return _information; } }

        public string Name
        { get { return _name; } }
        
        public int BasePower
        { get { return _basepower; } }

        public int Index
        { get { return _index; } }

        public int Accuracy
        { get { return _accuracy; } }

        public int PP
        { get { return _pp; } }

        public AttackType Attack
        { get { return _attack; } }

        public long MoveClass
        { get { return _movetype; } }
    }
}
