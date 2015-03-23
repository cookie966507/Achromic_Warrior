using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Menu.MenuHandlers
{
    class KeyBoard : ButtonHandler
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
        private bool running = false;

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
            prompt.enabled = false;
            Attack.text = CustomInput.KeyBoardAttack.ToString();
            Block.text = CustomInput.KeyBoardBlock.ToString();
            Jump.text = CustomInput.KeyBoardJump.ToString();
            CycleLeft.text = CustomInput.KeyBoardCycleLeft.ToString();
            CycleRight.text = CustomInput.KeyBoardCycleRight.ToString();
            Left.text = CustomInput.KeyBoardLeft.ToString();
            Right.text = CustomInput.KeyBoardRight.ToString();
            ChangeColor.text = CustomInput.KeyBoardChangeColor.ToString();
            Up.text = CustomInput.KeyBoardUp.ToString();
            Down.text = CustomInput.KeyBoardDown.ToString();
            Super.text = CustomInput.KeyBoardSuper.ToString();
            Accept.text = CustomInput.KeyBoardAccept.ToString();
            Cancel.text = CustomInput.KeyBoardCancel.ToString();
            Pause.text = CustomInput.KeyBoardPause.ToString();
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
                    int cursor = (int)currState - 1;
                    if (cursor >= 0 && cursor < 16)
                        cursors[cursor].SetActive(true);
                    else if (currState == ControlBinderStateMachine.State.Prep)
                        Kernel.disalble();
                    else if (prevState == ControlBinderStateMachine.State.Holding)
                        Kernel.enalble();
                }
                if (accept)
                {
                    if (currState == ControlBinderStateMachine.State.Default)
                    {
                        CustomInput.DefaultKey();
                        Attack.text = CustomInput.KeyBoardAttack.ToString();
                        Block.text = CustomInput.KeyBoardBlock.ToString();
                        Jump.text = CustomInput.KeyBoardJump.ToString();
                        CycleLeft.text = CustomInput.KeyBoardCycleLeft.ToString();
                        CycleRight.text = CustomInput.KeyBoardCycleRight.ToString();
                        Left.text = CustomInput.KeyBoardLeft.ToString();
                        Right.text = CustomInput.KeyBoardRight.ToString();
                        ChangeColor.text = CustomInput.KeyBoardChangeColor.ToString();
                        Up.text = CustomInput.KeyBoardUp.ToString();
                        Down.text = CustomInput.KeyBoardDown.ToString();
                        Super.text = CustomInput.KeyBoardSuper.ToString();
                        Accept.text = CustomInput.KeyBoardAccept.ToString();
                        Cancel.text = CustomInput.KeyBoardCancel.ToString();
                        Pause.text = CustomInput.KeyBoardPause.ToString();
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
            CustomInput.DefaultKey();
            Attack.text = CustomInput.KeyBoardAttack.ToString();
            Block.text = CustomInput.KeyBoardBlock.ToString();
            Jump.text = CustomInput.KeyBoardJump.ToString();
            CycleLeft.text = CustomInput.KeyBoardCycleLeft.ToString();
            CycleRight.text = CustomInput.KeyBoardCycleRight.ToString();
            Left.text = CustomInput.KeyBoardLeft.ToString();
            Right.text = CustomInput.KeyBoardRight.ToString();
            ChangeColor.text = CustomInput.KeyBoardChangeColor.ToString();
            Up.text = CustomInput.KeyBoardUp.ToString();
            Down.text = CustomInput.KeyBoardDown.ToString();
            Super.text = CustomInput.KeyBoardSuper.ToString();
            Accept.text = CustomInput.KeyBoardAccept.ToString();
            Cancel.text = CustomInput.KeyBoardCancel.ToString();
            Pause.text = CustomInput.KeyBoardPause.ToString();
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
            Event e = Event.current;
            if (e.isKey && e.keyCode != KeyCode.None)
                setButton(e.keyCode);
        }

        private void setButton(KeyCode button)
        {
            if (machine.getPrieviousState() == ControlBinderStateMachine.State.Accept)
            {
                if (button == CustomInput.KeyBoardCancel)
                    duplicate = true;
                else
                    duplicate = false;
            }
            else if (machine.getPrieviousState() == ControlBinderStateMachine.State.Cancel)
            {
                if (button == CustomInput.KeyBoardAccept)
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
                            if (button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Block:
                        {
                            if (button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Jump:
                        {
                            if (button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.CycleLeft:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.CycleRight:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.ChangeColor:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Super:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Up:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Down:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Left:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    case ControlBinderStateMachine.State.Right:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardAttack ||
                                button == CustomInput.KeyBoardPause)
                                duplicate = true;
                            else
                                duplicate = false;
                        }; break;
                    default:
                        {
                            if (button == CustomInput.KeyBoardJump ||
                                button == CustomInput.KeyBoardBlock ||
                                button == CustomInput.KeyBoardCycleLeft ||
                                button == CustomInput.KeyBoardCycleRight ||
                                button == CustomInput.KeyBoardChangeColor ||
                                button == CustomInput.KeyBoardSuper ||
                                button == CustomInput.KeyBoardUp ||
                                button == CustomInput.KeyBoardDown ||
                                button == CustomInput.KeyBoardLeft ||
                                button == CustomInput.KeyBoardRight ||
                                button == CustomInput.KeyBoardAttack)
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
                    case ControlBinderStateMachine.State.Attack: CustomInput.KeyBoardAttack = button; Attack.text = CustomInput.KeyBoardAttack.ToString(); break;
                    case ControlBinderStateMachine.State.Block: CustomInput.KeyBoardBlock = button; Block.text = CustomInput.KeyBoardBlock.ToString(); break;
                    case ControlBinderStateMachine.State.Jump: CustomInput.KeyBoardJump = button; Jump.text = CustomInput.KeyBoardJump.ToString(); break;
                    case ControlBinderStateMachine.State.CycleLeft: CustomInput.KeyBoardCycleLeft = button; CycleLeft.text = CustomInput.KeyBoardCycleLeft.ToString(); break;
                    case ControlBinderStateMachine.State.CycleRight: CustomInput.KeyBoardCycleRight = button; CycleRight.text = CustomInput.KeyBoardCycleRight.ToString(); break;
                    case ControlBinderStateMachine.State.ChangeColor: CustomInput.KeyBoardChangeColor = button; ChangeColor.text = CustomInput.KeyBoardChangeColor.ToString(); break;
                    case ControlBinderStateMachine.State.Super: CustomInput.KeyBoardSuper = button; Super.text = CustomInput.KeyBoardSuper.ToString(); break;
                    case ControlBinderStateMachine.State.Up: CustomInput.KeyBoardUp = button; Up.text = CustomInput.KeyBoardUp.ToString(); break;
                    case ControlBinderStateMachine.State.Down: CustomInput.KeyBoardDown = button; Down.text = CustomInput.KeyBoardDown.ToString(); break;
                    case ControlBinderStateMachine.State.Left: CustomInput.KeyBoardLeft = button; Left.text = CustomInput.KeyBoardLeft.ToString(); break;
                    case ControlBinderStateMachine.State.Right: CustomInput.KeyBoardRight = button; Right.text = CustomInput.KeyBoardRight.ToString(); break;
                    case ControlBinderStateMachine.State.Accept: CustomInput.KeyBoardAccept = button; Accept.text = CustomInput.KeyBoardAccept.ToString(); break;
                    case ControlBinderStateMachine.State.Cancel: CustomInput.KeyBoardCancel = button; Cancel.text = CustomInput.KeyBoardCancel.ToString(); break;
                    default: CustomInput.KeyBoardPause = button; Pause.text = CustomInput.KeyBoardPause.ToString(); break;
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
