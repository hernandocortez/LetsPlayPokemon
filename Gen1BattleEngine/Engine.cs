using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleEngine;
using System.Reflection;

namespace Gen1BattleEngine
{
    class Trainer : BattleEngine.Trainer
    {
        List<RBYPokemon> _party;
        public RBYPokemon TransformedPoke;
        int Alive = 3;

        int _identifier;

        public Trainer(List<RBYPokemon> party, string name,int identity)
        {
            _party = party;
            Alive = party.Count;
            Name = name;
            IndexPicked = 0;
            _identifier = identity;

            string calling = identity == 0 ? "The Trainer's" : "The Foe's";

            for (int i = 0; i < party.Count; i++)
            {
                party[i].Owner = identity;
                party[i].OwnerCall = calling;
            }     
        }

        public void Switch(int index, RBYPokemon CurrentPoke)
        {
            if (CurrentPoke.Transformed)
                CurrentPoke = (RBYPokemon)TransformedPoke.Clone();            

            _iSwitched = true;
            IndexPicked = index;
        }

        public int Identifier
        { get { return _identifier; } }

        public void FinishSwitch()
        { _iSwitched = false; }

        public List<RBYPokemon> Party
        { get { return _party; } }

        public void PokeDied()
        { Alive--; }

        public int RemainingPokemon
        { get { return Alive; } }

    }


    public partial class Engine : DefBattleEngine
    {
        
        Trainer Trainer1;
        Trainer Trainer2;
        RBYPokemon Trainer1CurrentPoke;
        RBYPokemon Trainer2CurrentPoke;
        List<Attacks> MoveEnyclopedia = new List<Attacks>();

        public Engine(List<RBYPokemon> party1, List<RBYPokemon> party2,float [,] matrix,List<Attacks> AllMoves)
        { 
            Trainer1 = new Trainer(party1,"User",0);
            Trainer2 = new Trainer(party2,"Opponent",1);
            seed = new Random();
            typematrix = matrix;
            MoveEnyclopedia = AllMoves;
        }


        private void MainBattleLoop()
        {
            while (true)
            {
                Displayer.DisplayCurrentPokemonInfo(Trainer1CurrentPoke, Trainer2CurrentPoke);
                if (!Trainer1CurrentPoke.IsTrapped && !Trainer1CurrentPoke.Recharging && !Trainer1CurrentPoke.TwoTurnMove && !Trainer1CurrentPoke.Biding 
                    && !Trainer1CurrentPoke.Dancing && !Trainer2CurrentPoke.IsTrapped && !Trainer1CurrentPoke.Biding)
                    SelectAction();

                if (WhoGoesFirst())
                {
                    ExecuteAttack(Trainer1CurrentPoke, Trainer2CurrentPoke, Trainer1);
                    ExecuteAttack(Trainer2CurrentPoke, Trainer1CurrentPoke, Trainer2);
                }
                else
                {
                    ExecuteAttack(Trainer2CurrentPoke, Trainer1CurrentPoke, Trainer2);
                    ExecuteAttack(Trainer1CurrentPoke, Trainer2CurrentPoke, Trainer1);
                }
                
                Trainer1CurrentPoke.ClickTurn();
                Trainer2CurrentPoke.ClickTurn();
                Trainer1.UpdateTurn();
                Trainer2.UpdateTurn();

                if (!Trainer1CurrentPoke.IsAlive)
                {
                    if (Trainer1.RemainingPokemon == 0)
                    {
                        Displayer.RelayMessage("And there goes the battle..");
                        Displayer.RelayMessage("Trainer 1 is out of pokemon!");
                        return;
                    }

                    Trainer1.PokeDied();
                    while (!PickSwitch()) ;
                    Trainer1.FinishSwitch();
                }
                
                if(!Trainer2CurrentPoke.IsAlive)
                {
                    Trainer2.PokeDied();

                    if (Trainer2.RemainingPokemon == 0)
                    {
                        Displayer.RelayMessage("And there goes the battle..");
                        Displayer.RelayMessage("Trainer 2 is out of pokemon!");
                        return;
                    }

                    Trainer2CurrentPoke = Trainer2.Party.First(p => p.IsAlive == true);
                    Trainer2.Switch(Trainer2.Party.IndexOf(Trainer2CurrentPoke), Trainer2CurrentPoke);
                    Displayer.RelayMessage($"The Foe threw out {Trainer2CurrentPoke.Name}");
                    Trainer2.FinishSwitch();
                }
                Displayer.ClearScreen();
            }
        }

