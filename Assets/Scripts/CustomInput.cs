using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class CustomInput : MonoBehaviour
    {
        public const string LEFT_STICK_RIGHT = "Left Stick Right";
        public const string LEFT_STICK_LEFT = "Left Stick Left";
        public const string LEFT_STICK_UP = "Left Stick Up";
        public const string LEFT_STICK_DOWN = "Left Stick Down";
        public const string RIGHT_STICK_RIGHT = "Right Stick Right";
        public const string RIGHT_STICK_LEFT = "Right Stick Left";
        public const string RIGHT_STICK_UP = "Right Stick Up";
        public const string RIGHT_STICK_DOWN = "Right Stick Down";
        public const string DPAD_RIGHT = "Dpad Right";
        public const string DPAD_LEFT = "Dpad Left";
        public const string DPAD_UP = "Dpad Up";
        public const string DPAD_DOWN = "Dpad Down";
        public const string LEFT_TRIGGER = "Left Trigger";
        public const string RIGHT_TRIGGER = "Right Trigger";
        public const string A = "A";
        public const string B = "B";
        public const string X = "X";
        public const string Y = "Y";
        public const string LB = "LB";
        public const string RB = "RB";
        public const string BACK = "Back";
        public const string START = "Start";
        public const string LEFT_STICK = "Left Stick Click";
        public const string RIGHT_STICK = "Right Stick Click";

        private const int ATTACK = 0x1;
        private const int BLOCK = 0x2;
        private const int JUMP = 0x4;
        private const int CYCLELEFT = 0x8;
        private const int CYCLERIGHT = 0x10;
        private const int LEFT = 0x20;
        private const int RIGHT = 0x40;
        private const int CHANGECOLOR = 0x80;
        private const int UP = 0x100;
        private const int DOWN = 0x200;
        private const int SUPER = 0x400;

        private const int ACCEPT = 0x800;
        private const int CANCEL = 0x1000;
        private const int PAUSE = 0x2000;

        private static int bools = 0;
        private static int boolsUp = 0;
        private static int boolsHeld = 0;
        private static int boolsFreshPress = 0;
        private static int boolsFreshPressAccessed = 0;
        private static int boolsFreshPressDeleteOnRead = 0;



        //true as long as the button is held
        public static bool Attack
        {
            get { return (bools & ATTACK) != 0; }
        }
        public static bool Jump
        {
            get { return (bools & JUMP) != 0; }
        }
        public static bool CycleLeft
        {
            get { return (bools & CYCLELEFT) != 0; }
        }
        public static bool CycleRight
        {
            get { return (bools & CYCLERIGHT) != 0; }
        }
        public static bool Left
        {
            get { return (bools & LEFT) != 0; }
        }
        public static bool Right
        {
            get { return (bools & RIGHT) != 0; }
        }
        public static bool Block
        {
            get { return (bools & BLOCK) != 0; }
        }
        public static bool ChangeColor
        {
            get { return (bools & CHANGECOLOR) != 0; }
        }
        public static bool Up
        {
            get { return (bools & UP) != 0; }
        }
        public static bool Down
        {
            get { return (bools & DOWN) != 0; }
        }
        public static bool Super
        {
            get { return (bools & SUPER) != 0; }
        }
        public static bool Accept
        {
            get { return (bools & ACCEPT) != 0; }
        }
        public static bool Cancel
        {
            get { return (bools & CANCEL) != 0; }
        }
        public static bool Pause
        {
            get { return (bools & PAUSE) != 0; }
        }

        //true for one frame after button is let go.
        public static bool AttackUp
        {
            get { return (boolsUp & ATTACK) != 0; }
        }
        public static bool JumpUp
        {
            get { return (boolsUp & JUMP) != 0; }
        }
        public static bool CycleLeftUp
        {
            get { return (boolsUp & CYCLELEFT) != 0; }
        }
        public static bool CycleRightUp
        {
            get { return (boolsUp & CYCLERIGHT) != 0; }
        }
        public static bool LeftUp
        {
            get { return (boolsUp & LEFT) != 0; }
        }
        public static bool RightUp
        {
            get { return (boolsUp & RIGHT) != 0; }
        }
        public static bool BlockUp
        {
            get { return (boolsUp & BLOCK) != 0; }
        }
        public static bool ChangeColorUp
        {
            get { return (boolsUp & CHANGECOLOR) != 0; }
        }
        public static bool UpUp
        {
            get { return (boolsUp & UP) != 0; }
        }
        public static bool DownUp
        {
            get { return (boolsUp & DOWN) != 0; }
        }
        public static bool SuperUp
        {
            get { return (boolsUp & SUPER) != 0; }
        }
        public static bool AcceptUp
        {
            get { return (boolsUp & ACCEPT) != 0; }
        }
        public static bool CancelUp
        {
            get { return (boolsUp & CANCEL) != 0; }
        }
        public static bool PauseUp
        {
            get { return (boolsUp & PAUSE) != 0; }
        }

        //true until the button is let go.
        public static bool AttackHeld
        {
            get { return (boolsHeld & ATTACK) != 0; }
        }
        public static bool JumpHeld
        {
            get { return (boolsHeld & JUMP) != 0; }
        }
        public static bool CycleLeftHeld
        {
            get { return (boolsHeld & CYCLELEFT) != 0; }
        }
        public static bool CycleRightHeld
        {
            get { return (boolsHeld & CYCLERIGHT) != 0; }
        }
        public static bool LeftHeld
        {
            get { return (boolsHeld & LEFT) != 0; }
        }
        public static bool RightHeld
        {
            get { return (boolsHeld & RIGHT) != 0; }
        }
        public static bool BlockHeld
        {
            get { return (boolsHeld & BLOCK) != 0; }
        }
        public static bool ChangeColorHeld
        {
            get { return (boolsHeld & CHANGECOLOR) != 0; }
        }
        public static bool UpHeld
        {
            get { return (boolsHeld & UP) != 0; }
        }
        public static bool DownHeld
        {
            get { return (boolsHeld & DOWN) != 0; }
        }
        public static bool SuperHeld
        {
            get { return (boolsHeld & SUPER) != 0; }
        }
        public static bool AcceptHeld
        {
            get { return (boolsHeld & ACCEPT) != 0; }
        }
        public static bool CancelHeld
        {
            get { return (boolsHeld & CANCEL) != 0; }
        }
        public static bool PauseHeld
        {
            get { return (boolsHeld & PAUSE) != 0; }
        }

        //true as until the data is read or the key is released
        public static bool AttackFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | ATTACK;
                return (boolsFreshPress & ATTACK) != 0;
            }
        }
        public static bool JumpFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | JUMP;
                return (boolsFreshPress & JUMP) != 0;
            }
        }
        public static bool CycleLeftFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | CYCLELEFT;
                return (boolsFreshPress & CYCLELEFT) != 0;
            }
        }
        public static bool CycleRightFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | CYCLERIGHT;
                return (boolsFreshPress & CYCLERIGHT) != 0;
            }
        }
        public static bool LeftFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | LEFT;
                return (boolsFreshPress & LEFT) != 0;
            }
        }
        public static bool RightFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | RIGHT;
                return (boolsFreshPress & RIGHT) != 0;
            }
        }
        public static bool BlockFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | BLOCK;
                return (boolsFreshPress & BLOCK) != 0;
            }
        }
        public static bool ChangeColorFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | CHANGECOLOR;
                return (boolsFreshPress & CHANGECOLOR) != 0;
            }
        }
        public static bool UpFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | UP;
                return (boolsFreshPress & UP) != 0;
            }
        }
        public static bool DownFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | DOWN;
                return (boolsFreshPress & DOWN) != 0;
            }
        }
        public static bool SuperFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | SUPER;
                return (boolsFreshPress & SUPER) != 0;
            }
        }
        public static bool AcceptFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | ACCEPT;
                return (boolsFreshPress & ACCEPT) != 0;
            }
        }
        public static bool CancelFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | CANCEL;
                return (boolsFreshPress & CANCEL) != 0;
            }
        }
        public static bool PauseFreshPress
        {
            get
            {
                boolsFreshPressAccessed = boolsFreshPressAccessed | PAUSE;
                return (boolsFreshPress & PAUSE) != 0;
            }
        }

        //true as until the data is read or the key is released
        public static bool AttackFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & ATTACK) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~ATTACK;
                return temp;
            }
        }
        public static bool JumpFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & JUMP) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~JUMP;
                return temp;
            }
        }
        public static bool CycleLeftFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & CYCLELEFT) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~CYCLELEFT;
                return temp;
            }
        }
        public static bool CycleRightFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & CYCLERIGHT) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~CYCLERIGHT;
                return temp;
            }
        }
        public static bool LeftFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & LEFT) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~LEFT;
                return temp;
            }
        }
        public static bool RightFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & RIGHT) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~RIGHT;
                return temp;
            }
        }
        public static bool BlockFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & BLOCK) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~BLOCK;
                return temp;
            }
        }
        public static bool ChangeColorFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & CHANGECOLOR) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~CHANGECOLOR;
                return temp;
            }
        }
        public static bool UpFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & UP) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~UP;
                return temp;
            }
        }
        public static bool DownFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & DOWN) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~DOWN;
                return temp;
            }
        }
        public static bool SuperFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & SUPER) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~SUPER;
                return temp;
            }
        }
        public static bool AcceptFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & ACCEPT) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~ACCEPT;
                return temp;
            }
        }
        public static bool CancelFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & CANCEL) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~CANCEL;
                return temp;
            }
        }
        public static bool PauseFreshPressDeleteOnRead
        {
            get
            {
                bool temp = (boolsFreshPressDeleteOnRead & PAUSE) != 0;
                boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~PAUSE;
                return temp;
            }
        }

        private static KeyCode keyBoardAttack;
        private static KeyCode keyBoardJump;
        private static KeyCode keyBoardCycleLeft;
        private static KeyCode keyBoardCycleRight;
        private static KeyCode keyBoardLeft;
        private static KeyCode keyBoardRight;
        private static KeyCode keyBoardBlock;
        private static KeyCode keyBoardChangeColor;
        private static KeyCode keyBoardUp;
        private static KeyCode keyBoardDown;
        private static KeyCode keyBoardSuper;
        private static KeyCode keyBoardAccept;
        private static KeyCode keyBoardCancel;
        private static KeyCode keyBoardPause;

        private static string gamePadAttack;
        private static string gamePadJump;
        private static string gamePadCycleLeft;
        private static string gamePadCycleRight;
        private static string gamePadLeft;
        private static string gamePadRight;
        private static string gamePadBlock;
        private static string gamePadChangeColor;
        private static string gamePadUp;
        private static string gamePadDown;
        private static string gamePadSuper;
        private static string gamePadAccept;
        private static string gamePadCancel;
        private static string gamePadPause;

        private static string KeyHash = "AchromicKeys";

        private static bool usePad = false;

        public static bool UsePad
        {
            get { return usePad; }
        }

        public static KeyCode KeyBoardAttack
        {
            get { return keyBoardAttack; }
            set
            {
                keyBoardAttack = value;
                PlayerPrefs.SetInt(0 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardJump
        {
            get { return keyBoardJump; }
            set
            {
                keyBoardJump = value;
                PlayerPrefs.SetInt(1 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardCycleLeft
        {
            get { return keyBoardCycleLeft; }
            set
            {
                keyBoardCycleLeft = value;
                PlayerPrefs.SetInt(2 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardCycleRight
        {
            get { return keyBoardCycleRight; }
            set
            {
                keyBoardCycleRight = value;
                PlayerPrefs.SetInt(3 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardLeft
        {
            get { return keyBoardLeft; }
            set
            {
                keyBoardLeft = value;
                PlayerPrefs.SetInt(4 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardRight
        {
            get { return keyBoardRight; }
            set
            {
                keyBoardRight = value;
                PlayerPrefs.SetInt(5 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardBlock
        {
            get { return keyBoardBlock; }
            set
            {
                keyBoardBlock = value;
                PlayerPrefs.SetInt(6 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardChangeColor
        {
            get { return keyBoardChangeColor; }
            set
            {
                keyBoardChangeColor = value;
                PlayerPrefs.SetInt(7 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardUp
        {
            get { return keyBoardUp; }
            set
            {
                keyBoardUp = value;
                PlayerPrefs.SetInt(8 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardDown
        {
            get { return keyBoardDown; }
            set
            {
                keyBoardDown = value;
                PlayerPrefs.SetInt(9 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardSuper
        {
            get { return keyBoardSuper; }
            set
            {
                keyBoardSuper = value;
                PlayerPrefs.SetInt(10 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardAccept
        {
            get { return keyBoardAccept; }
            set
            {
                keyBoardAccept = value;
                PlayerPrefs.SetInt(11 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardCancel
        {
            get { return keyBoardCancel; }
            set
            {
                keyBoardCancel = value;
                PlayerPrefs.SetInt(12 + KeyHash, (int)value);
            }
        }
        public static KeyCode KeyBoardPause
        {
            get { return keyBoardPause; }
            set
            {
                keyBoardPause = value;
                PlayerPrefs.SetInt(13 + KeyHash, (int)value);
            }
        }
        public static string GamePadAttack
        {
            get { return gamePadAttack; }
            set
            {
                gamePadAttack = value;
                PlayerPrefs.SetString(14 + KeyHash, value);
            }
        }
        public static string GamePadJump
        {
            get { return gamePadJump; }
            set
            {
                gamePadJump = value;
                PlayerPrefs.SetString(15 + KeyHash, value);
            }
        }
        public static string GamePadCycleLeft
        {
            get { return gamePadCycleLeft; }
            set
            {
                gamePadCycleLeft = value;
                PlayerPrefs.SetString(16 + KeyHash, value);
            }
        }
        public static string GamePadCycleRight
        {
            get { return gamePadCycleRight; }
            set
            {
                gamePadCycleRight = value;
                PlayerPrefs.SetString(17 + KeyHash, value);
            }
        }
        public static string GamePadLeft
        {
            get { return gamePadLeft; }
            set
            {
                gamePadLeft = value;
                PlayerPrefs.SetString(18 + KeyHash, value);
            }
        }
        public static string GamePadRight
        {
            get { return gamePadRight; }
            set
            {
                gamePadRight = value;
                PlayerPrefs.SetString(19 + KeyHash, value);
            }
        }
        public static string GamePadBlock
        {
            get { return gamePadBlock; }
            set
            {
                gamePadBlock = value;
                PlayerPrefs.SetString(20 + KeyHash, value);
            }
        }
        public static string GamePadChangeColor
        {
            get { return gamePadChangeColor; }
            set
            {
                gamePadChangeColor = value;
                PlayerPrefs.SetString(21 + KeyHash, value);
            }
        }
        public static string GamePadUp
        {
            get { return gamePadUp; }
            set
            {
                gamePadUp = value;
                PlayerPrefs.SetString(22 + KeyHash, value);
            }
        }
        public static string GamePadDown
        {
            get { return gamePadDown; }
            set
            {
                gamePadDown = value;
                PlayerPrefs.SetString(23 + KeyHash, value);
            }
        }
        public static string GamePadSuper
        {
            get { return gamePadDown; }
            set
            {
                gamePadSuper = value;
                PlayerPrefs.SetString(24 + KeyHash, value);
            }
        }
        public static string GamePadAccept
        {
            get { return gamePadAccept; }
            set
            {
                gamePadAccept = value;
                PlayerPrefs.SetString(25 + KeyHash, value);
            }
        }
        public static string GamePadCancel
        {
            get { return gamePadCancel; }
            set
            {
                gamePadCancel = value;
                PlayerPrefs.SetString(26 + KeyHash, value);
            }
        }
        public static string GamePadPause
        {
            get { return gamePadPause; }
            set
            {
                gamePadPause = value;
                PlayerPrefs.SetString(27 + KeyHash, value);
            }
        }

        void Awake()
        {
            bools = 0;
            boolsUp = 0;
            boolsHeld = 0;
            boolsFreshPress = 0;
            boolsFreshPressAccessed = 0;
            boolsFreshPressDeleteOnRead = 0;
            if (PlayerPrefs.HasKey(KeyHash + 0))
            {
                keyBoardAttack = (KeyCode)PlayerPrefs.GetInt(0 + KeyHash);
                keyBoardJump = (KeyCode)PlayerPrefs.GetInt(1 + KeyHash);
                keyBoardCycleLeft = (KeyCode)PlayerPrefs.GetInt(2 + KeyHash);
                keyBoardCycleRight = (KeyCode)PlayerPrefs.GetInt(3 + KeyHash);
                keyBoardLeft = (KeyCode)PlayerPrefs.GetInt(4 + KeyHash);
                keyBoardRight = (KeyCode)PlayerPrefs.GetInt(5 + KeyHash);
                keyBoardBlock = (KeyCode)PlayerPrefs.GetInt(6 + KeyHash);
                keyBoardChangeColor = (KeyCode)PlayerPrefs.GetInt(7 + KeyHash);
                keyBoardUp = (KeyCode)PlayerPrefs.GetInt(8 + KeyHash);
                keyBoardDown = (KeyCode)PlayerPrefs.GetInt(9 + KeyHash);
                keyBoardSuper = (KeyCode)PlayerPrefs.GetInt(10 + KeyHash);
                keyBoardAccept = (KeyCode)PlayerPrefs.GetInt(11 + KeyHash);
                keyBoardCancel = (KeyCode)PlayerPrefs.GetInt(12 + KeyHash);
                keyBoardPause = (KeyCode)PlayerPrefs.GetInt(13 + KeyHash);

                gamePadAttack = PlayerPrefs.GetString(14 + KeyHash);
                gamePadJump = PlayerPrefs.GetString(15 + KeyHash);
                gamePadCycleLeft = PlayerPrefs.GetString(16 + KeyHash);
                gamePadCycleRight = PlayerPrefs.GetString(17 + KeyHash);
                gamePadLeft = PlayerPrefs.GetString(18 + KeyHash);
                gamePadRight = PlayerPrefs.GetString(19 + KeyHash);
                gamePadBlock = PlayerPrefs.GetString(20 + KeyHash);
                gamePadChangeColor = PlayerPrefs.GetString(21 + KeyHash);
                gamePadUp = PlayerPrefs.GetString(22 + KeyHash);
                gamePadDown = PlayerPrefs.GetString(23 + KeyHash);
                gamePadSuper = PlayerPrefs.GetString(24 + KeyHash);
                gamePadAccept = PlayerPrefs.GetString(23 + KeyHash);
                gamePadCancel = PlayerPrefs.GetString(26 + KeyHash);
                gamePadPause = PlayerPrefs.GetString(27 + KeyHash);
            }
            else
                Default();
        }
        void Update()
        {
            if (Input.anyKey)
                usePad = false;
            if (AnyPadInput())
                usePad = true;
            if (!usePad)
            {
                updateKey(ATTACK, keyBoardAttack);
                updateKey(JUMP, keyBoardJump);
                updateKey(CYCLELEFT, keyBoardCycleLeft);
                updateKey(CYCLERIGHT, keyBoardCycleRight);
                updateKey(LEFT, keyBoardLeft, KeyCode.LeftArrow);
                updateKey(RIGHT, keyBoardRight, KeyCode.RightArrow);
                updateKey(BLOCK, keyBoardBlock);
                updateKey(CHANGECOLOR, keyBoardChangeColor);
                updateKey(UP, keyBoardUp, KeyCode.UpArrow);
                updateKey(DOWN, keyBoardDown, KeyCode.DownArrow);
                updateKey(SUPER, keyBoardSuper);
                updateKey(ACCEPT, keyBoardAccept);
                updateKey(CANCEL, keyBoardCancel);
                updateKey(PAUSE, keyBoardPause);
            }
            else
            {
                updatePad(ATTACK, gamePadAttack);
                updatePad(JUMP, gamePadJump);
                updatePad(CYCLELEFT, gamePadCycleLeft);
                updatePad(CYCLERIGHT, gamePadCycleRight);
                updatePad(LEFT, gamePadLeft);
                updatePad(RIGHT, gamePadRight);
                updatePad(BLOCK, gamePadBlock);
                updatePad(CHANGECOLOR, gamePadChangeColor);
                updatePad(UP, gamePadUp);
                updatePad(DOWN, gamePadDown);
                updatePad(SUPER, gamePadSuper);
                updatePad(ACCEPT, gamePadAccept);
                updatePad(CANCEL, gamePadCancel);
                updatePad(PAUSE, gamePadPause);
            }
        }
        private static void updateKey(int state, params KeyCode[] keys)
        {
            bool key = false, keyUp = false;
            foreach (KeyCode k in keys)
            {
                if (Input.GetKeyDown(k))
                    key = true;
                else if (Input.GetKeyUp(k))
                    keyUp = true;
            }
            if ((boolsFreshPressAccessed & state) != 0)
            {
                boolsFreshPressAccessed = (boolsFreshPressAccessed & ~state);
                boolsFreshPress = (boolsFreshPress & ~state);
                boolsFreshPressDeleteOnRead = (boolsFreshPressDeleteOnRead & ~state);
            }
            if (((bools & state) == 0) && key)
            {
                boolsFreshPress = boolsFreshPress | state;
                boolsFreshPressDeleteOnRead = (boolsFreshPressDeleteOnRead | state);
            }
            if (key)
            {
                bools = bools | state;
                boolsHeld = boolsHeld | state;
                boolsUp = boolsUp & ~state;
            }
            else if (keyUp)
            {
                bools = bools & ~state;
                boolsHeld = boolsHeld & ~state;
                boolsFreshPress = boolsFreshPress & ~state;
                boolsFreshPressDeleteOnRead = (boolsFreshPressDeleteOnRead & ~state);
                boolsFreshPressAccessed = (boolsFreshPressAccessed & ~state);
                boolsUp = boolsUp | state;
            }
            else
                boolsUp = boolsUp & ~state;
        }
        private static void updatePad(int state, string axes)
        {
            float input = Input.GetAxis(axes);
            bool key = false, keyUp = false;
            if (axes == LEFT_STICK_LEFT || axes == LEFT_STICK_UP || axes == RIGHT_STICK_LEFT || axes == RIGHT_STICK_UP || axes == DPAD_LEFT || axes == DPAD_DOWN || axes == RIGHT_TRIGGER)
            {
                if (input < 0)
                    key = true;
                else if ((bools & state) != 0)
                    keyUp = true;
            }
            else if (input > 0)
                key = true;
            else if ((bools & state) != 0)
                keyUp = true;

            if ((boolsFreshPressAccessed & state) != 0)
            {
                boolsFreshPressAccessed = (boolsFreshPressAccessed & ~state);
                boolsFreshPress = (boolsFreshPress & ~state);
                boolsFreshPressDeleteOnRead = (boolsFreshPressDeleteOnRead & ~state);
            }
            if (((bools & state) == 0) && key)
            {
                boolsFreshPress = boolsFreshPress | state;
                boolsFreshPressDeleteOnRead = (boolsFreshPressDeleteOnRead | state);
            }
            if (key)
            {
                bools = bools | state;
                boolsHeld = boolsHeld | state;
                boolsUp = boolsUp & ~state;
            }
            else if (keyUp)
            {
                bools = bools & ~state;
                boolsHeld = boolsHeld & ~state;
                boolsFreshPress = boolsFreshPress & ~state;
                boolsFreshPressDeleteOnRead = (boolsFreshPressDeleteOnRead & ~state);
                boolsUp = boolsUp | state;
                boolsFreshPressAccessed = (boolsFreshPressAccessed & ~state);
            }
            else
                boolsUp = boolsUp & ~state;
        }

        public static void Default()
        {
            DefaultKey();
            DefaultPad();
        }

        public static void DefaultKey()
        {
            keyBoardAttack = KeyCode.K;
            PlayerPrefs.SetInt(0 + KeyHash, (int)KeyCode.K);
            keyBoardJump = KeyCode.Space;
            PlayerPrefs.SetInt(1 + KeyHash, (int)KeyCode.Space);
            keyBoardCycleLeft = KeyCode.Q;
            PlayerPrefs.SetInt(2 + KeyHash, (int)KeyCode.Q);
            keyBoardCycleRight = KeyCode.E;
            PlayerPrefs.SetInt(3 + KeyHash, (int)KeyCode.E);
            keyBoardLeft = KeyCode.A;
            PlayerPrefs.SetInt(4 + KeyHash, (int)KeyCode.A);
            keyBoardRight = KeyCode.D;
            PlayerPrefs.SetInt(5 + KeyHash, (int)KeyCode.D);
            keyBoardBlock = KeyCode.L;
            PlayerPrefs.SetInt(6 + KeyHash, (int)KeyCode.L);
            keyBoardChangeColor = KeyCode.Return;
            PlayerPrefs.SetInt(7 + KeyHash, (int)KeyCode.Return);
            keyBoardUp = KeyCode.W;
            PlayerPrefs.SetInt(8 + KeyHash, (int)KeyCode.W);
            keyBoardDown = KeyCode.S;
            PlayerPrefs.SetInt(9 + KeyHash, (int)KeyCode.S);
            keyBoardSuper = KeyCode.J;
            PlayerPrefs.SetInt(10 + KeyHash, (int)KeyCode.J);
            keyBoardAccept = KeyCode.Return;
            PlayerPrefs.SetInt(11 + KeyHash, (int)KeyCode.Return);
            keyBoardCancel = KeyCode.Backspace;
            PlayerPrefs.SetInt(12 + KeyHash, (int)KeyCode.Backspace);
            keyBoardPause = KeyCode.Backspace;
            PlayerPrefs.SetInt(13 + KeyHash, (int)KeyCode.Backspace);
        }

        public static void DefaultPad()
        {
            gamePadAttack = RIGHT_TRIGGER;
            PlayerPrefs.SetString(14 + KeyHash, RIGHT_TRIGGER);
            gamePadJump = A;
            PlayerPrefs.SetString(15 + KeyHash, A);
            gamePadCycleLeft = LB;
            PlayerPrefs.SetString(16 + KeyHash, LB);
            gamePadCycleRight = RB;
            PlayerPrefs.SetString(17 + KeyHash, RB);
            gamePadLeft = LEFT_STICK_LEFT;
            PlayerPrefs.SetString(18 + KeyHash, LEFT_STICK_LEFT);
            gamePadRight = LEFT_STICK_RIGHT;
            PlayerPrefs.SetString(19 + KeyHash, LEFT_STICK_RIGHT);
            gamePadBlock = LEFT_TRIGGER;
            PlayerPrefs.SetString(20 + KeyHash, LEFT_TRIGGER);
            gamePadChangeColor = Y;
            PlayerPrefs.SetString(21 + KeyHash, Y);
            gamePadUp = LEFT_STICK_UP;
            PlayerPrefs.SetString(22 + KeyHash, LEFT_STICK_UP);
            gamePadDown = LEFT_STICK_DOWN;
            PlayerPrefs.SetString(23 + KeyHash, LEFT_STICK_DOWN);
            gamePadSuper = LEFT_STICK;
            PlayerPrefs.SetString(24 + KeyHash, LEFT_STICK);
            gamePadAccept = A;
            PlayerPrefs.SetString(23 + KeyHash, A);
            gamePadCancel = B;
            PlayerPrefs.SetString(26 + KeyHash, B);
            gamePadPause = START;
            PlayerPrefs.SetString(27 + KeyHash, START);
        }

        public static bool AnyInput()
        {
            return bools != 0 || boolsFreshPress != 0 || boolsHeld != 0 || boolsUp != 0;
        }

        public static bool AnyPadInput()
        {
            if (Input.GetAxis(LEFT_STICK_RIGHT) != 0)
                return true;
            if (Input.GetAxis(LEFT_STICK_UP) != 0)
                return true;
            if (Input.GetAxis(RIGHT_STICK_RIGHT) != 0)
                return true;
            if (Input.GetAxis(RIGHT_STICK_UP) != 0)
                return true;
            if (Input.GetAxis(DPAD_RIGHT) != 0)
                return true;
            if (Input.GetAxis(DPAD_UP) != 0)
                return true;
            if (Input.GetAxis(LEFT_TRIGGER) != 0)
                return true;
            if (Input.GetAxis(RIGHT_TRIGGER) != 0)
                return true;
            if (Input.GetAxis(A) != 0)
                return true;
            if (Input.GetAxis(B) != 0)
                return true;
            if (Input.GetAxis(X) != 0)
                return true;
            if (Input.GetAxis(Y) != 0)
                return true;
            if (Input.GetAxis(LB) != 0)
                return true;
            if (Input.GetAxis(RB) != 0)
                return true;
            if (Input.GetAxis(BACK) != 0)
                return true;
            if (Input.GetAxis(START) != 0)
                return true;
            if (Input.GetAxis(LEFT_STICK) != 0)
                return true;
            if (Input.GetAxis(RIGHT_STICK) != 0)
                return true;
            return false;
        }

        public static void UpdatePause()
        {
            bools = bools | PAUSE;
            boolsHeld = boolsHeld & ~PAUSE;
            boolsUp = boolsUp & ~PAUSE;
            boolsFreshPress = boolsFreshPress & ~PAUSE;
            boolsFreshPressAccessed = boolsFreshPressAccessed & ~PAUSE;
            boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~PAUSE;
        }

		public static void UpdateChangeColor()
		{
			bools = bools | CHANGECOLOR;
			boolsHeld = boolsHeld & ~CHANGECOLOR;
			boolsUp = boolsUp & ~CHANGECOLOR;
			boolsFreshPress = boolsFreshPress & ~CHANGECOLOR;
			boolsFreshPressAccessed = boolsFreshPressAccessed & ~CHANGECOLOR;
			boolsFreshPressDeleteOnRead = boolsFreshPressDeleteOnRead & ~CHANGECOLOR;
		}

        public static string GetText(string input)
        {
            string[] arr = input.Split(' ');
            if (arr[arr.Length - 1] != "\\Input/")
                return input;
            switch (arr[0])
            {
                case "Attack": return usePad ? GamePadAttack : KeyBoardAttack.ToString();
                case "Jump": return usePad ? GamePadJump : KeyBoardJump.ToString();
                case "CycleLeft": return usePad ? GamePadCycleLeft : KeyBoardCycleLeft.ToString();
                case "CycleRight": return usePad ? GamePadCycleRight : KeyBoardCycleRight.ToString();
                case "Left": return usePad ? GamePadLeft : KeyBoardLeft.ToString() + "/Left Arrow";
                case "Right": return usePad ? GamePadRight : KeyBoardRight.ToString() + "/Right Arrow";
                case "Block": return usePad ? GamePadBlock : KeyBoardBlock.ToString();
                case "ChangeColor": return usePad ? GamePadChangeColor : KeyBoardChangeColor.ToString();
                case "Up": return usePad ? GamePadUp : KeyBoardUp.ToString() + "/Up Arrow";
                case "Down": return usePad ? GamePadDown : KeyBoardDown.ToString() + "/Down Arrow";
                case "Super": return usePad ? GamePadSuper : KeyBoardSuper.ToString();
                case "Accept": return usePad ? GamePadAccept : KeyBoardAccept.ToString();
                case "Cancel": return usePad ? GamePadCancel : KeyBoardCancel.ToString();
                case "Pause": return usePad ? GamePadPause : KeyBoardPause.ToString();
                default: return "invalid input";
            }
        }
    }
}