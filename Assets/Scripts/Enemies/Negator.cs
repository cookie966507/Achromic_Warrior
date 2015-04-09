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
        public Transform[] jumpPoints;
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
            currState = machine.update(close, Mathf.Abs(_player.transform.position.y-transform.position.y)<.5, animDone);
            Debug.Log(currState);
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
            close = PatrolAround();
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
                Transform current = jumpPoints[0];
                for (int i = 0; i < jumpLen; i++)
                {
                    // Find the shortest point from the next patrolto jump to for jump points

                    if (Mathf.Abs(jumpPoints[i].position.y - transform.position.y) < 2)
                    {
                        // If no jump point is found, set the current one
                        if (!current)
                        {
                            current = jumpPoints[i];
                        }

                        // If a jump point is found, calculate the one with the closest x coord
                        else if (Mathf.Abs(jumpPoints[i].position.x - target.position.x) <
                                Mathf.Abs(current.position.x - target.position.x))
                        {
                            current = jumpPoints[i];
                        }
                    }
                }
                target = current;
            }

            if (transform.position == target.position)
            {
                this.ExitGhost();
                if (jumpMode)
                {
                    // Need to calculate jump vector
                    Vector2 vector = patrolPoints[currentPoint].position - target.position;
                    vector.y *= jumpForceY;
                    vector.x = 0;

                    vector.x *= jumpForceX;
                    GetComponent<Rigidbody2D>().AddForce(vector);
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
            if (Vector2.Distance(_player.transform.position, transform.position) < 3.0)
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
            if (inRange)
            {
                hold = 0;
                return state.shoot;
            }
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
