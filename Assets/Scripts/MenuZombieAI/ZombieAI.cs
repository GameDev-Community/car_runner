using UnityEngine;
using UnityEngine.AI;

namespace ZombieAI
{
    public class ZombieAI : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Animator animator;
        [SerializeField] private float sleepTime;

        private ZombieAIManager zombieAIManager;
        private float currentSleepTime = 0;
        private bool isWalking = false;
        private Vector3 targetPoint;

        public void Initialize(ZombieAIManager zombieAIManager)
        {
            this.zombieAIManager = zombieAIManager;
            GetNewTargetPoint();
        }

        private void Update()
        {
            if ((targetPoint - transform.position).sqrMagnitude < 2f)
            {
                if (isWalking)
                {
                    navMeshAgent.ResetPath();
                    isWalking = false;
                    animator.SetBool("isWalking", isWalking);
                }

                currentSleepTime += Time.deltaTime;
                if (currentSleepTime > sleepTime)
                {
                    currentSleepTime = 0;
                    GetNewTargetPoint();
                }
            }
        }

        private void GetNewTargetPoint()
        {
            targetPoint = zombieAIManager.GenerateTargetPosition();
            if ((targetPoint - transform.position).sqrMagnitude < 2f)
                return;
            navMeshAgent.SetDestination(targetPoint);
            isWalking = true;
            animator.SetBool("isWalking", isWalking);
        }
    }
}