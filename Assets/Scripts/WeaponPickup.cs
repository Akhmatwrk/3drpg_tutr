using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon pickupWeapon = null;
        [SerializeField] float waitTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(pickupWeapon);
                StartCoroutine(HideForSeconds());
            }
        }

        IEnumerator HideForSeconds()
        {
            ShowPickup(false);
            yield return new WaitForSeconds(waitTime);
            ShowPickup(true);
        }

        private void ShowPickup(bool isShowing)
        {
            GetComponent<SphereCollider>().enabled = isShowing;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isShowing);
            }
        }
    }
}
