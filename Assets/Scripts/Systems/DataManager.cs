using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MoonSharp;
using MoonSharp.Interpreter;

namespace FacSimiles.Systems
{
    public static class DataManager
    {
        public static Dictionary<string, DynValue> gameData = new Dictionary<string, DynValue>();

        public static DynValue GetData(string index)
        {
            return gameData[index];
        }

        public static void SetData(string index, DynValue value)
        {
            gameData[index] = value;
        }
		
		public static string GetBanksFolder() => Application.streamingAssetsPath + "/Banks";
		public static string GetBankPath(string name) => GetBanksFolder() + "/" + name + ".bank";
		
		public static string GetUnpackedBanksFolder() => Application.temporaryCachePath + "/Banks";
		public static string GetUnpackedBankPath(string name) => GetUnpackedBanksFolder() + "/" + name;
    }
}