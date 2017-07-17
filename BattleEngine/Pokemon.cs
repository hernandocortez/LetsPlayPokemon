using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public abstract class Pokemon
    {
        protected string _name;
        protected int _level;
        protected int _HealthPoints;
        protected int _outBattleAtk;
        protected int _outBattleDef;
        protected int _outBattleSpecialAtk;
        protected int _outBattleSpecialDef;
        protected int _outBattleSpeed;
        protected float _outBattleAccuracy;
        protected float _outBattleEvasion;

        protected int _atkStage = 0;
        protected int _defStage = 0;
        protected int _specialAtkStage = 0;
        protected int _specialDefStage = 0;
        protected int _speedStage = 0;
        protected int _accuracyStage = 0;
        protected int _evasionStage = 0;

        protected int _inBattleAtk;
        protected int _inBattleDef;
        protected int _inBattleSpecialAtk;
        protected int _inBattleSpecialDef;
        protected int _inBattleSpeed;
        protected float _inBattleAccuracy;
        protected float _inBattleEvasion;
        protected int _critChance;

        protected List<Attacks> _moveset;


        protected ElementTypes _type1;
        protected ElementTypes _type2;

        protected StatusConditionInflicted _currentStatus = StatusConditionInflicted.None;
        protected bool _iConfused = false;
        protected bool _iFlinched = false;
        protected bool _isAlive = true;
        protected bool _isTrapped = false;
        protected bool _activeSub = false;
        protected bool _isLeeched = false;


        protected int _turnOfMove = 0;
        protected bool _twoTurnMove = false;

        protected bool _recharging = false;

        protected int _subHPRemaining = 0;

        protected int SleepTurnSeed = 0;
        protected int TurnOfSleep = 0;
        protected int TrapTurnSeed = 0;
        protected int TurnOfTrap = 0;
        public int Owner = 0;
        public string OwnerCall;

        public int HealthPoints
        { get { return _HealthPoints; } }

        public int Level
        { get { return _level; } }

        public string Name
        { get { return _name; } }

        public bool IsFlinched
        { get { return _iFlinched; } }

        public StatusConditionInflicted CurrentStatus
        { get { return _currentStatus; } }

        public bool IsConfused
        { get { return _iConfused; } }

        public bool IsAlive
        { get { return _isAlive; } }

        public bool IsSeeded
        { get { return _isLeeched; } }

        public bool ActiveSub
        { get { return _activeSub; } }

        public bool IsTrapped
        {
            get { return _isTrapped; }
            set { _isTrapped = value; }
        }

         

        public bool TwoTurnMove
        { get { return _twoTurnMove; } }

        public bool Recharging
        {
            get { return _recharging; }
            set { _recharging = value; }
        }

        public int SubHPRemaining
        { get { return _subHPRemaining; } }

        public ElementTypes Type1
        { get { return _type1; } }

        public ElementTypes Type2
        { get { return _type2; } }

        public List<Attacks> Moveset
        {
            get { return _moveset; }
            set { _moveset = value; }
        }

        public int Speed
        { get { return _inBattleSpeed; } }

        public int AtkStage
        { get { return _atkStage; } }

        public int DefStage
        { get { return _defStage; } }

        public int SpecialStage
        { get { return _specialAtkStage; } }

        public int SpecialAtkStage
        { get { return _specialAtkStage; } }

        public int SpecialDefStage
        { get { return _specialDefStage; } }

        public int SpeedStage
        { get { return _speedStage; } }

        public int AccuracyStage
        { get { return _accuracyStage; } }

        public int EvasionStage
        { get { return _evasionStage; } }

        public int InBattleAtk
        { get { return _inBattleAtk; } }

        public int InBattleDef
        { get { return _inBattleDef; } }

        public int InBattleSpecialAtk
        { get { return _inBattleSpecialAtk; } }

        public int InBattleSpecialDef
        { get { return _inBattleSpecialDef; } }

        public float Accuracy
        { get { return _inBattleAccuracy; } }

        public float Evasion
        { get { return _inBattleEvasion; } }

        public int CritChance
        { get { return _critChance; } }

        private NBuffer<int> DamageBugger = new NBuffer<int>(3);

        public Pokemon Clone()
        {
            var poke = (Pokemon)MemberwiseClone();
            poke.Moveset = new List<Attacks>(poke.Moveset);

            return poke;
        }

        public void RecalcStats(bool crit = false)
        {
            _inBattleAtk = (int)(StatBootNum(StatBoostType.Atk, _atkStage) * _outBattleAtk);
            _inBattleSpeed = (int)(StatBootNum(StatBoostType.Speed, _speedStage) * _outBattleSpeed);
            _inBattleDef = (int)(StatBootNum(StatBoostType.Def, _defStage) * _outBattleDef);
            _inBattleSpecialAtk = (int)(StatBootNum(StatBoostType.SpAtk, _specialAtkStage) * _outBattleSpecialAtk);
            _inBattleSpecialDef = (int)(StatBootNum(StatBoostType.SpDef, _specialDefStage) * _outBattleSpecialDef);
            _inBattleAccuracy = (float)(StatBootNum(StatBoostType.Accuracy, _accuracyStage) * _outBattleAccuracy);
            _inBattleEvasion = (float)(StatBootNum(StatBoostType.Evasion, _evasionStage) * _outBattleEvasion);

            if(crit)
            {
                _inBattleAtk = _outBattleAtk;
                _inBattleSpeed = _outBattleSpeed;
                _inBattleDef = _outBattleDef;
                _inBattleSpecialAtk =  _outBattleSpecialAtk;
                _inBattleSpecialDef =  _outBattleSpecialDef;
            }
        }

        private int CappedAdd(int addend, int offset, string name)
        {
            addend += offset;

            if (addend > 6 || addend < -6)
                Displayer.TooFat(addend);
            else
                Displayer.Statup(offset,name);

            addend = addend < -6 ? addend = -6 : addend;
            addend = addend > 6 ? addend = 6 : addend;

            return addend;
        }

        public void IncreaseStage(StatBoostType boost, int offset)
        {
            switch (boost)
            {
                case StatBoostType.Atk:
                    _atkStage = CappedAdd(_atkStage, offset,"Attack");
                    break;
                case StatBoostType.Def:
                    _defStage = CappedAdd(_defStage, offset,"Defense");
                    break;
                case StatBoostType.Special:
                    _specialAtkStage = CappedAdd(_specialAtkStage, offset,"Special");
                    break;
                case StatBoostType.SpAtk:
                    _specialAtkStage = CappedAdd(_specialAtkStage, offset, "Special");
                    break;
                case StatBoostType.SpDef:
                    _specialDefStage = CappedAdd(_specialDefStage, offset, "Special");
                    break;
                case StatBoostType.Speed:
                    _speedStage = CappedAdd(_speedStage, offset,"Speed");
                    break;
                case StatBoostType.Evasion:
                    _evasionStage = CappedAdd(_evasionStage, offset,"Evasion");
                    break;
                case StatBoostType.Accuracy:
                    _accuracyStage = CappedAdd(_accuracyStage, offset,"Accuracy");
                    break;
            }
        }

        public void SwitchMeOut()
        {
            _iConfused = false;
            _iFlinched = false;
            _isLeeched = false;
            _isTrapped = false;
            MyPrevAtk = -1;
            OppPrevAtk = -1;

            ResetStatBoost();

            if (UsedMimic)
                Moveset[MimicIndex] = OldMimic;

        }

        public void SwitchMeIn(Trainer t)
        {
            LightScreen = t.LightScreen;
            Reflect = t.Reflect;
        }

        public void ResetStatBoost()
        {
            _atkStage = 0;
            _defStage = 0;
            _specialAtkStage = 0;
            _specialDefStage = 0;
            _speedStage = 0;
            _accuracyStage = 0;
            _evasionStage = 0;
        }

        private double StatBootNum(StatBoostType stat,int stage)
        {
            if(stat <= StatBoostType.SpAtk)
            {
                switch(stage)
                {
                    case -6: return 0.25;                        
                    case -5: return 0.285;
                    case -4: return 0.33;
                    case -3: return 0.4;
                    case -2: return 0.5;
                    case -1: return 0.66;
                    case 6: return 4;
                    case 5: return 3.5;
                    case 4: return 3;
                    case 3: return 2.5;
                    case 2: return 2;
                    case 1: return 1.5;
                    default: return 1;
                }
            }


            if(stat > StatBoostType.SpAtk)
            {
                int mock = stage;

                if (stat == StatBoostType.Evasion)
                    mock = -stage;
                
                switch (stage)
                {
                    case -6: return 0.33;
                    case -5: return 0.375;
                    case -4: return 0.428;
                    case -3: return 0.5;
                    case -2: return 0.6;
                    case -1: return 0.75;
                    case 6: return 3;
                    case 5: return 2.66;
                    case 4: return 2.33;
                    case 3: return 2;
                    case 2: return 1.66;
                    case 1: return 1.33;
                    default: return 1;
                }
            }
            return 1;
        }


        public void TakeDamage(int dmg, bool physical, float multiplier = 1, bool ignore = false)
        {
            if (LightScreen && !physical && !ignore)
                dmg = dmg / 2;
            else if (Reflect && physical && !ignore)
                dmg = dmg / 2;

            DamageBugger.Add(dmg);

            LastDmg = dmg;

            if(_activeSub)
            {
                SubstituteHP -= dmg;
                if (SubstituteHP <= 0)
                {
                    _activeSub = false;
                    Displayer.RelayMessage("Substitute broke");
                }
                dmg = 0;
            }


           _subHPRemaining -= dmg;

            Displayer.SuperEffective(multiplier);
            if (_subHPRemaining <= 0)
            {
                _subHPRemaining = 0;
                _isAlive = false;
                Displayer.Fainted(OwnerCall,_name);
            }
            else
                Displayer.ReceiveDamage(OwnerCall,_name, dmg);      
        }

        public void HealDamage(int dmg)
        {
            if(_subHPRemaining == _HealthPoints)
            {
                Displayer.FullHealth(OwnerCall,_name);
                return;
            }

            _subHPRemaining += dmg;

            if (_subHPRemaining >= _HealthPoints)
                _subHPRemaining = _HealthPoints;
            Displayer.HealDamage(OwnerCall,_name, dmg);             
        }

        public void TrappedSomeone(int turns)
        { TrapTurnSeed = turns; }

        public bool StillTrapped()
        {
            TurnOfTrap++;

            if (TurnOfTrap == TrapTurnSeed)
            {
                TurnOfTrap = 0;
                IsTrapped = false;
                return false;
            }
            return true;
        }

        Attacks curMove;
        public int TurnOfMove
        { get { return _turnOfMove; } }
        public bool Flying = false;
        public bool Digging = false;
        

        public void ActivateTwoTurn(Attacks a)
        {
            _twoTurnMove = true;
            curMove = a;

            if (a.Name == "FLY")
                Flying = true;
            else if (a.Name == "DIG")
                Digging = true;            
        }

        public AttackingMoveSecondary FinishTwoTurn()
        {
            _twoTurnMove = false;
            _turnOfMove = 0;
            Digging = true;
            Flying = true;
            return AttackingMoveSecondary.None;
        }
        
        public void TakePoison(bool badpoison)
        {
            if (badpoison)
                _currentStatus = StatusConditionInflicted.BadPoison;
            else
                _currentStatus = StatusConditionInflicted.Poison;
        }

        public void TakeBurn()
        { _currentStatus = StatusConditionInflicted.Burn; }

        public void Paralyze()
        { _currentStatus = StatusConditionInflicted.Para; }

        int ConfuseTurnSeed = 0;
        int TurnOfConfuse = 0;
        

        public void TakeConfusion(int confturns)
        {
            ConfuseTurnSeed = confturns;
            _iConfused = true;
        }

        public void HealConfusion()
        { _iConfused = false; }

        public bool QuestionMarks()
        {
            TurnOfConfuse++;

            if (TurnOfConfuse == ConfuseTurnSeed)
            {
                TurnOfConfuse = 0;
                _iConfused = false;
                return false;
            }
            return true;
        }

        public void PutToSleep(int sleepturns)
        {
            SleepTurnSeed = sleepturns;
            _currentStatus = StatusConditionInflicted.Sleep;
            _iConfused = false;
        }

        public bool Snoozing()
        {
            TurnOfSleep++;

            if (TurnOfSleep == SleepTurnSeed)
            {
                TurnOfSleep = 0;
                _currentStatus = StatusConditionInflicted.None;
                return false;
            }
            return true;
        }

        public void Freeze()
        { _currentStatus = StatusConditionInflicted.Freeze; }

        public void UnFreez()
        { _currentStatus = StatusConditionInflicted.None; }

        public void Flinched()
        { _iFlinched = true; }

        public void Leeched()
        { _isLeeched = true; }

        public virtual void NormalPoisonDmg()
        {
            Displayer.RelayMessage($"{Name} took poison damage");
            TakeDamage(_HealthPoints / 8);
        }

        protected int T = 1;
        public virtual void BadlyPoisonDmg()
        {
            int dmg = (_HealthPoints * T) / 16;
            dmg = dmg == 0 ? 1 : dmg;
            Displayer.RelayMessage($"{Name} took poison damage");
            TakeDamage(dmg);
            
            T++;
        }

        public virtual int LeechSeedDmg()
        {
            int dmg = _HealthPoints / 8;
            Displayer.RelayMessage($"Leech seed drained damage from {Name}");
            TakeDamage(dmg);           
            return dmg;
        }

        public virtual void BurnDmg()
        {
            TakeDamage(_HealthPoints / 8);
            Displayer.RelayMessage($"{Name} took burn damage");
        }

        protected void TakeDamage(int dmg)
        {           
            _subHPRemaining -= dmg;
            if (_subHPRemaining <= 0)
            {
                _subHPRemaining = 0;
                _isAlive = false;
                Displayer.Fainted(OwnerCall,_name);
            }
        }

        ///////////////////////////
        public bool Biding;
        int BidTurns = 0;

        public int LastDmg = 0;
        public bool WasAttacked = false;
        public int MyPrevAtk = -1;
        public int OppPrevAtk = -1;

        public void ClickTurn()
        {
            if (!WasAttacked)
                NonDmgHit();
            WasAttacked = false;

            if(IsMisted)
            {
                MistTurns++;
                if(MistTurns == 5)
                {
                    IsMisted = false;
                    MistTurns = 0;
                }
            }

            if(Dancing)
            {
                DanceTurns++;
                if(DanceTurns == DanceTurnSeed)
                {
                    Dancing = false;
                    if (!_iConfused)
                    {
                        Displayer.RelayMessage("Pokemon confused due to fatigue");
                        TakeConfusion(3);
                    }
                }
            }

        }

        public void NonDmgHit()
        {
            DamageBugger.Add(0);
            LastDmg = 0;
        }

        public int PokemonBide()
        {
            if (BidTurns == 0)
                Displayer.RelayMessage("Bide start");
            else if (BidTurns == 1)
                Displayer.RelayMessage("Pokemon storing energy");
            else if(BidTurns == 2)
            {
                Biding = false;
                BidTurns = 0;
                return DamageBugger.Sum();
            }
            BidTurns++;
            return 0;
        }
        
        public bool IsFocused = false;

        public bool LightScreen = false;
        public bool Reflect = false;

        public bool UsedMimic = false;
        public Attacks OldMimic;
        public int MimicIndex;

        public bool IsMisted = false;
        public int MistTurns = 0;

        public bool Dancing = false;
        public int DanceTurns = 0;
        public int DanceTurnSeed = 0;
        

        public void StartDancing(int dance_seed)
        {
            Dancing = true;
            DanceTurnSeed = dance_seed;
        }

        int SubstituteHP;

        public bool SetUpSubstitute()
        {
            if (_subHPRemaining < _HealthPoints / 4)
                return false;
            else
                SubstituteHP = _HealthPoints / 4;
            _activeSub = true;

            return true;
        }

        public bool Transformed = false;
        public int MetronomedMove = 0;

    }
}
