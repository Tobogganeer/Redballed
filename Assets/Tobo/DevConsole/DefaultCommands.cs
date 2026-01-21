using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tobo.DevConsole
{
    public static class DefaultCommands
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnLoad()
        {
            sv_cheats = null;

            cl_testcheat = null;
            cl_test = null;
            mp_thing = null;
            fps_max = null;

            help = null;
        }

        public static ConVar sv_cheats;

        public static ConVar cl_testcheat;
        public static ConVar cl_test;
        public static ConVar mp_thing;
        public static ConVar fps_max;

        public static ConCommand help;

        public static void Register()
        {
            sv_cheats = new ConVar("sv_cheats", "0", CVarFlags.ServerOnly | CVarFlags.Replicated | CVarFlags.Notify, "Enables cheats", CVarType.Bool);

            cl_testcheat = new ConVar("cl_testcheat", "0", CVarFlags.Cheat, "test", CVarType.Bool);
            cl_test = new ConVar("cl_test", "test value", CVarFlags.None, "test");
            mp_thing = new ConVar("mp_thing", "idk", CVarFlags.None, "test");
            fps_max = new ConVar("fps_max", "144", CVarFlags.None, "test (does nothing)");

            help = new ConCommand("help", Help, "Prints help for a command, or all commands if none are specified.", CVarFlags.None, HelpCompletion);
        }

        static void Help(CmdArgs args)
        {
            // TODO: help command
        }

        static List<string> HelpCompletion(string partialFirstArg)
        {
            List<string> allCommandsAndArgs = ConCommand.cCommands.Keys.ToList();
            allCommandsAndArgs.AddRange(ConVar.cVars.Keys);
            return allCommandsAndArgs;
        }
    }
}
