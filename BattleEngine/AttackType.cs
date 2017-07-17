using BattleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    

    public struct StatBoostInfo
    {
        public StatBoostDegree Boost;
        public StatBoostType Stat;
    }

    public abstract class AttackType
    {
        protected void GetStatBoosts(ulong temp, List<StatBoostInfo> list)
        {
            if (temp == 0) return;
            
            for (int i = 0; i < 5; i++)
            {
                StatBoostInfo boost;
                boost.Stat = (StatBoostType)((temp & 0xF000000000000000) >> 60);
                boost.Boost = (StatBoostDegree)((temp & 0x0F00000000000000) >> 56);

                list.Add(boost);
                temp = temp << 8;
                if (temp == 0) break;
            }           
        }

        abstract public StatusConditionInflicted Status { get; }
        abstract public AttackingMoveSecondary Movetype { get; }
        abstract public int Chance { get; }
        abstract public List<StatBoostInfo> Stats { get; }
    }

    class DamageMove : AttackType
    {
        const long MoveTypeMask = 0x0F00000000000000;
        const int MoveTypeShift = 56;
        

        const long StatusMask = 0x00F0000000000000;
        const int StatusTypeShift = 52;

        const long StatsMask = 0x000FFFFFFFFFF000;
        const int StatsTypeShift = 12;

        const long ChanceMask = 0x0000000000000F00;
        const int ChanceMaskSShift = 8;


        StatusConditionInflicted _status;
        int _chance;
        AttackingMoveSecondary _movetype;

        List<StatBoostInfo> _stats;

        public override StatusConditionInflicted Status
        { get { return _status; } }
    
        public override int Chance
        { get { return _chance; } }
                              
        public override AttackingMoveSecondary Movetype
        { get { return _movetype; } }
            

        public override List<StatBoostInfo> Stats
        { get { return _stats; } }
        

        public DamageMove(long info)
        {
            _movetype = (AttackingMoveSecondary)((info & MoveTypeMask) >> MoveTypeShift);
            _status = (StatusConditionInflicted)((info & StatusMask) >> StatusTypeShift);

            _stats = new List<StatBoostInfo>();
            GetStatList(_stats, info);
            _chance = (int)((info & ChanceMask) >> ChanceMaskSShift);
        }

        private void GetStatList(List<StatBoostInfo> list, long info)
        {
            ulong temp = (ulong)((info & StatsMask) << StatsTypeShift);
            GetStatBoosts(temp, list);
        } 

    }

    class StatBoost : AttackType
    {
        long StatTypeMask = 0x0FFFFFFFFFF00000;
        int StatTypeShift = 4;

        List<StatBoostInfo> _stats;

        public StatBoost(long info)
        {
            _stats = new List<StatBoostInfo>();
            GetStatList(Stats, info);
        }

        public override int Chance
        { get { return 9999; } }
                
        public override AttackingMoveSecondary Movetype
        { get { return AttackingMoveSecondary.None; } }

        public override List<StatBoostInfo> Stats
        { get { return _stats; } }

        public override StatusConditionInflicted Status
        { get { return StatusConditionInflicted.None; } }

        private void GetStatList(List<StatBoostInfo> list,long info)
        {
            ulong temp = (ulong)((info & StatTypeMask) << StatTypeShift);
            GetStatBoosts(temp, list);
        }
    }

    class StatusMove : AttackType
    {
        long StatusTypeMask = 0x0F00000000000000;
        int StatusTypeShift = 56;

        StatusConditionInflicted _status;

        public StatusMove(long info)
        { _status = (StatusConditionInflicted)((info & StatusTypeMask) >> StatusTypeShift); }

        public override AttackingMoveSecondary Movetype
        { get { return AttackingMoveSecondary.None; } }

        public override List<StatBoostInfo> Stats
        { get { return null; } }

        public override int Chance
        { get { return 9999; } }

        public override StatusConditionInflicted Status
        { get { return _status; } }        
    }

    class Recover : AttackType
    {
        public Recover ()
        { }

        public override AttackingMoveSecondary Movetype
        { get { return AttackingMoveSecondary.None; } }

        public override List<StatBoostInfo> Stats
        { get { return null; } }

        public override int Chance
        { get { return 9999; } }

        public override StatusConditionInflicted Status
        { get { return StatusConditionInflicted.None; } }

    }

}
