using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(CanonManager))]
public class CanonVisual : MonoBehaviour
{
    CanonManager manager;
    [SerializeField] GameObject visuel;
    [SerializeField] ParticleSystem fillEffect;
    [SerializeField] int maxDamage = 70;
    [SerializeField] float timeFilling = 2;
    int currentDamageShown = 0;
    Animator animator;
    [SerializeField] Slider slider;

   

    private void Start()
    {
        animator = GetComponent<Animator> ();
        currentDamageShown = 0;
        manager = GetComponent<CanonManager>();
        slider.maxValue = maxDamage;
        slider.value = currentDamageShown;
        fillEffect.gameObject.SetActive(false);
    }

    public void UpdateDamage(int newDamage)
    {
        if (newDamage > maxDamage)
        {
            newDamage = maxDamage;

        }
        fillEffect.gameObject.SetActive(true);
        slider.DOValue(newDamage,timeFilling).OnComplete(()=>fillEffect.gameObject.SetActive(false));

        currentDamageShown = newDamage;

        float calc = (float)currentDamageShown / (float)maxDamage;
        print($"{calc} + {currentDamageShown} + {maxDamage}");
        animator.SetFloat("Shaking", calc);
    }

    public void Shoot()
    {
        currentDamageShown = 0;
        animator.SetFloat("Shaking", 0);
        animator.SetTrigger("Shoot");
        slider.value = currentDamageShown;
    }

}
