using Cinemachine;

public class CameraShakeManager : SingletonPatternBase<CameraShakeManager>
{
    public void CameraShake(CinemachineImpulseSource impulseSource, float shakeForce)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }
    
    
}