        private void ExecuteAttack(RBYPokemon Attacker, RBYPokemon Defender, Trainer Agressor)
        {
            if (Agressor.ISwitched)
            {
                Agressor.FinishSwitch();
                return;
            }

            if (!Attacker.IsAlive)
                return;

            if (Attacker.Biding)
            {
                int dmg = Attacker.PokemonBide();
                if (!Attacker.Biding)
                {
                    if (dmg == 0)
                        Displayer.RelayMessage("Nothing was unleashed");
                    else
                        Defender.TakeDamage(dmg, true);
                }
                return;
            }

            if (Attacker.Recharging)
            { Displayer.Recharging(); Attacker.Recharging = false; return; }

            if (Attacker.IsFlinched)
            { Displayer.FlinchMsg(Attacker); return; }

            if (Attacker.IsTrapped)
            { return; } 

            if (Defender.IsTrapped)
            {
                if (Defender.StillTrapped())
                {
                    Displayer.StillWrapped();
                    Defender.TakeDamage(1, false);                   
                }
                else
                    Displayer.WrappedUp();
                return;
            }

            if (Attacker.CurrentStatus == StatusConditionInflicted.Para)
            { if (FullParalyzed()) { Displayer.ParalyzedMsg(Attacker); Attacker.FinishTwoTurn(); return; } }
            else if (Attacker.CurrentStatus == StatusConditionInflicted.Sleep)
            {
                if (Attacker.Snoozing())
                    Displayer.AsleepMsg(Attacker);
                else
                    Displayer.WokeUpMsg(Attacker);
                return;
            }

            if (Attacker.IsConfused)
            {
                if (Confused())
                {
                    int dmg = DamageCalculation(Attacker.InBattleAtk, Attacker.InBattleDef, Attacker.Level, 20, 1, 1,false);
                    Displayer.ConfusedHitMsg();
                    Attacker.FinishTwoTurn();
                    Attacker.TakeDamage(dmg, true);
                    return;
                }
                Displayer.OutOfConfusionMsg();
            }

            var TheAttack = Attacker.Moveset[Agressor.IndexPicked];


            if (Attacker.TwoTurnMove  && TheAttack.Name == "METRONOME" )
                TheAttack = MoveEnyclopedia[Attacker.MetronomedMove];
            
            
            DeliverAttack(Attacker, Defender, TheAttack,TheAttack.Attack.Movetype);

            if (Attacker.IsAlive && Defender.IsAlive)
                EndOfTurnEffects(Attacker, Defender);

        }

        private void EndOfTurnEffects(RBYPokemon Attacker, RBYPokemon Defender)
        {
            switch(Attacker.CurrentStatus)
            {
                case StatusConditionInflicted.Poison: Attacker.NormalPoisonDmg();
                    break;
                case StatusConditionInflicted.BadPoison: Attacker.BadlyPoisonDmg();
                    break;
                case StatusConditionInflicted.Burn: Attacker.BurnDmg();
                    break;
                case StatusConditionInflicted.Leech: Defender.HealDamage(Attacker.LeechSeedDmg());
                    break;
            }
        }

