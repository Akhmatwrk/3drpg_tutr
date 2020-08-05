using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;

        bool isDead = false;

        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead()
        {
            return isDead;
        }


        public void TakeDamage(GameObject instigator, float damage)
        {
            print("Damaget done " + damage);
            health = Mathf.Max(health - damage, 0);

            if (health == 0)
            {
                AwardRxp(instigator);
                Death();
            }
        }

        public float GetPrcentage()
        {
            return health / GetComponent<BaseStats>().GetHealth() * 100;
        }

        private void AwardRxp(GameObject instigator)
        {
            Experience instigatorExp = instigator.GetComponent<Experience>();
            if (instigatorExp == null) return;

            instigatorExp.GainExp(GetComponent<BaseStats>().GetExpReward());
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

        public object CaptureState()
        {
            return health;
        }
        public void RestoreState(object state)
        {
            health = (float)state;

            if (health == 0)
            {
                Death();
            }
        }
    }
}

