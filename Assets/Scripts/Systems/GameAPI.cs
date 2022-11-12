using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MoonSharp;
using MoonSharp.Interpreter;


namespace FacSimiles.Systems
{
    [MoonSharpUserData]
    public class GameAPI
    {
        public void Log(string text)
        {
            Logger.Log(Channel.Lua, text);
        }

        public DynValue GetData(string index)
        {
            return DataManager.GetData(index);
        }

        public void SetData(string index, DynValue value)
        {
            DataManager.SetData(index, value);
        }
		
		public float CurTime()
		{
			return Time.time;
		}
		
		public float DeltaTime()
		{
			return Time.deltaTime;
		}
    }
}