using com.sluggagames.keepUsAlive.CharacterSystem;
using com.sluggagames.keepUsAlive.Obstacle;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace com.sluggagames.keepUsAlive.Core
{
    /// <summary>
    /// Allows information of the Gameobject to be displayed in game.
    /// </summary>
    public class DisplayData : MonoBehaviour
    {
        [SerializeField]
        Slider healthSlider;
        Health target;
        [SerializeField]
        Vector3 screenOffset = new Vector3(0, 30, 0);
        float navMeshHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup canvasGroup;
        Vector3 targetPosition;

        private void Awake()
        {
            this.transform.SetParent(GameObject.Find("UI").transform, false);
            canvasGroup = this.GetComponent<CanvasGroup>();
            
           
            

        }

        private void Update()
        {
            if(target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            if(healthSlider != null)
            {
               
                DisplayCharacterHealth();
            }
        }

        private void LateUpdate()
        {
            if(targetRenderer != null)
            {
                this.canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
                targetTransform = target.transform;
                if(targetTransform != null)
                {
                    targetPosition = target.transform.position;
                    targetPosition.y += navMeshHeight;
                    this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
                }
            }
        }

        void DisplayCharacterHealth()
        {
          
            if (target)
            {
                healthSlider.value = target.CharacterHealth;
            }
            else
            {
                return;
            }
        }

        public void SetTarget(Health _target)
        {
            if(_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Character target for PlayerUI.SetTarget.", this);
            }
            target = _target;
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
           
            NavMeshAgent navAgent = target.GetComponent<NavMeshAgent>();

            if(navAgent != null)
            {
                navMeshHeight = navAgent.height;

            }

        }


    }
}
