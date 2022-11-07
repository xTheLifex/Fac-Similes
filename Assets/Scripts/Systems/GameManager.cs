using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MoonSharp;
using MoonSharp.Interpreter;

namespace FacSimiles.Systems
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public enum GameState
        {
            MENU, // Main menu
            CYS, // Create Your FacSimile
            GAME, // In-Game
            BUILD, // Build mode
            WORLD // World view mode.
        }

        private GameState _gameState;

        public GameState gameState
        {
            get { return _gameState; }
            set { _gameState = value; }
        }

        

        private void Start() {
            if (instance)
            {
                Destroy(gameObject);
            } else
            {
                instance = this;
            }
        }

        IEnumerator IBeginLoad()
        {
            // TODO: Load lua and VM.
            yield return new WaitForSeconds(1f);
            
            // TODO: Fade in loading screen stuff
        }

        IEnumerator IStopLoad()
        {
            yield return new WaitForSeconds(1f);
        }

        IEnumerator ILoadScene(int index)
        {
            yield return IBeginLoad();
            // TODO: Load scene here.
            yield return new WaitForSeconds(10f);
            yield return IStopLoad();
        }
    }
}