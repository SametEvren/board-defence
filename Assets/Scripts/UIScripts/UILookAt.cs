using UnityEngine;

namespace UIScripts
{
    public class UILookAt : MonoBehaviour
    {
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            var rotation = mainCamera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }
    }
}