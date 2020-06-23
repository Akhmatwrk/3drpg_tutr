using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        bool isDead = false;
        
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            print("Damaget done " + damage);
            health = Mathf.Max(health - damage, 0);

            if (health == 0)
            {
                Death();
            }
        }

        private void Death()
        {
            if (!isDead)
            {
                GetComponent<Animator>().SetTrigger("Death");
                GetComponent<ActionScheduler>().CancelCurrentAction();
                isDead = true;
            }
        }
    }
}

