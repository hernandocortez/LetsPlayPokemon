using BattleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gen1BattleEngine
{
    public partial class Engine
    {
        public void BIDE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Attacker.Biding = true;
        }

        void CONVERSION()
        { }

        public void COUNTER(RBYPokemon Attacker, RBYPokemon Defender)
        {
            var type = MoveEnyclopedia[Attacker.OppPrevAtk].MoveType;

            if (type == ElementTypes.NORMAL || type == ElementTypes.FIGHTING && Attacker.LastDmg != 0)
                Defender.TakeDamage(Defender.LastDmg << 1, true, 1, true);
                
            
            Defender.NonDmgHit();
        }

        void DISABLE()
        { }

        public void DRAGON_RAGE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 100))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.NonDmgHit();
                return;
            }
            Defender.TakeDamage(40, false, 1, true);
        }

        public void DREAM_EATER(RBYPokemon Attacker, RBYPokemon Defender)
        {
            var query = MoveEnyclopedia.Where(p => p.Name == "DREAM EATER").ToList()[0];

            if (Defender.CurrentStatus == StatusConditionInflicted.Sleep)
                DamageTypeMove(Attacker, Defender, query, AttackingMoveSecondary.Absorbing);
            else
                Displayer.RelayMessage("The move failed");
        }
        
        public void EXPLOSION(RBYPokemon Attacker,RBYPokemon Defender)
        {
            var query = MoveEnyclopedia.Where(p => p.Name == "EXPLOSION").ToList()[0];
            DamageTypeMove(Attacker, Defender, query, AttackingMoveSecondary.None);

            Attacker.TakeDamage(Attacker.HealthPoints,true,1,true);
        }

        public void FISSURE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 30))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.NonDmgHit();
                return;
            }
            Defender.TakeDamage(Defender.SubHPRemaining,true,1,true);
        }

        public void FOCUS_ENERGY(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!Attacker.IsFocused)
            {
                Attacker.IsFocused = true;
                Displayer.RelayMessage("Getting pumped up");
            }
            else
                Displayer.RelayMessage("The move failed");
            Defender.NonDmgHit();
        }

        public void GUILLOTINE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 30))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.NonDmgHit();
                return;
            }
            Defender.TakeDamage(Defender.SubHPRemaining,true,1,true);
        }

        public void HAZE(RBYPokemon Attacker,RBYPokemon Defender)
        {
            Attacker.ResetStatBoost();
            Defender.ResetStatBoost();
            Attacker.IsMisted = false;
            Defender.IsMisted = false;
            Defender.NonDmgHit();
            if (Defender.CurrentStatus == StatusConditionInflicted.Freeze)
                Defender.UnFreez();
        }

        public void HORN_DRILL(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 30))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.LastDmg = 0;
                Defender.NonDmgHit();
                return;
            }
            Defender.TakeDamage(Defender.SubHPRemaining,true,1,true);
        }

        public void LIGHT_SCREEN(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Attacker.LightScreen = true;
            if (Attacker.Owner == 0)
                Trainer1.LightScreen = true;
            else
                Trainer2.LightScreen = true;
            Defender.NonDmgHit();
        }

        public void REFLECT(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Attacker.Reflect = true;
            if (Attacker.Owner == 0)
                Trainer1.Reflect = true;
            else
                Trainer2.Reflect = true;
            Defender.NonDmgHit();
        }

        public void METRONOME(RBYPokemon Attacker, RBYPokemon Defender)
        {
            var move = MoveEnyclopedia[RandomNumberGenerator(1, MoveEnyclopedia.Count)];
            Attacker.MetronomedMove = move.Index;
            DeliverAttack(Attacker, Defender, move,move.Attack.Movetype);
        }

        public void MIMIC(RBYPokemon Attacker, RBYPokemon Defender)
        {
            int replace_index;
            var move = MoveEnyclopedia[Defender.Moveset[RandomNumberGenerator(1, 4)].Index];
            if (Attacker.Owner == 0)
                replace_index = Trainer1.IndexPicked;
            else
                replace_index = Trainer2.IndexPicked;

            var query = Attacker.Moveset.Where(p => p.Name == move.Name);

            if(query.Count() == 0 )
            {
                Displayer.RelayMessage("The move failed");
                return;
            }

            Attacker.UsedMimic = true;
            Attacker.MimicIndex = replace_index;
            Attacker.OldMimic = Attacker.Moveset[replace_index];
            Attacker.Moveset[replace_index] = move;
            Defender.NonDmgHit();
        }

        public void MIRROR_MOVE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if(Attacker.OppPrevAtk != -1)
            {
                var move = MoveEnyclopedia[Defender.OppPrevAtk];
                if (move.Name != "MIRROR MOVE")
                {
                    DeliverAttack(Attacker, Defender, move,move.Attack.Movetype);
                    return;
                }
            }

            Displayer.RelayMessage("Move failed");
            Defender.NonDmgHit();
        }

        public void MIST(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Attacker.IsMisted = true;
            Defender.NonDmgHit();
        }

        public void NIGHT_SHADE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 100))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.NonDmgHit();
                return;
            }
            Defender.TakeDamage(Attacker.Level, true, 1, true);
        }

        public void PETAL_DANCE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            var query = MoveEnyclopedia.Where(p => p.Name == "PETAL DANCE").ToList()[0];
            DeliverAttack(Attacker, Defender,query,AttackingMoveSecondary.None);
            Attacker.StartDancing(RandomNumberGenerator(2, 3));
        }

        public void PSYWAVE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 100))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.NonDmgHit();
                return;
            }

            int rng = RandomNumberGenerator(10, 15);
            int dmg = (rng * Attacker.Level) / 10;
            Defender.TakeDamage(dmg, false, 1, true);

        }

        void RAGE(RBYPokemon Attacker, RBYPokemon Defender)
        { }

        public void REST(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if(Attacker.SubHPRemaining == Attacker.HealthPoints)
            {
                Displayer.RelayMessage("Pokemon is already healthy");
                return;
            }

            Attacker.PutToSleep(2);
            Attacker.HealDamage(Attacker.HealthPoints);
            Defender.NonDmgHit();
        }

        public void ROAR(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Displayer.RelayMessage("Move failed");
            Defender.NonDmgHit();
        }

        public void SEISMIC_TOSS(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 100))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.NonDmgHit();
                return;
            }
            Defender.TakeDamage(Attacker.Level, true, 1, true);
        }

        public void SELFDESTRUCT(RBYPokemon Attacker, RBYPokemon Defender)
        {
            var query = MoveEnyclopedia.Where(p => p.Name == "SELFDESTRUCT").ToList()[0];
            DamageTypeMove(Attacker, Defender, query, BattleEngine.AttackingMoveSecondary.None);
            Attacker.TakeDamage(Attacker.HealthPoints, true,1,true);
        }

        public void SONICBOOM(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 100))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.NonDmgHit();
                return;
            }
            Defender.TakeDamage(20, true, 1, true);
        }

        public void SUPER_FANG(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, 100))
            {
                Displayer.AtkMissed();
                Displayer.Enter();
                Defender.NonDmgHit();
                return;
            }
            Defender.TakeDamage(Attacker.SubHPRemaining / 2, true);
        }

        public void SUBSTITUTE(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Attacker.SetUpSubstitute();
            Defender.NonDmgHit();
        }

        public void SWIFT(RBYPokemon Attacker, RBYPokemon Defender)
        {
            var query = MoveEnyclopedia.Where(p => p.Name == "SWIFT").ToList()[0];
            DamageTypeMove(Attacker, Defender, query, AttackingMoveSecondary.None,true);
        }

        public void TELEPORT(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Displayer.RelayMessage("Move Failed");
            Defender.NonDmgHit();
        }

        public void SPLASH(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Displayer.RelayMessage("Move Failed");
            Defender.NonDmgHit();
        }

        public void THRASH(RBYPokemon Attacker, RBYPokemon Defender)
        {
            var query = MoveEnyclopedia.Where(p => p.Name == "THRASH").ToList()[0];
            DeliverAttack(Attacker, Defender, query,AttackingMoveSecondary.None);
            Attacker.StartDancing(RandomNumberGenerator(2, 3));
        }

        public void TRANSFORM(RBYPokemon Attacker, RBYPokemon Defender)
        {
            if (Attacker.Owner == 0)
                Trainer1.TransformedPoke = (RBYPokemon)Attacker.Clone();
            else
                Trainer2.TransformedPoke = (RBYPokemon)Attacker.Clone();

            Attacker = new RBYPokemon(Defender, Attacker.Level);
            Attacker.Transformed = true;
            Defender.NonDmgHit();
        }

        public void WHIRLWIND(RBYPokemon Attacker, RBYPokemon Defender)
        {
            Displayer.RelayMessage("Move failed");
            Defender.NonDmgHit();
        }


    }
}
