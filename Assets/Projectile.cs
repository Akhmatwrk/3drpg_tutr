using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat 
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 5f;
        [SerializeField] bool isHoming = true;
        [SerializeField] float lifeTime = 10f;

        float damage = 0f;
        Health target = null;

        void Update()
        {
            if (isHoming)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position; // + Vector3.up * targetCapsule.height / 2; for very low target position
        }

        public void SetTarget(Health targetPosition, float weaponDamage)
        {
            target = targetPosition;
            damage = weaponDamage;
            transform.LookAt(GetAimLocation());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;

            if (target.IsDead())
            {
                isHoming = false;
                return;
            }

            target.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}


