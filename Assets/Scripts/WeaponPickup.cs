using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon pickupWeapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Fighter playerFighter = other.GetComponent<Fighter>();
                playerFighter.EquipWeapon(pickupWeapon);
                Destroy(gameObject);
            }
        }
    }
}
