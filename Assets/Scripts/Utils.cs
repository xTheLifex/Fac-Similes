using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace FacSimiles.Utility
{
	public static class Utils
	{
		public static string[] GetFilesRecursive(string path)
		{
			List<string> results = new List<string>();
			
			string[] files = Directory.GetFiles(path);
			string[] dirs = Directory.GetDirectories(path);
			
			foreach(string file in files)
			{
				results.Add(file);
			}
			
			foreach(string dir in dirs)
			{
				string[] dirfiles = GetFilesRecursive(dir);
				foreach(string file in dirfiles)
				{
					results.Add(file);
				}
			}
			return results.ToArray();
		}
	}
}