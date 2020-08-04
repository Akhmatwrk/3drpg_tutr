using RPG.Core;
using System;
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
        [SerializeField] GameObject impactVFX;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float destoryTimeAfterHit = 0.5f;

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

            Destroy(gameObject, lifeTime);
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
            projectileSpeed = 0;

            if (impactVFX != null)
            {
                ImpactEffect(GetAimLocation());
            }            

            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, destoryTimeAfterHit);
        }

        private void ImpactEffect(Vector3 position)
        {
            Instantiate(impactVFX, position, transform.rotation);
        }


    }
}


