using UnityEngine;

namespace UIScripts
{
    public class UISetTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;

        private void Start()
        {
            if (target == null) return;
            transform.position = target.position + offset;
        }
    }
}