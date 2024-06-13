using UnityEngine;
using UnityEngine.Assertions;

namespace UIScripts
{
    public class DamageUI : MonoBehaviour
    {
        public GameObject damagePopUpPrefab;

        private void OnValidate()
        {
            Assert.IsNotNull(damagePopUpPrefab);
        }
    }
}