using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    

    public abstract class DefBattleEngine
    {
        protected Random seed;
        protected float [,] typematrix;

        protected int RandomNumberGenerator(int min, int max)
        {
            return seed.Next(min, max); }

        protected bool  PhysicalSpecial(ElementTypes type)
        {
            if (type < ElementTypes.DRAGON)
                return true; //true physical
            else
                return false; //special
        }


        protected int DamageCalculation(int atk,int def,int lvl,int basepw,double stab,double multiplier, bool crit)
        {
            int dmgstage1 = (2 * lvl / 5 + 2);
            int dmgstage2 = (atk * basepw / def);
            int dmgstage3 = (dmgstage1 * dmgstage2) / 50 + 2;
            double extra = multiplier * stab;
            int total = (int)(dmgstage3 * extra);
            int rng = RandomNumberGenerator(85, 100);
            total = (total * rng) / 100;
            return crit ? total << 1  : total ;
        }

        private bool AccuracyGenerator(int accuracy)
        {
            int rng = RandomNumberGenerator(0, accuracy);

            if (rng > accuracy)
                return false;
            else
                return true;
        }

        protected float TypeMatrixCalc(ElementTypes move, ElementTypes type1, ElementTypes type2)
        {
            float mult;
            mult = typematrix[(int)move,(int)type1];
            mult = typematrix[(int)move, (int)type2] * mult;
            return mult;
        }

        protected bool HitOrMiss(float accuracy, float evasion,int move_accuracy)
        {
            accuracy = accuracy * evasion;

            int int_acc = (int)(accuracy * ((float)(move_accuracy)/100) * 1000);

            return (Odds(int_acc, 1000));

        }

        protected bool Odds(int num, int inchance)
        {
            int rng = RandomNumberGenerator(0, inchance);

            if (rng <= num)
                return true;
            else
                return false;
        }

        protected void CallSpecialMove(string name, Pokemon Attacker, Pokemon Defender)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(name.Replace(' ', '_'));
            theMethod.Invoke(this, new[] { Attacker, Defender });
        }

    }
}
