using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Player
{
    /* This file controls all of the transitions between states*/
    class PlayerStateMachine
    {

        private delegate Enums.PlayerState machine(bool inAir, bool blockSuccess, bool hit, bool animDone);//function pointer
        private machine[] getNextState;//array of function pointers
        private Enums.PlayerState currState;
        private static float hold = 0;//used for delays
        private static bool die = false;
        private static bool doubleJumped = false;
        private static int attack = 0;
        private static float reset = 0;

        public PlayerStateMachine()
        {
            currState = Enums.PlayerState.idle;
            //fill array with functions
            getNextState = new machine[] { Idle, Move, Jump, Jump2, InAirNow, Attack1, Attack2, Attack3, MovingAttack, InAirAttack, Parry, Block, Crouch, Hit, Dead };
        }

        public Enums.PlayerState update(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (attack > 0)
            {
                reset += UnityEngine.Time.deltaTime;
                if (reset > 1f)
                {
                    attack = 0;
                    reset = 0;
                }
            }
            int prevAt = attack;
            currState = getNextState[((int)currState)](inAir, blockSuccess, hit, animDone);//gets te next Enums.PlayerState
            if (prevAt != attack)
                reset = 0;
            return currState;
        }


        //The following methods control when and how you can transition between states

        private static Enums.PlayerState Idle(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (inAir)
                return Enums.PlayerState.inAir;
            if (CustomInput.BlockFreshPress)
                return Enums.PlayerState.block;
            if (CustomInput.AttackFreshPress)
            {
                if (attack == 0)
                {
                    return Enums.PlayerState.attack1;
                }
                if (attack == 1)
                {
                    return Enums.PlayerState.attack2;
                }
                return Enums.PlayerState.attack3;
            }
            if (CustomInput.JumpFreshPress)
                return Enums.PlayerState.jump;
            if (CustomInput.Left || CustomInput.Right)
                return Enums.PlayerState.move;
            if (CustomInput.Down)
                return Enums.PlayerState.crouch;
            return Enums.PlayerState.idle;
        }

        //the attack methods are very similar, but their counterparts call different animations
        private static Enums.PlayerState Attack1(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (CustomInput.Block)
                return Enums.PlayerState.block;
            if (animDone)
            {
                attack = 1;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.attack1;
        }
        private static Enums.PlayerState Attack2(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
            {
                attack = 0;
                return Enums.PlayerState.hit;
            }
            if (CustomInput.Block)
            {
                attack = 0;
                return Enums.PlayerState.block;
            }
            if (animDone)
            {
                attack = 2;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.attack2;
        }
        private static Enums.PlayerState Attack3(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
            {
                attack = 0;
                return Enums.PlayerState.hit;
            }
            if (CustomInput.Block)
            {
                attack = 0;
                return Enums.PlayerState.block;
            }
            if (animDone)
            {
                attack = 0;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.attack3;
        }

        private static Enums.PlayerState MovingAttack(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (CustomInput.BlockFreshPress)
                return Enums.PlayerState.block;
            if (!(CustomInput.Left || CustomInput.Right))
                return Enums.PlayerState.idle;
            if (animDone)
            {
                attack = 1;
                if (CustomInput.Left || CustomInput.Right)
                    return Enums.PlayerState.move;
                if (CustomInput.JumpFreshPress)
                    return Enums.PlayerState.jump;
                if (CustomInput.DownFreshPress)
                    return Enums.PlayerState.crouch;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.movingAttack;
        }

        private static Enums.PlayerState InAirAttack(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (!inAir)
                return Enums.PlayerState.idle;
            if (animDone)
                return Enums.PlayerState.inAir;
            return Enums.PlayerState.inAirAttack;
        }
        private static Enums.PlayerState Move(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (CustomInput.Left || CustomInput.Right)
            {
                if (inAir)
                    return Enums.PlayerState.inAir;
                if (CustomInput.AttackFreshPress)
                {
                    if (attack == 1)
                        return Enums.PlayerState.attack2;
                    return Enums.PlayerState.movingAttack;
                }
                if (CustomInput.DownFreshPress)
                    return Enums.PlayerState.crouch;
                if (CustomInput.JumpFreshPress)
                    return Enums.PlayerState.jump;
                return Enums.PlayerState.move;
            }
            return Enums.PlayerState.idle;
        }
        private static Enums.PlayerState Parry(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (CustomInput.Block)
                return Enums.PlayerState.block;
            if (animDone)
            {
                attack = 1;
                if (CustomInput.AcceptFreshPress)
                    return Enums.PlayerState.attack2;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.parry;
        }
        private static Enums.PlayerState Block(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (inAir)
                return Enums.PlayerState.inAir;
            if (blockSuccess && CustomInput.AttackFreshPress)
                return Enums.PlayerState.parry;
            if (CustomInput.BlockUp)
            {
                if (CustomInput.AttackFreshPress)
                    return Enums.PlayerState.attack1;
                if (CustomInput.DownFreshPress)
                    return Enums.PlayerState.crouch;
                if (CustomInput.JumpFreshPress)
                    return Enums.PlayerState.jump;
                if (CustomInput.Left || CustomInput.Right)
                    return Enums.PlayerState.move;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.block;
        }
        private static Enums.PlayerState Crouch(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (inAir)
                return Enums.PlayerState.inAir;
            if (CustomInput.DownUp)
            {
                if (CustomInput.AttackFreshPress)
                    return Enums.PlayerState.attack1;
                if (CustomInput.BlockFreshPress)
                    return Enums.PlayerState.block;
                if (CustomInput.JumpFreshPress)
                    return Enums.PlayerState.jump;
                if (CustomInput.Left || CustomInput.Right)
                    return Enums.PlayerState.move;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.crouch;
        }
        private static Enums.PlayerState Jump(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (CustomInput.AttackFreshPress)
                return Enums.PlayerState.inAirAttack;
            if (animDone || !CustomInput.Jump)
                return Enums.PlayerState.inAir;
            return Enums.PlayerState.jump;
        }
        private static Enums.PlayerState Jump2(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (CustomInput.AttackFreshPress)
                return Enums.PlayerState.inAirAttack;
            if (animDone || !CustomInput.Jump)
                return Enums.PlayerState.inAir;
            return Enums.PlayerState.jump2;
        }
        private static Enums.PlayerState InAirNow(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return Enums.PlayerState.hit;
            if (CustomInput.AttackFreshPress)
                return Enums.PlayerState.inAirAttack;
            if (!doubleJumped && CustomInput.JumpFreshPress && !CustomInput.Down)
            {
                doubleJumped = true;
                return Enums.PlayerState.jump2;
            }
            if (!inAir)
            {
                doubleJumped = false;
                if (CustomInput.JumpFreshPress)
                    return Enums.PlayerState.jump;
                if (CustomInput.Left || CustomInput.Right)
                    return Enums.PlayerState.move;
                if (CustomInput.AttackFreshPress)
                    return Enums.PlayerState.attack1;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.inAir;
        }

        private static Enums.PlayerState Hit(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            hold += UnityEngine.Time.deltaTime;
            if (hold > .4f)
            {
                hold = 0;
                if (die)
                    return Enums.PlayerState.dead;
                return Enums.PlayerState.idle;
            }
            return Enums.PlayerState.hit;
        }

        //this is used to prevent the player character from doing any thing while dead
        private static Enums.PlayerState Dead(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            return Enums.PlayerState.dead;
        }

        internal void Die()
        {
            die = true;
        }

        internal void Revive()
        {
            currState = Enums.PlayerState.idle;
            die = false;
        }
    }
}