        private void DeliverAttack(RBYPokemon Attacker, RBYPokemon Defender, Attacks TheAttack,AttackingMoveSecondary typeOfDamage)
        {

      

            if (Attacker.TwoTurnMove)
                typeOfDamage = Attacker.FinishTwoTurn();

            if (typeOfDamage != AttackingMoveSecondary.Turn2)
                Displayer.ShoutAttack(Attacker.OwnerCall, TheAttack.Name, Attacker.Name);
            else
                Displayer.TwoTurn();

            if (Attacker.Dancing)
                typeOfDamage = AttackingMoveSecondary.None;


            Defender.WasAttacked = true;
            Defender.OppPrevAtk = TheAttack.Index;
            Attacker.MyPrevAtk = TheAttack.Index;

            switch (TheAttack.MoveClass)
            {
                case 0:
                    DamageTypeMove(Attacker, Defender, TheAttack, typeOfDamage);
                    break;
                case 1:
                    StatusTypeMove(Attacker, Defender, TheAttack);
                    break;
                case 2:
                    StatBoostTypeMove(Attacker, Defender, TheAttack.Attack, TheAttack.Accuracy);
                    break;
            }
        }

        public void StartBattle()
        {
            Trainer1CurrentPoke = Trainer1.Party[0];
            Trainer2CurrentPoke = Trainer2.Party[0];
            MainBattleLoop();
        }

        private bool WhoGoesFirst()
        {
            if (Trainer1.ISwitched)
                return false;

            var Atk1 = Trainer1CurrentPoke.Moveset[Trainer1.IndexPicked].Name;
            var Atk2 = Trainer2CurrentPoke.Moveset[Trainer2.IndexPicked].Name;

            if (Atk1 == "COUNTER")
                return false;
            else if (Atk2 == "COUNTER")
                return true;

            if(Atk1 == "QUICK ATTACK")
            {
                if (Atk1 != Atk2)
                    return true;
            }

            else if (Atk2 == "QUICK ATTACK")
            {
                if (Atk1 != Atk2)
                    return false;
            }


            int delta = Trainer1CurrentPoke.Speed - Trainer2CurrentPoke.Speed;
            if (delta == 0)
            {
                int rng = RandomNumberGenerator(1, 2);
                if (rng == 1) return true; //true = player1
                else return false; //false = player2
            }
            else if (delta > 0) return true;
            else return false;
        }

        private void SelectAction()
        {            
            Displayer.DisplayBattleChoice();

            bool done = false; ;

            while (!done)
            {
                switch (Input_Handler())
                {
                    case 0:
                        done = PickMove();
                        break;
                    case 1:
                        done = PickSwitch();
                        break;
                }
            }

        }

        private int Input_Handler()
        {
            var choice = Console.ReadLine();
            int number;

            if (!int.TryParse(choice, out number))
                number = 0;
            return number;
        }


        private bool PickMove()
        {
            Displayer.DisplayMovesToSelect(Trainer1CurrentPoke);
            //Write a function that does logic for PP

            int selection = Input_Handler();
            if (selection > Trainer1CurrentPoke.Moveset.Count)
                return false;
            else
            {
                //More logic for PP
                Trainer1.IndexPicked = selection;
                return true;
            }

        }

        private bool PickSwitch(bool dead = false)
        {

            //Write a function to list switchable pokes     
            //var alive = Trainer1.Party.Where(p => p.IsAlive == true).ToList();

            //if (alive.Count > 1)
            //    Displayer.DisplayPokesToSwitch(alive.ConvertAll(x => (Pokemon)x));

            Displayer.DisplayPokesToSwitch(Trainer1.Party.ConvertAll(x => (Pokemon)x));

            int selection = Input_Handler();

            if (selection > Trainer1.Party.Count || Trainer1.Party[selection] == Trainer1CurrentPoke || !Trainer1.Party[selection].IsAlive)
                return false;
            else
            {
                Trainer1CurrentPoke = Trainer1.Party[selection];
                Trainer1.Switch(selection,Trainer1CurrentPoke);

                if (!dead)
                    Displayer.TrainerSwitchedPokemon(Trainer1.Name, Trainer1CurrentPoke.Name);
                else
                    Displayer.RelayMessage($"Trainer threw out {Trainer1CurrentPoke.Name}");
                return true;
            }
        }

