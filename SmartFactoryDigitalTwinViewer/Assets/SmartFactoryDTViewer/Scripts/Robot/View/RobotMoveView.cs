using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class RobotMoveView : MonoBehaviour
{
    [SerializeField] private Behaviour outlineBehaviour;

    public Action<RobotMoveView> OnClicked;
    public Action<RobotMoveView> OnHoverEnter;
    public Action<RobotMoveView> OnHoverExit;

    private NavMeshAgent navAgent;
    [Header("회피 설정")]
    [SerializeField] private float avoidanceRadius = 0.8f;
    [SerializeField] private int avoidancePriority = 50;  // 낮을수록 우선순위 높음
    private Vector3 targetPosition;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        if (navAgent != null)
        {
            // 로컬 회피 설정
            navAgent.radius = avoidanceRadius;
            navAgent.avoidancePriority = avoidancePriority;
            navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

            navAgent.speed = 3.5f;
            navAgent.acceleration = 8f;
            navAgent.stoppingDistance = 0.3f;
            navAgent.autoBraking = true;
        }
    }

    public void SetOutlineVisible(bool visible)
    {
        if (outlineBehaviour != null)
            outlineBehaviour.enabled = visible;
    }

    public void OnRender(RobotViewModel vm)
    {
       
        targetPosition = vm.position;
        if (navAgent != null && navAgent.isOnNavMesh)
        {
           // Debug.Log("RobotViewModelID"+vm.id);
            // NavMesh 위의 가장 가까운 지점으로 보정
            NavMeshHit hit;
            if (NavMesh.SamplePosition(vm.position, out hit, 1f, NavMesh.AllAreas))
            {
                navAgent.SetDestination(hit.position);
            }
        }
       
        transform.rotation = vm.rotation;
    }

    private void OnMouseEnter(){
        OnHoverEnter?.Invoke(this);
}
    private void OnMouseExit() => OnHoverExit?.Invoke(this);

    private void OnMouseDown() => OnClicked?.Invoke(this);

    void OnDrawGizmosSelected()
    {
        // 회피 반경 표시
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }
}
