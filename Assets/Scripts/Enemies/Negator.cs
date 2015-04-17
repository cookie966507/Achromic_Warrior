using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Enemies
{
    class Negator : Enemy
    {
        public Util.ObjectHider puncher;
        public GameObject bullet;
        public Transform barrel;

        public Transform[] patrolPoints;
        public Util.JumpData[] jumpPoints;
        public float changeThreshold = 0.03f;
        public float moveSpeed;

        private Transform target;
        private int currentPoint;
        private float damping = 3.0f;
        private float jumpForceX = 30f;
        private float jumpForceY = 100f;
        private bool jumpMode = false;
        private int patLen;
        private int jumpLen;
        private float changeInY = 0.0f;
        private float lastYPos = 0.0f;
        private float noWait = 0;

        private NegateStateMachine machine;
        private NegateStateMachine.state currState = NegateStateMachine.state.wait;
        private bool close = false;
        private bool doOnce = false;
        private bool animDone = true;
        private Player.PlayerColorData color;


        protected override void StartUp()
        {
            _player = FindObjectOfType<Player.PlayerController>().gameObject;
            color = _player.GetComponent<Player.PlayerColorData>();
            machine = new NegateStateMachine();
            currentPoint = 0;
            target = patrolPoints[currentPoint];
            patLen = patrolPoints.Length;
            jumpLen = jumpPoints.Length;
        }
        public void punchTime()
        {
            puncher.Show();
        }
        public void hidePunch()
        {
            puncher.Hide();
        }
        public void shootTime()
        {
            GameObject b = ((GameObject)Instantiate(bullet));
            b.transform.position = barrel.position;
            b.GetComponent<Util.Bullet>().Direction = new Vector2(Mathf.Sign(this.transform.localScale.x), 0); 
        }
        public void spawnTime()
        {
            FindObjectOfType<EnemySpawner>().SpawnEnemy();
        }
        public void setAnimDone()
        {
            animDone = true;
        }
        protected override void Run()
        {
            NegateStateMachine.state prevState = currState;
            if (color.Color == Enums.ColorElement.White)
                Color = Enums.ColorElement.Black;
            else if (color.Color == Enums.ColorElement.Black)
                Color = Enums.ColorElement.White;
            else
                Color = (Enums.ColorElement)(((int)color.Color+6)%12);
            bool inRange = Mathf.Abs(_player.transform.position.y - transform.position.y) < .5 && Mathf.Abs(this.GetComponent<Rigidbody2D>().velocity.y) < .001 && 
                Mathf.Abs(_player.transform.position.x - transform.position.x) > 10;
            currState = machine.update(close, inRange, animDone);
            switch (currState)
            {
                case NegateStateMachine.state.wait: wait(); break;
                case NegateStateMachine.state.run: run(); break;
                case NegateStateMachine.state.follow: follow(); break;
                case NegateStateMachine.state.punch: punch(); break;
                case NegateStateMachine.state.shoot: shoot(); break;
                case NegateStateMachine.state.summon: summon(); break;
            }
            if (prevState != currState)
            {
                doOnce = false;
                close = false;
                puncher.Hide();
            }
        }
        private void wait()
        {

        }
        private void run()
        {
            close = PatrolAround();
        }
        private void follow()
        {
            close = FollowHim();
        }
        private void punch()
        {
            if(!doOnce)
            {
                puncher.Show();
                doOnce = false;
            }
        }
        private void shoot()
        {
            if(!doOnce)
            {
                if (_player.transform.position.x < transform.position.x)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
                GameObject b = ((GameObject)Instantiate(bullet));
                b.transform.position = barrel.position;
                b.GetComponent<Util.Bullet>().Direction = new Vector2(Mathf.Sign(this.transform.localScale.x), 0); 
                doOnce = false;
            }
        }
        private void summon()
        {
            if(!doOnce)
            {
                FindObjectOfType<EnemySpawner>().SpawnEnemy();
                doOnce = false;
            }
        }

        protected bool PatrolAround()
        {
            changeInY = transform.position.y - lastYPos;
            lastYPos = transform.position.y;
            // Check if you need to jump to get to your position
            if ((target.position.y - transform.position.y) >= 2 && Mathf.Abs(changeInY) < changeThreshold) // && target.tag == "pat") 
            {
                jumpMode = true;
                Transform current = jumpPoints[0].gameObject.transform;
                jumpForceX = jumpPoints[0].jumpForceX;
                jumpForceY = jumpPoints[0].jumpForceY;
                for (int i = 0; i < jumpLen; i++)
                {
                    // Find the shortest point from the next patrolto jump to for jump points

                    if (Mathf.Abs(jumpPoints[i].gameObject.transform.position.y - transform.position.y) < 2)
                    {
                        // If no jump point is found, set the current one
                        if (!current)
                        {
                            current = jumpPoints[i].gameObject.transform;
                            jumpForceX = jumpPoints[i].jumpForceX;
                            jumpForceY = jumpPoints[i].jumpForceY;
                        }

                        // If a jump point is found, calculate the one with the closest x coord
                        else if (Mathf.Abs(jumpPoints[i].gameObject.transform.position.x - target.position.x) <
                                Mathf.Abs(current.position.x - target.position.x))
                        {
                            current = jumpPoints[i].gameObject.transform;
                            jumpForceX = jumpPoints[i].jumpForceX;
                            jumpForceY = jumpPoints[i].jumpForceY;
                        }
                    }
                }
                target = current;
            }

            if ((target.position.y - transform.position.y) < 2)
                EnterGhost();

            if (Vector3.Distance(transform.position, target.position) < .5) 
            {
                this.ExitGhost();
                if (jumpMode)
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(jumpForceX, jumpForceY));
                    EnterGhost();
                    jumpMode = false;
                    target = patrolPoints[currentPoint];
                }
                else
                {
                    if (noWait > 0.25)
                    {
                        currentPoint = Random.Range(0, patLen);
                        target = patrolPoints[currentPoint];
                        noWait = 0;
                    }
                    noWait += Time.deltaTime;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            if (transform.rotation.z <= 1 || transform.rotation.z >= -1)
            {
                Quaternion adjust = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, adjust, Time.deltaTime * damping);	// Is shifting from rotation to player view gradually
            }

            if (target.transform.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            // Return true if the player is close, false if he is not within range
            if (Vector2.Distance(_player.transform.position, transform.position) < 2.0)
                return true;
            return false;
        }

        protected bool FollowHim()
        {
            changeInY = transform.position.y - lastYPos;
            lastYPos = transform.position.y;
            // Check if you need to jump to get to your position
            if ((target.position.y - transform.position.y) >= 2 && Mathf.Abs(changeInY) < changeThreshold) // && target.tag == "pat") 
            {
                jumpMode = true;
                Transform current = jumpPoints[0].gameObject.transform;
                jumpForceX = jumpPoints[0].jumpForceX;
                jumpForceY = jumpPoints[0].jumpForceY;
                for (int i = 0; i < jumpLen; i++)
                {
                    // Find the shortest point from the next patrolto jump to for jump points

                    if (Mathf.Abs(jumpPoints[i].gameObject.transform.position.y - transform.position.y) < 2)
                    {
                        // If no jump point is found, set the current one
                        if (!current)
                        {
                            current = jumpPoints[i].gameObject.transform;
                            jumpForceX = jumpPoints[i].jumpForceX;
                            jumpForceY = jumpPoints[i].jumpForceY;
                        }

                        // If a jump point is found, calculate the one with the closest x coord
                        else if (Mathf.Abs(jumpPoints[i].gameObject.transform.position.x - target.position.x) <
                                Mathf.Abs(current.position.x - target.position.x))
                        {
                            current = jumpPoints[i].gameObject.transform;
                            jumpForceX = jumpPoints[i].jumpForceX;
                            jumpForceY = jumpPoints[i].jumpForceY;
                        }
                    }
                }
                target = current;
            }

            if ((target.position.y - transform.position.y) < 2)
                EnterGhost();

            if (Vector3.Distance(transform.position, target.position) < .5)
            {
                this.ExitGhost();
                if (jumpMode)
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(jumpForceX, jumpForceY));
                    EnterGhost();
                    jumpMode = false;
                    target = _player.transform;
                }
                else
                {
                    if (noWait > 0.25)
                    {
                        currentPoint = Random.Range(0, patLen);
                        target = _player.transform;
                        noWait = 0;
                    }
                    noWait += Time.deltaTime;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            if (transform.rotation.z <= 1 || transform.rotation.z >= -1)
            {
                Quaternion adjust = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, adjust, Time.deltaTime * damping);	// Is shifting from rotation to player view gradually
            }

            if (target.transform.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            // Return true if the player is close, false if he is not within range
            if (Vector2.Distance(_player.transform.position, transform.position) < 2.0)
                return true;
            return false;
        }
    }
    class NegateStateMachine
    {
        internal enum state { wait, run, follow, punch, shoot, summon };
        private state currState;
        private float hold;

        public NegateStateMachine()
        {
            currState = state.wait;
            hold = 0;
        }

        public state update(bool close, bool inRange, bool animDone)
        {
            switch (currState)
            {
                case NegateStateMachine.state.wait: currState = wait(); break;
                case NegateStateMachine.state.run: currState = run(close,inRange); break;
                case NegateStateMachine.state.follow: currState = follow(close); break;
                case NegateStateMachine.state.punch: currState = punch(animDone); break;
                case NegateStateMachine.state.shoot: currState = shoot(animDone); break;
                case NegateStateMachine.state.summon: currState = summon(animDone); break;
            }
            return currState;
        }
        private state wait()
        {
            hold += UnityEngine.Time.deltaTime;
            if (hold > 1f)
            {
                hold = 0;
                return state.run;
            }
            return state.wait;
        }
        private state run(bool close, bool inRange)
        {
            if (close)
            {
                hold = 0;
                return state.punch;
            }
            hold += UnityEngine.Time.deltaTime;
            if (hold > 5f)
            {
                hold = 0;
                return state.follow;
            }
            if (inRange)
            {
                hold = 0;
                return state.shoot;
            }
            return state.run;
        }
        private state follow(bool close)
        {
            if (close)
            {
                hold = 0;
                return state.punch;
            }
            hold += UnityEngine.Time.deltaTime;
            if (hold > 5f)
            {
                hold = 0;
                return state.summon;
            }
            return state.follow;
        }
        private state punch(bool animDone)
        {
            if (animDone)
                return state.wait;
            return state.punch;
        }
        private state shoot(bool animDone)
        {
            if (animDone)
                return state.wait;
            return state.shoot;
        }
        private state summon(bool animDone)
        {
            if (animDone)
                return state.wait;
            return state.summon;
        }
    }
}
