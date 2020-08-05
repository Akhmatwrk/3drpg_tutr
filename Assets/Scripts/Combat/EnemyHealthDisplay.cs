using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Update()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();

            if (fighter.GetTarget() != null)
            {
                gameObject.GetComponent<Text>().text = String.Format("{0:0}%", fighter.GetTarget().GetPrcentage());
            }
            else
            {
                gameObject.GetComponent<Text>().text = "none";
            }
        }
    }
}

