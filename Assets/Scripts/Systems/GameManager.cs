using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using MoonSharp;
using MoonSharp.Interpreter;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using SimpleZip;
using FacSimiles.Utility;


namespace FacSimiles.Systems
{
    public class GameManager : MonoBehaviour
    {
		/* -------------------------------------------------------------------------- */
		/*                                Definitions                                 */
		/* -------------------------------------------------------------------------- */

        public static GameManager instance;

        public Transform loadingScreen;
        public enum GameState
        {
            MENU, // Main menu
            CYS, // Create Your FacSimile
            GAME, // In-Game
            BUILD, // Build mode
            WORLD // World view mode.
        }
		
		public string MenuScene;
		public string CYSScene;
		public string GameScene; // Includes Build and World.
		
		public bool loading = false;

        private GameState _gameState = GameState.MENU;

        public GameState gameState
        {
            get { return _gameState; }
            set 
			{
				OnChangeGameState(_gameState, value);
				_gameState = value;
			}
        }
		
		public Script Lua;
		
		#if UNITY_EDITOR
		public bool skipPacking = false;
		#endif

		
		/* -------------------------------------------------------------------------- */
		/*                                 Methods                                    */
		/* -------------------------------------------------------------------------- */		

        private void OnChangeGameState(GameState oldState, GameState newState)
		{
			// TODO: Call GameAPI.
		}

        private void Awake() 
        {
            if (instance)
            {
                Destroy(gameObject);
            } else
            {
                instance = this;
            }
			DontDestroyOnLoad(gameObject);
            StartCoroutine(IGameSetup());
        }

        private Color Alpha(Color c, float alpha)
        {
            return new Color(c.r, c.g, c.b, alpha);
        }
		
		public void SetLoadingText(string text)
		{
			if (loadingScreen)
			{
                Transform elements = loadingScreen.Find("Elements");
				
				Transform t = elements.Find("TextProgress");
                TextMeshProUGUI loading = t.GetComponent<TextMeshProUGUI>();
                if (t)
				    loading.text = text;
			}
		}
		
		public void SetLoadingTipText(string text)
		{
			if (loadingScreen)
			{
                Transform elements = loadingScreen.Find("Elements");
				
				Transform t = elements.Find("TextTip");
                TextMeshProUGUI tip = t.GetComponent<TextMeshProUGUI>();
                if (t)
				    tip.text = text;
			}
		}
		
		public string Format(string str)
		{
			// TODO: Add replacements for the # values.
            return str;
		}
		
		/* -------------------------------------------------------------------------- */
		/*                           Loading Transitions                              */
		/* -------------------------------------------------------------------------- */

        IEnumerator IBeginLoad(float speed = 1f)
        {
            if (loadingScreen)
            {
				if (loadingScreen.gameObject.activeSelf)
					yield break;

				// Enable our loading screen object.
                loadingScreen.gameObject.SetActive(true);
				loading = true;
				
                Transform background = loadingScreen.Find("Background");
                Transform elements = loadingScreen.Find("Elements");

                // Make sure our background is enabled too
                background.gameObject.SetActive(true);

                // Fade in the loading background before the elements show up. 
                RawImage bgimg = background.GetComponent<RawImage>();
                int timeout = 0;
                bgimg.color = new Color(1f,1f,1f,0f);
				if (speed > 0f)
				{
					while (timeout < 9999 && bgimg.color.a < 1f)
					{
						bgimg.color = Alpha(bgimg.color, bgimg.color.a + speed * Time.deltaTime);
						yield return null;
					}
				}
				bgimg.color = new Color(1f,1f,1f,1f);
                //Enable the elements.
                elements.gameObject.SetActive(true);
            }
        }

        IEnumerator IStopLoad(float speed = 1f)
        {
            if (loadingScreen)
            {
                if (!loadingScreen.gameObject.activeSelf)
                    yield break;
                
                Transform background = loadingScreen.Find("Background");
                Transform elements = loadingScreen.Find("Elements");
                
                // Disable the elements such as logo and loading effects.
                elements.gameObject.SetActive(false);

                // Fade away the background before disabling it.
                RawImage bgimg = background.GetComponent<RawImage>();
                int timeout = 0;
                bgimg.color = new Color(1f,1f,1f,1f);
				if (speed > 0f)
				{ 
					while (timeout < 9999 && bgimg.color.a > 0f)
					{
						bgimg.color = Alpha(bgimg.color, bgimg.color.a - speed * Time.deltaTime);
						yield return null;
					}
				}
                bgimg.color = new Color(1f,1f,1f,0f);
                background.gameObject.SetActive(false);
				
				// Disable our loading screen object
				loadingScreen.gameObject.SetActive(false);
            }
        }


