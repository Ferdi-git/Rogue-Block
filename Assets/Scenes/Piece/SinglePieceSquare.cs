using UnityEngine;

public class SinglePieceSquare : MonoBehaviour
{
    public ParticleSystem healParticule;
    public ParticleSystem shieldParticule;
    public SpriteRenderer spriteRenderer;
    public GameObject shieldGO;
    public bool generateMana = false;

    void Start()
    {
       spriteRenderer = GetComponent<SpriteRenderer>();
    }

}
