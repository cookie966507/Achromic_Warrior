using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
	class Lose : MonoBehaviour
	{
		public GameObject[] cursors;
		private static Canvas win;
		
		private static LoseStateMachine machine = new LoseStateMachine();
		private delegate void state();
		private state[] doState;
		private LoseStateMachine.lose currState;

		void Start()
		{
			win = this.gameObject.GetComponent<Canvas>();
			doState = new state[] { Sleep, Restart, Quit };
			win.enabled = false;
		}
		
		void Update()
		{
			LoseStateMachine.lose prevState = currState;
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
			if(Data.GameManager.State==Enums.GameState.Lose)
			{
				machine.goTo(LoseStateMachine.lose.restart);
				win.enabled = true;
			}
		}
		
		private static void Restart()
		{
			if (CustomInput.AcceptFreshPressDeleteOnRead)
				doRestart();
		}
		private static void doRestart()
		{

			win.enabled = false;
			machine.goTo(LoseStateMachine.lose.sleep);
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
			win.enabled = false;
			machine.goTo(LoseStateMachine.lose.sleep);
			Data.GameManager.GotoLevel("Level_Select");
		}
		
		public void RestartClick()
		{
			machine.goTo(LoseStateMachine.lose.restart);
			foreach (GameObject g in cursors)
				g.SetActive(false);
			cursors[(int)LoseStateMachine.lose.restart - 1].SetActive(true);
			doRestart();
		}
		
		public void QuitClick()
		{
			machine.goTo(LoseStateMachine.lose.quit);
			foreach (GameObject g in cursors)
				g.SetActive(false);
			cursors[(int)LoseStateMachine.lose.restart - 1].SetActive(true);
			doQuit();
		}
	}
	class LoseStateMachine
	{
		internal enum lose { sleep, restart, quit };
		private delegate lose machine();//function pointer
		private machine[] getNextState;//array of function pointers
		private lose currState;
		
		internal LoseStateMachine()
		{
			currState = lose.sleep;
			//fill array with functions
			getNextState = new machine[] { Sleep, Restart, Quit };
		}
		
		internal lose update()
		{
			return currState = getNextState[((int)currState)]();
		}
		
		internal void goTo(lose state)
		{
			currState = state;
		}
		
		//The following methods control when and how you can transition between states
		private static lose Sleep()
		{
			return lose.sleep;
		}
		
		private static lose Restart()
		{
			if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead)
					return lose.quit;
			return lose.restart;
		}
		private static lose Quit()
		{
			if (CustomInput.LeftFreshPressDeleteOnRead || CustomInput.RightFreshPressDeleteOnRead)
					return lose.restart;
			return lose.quit;
		}
	}
}
