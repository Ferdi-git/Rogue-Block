using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PieceInfoPanel : MonoBehaviour
{

    [SerializeField] PieceInfo pieceInfo;
    [SerializeField] StatsEnnemi statsEnnemi;
    Canvas canvas;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject panelInfo;
    [SerializeField] float timeBeforeApearing = 0.15f;
    float baseScaleY;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
    private void Start()
    {
        baseScaleY = panelInfo.transform.localScale.y;
        HidePanel();
        Initialyse();
    }

    public void Initialyse()
    {
        text.gameObject.SetActive(false);
        RefreshPanel();
    }

    public void ShowPanel()
    {
        transform.rotation = Camera.main.transform.rotation;


        float rotZ = GetComponent<RectTransform>().localRotation.eulerAngles.z;
        float offsetX = 0.5f;
        float offsetY = 0.5f;

        if (Mathf.Approximately(rotZ, -90f)) offsetX = -0.5f;
        if (Mathf.Approximately(rotZ, 180f)) { offsetY = -0.5f; offsetX = -0.5f; }
        

        GetComponent<RectTransform>().localPosition = new Vector3(offsetX, offsetY, transform.localPosition.z);

        RefreshPanel();
        panelInfo.transform.DOKill();
        text.gameObject.SetActive (false);
        panelInfo.transform.localScale = new Vector3(panelInfo.transform.localScale.x , 0, panelInfo.transform.localScale.z) ;
        panelInfo.SetActive(true);
        panelInfo.transform.DOScaleY(baseScaleY,0.2f).SetEase(Ease.InOutSine).SetDelay(timeBeforeApearing).OnComplete(() => {
            audioSource.pitch = Random.Range(1,1.1f);
            audioSource.Play();
            text.gameObject.SetActive(true);

        }
        );

    }

    public void HidePanel()
    {
        panelInfo.transform.DOKill();
        text.gameObject.SetActive(false);
        panelInfo.transform.DOScaleY(0, 0.2f).OnComplete(() => panelInfo.SetActive(false));

    }


    private void RefreshPanel()
    {
        int nbrAttacked = pieceInfo.currentBoardPiece.context.NbrCaseAtk * statsEnnemi.actualAtkDamage;
        string stringToShow = "";
        stringToShow += $"HP : <color=green>{pieceInfo.currentBoardPiece.healthPoint}</color>";
        stringToShow += nbrAttacked == 0 ? "\n" : $" -<color=red>{nbrAttacked}\n </color>";
        if (pieceInfo.currentBoardPiece.shield !=0) 
            stringToShow += $"Shield : <color=cyan>{pieceInfo.currentBoardPiece.shield}\n </color>";

        string description = pieceInfo.soPiece.description;

        for (int i = 0; i < pieceInfo.soPiece.TempEffectValues.Count; i++)
        {
            description = description.Replace($"{{p{i}}}", pieceInfo.soPiece.TempEffectValues[i].ToString());
        }

        stringToShow += "\n" + description;
        text.text = stringToShow;
    }

}
