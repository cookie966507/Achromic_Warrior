using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class Gamepad : ButtonHandler
    {
        public Canvas prompt;
        public UnityEngine.UI.Text promptText;
        public GameObject[] cursors;
        public UnityEngine.UI.Text Attack;
        public UnityEngine.UI.Text Block;
        public UnityEngine.UI.Text Jump;
        public UnityEngine.UI.Text CycleLeft;
        public UnityEngine.UI.Text CycleRight;
        public UnityEngine.UI.Text Left;
        public UnityEngine.UI.Text Right;
        public UnityEngine.UI.Text ChangeColor;
        public UnityEngine.UI.Text Up;
        public UnityEngine.UI.Text Down;
        public UnityEngine.UI.Text Super;
        public UnityEngine.UI.Text Accept;
        public UnityEngine.UI.Text Cancel;
        public UnityEngine.UI.Text Pause;
        
        private ControlBinderStateMachine machine = new ControlBinderStateMachine();
        private delegate void state();
        private ControlBinderStateMachine.State currState;
        private bool duplicate;
        private bool running=false;

        private static bool isLeft;

        public override void setLeft()
        {
            isLeft = true;
        }

        public override void setRight()
        {
            isLeft = false;
        }

        void Start()
        {
            Attack.text = CustomInput.GamePadAttack;
            Block.text = CustomInput.GamePadBlock;
            Jump.text = CustomInput.GamePadJump;
            CycleLeft.text = CustomInput.GamePadCycleLeft;
            CycleRight.text = CustomInput.GamePadCycleRight;
            Left.text = CustomInput.GamePadLeft;
            Right.text = CustomInput.GamePadRight;
            ChangeColor.text = CustomInput.GamePadChangeColor;
            Up.text = CustomInput.GamePadUp;
            Down.text = CustomInput.GamePadDown;
            Super.text = CustomInput.GamePadSuper;
            Accept.text = CustomInput.GamePadAccept;
            Cancel.text = CustomInput.GamePadCancel;
            Pause.text = CustomInput.GamePadPause;
        }

        void Update()
        {
            ControlBinderStateMachine.State prevState = currState;
            if (running)
            {
                bool accept = CustomInput.AcceptFreshPressDeleteOnRead;
                currState = machine.update(accept);
                if (prevState != currState)
                {
                    foreach (GameObject g in cursors)
                        g.SetActive(false);
                    int cursor = (int)currState-1;
                    if (cursor >= 0 && cursor<18)
                        cursors[cursor].SetActive(true);
                    else if(currState==ControlBinderStateMachine.State.Prep)
                        Kernel.disalble();
                    else if(prevState==ControlBinderStateMachine.State.Holding)
                        Kernel.enalble();
                }
                if (accept)
                {
                    if (currState == ControlBinderStateMachine.State.Default)
                    {
                        CustomInput.DefaultPad();
                        Attack.text = CustomInput.GamePadAttack;
                        Block.text = CustomInput.GamePadBlock;
                        Jump.text = CustomInput.GamePadJump;
                        CycleLeft.text = CustomInput.GamePadCycleLeft;
                        CycleRight.text = CustomInput.GamePadCycleRight;
                        Left.text = CustomInput.GamePadLeft;
                        Right.text = CustomInput.GamePadRight;
                        ChangeColor.text = CustomInput.GamePadChangeColor;
                        Up.text = CustomInput.GamePadUp;
                        Down.text = CustomInput.GamePadDown;
                        Super.text = CustomInput.GamePadSuper;
                        Accept.text = CustomInput.GamePadAccept;
                        Cancel.text = CustomInput.GamePadCancel;
                        Pause.text = CustomInput.GamePadPause;
                    }
                    if (currState == ControlBinderStateMachine.State.Exit)
                        Kernel.controlsExit();
                }
            }
            if ((currState != ControlBinderStateMachine.State.Holding && currState != ControlBinderStateMachine.State.GettingKey) && CustomInput.CancelFreshPressDeleteOnRead)
                Kernel.controlsExit();            
        }

        public override void wake()
        {
            machine.wake();
            running = true;
            foreach (GameObject g in cursors)
                g.SetActive(false);
            int cursor = (int)currState - 1;
            if (cursor >= 0)
                cursors[cursor].SetActive(true);
        }

        public override void sleep()
        {
            machine.sleep();
            running = false;
        }

        private static void Sleep()
        {
        }

        public void AttackClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.AttackClicked();
        }
        public void CycleLeftClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.CycleLeftClicked();
        }
        public void CycleRightClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.CycleRightClicked();
        }
        public void ChangeColorClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.ChangeColorClicked();
        }
        public void SuperClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.SuperClicked();
        }
        public void JumpClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.JumpClicked();
        }
        public void BlockClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.BlockClicked();
        }
        public void PauseClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.PauseClicked();
        }
        public void AcceptClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.AcceptClicked();
        }
        public void CancelClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.CancelClicked();
        }
        public void UpClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.UpClicked();
        }
        public void DownClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.DownClicked();
        }
        public void LeftClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.LeftClicked();
        }
        public void RightClicked()
        {
            foreach (GameObject g in cursors)
                g.SetActive(false);
            machine.RightClicked();
        }
        public void DefaultClicked()
        {
            CustomInput.DefaultPad();
            Attack.text = CustomInput.GamePadAttack;
            Block.text = CustomInput.GamePadBlock;
            Jump.text = CustomInput.GamePadJump;
            CycleLeft.text = CustomInput.GamePadCycleLeft;
            CycleRight.text = CustomInput.GamePadCycleRight;
            Left.text = CustomInput.GamePadLeft;
            Right.text = CustomInput.GamePadRight;
            ChangeColor.text = CustomInput.GamePadChangeColor;
            Up.text = CustomInput.GamePadUp;
            Down.text = CustomInput.GamePadDown;
            Super.text = CustomInput.GamePadSuper;
            Accept.text = CustomInput.GamePadAccept;
            Cancel.text = CustomInput.GamePadCancel;
            Pause.text = CustomInput.GamePadPause;
        }
        public void ExitClicked()
        {
            Kernel.controlsExit();
        }

        private void GetNewButton()
        {
            if (!duplicate)
                promptText.text = "Press the new key you want to use, Escape to cancel.";
            else
                promptText.text = "Press the new key you want to use, Escape to cancel.\nError duplicate Key";

            if (Input.GetAxis(CustomInput.LEFT_STICK_RIGHT) > 0)
                setButton(CustomInput.LEFT_STICK_RIGHT);
            if (Input.GetAxis(CustomInput.LEFT_STICK_LEFT) < 0)
                setButton(CustomInput.LEFT_STICK_LEFT);
            else if (Input.GetAxis(CustomInput.LEFT_STICK_UP) < 0)
                setButton(CustomInput.LEFT_STICK_UP);
            else if (Input.GetAxis(CustomInput.LEFT_STICK_DOWN) > 0)
                setButton(CustomInput.LEFT_STICK_DOWN);
            else if (Input.GetAxis(CustomInput.RIGHT_STICK_RIGHT) > 0)
                setButton(CustomInput.RIGHT_STICK_RIGHT);
            else if (Input.GetAxis(CustomInput.RIGHT_STICK_LEFT) < 0)
                setButton(CustomInput.RIGHT_STICK_LEFT);
            else if (Input.GetAxis(CustomInput.RIGHT_STICK_UP) < 0)
                setButton(CustomInput.RIGHT_STICK_UP);
            else if (Input.GetAxis(CustomInput.RIGHT_STICK_DOWN) > 0)
                setButton(CustomInput.RIGHT_STICK_DOWN);
            else if (Input.GetAxis(CustomInput.DPAD_RIGHT) > 0)
                setButton(CustomInput.DPAD_RIGHT);
            else if (Input.GetAxis(CustomInput.DPAD_LEFT) < 0)
                setButton(CustomInput.DPAD_LEFT);
            else if (Input.GetAxis(CustomInput.DPAD_UP) > 0)
                setButton(CustomInput.DPAD_UP);
            else if (Input.GetAxis(CustomInput.DPAD_DOWN) < 0)
                setButton(CustomInput.DPAD_DOWN);
            else if (Input.GetAxis(CustomInput.LEFT_TRIGGER) != 0)
                setButton(CustomInput.LEFT_TRIGGER);
            else if (Input.GetAxis(CustomInput.RIGHT_TRIGGER) != 0)
                setButton(CustomInput.RIGHT_TRIGGER);
            else if (Input.GetAxis(CustomInput.A) != 0)
                setButton(CustomInput.A);
            else if (Input.GetAxis(CustomInput.B) != 0)
                setButton(CustomInput.B);
            else if (Input.GetAxis(CustomInput.X) != 0)
                setButton(CustomInput.X);
            else if (Input.GetAxis(CustomInput.Y) != 0)
                setButton(CustomInput.Y);
            else if (Input.GetAxis(CustomInput.LB) != 0)
                setButton(CustomInput.LB);
            else if (Input.GetAxis(CustomInput.RB) != 0)
                setButton(CustomInput.RB);
            else if (Input.GetAxis(CustomInput.BACK) != 0)
                setButton(CustomInput.BACK);
            else if (Input.GetAxis(CustomInput.START) != 0)
                setButton(CustomInput.START);
            else if (Input.GetAxis(CustomInput.LEFT_STICK) != 0)
                setButton(CustomInput.LEFT_STICK);
            else if (Input.GetAxis(CustomInput.RIGHT_STICK) != 0)
                setButton(CustomInput.RIGHT_STICK);
        }

        private void setButton(string button)
        {
            if (machine.getPrieviousState() == ControlBinderStateMachine.State.Accept)
            {
                if (button == CustomInput.GamePadCancel)
                    duplicate = true;
                else
                    duplicate = false;
            }
            else if (machine.getPrieviousState() == ControlBinderStateMachine.State.Cancel)
            {
                if (button == CustomInput.GamePadAccept)
                    duplicate = true;
                else
                    duplicate = false;
            }
            else
            {
                switch (machine.getPrieviousState())
                {
                    case ControlBinderStateMachine.State.Attack:
                        {
                            if (button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Block:
                        {
                            if (button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Jump:
                        {
                            if (button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.CycleLeft:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.CycleRight:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.ChangeColor:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Super:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Up:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Down:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Left:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Right:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadAttack ||
                                button == CustomInput.GamePadPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    default:
                        {
                            if (button == CustomInput.GamePadJump ||
                                button == CustomInput.GamePadBlock ||
                                button == CustomInput.GamePadCycleLeft ||
                                button == CustomInput.GamePadCycleRight ||
                                button == CustomInput.GamePadChangeColor ||
                                button == CustomInput.GamePadSuper ||
                                button == CustomInput.GamePadUp ||
                                button == CustomInput.GamePadDown ||
                                button == CustomInput.GamePadLeft ||
                                button == CustomInput.GamePadRight ||
                                button == CustomInput.GamePadAttack)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                }
            }

            if (!duplicate)
            {
                switch (machine.getPrieviousState())
                {
                    case ControlBinderStateMachine.State.Attack: CustomInput.GamePadAttack = button; Attack.text = CustomInput.GamePadAttack; break;
                    case ControlBinderStateMachine.State.Block: CustomInput.GamePadBlock = button; Block.text = CustomInput.GamePadBlock; break;
                    case ControlBinderStateMachine.State.Jump: CustomInput.GamePadJump = button; Jump.text = CustomInput.GamePadJump; break;
                    case ControlBinderStateMachine.State.CycleLeft: CustomInput.GamePadCycleLeft = button; CycleLeft.text = CustomInput.GamePadCycleLeft; break;
                    case ControlBinderStateMachine.State.CycleRight: CustomInput.GamePadCycleRight = button; CycleRight.text = CustomInput.GamePadCycleRight; break;
                    case ControlBinderStateMachine.State.ChangeColor: CustomInput.GamePadChangeColor = button; Left.text = CustomInput.GamePadLeft; break;
                    case ControlBinderStateMachine.State.Super: CustomInput.GamePadSuper = button; Right.text = CustomInput.GamePadRight; break;
                    case ControlBinderStateMachine.State.Up: CustomInput.GamePadUp = button; ChangeColor.text = CustomInput.GamePadChangeColor; break;
                    case ControlBinderStateMachine.State.Down: CustomInput.GamePadDown = button; Up.text = CustomInput.GamePadUp; break;
                    case ControlBinderStateMachine.State.Left: CustomInput.GamePadLeft = button; Down.text = CustomInput.GamePadDown; break;
                    case ControlBinderStateMachine.State.Right: CustomInput.GamePadRight = button; Super.text = CustomInput.GamePadSuper; break;
                    case ControlBinderStateMachine.State.Accept: CustomInput.GamePadAccept = button; Accept.text = CustomInput.GamePadAccept; break;
                    case ControlBinderStateMachine.State.Cancel: CustomInput.GamePadCancel = button; Cancel.text = CustomInput.GamePadCancel; break;
                    default: CustomInput.GamePadPause = button; Pause.text = CustomInput.GamePadPause; break;
                }
                machine.Hold();
            }
        }

        void OnGUI()
        {
            if (currState != ControlBinderStateMachine.State.Sleep)
            {
                if (currState == ControlBinderStateMachine.State.GettingKey)
                {
                    prompt.enabled = true;
                    if (Input.GetKey(KeyCode.Escape))
                        machine.Hold();
                    else
                        GetNewButton();
                }
                else
                    prompt.enabled = false;
            }
        }
    }
}
