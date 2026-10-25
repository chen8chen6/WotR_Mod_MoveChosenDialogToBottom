using HarmonyLib;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.Controllers.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Localization;

namespace MoveChosenDialogToBottom
{
    internal class MoveChosenDialogToBottom
    {
        public static Settings Settings => Main.Settings;

        [HarmonyPatch(typeof(DialogController))]
        [HarmonyPatch(nameof(DialogController.AddAnswers))]
        private static class DialogController_Patch
        {
            private static void Postfix(ref DialogController __instance)
            {
                if (!Main.Enabled)
                    return;
                if (__instance.m_Answers.Count <= 1)
                    return;

                //Check if sorted
                int toMoveCnt = 0;
                foreach (var ans in __instance.m_Answers)
                {
                    if (IsToMove(ans))
                        toMoveCnt++;
                }
                bool isSorted = (0 == toMoveCnt || __instance.m_Answers.Count == toMoveCnt);
                if (isSorted)
                    return;

                //Sort m_answer
                List<BlueprintAnswer> ansTop = new List<BlueprintAnswer> { };
                List<BlueprintAnswer> ansBottom = new List<BlueprintAnswer> { };
                foreach (var ans in __instance.m_Answers)
                {
                    if (!IsToMove(ans))
                        ansTop.Add(ans);
                    else
                        ansBottom.Add(ans);
                }

                //Replace m_answers with sorted answers
                __instance.m_Answers.Clear();
                foreach (var ans in ansTop)
                    __instance.m_Answers.Add(ans);
                foreach (var ans in ansBottom)
                    __instance.m_Answers.Add(ans);
            }
        }

        private static List<AlignmentShiftDirection> EVIL_ALIGNMENT = new List<AlignmentShiftDirection>{
            AlignmentShiftDirection.Evil, AlignmentShiftDirection.LawfulEvil, AlignmentShiftDirection.NeutralEvil, AlignmentShiftDirection.ChaoticEvil,
        };

        private static bool IsToMove(BlueprintAnswer ans)
        {
            return (Settings.isToMove_previouslyChosen && IsPreviouslyChosen(ref ans))
                || (Settings.isToMove_evilShifting && IsEvilShifting(ref ans))
                || (Settings.isToMove_attackingNPC && IsAttackingNPC(ref ans))
                || (Settings.isToMove_unrecruit && IsUnrecruit(ref ans));
        }

        private static bool IsPreviouslyChosen(ref BlueprintAnswer ans)
        {
            return ans.IsAlreadySelected();
        }

        private static bool IsEvilShifting(ref BlueprintAnswer ans)
        {
            return EVIL_ALIGNMENT.Contains(ans.AlignmentShift.Direction);
        }

        private static bool IsAttackingNPC(ref BlueprintAnswer ans)
        {
            return ChoiceKeys.ATTACK_NPC.Contains(ans.Text.Key);
        }

        private static bool IsUnrecruit(ref BlueprintAnswer ans)
        {
            return ChoiceKeys.UNRECRUIT.Contains(ans.Text.Key);
        }
    }
}
