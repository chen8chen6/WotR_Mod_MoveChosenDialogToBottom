using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;

namespace MoveChosenDialogToBottom
{
    static class Main
    {
        public static Settings Settings;
        public static bool Enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Settings = Settings.Load<Settings>(modEntry);
            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Dialogs Moved to Bottom: ", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            DrawToggler("Previously Chosen", ref Settings.isToMove_previouslyChosen);
            DrawToggler("Evil Shifting    ", ref Settings.isToMove_evilShifting);
            DrawToggler("Attacking NPC    ", ref Settings.isToMove_attackingNPC);
            DrawToggler("Unrecruit        ", ref Settings.isToMove_unrecruit);
        }

        static void DrawToggler(string label, ref bool option)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(label, GUILayout.ExpandWidth(false));
            GUILayout.Space(10);
            option = GUILayout.Toggle(option, "", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }
    }
}