using UnityEngine;

public class EnemyView : MonoBehaviour
{
    //战斗相关
    [SerializeField] private Transform detectionCenter;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Collider[] targets = new Collider[1];
    [SerializeField, Header("攻击目标")] private Transform currentTarget = null;
    public Transform CurrentTarget => currentTarget;
    [SerializeField, Range(0f, 360f)] private float detectAngle;
    
    void Update()
    {
        View();
    }
    
    //视野
    private void View()
    {
        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRadius, targets, playerLayer);
        bool isInView = false;
        //若玩家在检测范围内
        if (targetCount > 0)
        {
            //射线检测障碍物
            if (IsInView(targets[0].transform))
            {
                //检测玩家是否在该对象面前一定角度的范围内
                if (Vector3.Dot(((targets[0].transform.position + new Vector3(0, 1f, 0)) - (transform.position + new Vector3(0, 1.2f, 0))).normalized,
                        transform.forward) > Mathf.Cos(Mathf.Deg2Rad * detectAngle / 2))
                {
                    currentTarget = targets[0].transform;
                    isInView = true;
                }
            }
        }

        if (!isInView)
        {
            currentTarget = null;
            targets[0] = null;
        }
    }
    
    /// <summary>
    /// 检测玩家对象在视野中是否可见
    /// </summary>
    /// <param name="target"></param>
    /// <returns>true为可见，false为不可见</returns>
    private bool IsInView(Transform target)
    {
        for (int i = 5; i <= 10; i += 5)
        {
            float offset = i / 10f;
            //若检测到了障碍物(只检测障碍物层)
            //从头部向target从root开始依次向上每隔0.5f发射一条射线，若有一条射线命中了（检测不到障碍物）则说明看得到，返回true
            //TODO: 修改玩家在下蹲时的碰撞体大小
            if (Physics.Raycast((detectionCenter.position),
                    ((target.position + target.up * offset) - detectionCenter.position).normalized,
                    out RaycastHit hit, Vector3.Distance(detectionCenter.position, target.position + target.up * offset), obstacleLayer) == false)
            {
                return true;
            }
        }
        return false;
    }
    
    #region Gizmos绘图
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectionCenter.position, detectionRadius);

        if (targets[0] != null && currentTarget != null)
        {
            Gizmos.DrawRay(detectionCenter.position, ((targets[0].transform.root.position + targets[0].transform.root.up * 0f) - detectionCenter.position).normalized);
            Gizmos.DrawRay(detectionCenter.position, ((targets[0].transform.root.position + targets[0].transform.root.up * 0.5f) - detectionCenter.position).normalized);
            Gizmos.DrawRay(detectionCenter.position, ((targets[0].transform.root.position + targets[0].transform.root.up * 1f) - detectionCenter.position).normalized);
            Gizmos.DrawRay(detectionCenter.position, ((targets[0].transform.root.position + targets[0].transform.root.up * 1.5f) - detectionCenter.position).normalized);   
        }
    }
    
    #endregion
}
