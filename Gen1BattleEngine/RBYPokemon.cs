using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleEngine;

namespace Gen1BattleEngine
{
    public class RBYPokemon : Pokemon
    {
       

        //There is an EV calculation formula, just leave that out

        public RBYPokemon(UserPokemon poke,int lvl)
        {
            int specialstat = poke.Spatk > poke.Spdef ? poke.Spatk : poke.Spdef;
            _level = lvl;

            _name = poke.Name;
            _HealthPoints = poke.HP;

            _outBattleAtk = poke.Atk;
            _inBattleAtk = _outBattleAtk;

            _outBattleDef = poke.Def;
            _inBattleDef = _outBattleDef;

            _outBattleSpecialAtk = specialstat;
            _inBattleSpecialAtk = _outBattleSpecialAtk;

            _outBattleSpecialDef = specialstat;
            _inBattleSpecialDef = _outBattleSpecialDef;

            _outBattleSpeed = poke.Speed;
            _inBattleSpeed = _outBattleSpeed;

            _outBattleAccuracy = 1;
            _outBattleEvasion = 1;
            _inBattleAccuracy = 1;
            _inBattleEvasion = 1;

            _critChance = (poke.Speed *100) >> 9 ;
            _type1 = poke.Type1;
            _type2 = poke.Type2;
            _moveset = new List<Attacks>();
            _moveset = poke.Moves;
            _subHPRemaining = _HealthPoints;
        }

        public RBYPokemon(RBYPokemon poke, int lvl)
        {
            int specialstat = poke._outBattleSpecialAtk;
            _level = lvl;

            _name = poke.Name;
            _HealthPoints = poke._HealthPoints;

            _outBattleAtk = poke._outBattleAtk;
            _inBattleAtk = _outBattleAtk;

            _outBattleDef = poke._outBattleDef;
            _inBattleDef = _outBattleDef;

            _outBattleSpecialAtk = specialstat;
            _inBattleSpecialAtk = _outBattleSpecialAtk;

            _outBattleSpecialDef = specialstat;
            _inBattleSpecialDef = _outBattleSpecialDef;

            _outBattleSpeed = poke.Speed;
            _inBattleSpeed = _outBattleSpeed;

            _outBattleAccuracy = 1;
            _outBattleEvasion = 1;
            _inBattleAccuracy = 1;
            _inBattleEvasion = 1;

            _critChance = poke.Speed / 2;
            _type1 = poke.Type1;
            _type2 = poke.Type2;
            _moveset = new List<Attacks>();
            _moveset = poke.Moveset;
            _subHPRemaining = _HealthPoints;

            _atkStage = poke.AtkStage;
            _defStage = poke.DefStage;
            _specialAtkStage = poke.SpecialAtkStage;
            _specialDefStage = poke.SpecialDefStage;
            _speedStage = poke.SpeedStage;
            _accuracyStage = poke.AccuracyStage;
            _evasionStage = poke.EvasionStage;
        }

        public override void NormalPoisonDmg()
        {
            int dmg = _HealthPoints / 16;
            dmg = dmg == 0 ? 1 : dmg;
            Displayer.RelayMessage($"{Name} took poison damage");
            TakeDamage(dmg);            
        }

        public override int LeechSeedDmg()
        {
            int dmg = (_HealthPoints * T) / 16;
            dmg = dmg == 0 ? 1 : dmg;
            Displayer.RelayMessage($"{Name} took poison damage");
            TakeDamage(dmg);            
            return dmg;
        }

        public override void BurnDmg()
        {
            int dmg = _HealthPoints / 16;
            dmg = dmg == 0 ? 1 : dmg;
            Displayer.RelayMessage($"{Name} took burn damage");
            TakeDamage(dmg);
            
        }
    }
}
