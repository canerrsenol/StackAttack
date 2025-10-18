using UnityEngine;

public class HexaParticlePool : GenericPoolBase<HexaParticlePool, HexaParticle>
{
    public HexaParticle GetHexaParticle() => GetFromPool();

    public void ReleaseHexaParticle(HexaParticle hexaParticle) => SendToPool(hexaParticle);
}