		/* -------------------------------------------------------------------------- */
		/*                                IEnumerators                                */
		/* -------------------------------------------------------------------------- */
        IEnumerator IGameSetup()
        {
            yield return StartCoroutine(IBeginLoad(-1f)); // Instant.

			#if UNITY_EDITOR
			// We are running in Unity Editor.
			/* -------------------------------------------------------------------------- */
			if (!skipPacking)
			{
				SetLoadingText("Editor: Clearing game cache...");
				yield return StartCoroutine(IClearCache());
				
				// Compile our main game package.
				SetLoadingText("Editor: Compiling game banks...");
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/Game", "game"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC0", "dlc0"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC1", "dlc1"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC2", "dlc2"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC3", "dlc3"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC4", "dlc4"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC5", "dlc5"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC6", "dlc6"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC7", "dlc7"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC8", "dlc8"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC9", "dlc9"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC10", "dlc10"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC11", "dlc11"));
				yield return StartCoroutine(IPackBank(Application.streamingAssetsPath + "/BankSource/DLC12", "dlc12"));
			}
			/* -------------------------------------------------------------------------- */
			#endif

			// Support for future FREE DLCs.
			SetLoadingText("Preparing game banks...");
			yield return StartCoroutine(IUnpackBank("game"));
			yield return StartCoroutine(IUnpackBank("dlc0"));
			yield return StartCoroutine(IUnpackBank("dlc1"));
			yield return StartCoroutine(IUnpackBank("dlc2"));
			yield return StartCoroutine(IUnpackBank("dlc3"));
			yield return StartCoroutine(IUnpackBank("dlc4"));
			yield return StartCoroutine(IUnpackBank("dlc5"));
			yield return StartCoroutine(IUnpackBank("dlc6"));
			yield return StartCoroutine(IUnpackBank("dlc7"));
			yield return StartCoroutine(IUnpackBank("dlc8"));
			yield return StartCoroutine(IUnpackBank("dlc9"));
			yield return StartCoroutine(IUnpackBank("dlc10"));
			yield return StartCoroutine(IUnpackBank("dlc11"));
			yield return StartCoroutine(IUnpackBank("dlc12"));
			yield return StartCoroutine(IUnpackBank("dlc13"));
			yield return StartCoroutine(IUnpackBank("dlc14"));
			yield return StartCoroutine(IUnpackBank("dlc15"));
			yield return StartCoroutine(IUnpackBank("dlc16"));
			

			SetLoadingText("Loading Resources...");
			yield return StartCoroutine(ILoadResources());

			SetLoadingText("Initializing Lua...");
			yield return StartCoroutine(ILoadLua());
			

			
			// TODO: Make sure the game works correctly if we are hotloading into a scene
			// that isn't the menu.
			
            yield return StartCoroutine(IStopLoad());
			Logger.Log(Channel.Loading, "Game has loaded.");
        }
		
		IEnumerator IClearCache()
		{
			string path = Application.temporaryCachePath;
			DirectoryInfo info = new DirectoryInfo(path);
			
			foreach(FileInfo file in info.GetFiles())
			{
				file.Delete();
				yield return null;
			}
			
			foreach(DirectoryInfo dir in info.GetDirectories())
			{
				dir.Delete(true);
				yield return null;
			}
		}
		
		#if UNITY_EDITOR
		// This section will pass our lua code into a zip compression and save it in our own format.
		// This should be enough to deter any curious one from tampering with the game'sbyte
		// code and complaining after they broke the game.
		/* -------------------------------------------------------------------------- */
		private IEnumerator IPackBank(string path, string output)
		{
			if (!Directory.Exists(path))
			{
				Logger.Log(Channel.Loading, "Skipping Packing of " + path + ": Not found");
				yield break;
			}
			
			// Compiled Files Dictionary
			Dictionary<string[], string> cfd = new Dictionary<string[], string>();
			
			string[] files = Utils.GetFilesRecursive(path);
			int counter = 0;
			foreach(string file in files)
			{
				SetLoadingText("Editor: Compiling Banks...  " + counter + "/" + files.Length);
				counter++;
				if (file.EndsWith(".meta"))
					continue;
				
				List<string> lines = new List<string>();
				
				using(StreamReader sr = new StreamReader(file))
				{
					
					string line;
					int i = 0;
					while((line = sr.ReadLine()) != null)
					{
						i++;
						lines.Add(line);
						if (i > 300)
						{
							i = 0;
							yield return null;
						}
					}
				}
				
				
				
				List<string> zlines = new List<string>();
				int j = 0;
				foreach(string line in lines.ToArray())
				{
					j++;
					string zline = Zip.CompressToString(line);
					zlines.Add(zline);
					if (j > 300)
					{
						j = 0;
						yield return null;
					}
				}
				
				string[] compiled = zlines.ToArray();
				string relpath = Path.GetRelativePath(Application.streamingAssetsPath + "/BankSource", file);
				if (relpath != null && relpath != "")
				{
					cfd.Add(compiled, relpath);	
				}
				
				yield return null;
			}
			
			SetLoadingText("Editor: Packing Banks...");
			string bfolder = DataManager.GetBanksFolder();
			if (!Directory.Exists(bfolder))
			{
				Directory.CreateDirectory(bfolder);
			}
			
			using(StreamWriter sw = new StreamWriter(DataManager.GetBankPath(output)))
			{
				foreach(KeyValuePair<string[], string> kvp in cfd)
				{
					sw.WriteLine("BEGIN");
					sw.WriteLine(kvp.Value);
					foreach(string line in kvp.Key)
					{
						sw.WriteLine(line);
					}
					sw.WriteLine("END");
					yield return null;
				}
			}
			

		}

