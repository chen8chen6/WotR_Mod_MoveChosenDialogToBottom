using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace MoveChosenDialogToBottom
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool isToMove_previouslyChosen = true;
        public bool isToMove_evilShifting = true;
        public bool isToMove_attackingNPC = true;
        public bool isToMove_unrecruit = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