        private bool FullParalyzed()
        {
            if (Odds(1,4)) return true;
            else return false;
        }

        private bool Confused()
        {
            if (Odds(1,2)) return true;
            else return false;
        }

        private bool Unhittable(RBYPokemon poke)
        {
            return poke.Flying | poke.Digging;
        }

        private void DamageTypeMove(RBYPokemon Attacker, RBYPokemon Defender, Attacks atk, AttackingMoveSecondary attacktype, bool nomiss= false)
        {

            bool crit_chance = CriticalChance(Attacker, false);

            Attacker.RecalcStats(crit_chance);
            Defender.RecalcStats(crit_chance);

            if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, atk.Accuracy) && !Unhittable(Defender) && !nomiss)
            {                
                Displayer.AtkMissed();
                Defender.NonDmgHit();
                return;
            }

            ElementTypes type = atk.MoveType;
            int basepwr = atk.BasePower;
            bool physpec = PhysicalSpecial(type);

            float multipler = TypeMatrixCalc(atk.MoveType, Defender.Type1, Defender.Type2);
            double stab = 1;
            

            int power = physpec == true ? Attacker.InBattleAtk : Attacker.InBattleSpecialAtk;
            int defense = physpec == true ? Defender.InBattleDef : Defender.InBattleSpecialAtk;

            if (Attacker.Type1 == type || Attacker.Type2 == type)
                stab = 1.5;
            int dmg = 0;

            if(attacktype != AttackingMoveSecondary.Turn2 && attacktype != AttackingMoveSecondary.SpecialMech)
            {
                if (crit_chance)
                    Displayer.RelayMessage("Critical Hit!");
            }

            switch (attacktype)
            {
                case AttackingMoveSecondary.None:
                    dmg = DamageCalculation(power, defense, Attacker.Level, basepwr, stab, multipler,crit_chance);
                    Defender.TakeDamage(dmg, physpec,multipler);
                    break;
                case AttackingMoveSecondary.MultiHit:
                    MultiHitMove(atk.Name,power, defense, Attacker.Level, basepwr, stab, multipler, Defender,physpec, crit_chance);
                    break;
                case AttackingMoveSecondary.Recoil:
                    dmg = DamageCalculation(power, defense, Attacker.Level, basepwr, stab, multipler, crit_chance);
                    Defender.TakeDamage(dmg, physpec, multipler);
                    RecoilLogic(Defender, Attacker, dmg);
                    break;
                case AttackingMoveSecondary.SpecialMech: CallSpecialMove(atk.Name, Attacker, Defender);
                    break;
                case AttackingMoveSecondary.Trap:
                    dmg = DamageCalculation(power, defense, Attacker.Level, basepwr, stab, multipler, crit_chance);
                    Defender.TakeDamage(dmg, physpec, multipler);
                    TrapLogic(Defender, Attacker);
                    break;
                case AttackingMoveSecondary.Turn2:
                    Attacker.ActivateTwoTurn(atk);
                    break;
                case AttackingMoveSecondary.Absorbing:
                    dmg = DamageCalculation(power, defense, Attacker.Level, basepwr, stab, multipler, crit_chance);
                    Defender.TakeDamage(dmg, physpec, multipler);
                    Attacker.HealDamage(dmg / 2);
                    break;
                case AttackingMoveSecondary.Recharge:
                    dmg = DamageCalculation(power, defense, Attacker.Level, basepwr, stab, multipler, crit_chance);
                    Defender.TakeDamage(dmg, physpec, multipler);
                    RechargeLogic(Defender, Attacker);
                    break;
            }
            Defender.LastDmg = dmg;

            if (Defender.CurrentStatus == StatusConditionInflicted.Freeze && atk.MoveType == ElementTypes.FIRE)
                Defender.UnFreez();

