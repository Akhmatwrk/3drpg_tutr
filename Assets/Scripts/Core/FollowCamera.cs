using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target;

        // Update is called once per frame
        void Update()
        {
            transform.position = target.transform.position;
        }
    }
}
