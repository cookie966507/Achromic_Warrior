using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Player
{
    /* This file controls all of the transitions between states*/
    class PlayerStateMachine
    {
        public enum State { idle = 0, move, jump, inAir, attack1, attack2, attack3, movingAttack, inAirAttack, parry, block, crouch, hit, dead };

        private delegate State machine(bool inAir, bool blockSuccess, bool hit, bool animDone);//function pointer
        private machine[] getNextState;//array of function pointers
        private State currState;
        private static float hold = 0;//used for delays
        private static bool die = false;

        public PlayerStateMachine()
        {
            currState = State.idle;
            //fill array with functions
            getNextState = new machine[] { Idle, Move, Jump, InAirNow, Attack1, Attack2, Attack3, MovingAttack, InAirAttack, Parry, Block, Crouch, Hit, Dead };
        }

        public State update(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            return currState = getNextState[((int)currState)](inAir, blockSuccess, hit, animDone);//gets te next state
        }


        //The following methods control when and how you can transition between states

        private static State Idle(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit; ;
            if (inAir)
                return State.inAir;
            if (CustomInput.AttackFreshPress)
                return State.attack1;
            if (CustomInput.JumpFreshPress)
                return State.jump;
            if (CustomInput.Left || CustomInput.Right)
                return State.move;
            if (CustomInput.DownFreshPress)
                return State.crouch;
            return State.idle;
        }

        //the attack methods are very similar, but their counterparts call different animations
        private static State Attack1(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (CustomInput.Block)
                return State.block;
            if(animDone)
            {
                if (CustomInput.AcceptFreshPress)
                    return State.attack2;
                return State.idle;
            }
            return State.attack1;
        }
        private static State Attack2(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (CustomInput.Block)
                return State.block;
            if (animDone)
            {
                if (CustomInput.AcceptFreshPress)
                    return State.attack3;
                return State.idle;
            }
            return State.attack2;
        }
        private static State Attack3( bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (CustomInput.Block)
                return State.block;
            if (animDone)
                return State.idle;
            return State.attack3;
        }

        private static State MovingAttack(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (CustomInput.BlockFreshPress)
                return State.block;
            if (!(CustomInput.Left || CustomInput.Right))
                return State.idle;
            if (animDone)
            {
                if (CustomInput.Left || CustomInput.Right)
                    return State.move;
                if (CustomInput.JumpFreshPress)
                    return State.jump;
                if (CustomInput.DownFreshPress)
                    return State.crouch;
                return State.idle;
            }
            return State.movingAttack;
        }

        private static State InAirAttack(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (!inAir)
                return State.idle;
            if (animDone)
                return State.inAir;
            return State.inAirAttack;
        }
        private static State Move(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (CustomInput.Left || CustomInput.Right)
            {
                if (inAir)
                    return State.inAir;
                if (CustomInput.AttackFreshPress)
                    return State.movingAttack;
                if (CustomInput.DownFreshPress)
                    return State.crouch;
                if (CustomInput.JumpFreshPress)
                    return State.jump;
                return State.move;
            }
            return State.idle;
        }
        private static State Parry(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (CustomInput.Block)
                return State.block;
            if (animDone)
            {
                if (CustomInput.AcceptFreshPress)
                    return State.attack2;
                return State.idle;
            }
            return State.parry;
        }
        private static State Block(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (inAir)
                return State.inAir;
            if (blockSuccess && CustomInput.AttackFreshPress)
                return State.parry;
            if (CustomInput.BlockUp)
            {
                if (CustomInput.AttackFreshPress)
                    return State.attack1;
                if (CustomInput.DownFreshPress)
                    return State.crouch;
                if (CustomInput.JumpFreshPress)
                    return State.jump;
                if (CustomInput.Left || CustomInput.Right)
                    return State.move;
                return State.idle;
            }
            return State.block;
        }
        private static State Crouch(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (inAir)
                return State.inAir;
            if (CustomInput.DownUp)
            {
                if (CustomInput.AttackFreshPress)
                    return State.attack1;
                if (CustomInput.BlockFreshPress)
                    return State.block;
                if (CustomInput.JumpFreshPress)
                    return State.jump;
                if (CustomInput.Left || CustomInput.Right)
                    return State.move;
                return State.idle;
            }
            return State.crouch;
        }
        private static State Jump(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (CustomInput.AttackFreshPress)
                return State.inAirAttack;
            if (animDone || !CustomInput.Jump)
                return State.inAir;
            return State.jump;
        }
        private static State InAirNow(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            if (hit)
                return State.hit;
            if (CustomInput.AttackFreshPress)
                return State.inAirAttack;
            if (!inAir)
            {
                if (CustomInput.JumpFreshPress)
                    return State.jump;
                if (CustomInput.Left || CustomInput.Right)
                    return State.move;
                if (CustomInput.AttackFreshPress)
                    return State.attack1;
                return State.idle;
            }
            return State.inAir;
        }

        private static State Hit(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            hold += UnityEngine.Time.deltaTime;
            if (hold > .4f)
            {
                hold = 0;
                if (die)
                    return State.dead;
                return State.idle;
            }
            return State.hit;
        }

        //this is used to prevent the player character from doing any thing while dead
        private static State Dead(bool inAir, bool blockSuccess, bool hit, bool animDone)
        {
            return State.dead;
        }

        internal void Die()
        {
            die = true;
        }

        internal void Revive()
        {
            currState = State.idle;
            die = false;
        }
    }
}
