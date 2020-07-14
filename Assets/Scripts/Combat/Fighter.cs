using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttack = 2f;
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        
        Weapon currentWeapon = null;
        Health target = null;
        float timeSinceLastAttack = Mathf.Infinity;


        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            MovingToTarget();
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            currentWeapon.Spawn(handTransform, GetComponent<Animator>());
        }

        private void MovingToTarget()
        {
            if (target == null) return;

            if (!CanAttack(target.gameObject)) return;
            
            if (GetIsInRange())
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
            else
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }

            
        }

        public bool CanAttack(GameObject target)
        {
            return (target != null && !target.GetComponent<Health>().IsDead());
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            // Damage done in animation event Hit()
            if (timeSinceLastAttack > timeBetweenAttack)
            {
                GetComponent<Animator>().SetTrigger("Attack");
                timeSinceLastAttack = 0f;
            }
        }

        // Animation Event
        private void Hit()
        {
            if (target == null) return;
            DoDamage();
        }

        private void DoDamage()
        {
            target.TakeDamage(currentWeapon.GetDamage());
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) <= currentWeapon.GetRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            print("Target is " + combatTarget);
            target = combatTarget.GetComponent<Health>();

        }

        public void Cancel()
        {
            target = null;
            GetComponent<Mover>().Cancel();
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("Somewhere");
        }


    }
}



