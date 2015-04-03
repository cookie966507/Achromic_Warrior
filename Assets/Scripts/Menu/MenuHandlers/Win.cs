using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
	class Win : MonoBehaviour
	{
		public GameObject[] cursors;
		private static Canvas window;
		
		private static WinStateMachine machine = new WinStateMachine();
		private delegate void state();
		private state[] doState;
		private WinStateMachine.win currState;
		
		void Awake()
		{
			window = this.gameObject.GetComponent<Canvas>();
			doState = new state[] { Sleep, Restart, Quit };
			window.enabled = false;
		}
		
		void Update()
		{
			WinStateMachine.win prevState = currState;
			currState = machine.update();
			if (prevState != currState)
			{
				foreach (GameObject g in cursors)
					g.SetActive(false);
				int cursor = (int)currState - 1;
				if (cursor >= 0)
					cursors[cursor].SetActive(true);
			}
			doState[(int)currState]();
		}
		private static void Sleep()
		{
			if(Data.GameManager.State==Enums.GameState.Win)
			{
				machine.goTo(WinStateMachine.win.restart);
				window.enabled = true;
			}
		}
		
		private static void Restart()
		{
			if (CustomInput.AcceptFreshPressDeleteOnRead)
				doRestart();
		}
		private static void doRestart()
		{
			
			window.enabled = false;
			machine.goTo(WinStateMachine.win.sleep);
			Data.GameManager.GotoLevel(Application.loadedLevelName);
			Data.GameManager.Unpause();
		}
		
		private static void Quit()
		{
			if (CustomInput.AcceptFreshPressDeleteOnRead)
				doQuit();
		}
		private static void doQuit()
		{
			window.enabled = false;
			machine.goTo(WinStateMachine.win.sleep);
			Data.GameManager.GotoLevel("Level_Select");
		}
		
		public void RestartClick()
		{
			machine.goTo(WinStateMachine.win.restart);
			foreach (GameObject g in cursors)
				g.SetActive(false);
			cursors[(int)WinStateMachine.win.restart - 1].SetActive(true);
			doRestart();
		}
		
		public void QuitClick()
		{
			machine.goTo(WinStateMachine.win.quit);
			foreach (GameObject g in cursors)
				g.SetActive(false);
			cursors[(int)WinStateMachine.win.restart - 1].SetActive(true);
			doQuit();
		}
	}
	class WinStateMachine
	{
		internal enum win { sleep, restart, quit };
		private delegate win machine();//function pointer
		private machine[] getNextState;//array of function pointers
		private win currState;
		
		internal WinStateMachine()
		{
			currState = win.sleep;
			//fill array with functions
			getNextState = new machine[] { Sleep, Restart, Quit };
		}
		
		internal win update()
		{
			return currState = getNextState[((int)currState)]();
		}
		
		internal void goTo(win state)
		{
			currState = state;
		}
		
		//The following methods control when and how you can transition between states
		private static win Sleep()
		{
			return win.sleep;
		}
		
		private static win Restart()
		{
			if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
				return win.quit;
			return win.restart;
		}
		private static win Quit()
		{
			if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
				return win.restart;
			return win.quit;
		}
	}
}
