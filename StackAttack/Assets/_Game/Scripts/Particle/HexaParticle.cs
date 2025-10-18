using UnityEngine;

public class HexaParticle : MonoBehaviour
{
    void OnParticleSystemStopped()
    {
        HexaParticlePool.Instance.ReleaseHexaParticle(this);
    }
}
