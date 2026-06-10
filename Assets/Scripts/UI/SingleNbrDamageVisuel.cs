using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleNbrDamageVisuel : MonoBehaviour
{
    [SerializeField] TextMeshPro mainText;
    [SerializeField] TextMeshPro outlineText;
    Vector3 baseScale;


    public void Initialise(DataUIVisuel data)
    {
        mainText.color = data.textColor;
        mainText.text = data.text;
        outlineText.text = data.text;
        StartAnim();
    }


    private void StartAnim()
    {
        baseScale = transform.localScale;
        float basLocalY = transform.localPosition.y;

        transform.localScale = baseScale*3;

        transform.DOScale(baseScale, 0.3f);

        transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 1.5f)
            .SetDelay(0.3f)
            .OnComplete(() => Destroy(gameObject));
    }

}



public class DataUIVisuel
{
    public string text;
    public Color textColor;
}
