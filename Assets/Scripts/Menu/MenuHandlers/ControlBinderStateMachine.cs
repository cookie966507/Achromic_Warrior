using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class ControlBinderStateMachine
    {
        public enum State
        {
            Sleep, Attack, Block, Jump, CycleLeft, CycleRight, Left, Right, ChangeColor, Up, Down, Super, Accept, Cancel, Pause, Default, Exit, GettingKey, Holding, Prep
        }
        private delegate State machine(bool accept);
        private machine[] getNextState;
        private State currState;
        private static State prevState;
        private State sleepState;

        public ControlBinderStateMachine()
        {
            currState = State.Sleep;
            prevState = State.Attack;
            sleepState = State.Attack;
            getNextState = new machine[] { Sleep, Attack, Block, Jump, CycleLeft, CycleRight, Left, Right, ChangeColor, Up, Down, Super, Accept, Cancel, Pause, Default, Exit, GettingKey, Holding, Prep };
        }

        public State update(bool accept)
        {
            return currState = getNextState[((int)currState)](accept);
        }

        internal void wake()
        {
            currState = sleepState;
        }

        internal void sleep()
        {
            if (currState != State.Sleep)
            {
                sleepState = currState;
                currState = State.Sleep;
            }
        }

        public State getPrieviousState()
        {
            return prevState;
        }
        public void Hold()
        {
            currState = State.Holding;
        }
        private static State Holding(bool accept)
        {
            if (CustomInput.AnyInput())
                return State.Holding;
            return prevState;
        }

        private static State Sleep(bool accept)
        {
            return State.Sleep;
        }

        private static State Prep(bool accept)
        {
            if (CustomInput.AnyInput())
                return State.Prep;
            return State.GettingKey;
        }

        private static State Attack(bool accept)
        {
            if (accept)
            {
                prevState = State.Attack;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Default;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Cancel;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Up;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Block;
            return State.Attack;
        }
        private static State CycleLeft(bool accept)
        {
            if (accept)
            {
                prevState = State.CycleLeft;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Cancel;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Down;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Block;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.CycleRight;
            return State.CycleLeft;
        }
        private static State CycleRight(bool accept)
        {
            if (accept)
            {
                prevState = State.CycleRight;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.CycleLeft;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Left;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Jump;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.ChangeColor;
            return State.CycleRight;
        }
        private static State ChangeColor(bool accept)
        {
            if (accept)
            {
                prevState = State.ChangeColor;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.CycleRight;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Right;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Super;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Exit;
            return State.ChangeColor;
        }
        private static State Super(bool accept)
        {
            if (accept)
            {
                prevState = State.Super;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Jump;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.ChangeColor;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Right;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Pause;
            return State.Super;
        }
        private static State Jump(bool accept)
        {
            if (accept)
            {
                prevState = State.Jump;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Block;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
				return State.CycleRight;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Left;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Super;
            return State.Jump;
        }
        private static State Block(bool accept)
        {
            if (accept)
            {
                prevState = State.Block;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Attack;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.CycleLeft;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Down;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Jump;
            return State.Block;
        }
        private static State Pause(bool accept)
        {
            if (accept)
            {
                prevState = State.Pause;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Super;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Accept;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Accept;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Default;
            return State.Pause;
        }
        private static State Accept(bool accept)
        {
            if (accept)
            {
                prevState = State.Accept;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Right;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Pause;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Pause;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Exit;
            return State.Accept;
        }
        private static State Cancel(bool accept)
        {
            if (accept)
            {
                prevState = State.Cancel;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Exit;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Up;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Attack;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.CycleLeft;
            return State.Cancel;
        }
        private static State Up(bool accept)
        {
            if (accept)
            {
                prevState = State.Up;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Exit;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Attack;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Cancel;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Down;
            return State.Up;
        }
        private static State Down(bool accept)
        {
            if (accept)
            {
                prevState = State.Down;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Up;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Block;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.CycleLeft;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Left;
            return State.Down;
        }
        private static State Left(bool accept)
        {
            if (CustomInput.AcceptFreshPress)
            {
                prevState = State.Left;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Down;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Jump;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.CycleRight;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Right;
            return State.Left;
        }
        private static State Right(bool accept)
        {
            if (accept)
            {
                prevState = State.Right;
                return State.Prep;
            }
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Left;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Super;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.ChangeColor;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Accept;
            return State.Right;
        }
        private static State Default(bool accept)
        {
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Pause;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Exit;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Exit;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Attack;
            return State.Default;
        }
        private static State Exit(bool accept)
        {
			if (CustomInput.UpFreshPress || Input.GetKeyDown(KeyCode.UpArrow))
                return State.Accept;
			if (CustomInput.LeftFreshPress || Input.GetKeyDown(KeyCode.LeftArrow))
                return State.Default;
			if (CustomInput.RightFreshPress || Input.GetKeyDown(KeyCode.RightArrow))
                return State.Default;
			if (CustomInput.DownFreshPress || Input.GetKeyDown(KeyCode.DownArrow))
                return State.Up;
            return State.Exit;
        }
        private static State GettingKey(bool accept)
        {
            return State.GettingKey;
        }

        public State AttackClicked()
        {
            prevState = State.Attack;
            currState = State.GettingKey;
            return currState;
        }
        public State CycleLeftClicked()
        {
            prevState = State.CycleLeft;
            currState = State.GettingKey;
            return currState;
        }
        public State CycleRightClicked()
        {
            prevState = State.CycleRight;
            currState = State.GettingKey;
            return currState;
        }
        public State ChangeColorClicked()
        {
            prevState = State.ChangeColor;
            currState = State.GettingKey;
            return currState;
        }
        public State SuperClicked()
        {
            prevState = State.Super;
            currState = State.GettingKey;
            return currState;
        }
        public State JumpClicked()
        {
            prevState = State.Jump;
            currState = State.GettingKey;
            return currState;
        }
        public State BlockClicked()
        {
            prevState = State.Block;
            currState = State.GettingKey;
            return currState;
        }
        public State PauseClicked()
        {
            prevState = State.Pause;
            currState = State.GettingKey;
            return currState;
        }
        public State AcceptClicked()
        {
            prevState = State.Accept;
            currState = State.GettingKey;
            return currState;
        }
        public State CancelClicked()
        {
            prevState = State.Cancel;
            currState = State.GettingKey;
            return currState;
        }
        public State UpClicked()
        {
            prevState = State.Up;
            currState = State.GettingKey;
            return currState;
        }
        public State DownClicked()
        {
            prevState = State.Down;
            currState = State.GettingKey;
            return currState;
        }
        public State LeftClicked()
        {
            prevState = State.Left;
            currState = State.GettingKey;
            return currState;
        }
        public State RightClicked()
        {
            prevState = State.Right;
            currState = State.GettingKey;
            return currState;
        }
    }
}