            SecondaryAffects(Attacker, Defender, atk);            
        }

        private bool CriticalChance(RBYPokemon Attacker, bool crit_move)
        {
            int multiplier = crit_move ? 8 : 1;
            multiplier = Attacker.IsFocused ? multiplier << 2 : multiplier;
            int nominal = Attacker.CritChance * multiplier;

            if (nominal >= 100)
                return true;
            else
                return Odds((nominal << 8)/100, 256);
        }

        private void SecondaryAffects(RBYPokemon Attacker, RBYPokemon Defender, Attacks atk)
        {
            int chance = 0;
            switch (atk.Attack.Chance)
            {
                case 0:
                    return;
                case 1:
                    chance = 10;
                    break;
                case 2:
                    chance = 30;
                    break;
                case 3:
                    chance = 50;
                    break;
            }

            if (Odds(chance,100))
            {
                StatusTypeMove(Attacker, Defender, atk, true);
                StatBoostTypeMove(Attacker, Defender, atk.Attack, atk.Accuracy, true);
            }
        }

        private void RechargeLogic(RBYPokemon Defender, RBYPokemon Attacker)
        {
            if (Defender.IsAlive)
                Attacker.Recharging = true;
        }


        private void TrapLogic(RBYPokemon Defender, RBYPokemon Attacker)
        {
            if (Defender.IsAlive)
            {
                Defender.IsTrapped = true;
                Defender.TrappedSomeone(RandomNumberGenerator(2, 5));
            }

        }

        private void RecoilLogic(RBYPokemon Defender, RBYPokemon Attacker, int dmg)
        {
            if (Defender.IsAlive)
            {
                Attacker.TakeDamage(dmg/10,false);
                Displayer.RecoilHappened(Attacker.OwnerCall,Attacker.Name);
            }
        }

        private void MultiHitMove(string name,int atk, int def, int lvl, int basepw, double stab, double multiplier, RBYPokemon Defender,bool physpec,bool crit_chance)
        {
            int hits = RandomNumberGenerator(2, 5);
            int dmg;

            if (name == "DOUBLE KICK")
                hits = 2;

            for (int i = 0; i < hits; i++)
            {
                dmg = DamageCalculation(atk, def, lvl, basepw, stab, multiplier, crit_chance);
                Defender.TakeDamage(dmg,physpec);

                if (i != hits - 1)
                    Displayer.AttackContiunes();

               if (!Defender.IsAlive)
                    break;
            }

        }


        private void StatusTypeMove(RBYPokemon Attacker, RBYPokemon Defender, Attacks atk, bool sec = false)
        {

            if(!sec)
            {
                Defender.LastDmg = 0;
                Defender.NonDmgHit();
                if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, atk.Accuracy) && !Defender.ActiveSub && !Unhittable(Defender))
                {
                    Displayer.AtkMissed();                    
                    return;
                }
            }

            switch (atk.Attack.Status)
            {
                case StatusConditionInflicted.BadPoison:
                    PoisonLogic(Defender, true,sec);
                    break;
                case StatusConditionInflicted.Burn:
                    BurnLogic(Defender,sec);
                    break;
                case StatusConditionInflicted.Confuse:
                    ConfuseLogic(Defender);
                    break;
                case StatusConditionInflicted.Flinch:
                    FlinchLogic(Defender, atk.Attack.Chance);
                    break;
                case StatusConditionInflicted.Freeze:
                    YouAreFrozen(Defender);
                    break;
                case StatusConditionInflicted.Leech:
                    LeechedLogic(Defender);
                    break;
                case StatusConditionInflicted.Para:
                    ParalysisLogic(Defender, atk.MoveType);
                    break;
                case StatusConditionInflicted.Poison:
                    PoisonLogic(Defender, false,sec);
                    break;
                case StatusConditionInflicted.Sleep:
                    SleepLogic(Defender);
                    break;
            }             
        }

        private void LeechedLogic(RBYPokemon Defender)
        {
            if (Defender.Type1 != ElementTypes.GRASS && Defender.Type2 != ElementTypes.GRASS && !Defender.IsSeeded)
            {
                Defender.Leeched();
                Displayer.Seeded(Defender.OwnerCall,Defender.Name);
            }
            else
                Displayer.SuperEffective(0);            
        }

        private void FlinchLogic(RBYPokemon Defender, int chance)
        {
            int inchance = 10;
            if (Odds(chance, inchance))
                Defender.Flinched();
        }

        private void PoisonLogic(RBYPokemon Defender, bool badpoison, bool secondary)
        {
            if (Defender.Type1 != ElementTypes.POISON && Defender.Type2 != ElementTypes.POISON)
            {
                if (Defender.CurrentStatus == StatusConditionInflicted.None)
                {
                    Defender.TakePoison(badpoison);
                    Displayer.Poisoned(badpoison,Defender);
                }

            }
            else
            { if (!secondary) Displayer.SuperEffective(0); }
        }

        private void BurnLogic(RBYPokemon Defender, bool secondary)
        {
            if (Defender.Type1 != ElementTypes.FIRE && Defender.Type2 != ElementTypes.FIRE)
            {
                if (Defender.CurrentStatus == StatusConditionInflicted.None)
                {
                    Defender.TakeBurn();
                    Displayer.Burned(Defender.OwnerCall, Defender.Name);
                }
            }
            else
            { if (!secondary) Displayer.SuperEffective(0); }
        }

        private void ConfuseLogic(RBYPokemon Defender)
        {
            if (!Defender.IsConfused)
            {
                Defender.TakeConfusion(RandomNumberGenerator(2, 5));
                Displayer.Confused(Defender.OwnerCall, Defender.Name);
            }
        }

        private void ParalysisLogic(RBYPokemon Defender, ElementTypes movetype)
        {
            if (Defender.Type1 == ElementTypes.GROUND && Defender.Type2 == ElementTypes.GROUND && movetype == ElementTypes.ELECTRIC)
                Displayer.SuperEffective(0);
            else
            {
                if (Defender.CurrentStatus == StatusConditionInflicted.None)
                {
                    Defender.Paralyze();
                    Displayer.Paralyzed(Defender.OwnerCall, Defender.Name);
                }
            }
        }

        private void SleepLogic(RBYPokemon Defender)
        {
            if (Defender.CurrentStatus == StatusConditionInflicted.None)
            {
                Displayer.PutAsleep(Defender.OwnerCall, Defender.Name);

                Defender.PutToSleep(RandomNumberGenerator(2, 5));
            }
        }

        private void YouAreFrozen(RBYPokemon Defender)
        {
            if (Defender.CurrentStatus == StatusConditionInflicted.None)
            {
                Defender.Freeze();
                Displayer.Freezed(Defender.OwnerCall, Defender.Name);
            }
        }

        private void StatBoostTypeMove(RBYPokemon Attacker, RBYPokemon Defender, AttackType atk, int accuracy, bool sec = false)
        {
            var query = atk.Stats.Where(p => p.Boost < StatBoostDegree.MeDown);
            
            if(query.Count() != 0 && !sec)
            {
                Defender.LastDmg = 0;
                Defender.NonDmgHit();
                if (!HitOrMiss(Attacker.Accuracy, Defender.Evasion, accuracy) || Defender.IsMisted && !Unhittable(Defender))
                {
                    Displayer.AtkMissed();
                    return;
                }
            }    

            foreach (var p in atk.Stats)
            {
                if (p.Boost < StatBoostDegree.MeDown)
                    Defender.IncreaseStage(p.Stat, DetermineStatDegree(p.Boost));
                else
                    Attacker.IncreaseStage(p.Stat, DetermineStatDegree(p.Boost));
            }
        }

        private int DetermineStatDegree(StatBoostDegree deg)
        {
            int boost = (int)deg % 4;

            switch (boost)
            {
                case 0: return 1;
                case 1: return -1;
                case 2: return 2;
                case 3: return -2;
            }
            return 0;
        }

        


        private void Initialization()
        {

        }
    }
}
