using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float expPoints = 0;

        public void GainExp(float experience)
        {
            expPoints += experience;
        }

    }
}

