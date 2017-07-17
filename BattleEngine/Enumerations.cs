namespace BattleEngine
{
    public enum StatBoostDegree : int
    {
        YouUp = 0x00, 
        YouDown = 0x01,
        YouUpUp = 0x02,
        YouDownDown = 0x03,
        MeUp = 0x04,
        MeDown = 0x05,
        MeUpUp = 0x06,
        MeDownDown = 0x07
    }


    public enum StatusConditionInflicted : int
    {
        None = 0x00,
        Poison = 0x01,
        BadPoison = 0x02,
        Sleep = 0x03,
        Para = 0x04,
        Flinch = 0x05,
        Burn = 0x06,
        Confuse = 0x07,
        Leech = 0x08,
        Freeze = 0x09
    }

    public enum AttackingMoveSecondary : int
    {
        None = 0x00,
        MultiHit = 0x1,
        Recoil = 0x2,
        Trap = 0x3,
        SpecialMech = 0x4,
        Turn2 = 0x5,
        Absorbing = 0x6,
        Recharge = 0x7
    }

    public enum StatBoostType : int
    {
        Atk = 0x1,
        Def = 0x2,
        Special = 0x3,
        Speed = 0x4,
        SpDef = 0x5,
        SpAtk = 0x6,
        Evasion = 0x7,
        Accuracy = 0x8
    }

    public enum ElementTypes : byte
    {
        NORMAL,
        FIGHTING,
        BUG,
        POISON,
        GHOST,
        FLYING,
        ROCK,
        GROUND,
        STEEL,
        DRAGON,
        FIRE,
        WATER,
        GRASS,
        ELECTRIC,
        ICE,
        PSYCHIC,
        DARK,
        NONE
    }
}