		/* -------------------------------------------------------------------------- */
		#endif
		
		
		IEnumerator IUnpackBank(string bankname)
		{
			string path = DataManager.GetBankPath(bankname);
			
			if (!File.Exists(path)) 
			{
				Logger.Log(Channel.Loading, "Cannot unpack bank '" + bankname + "': File does not exist.");
				yield break;
			}
			
			string unpackedDir = DataManager.GetUnpackedBanksFolder();

			if (!Directory.Exists(unpackedDir))
			{
				Directory.CreateDirectory(unpackedDir);
			}
			
			using (StreamReader sr = new StreamReader(path))
			{
				string line;
				while((line = sr.ReadLine()) != null)
				{
					if (line == "BEGIN")
					{
						string relativePath = sr.ReadLine();
						List<string> contents = new List<string>();
						string cline = "";						
						while (cline != "END")
						{
							cline = sr.ReadLine();
							
							if (cline == null)
							{
								Logger.Log(Channel.Loading, Priority.FatalError, "Bank unpack failed: EOF reached. Expected closure statement.");
								yield break;
							}
							
							if (cline != "END")
							{
								contents.Add(cline);
							}
							yield return null;
						}
						
						string fpath = DataManager.GetUnpackedBankPath(relativePath);
						Directory.CreateDirectory(Path.GetDirectoryName(fpath));
						
						//TODO: Write file
						using (StreamWriter sw = new StreamWriter(fpath))
						{
							int i=0;
							foreach(string contentLine in contents.ToArray())
							{
								i++;
								sw.WriteLine(Zip.Decompress(contentLine));
								if (i > 200)
								{
									i=0;
									yield return null;
								}
							}
						}
					}
				}
			}
			Logger.Log(Channel.Loading, "Unpacked bank '" + bankname + "' to " + unpackedDir);
		}
		
		IEnumerator ILoadLua()
		{
			Logger.Log(Channel.Loading, "Starting Lua scripts...");
			Lua = new Script();
			Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.UnityAssetsScriptLoader();
			Script.DefaultOptions.DebugPrint = s => Logger.Log(Channel.Lua, Format(s));
			UserData.RegisterAssembly();
			Lua.Globals["game"] = new GameAPI();
			
			// We should check our bank files for lua files and load them
			string[] files = Utils.GetFilesRecursive(DataManager.GetBanksFolder());
			foreach(string file in files)
			{
				// We only need "bank.lua" files. Any additional files are called by the bank.lua file itself.
				string fname = Path.GetFileName(file);
				if (fname == "bank.lua")
				{
					//TODO: Fix why global is not being called. Probably not loading file? Try executing it.
					Lua.LoadFile(fname);
					yield return null;
				}
			}

			// Call our init method.
			Lua.Call(Lua.Globals["Init"]);

			/*
			if (scriptFile)
			{
				Lua.DoString(scriptFile.text);
				
				Lua.Call(Lua.Globals["Init"]);
			} else
			{
				Logger.Log(Channel.Loading, Priority.Error, "Unable to start lua: game.lua cannot be found.");
			}
			*/
			yield return null;
		}
		
		IEnumerator ILoadResources()
		{
            Logger.Log(Channel.Loading, "Loading Resources...");
			yield return null;
		}
		
		IEnumerator ILoadScene(string name, bool single)
        {
			Logger.Log(Channel.Loading, "Loading scene..." + name);
            yield return StartCoroutine(IBeginLoad());
			AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(name, single ? LoadSceneMode.Single : LoadSceneMode.Additive);
			while (!asyncLoadScene.isDone)
			{
				yield return null;
			}
            yield return StartCoroutine(IStopLoad());
			Logger.Log(Channel.Loading, "Scene " + name + " loaded.");
        }
    }
}