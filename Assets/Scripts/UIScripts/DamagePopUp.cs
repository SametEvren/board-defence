using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UIScripts
{
    public class DamagePopUp : MonoBehaviour
    {
        public static DamagePopUp Create(GameObject damagePopUpPrefab, Vector3 position, string popUpText, PopUpType popUpType)
        {
            var damagePopUpTransform = Instantiate(damagePopUpPrefab);
            damagePopUpTransform.transform.position = position + Vector3.up * 2;
            
            var damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.Setup(popUpText, popUpType);

            return damagePopUp;
        }

        private static int _sortingOrder;

        private const float DisappearTimerMax = 0.9f;
        
        private TextMeshPro textMesh;
        private float disappearTimer;
        private Color textColor;
        private Vector3 moveVector;
        private float setOff = 2;
        
        private void Awake()
        {
            textMesh = transform.GetComponent<TextMeshPro>();
        }
        
        private void OnEnable()
        {
                var sequence = DOTween.Sequence();
                sequence.Append(transform.DOScale(0.1f, 0.5f))
                    .Append(transform.DOScale(0.05f, 0.5f))
                    .Join(DOVirtual.Float(1,0f,0.5f, v =>
                    {
                        textColor.a = v; 
                        textMesh.color = textColor;
                    }))
                    .OnComplete(()=>Destroy(gameObject));
        }

        private void Setup(string popUpText, PopUpType popUpType)
        {
            textMesh.SetText(popUpText);
            if (popUpType == PopUpType.EnemyDamage)
            {
                textMesh.fontSize = 36;
                ColorUtility.TryParseHtmlString( "#FFC500" , out Color myColor );
                textColor = myColor;
            }
            if(popUpType == PopUpType.DefenceItemDamage)
            {
                textMesh.fontSize = 45;
                ColorUtility.TryParseHtmlString( "#FF2B00" , out Color myColor );
                textColor = myColor;
            }

            textMesh.color = textColor;
            disappearTimer = DisappearTimerMax;
            
            var modSortingOrder = (_sortingOrder % 1000) + 200;
            
            textMesh.sortingOrder = modSortingOrder;
        }

        private void Update()
        {
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 8f * Time.deltaTime;
        }
    }

    public enum PopUpType
    {
        EnemyDamage,
        DefenceItemDamage,
    }
}