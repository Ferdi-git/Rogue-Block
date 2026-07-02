using Sirenix.OdinInspector;
using UnityEngine;

public class CanonManager : MonoBehaviour
{
    CanonVisual canonVisual;
    private int currentDamage;

    private void Start()
    {
        canonVisual = GetComponent<CanonVisual>();
        currentDamage = 0;
    }
    [Button]
    private void GainDamage(int damage)
    {
        currentDamage += damage;
        if(canonVisual != null)  canonVisual.UpdateDamage(currentDamage);
    }
        
    [Button]
    private int Shoot()
    {
        int damage = currentDamage;
        currentDamage = 0;
        if (canonVisual != null) canonVisual.Shoot();
        return damage;
    }

}
