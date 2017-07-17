using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public abstract class Trainer
    {
        protected bool _iSwitched;
        int _indexPicked;
        string _name;

        

        public string Name
        { get { return _name; }
            set { _name = value; }
        }

        public bool ISwitched
        { get { return _iSwitched; } }

        public int IndexPicked
        {
            get { return _indexPicked; }
            set { _indexPicked = value; }
        }

        public bool LightScreen;
        public bool Reflect;

        int LightScreenTurns = 0;
        int ReflectTurns = 0;

        private void RFLS(ref bool screen, ref int turns)
        {
            if (screen)
            {
                turns++;
                if (turns == 5)
                    screen = false;
                turns = 0;
            }
        }


        public void UpdateTurn()
        {
            RFLS(ref LightScreen, ref LightScreenTurns);
            RFLS(ref Reflect, ref ReflectTurns);
        }

    }
}
