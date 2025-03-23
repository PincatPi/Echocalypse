using UnityEngine;
using Cinemachine;

public class MidpointOffset : CinemachineExtension 
{
    [Header("矫正参数")]
    [Tooltip("玩家在屏幕中心点 (0.5,0.5)")]
    [SerializeField] private Vector2 screenCenter = new Vector2(0.5f, 0.5f);
    
    [Tooltip("位置矫正速度")]
    [Range(3, 10)] 
    [SerializeField] private float correctionSpeed = 5f;

    [Header("目标绑定")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform enemyTransform;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, 
        ref CameraState state, 
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body && playerTransform && enemyTransform)
        {
            // 计算玩家在屏幕中的位置
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(playerTransform.position);
            Vector2 screenPos = new Vector2(
                playerScreenPos.x / Screen.width,
                playerScreenPos.y / Screen.height
            );

            // 计算偏移量
            Vector2 offset = screenCenter - screenPos;
            
            //TODO: 检查此处的问题
            // 应用位置修正
            state.PositionCorrection += 
                state.ReferenceUp * offset.y * correctionSpeed * deltaTime +
                new Vector3(state.RawPosition.x, 0, 0) * offset.x * correctionSpeed * deltaTime;

            // 动态调整摄像机距离（可选）
            float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
            CinemachineFramingTransposer transposer = vcam.GetComponent<CinemachineFramingTransposer>();
            if (transposer != null)
            {
                transposer.m_CameraDistance = Mathf.Lerp(
                    transposer.m_CameraDistance,
                    distance * 1.2f, // 距离系数
                    deltaTime * correctionSpeed
                );
            }
        }
    }
}