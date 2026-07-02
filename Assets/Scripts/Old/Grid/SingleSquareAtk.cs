using TMPro;
using UnityEngine;

public class SingleSquareAtk : MonoBehaviour
{
    [SerializeField] Transform atkPoint;
    [SerializeField] TextMeshPro text;
    [SerializeField] Transform Warning;
    
    public Transform GetTransform() { return transform; }

    public void SetText(string texte) { text.text = texte; }

    public void TurnTextRightSide() { Warning.transform.rotation = Quaternion.identity; }
}
