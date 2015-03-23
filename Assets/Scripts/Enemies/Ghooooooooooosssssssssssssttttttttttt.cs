using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Enemies
{
    class Ghooooooooooosssssssssssssttttttttttt : Enemy
    {
        protected override void StartUp()
        {

        }
        protected override void Run()
        {
            if (CustomInput.JumpFreshPress)
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 10), ForceMode2D.Impulse);
            if (CustomInput.AttackFreshPress)
                EnterGhost();
        }
    }
}
