using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SOEventPlayer eventPlayer;
    [SerializeField] private StatsPlayer statsPlayer;
    [SerializeField] private StatsEnnemi statsEnnemi;
    [SerializeField] private SOEventEnnemy EventEnnemy;

    [SerializeField] private Slider PlayerSlider;
    [SerializeField] private Slider SecondPlayerSlider;
    [SerializeField] private Slider ManaSlider;
    [SerializeField] private Slider EnnemiSlider;

    [SerializeField] private SpriteRenderer ennemiSprite;
    [SerializeField] private TextMeshProUGUI PVPlayerText;
    [SerializeField] private TextMeshProUGUI PvEnnemiText;

    [SerializeField] private SOEventUpdateUI updateEvent;
    [SerializeField] private SOEventGridManager gridManager;

    [SerializeField] private TextMeshProUGUI shopHealthText;

    [SerializeField] private SOEventPlayerLostMessage lostMessage;
    [SerializeField] private SOEventPLayerWinMessage winMessage;


    private void OnEnable()
    {
        eventPlayer.GainPV += GainPV;
        eventPlayer.LoosePV += LoosePV;

        eventPlayer.GainMana += GainMana;
        eventPlayer.LooseMana += LooseMana;

        eventPlayer.GainShield += GainShield;
        eventPlayer.LooseShield += LooseShield;


        EventEnnemy.EnnemiGainPV += EnnemiGainPV;
        EventEnnemy.EnnemiLoosePV += EnnemiLoosePV;

        EventEnnemy.EnnemiGainShield += EnnemiGainShield;
        EventEnnemy.EnnemiLooseShield += EnnemiLostShield;
        EventEnnemy.NewEnnemi += UpdateUI;

        updateEvent.UpdateUI += UpdateUI;

        lostMessage.ActiveLostMessage += OpenLostMessage;
        winMessage.WinMessageEvent += OpenWinMessage;
    }
    private void OnDisable()
    {
        eventPlayer.GainPV -= GainPV;
        eventPlayer.LoosePV -= LoosePV;

        eventPlayer.GainMana -= GainMana;
        eventPlayer.LooseMana -= LooseMana;

        eventPlayer.GainShield -= GainShield;
        eventPlayer.LooseShield -= LooseShield;


        EventEnnemy.EnnemiGainPV -= EnnemiGainPV;
        EventEnnemy.EnnemiLoosePV -= EnnemiLoosePV;

        EventEnnemy.EnnemiGainShield -= EnnemiGainShield;
        EventEnnemy.EnnemiLooseShield -= EnnemiLostShield;
        EventEnnemy.NewEnnemi -= UpdateUI;

        updateEvent.UpdateUI -= UpdateUI;

        lostMessage.ActiveLostMessage -= OpenLostMessage;
        winMessage.WinMessageEvent -= OpenWinMessage;
    }

    

    public void UpdateUI ()
    {
        int pvPlayer = statsPlayer.GetPV();
        int pvMaxPlayer = statsPlayer.pvMax;

        int manaPlayer = statsPlayer.GetMana();
        int manaMaxPlayer = statsPlayer.manaMax;


        int shieldPlayer = statsPlayer.GetShield();

        int pvEnnemi = statsEnnemi.GetPV();
        int pvMaxEnnemi = statsEnnemi.pvMax;
        int shieldEnnemi = statsEnnemi.GetShield();

        PlayerSlider.maxValue = pvMaxPlayer;
        SecondPlayerSlider.maxValue = pvMaxPlayer;
        EnnemiSlider.maxValue =  pvMaxEnnemi;
        ManaSlider.maxValue =  manaMaxPlayer;
        PlayerSlider.value = pvPlayer;
        SecondPlayerSlider.value = pvPlayer;
        EnnemiSlider.value = pvEnnemi;
        ManaSlider.value = manaPlayer;

        PVPlayerText.text  = pvPlayer.ToString() + "/" + statsPlayer.pvMax.ToString();
        PvEnnemiText.text = pvEnnemi.ToString() + "/" + statsEnnemi.pvMax.ToString();
        shopHealthText.text = pvPlayer.ToString() + "/" + statsPlayer.pvMax.ToString();
        ennemiSprite.sprite = statsEnnemi.sprite;
        //Buy KORRIDOR ON STEAM 
    }

    private void GainPV (int amount)
    {
        UpdateUI();
    }
    private void LoosePV (int amount)
    {
        UpdateUI();
    }

    private void GainMana(int amount)
    {
        UpdateUI();
    }
    private void LooseMana(int amount)
    {
        UpdateUI();
    }

    private void GainShield (int amount)
    {
        UpdateUI();
    }
    private void LooseShield(int amount)
    {
        UpdateUI();
    }


    private void EnnemiGainPV (int amount )
    {
        UpdateUI();
    }
    private void EnnemiLoosePV (int amount )
    {
        UpdateUI();
    }
    private void EnnemiGainShield (int amount )
    {
        UpdateUI();
    }
    private void EnnemiLostShield (int amount )
    {
        UpdateUI();
    }


    // fonction du menu 

    [SerializeField] private GameObject EchapMenu;
    private bool isOpen;
    private bool canOpenEchapMenu;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canOpenEchapMenu)
            {
                OpenEchapMenu();
            }
            
        }
    }
    private void Start()
    {
        isOpen = false;
        canOpenEchapMenu = true;
        Time.timeScale = 1.0f;
        EchapMenu.SetActive(false);
        lostMessageGO.SetActive(false);
        UpdateUI();
    }
    public void OpenEchapMenu ()
    {
        if (!isOpen)
        {
            gridManager.InvokeSetAllPieceCanMove(false);
            EchapMenu.SetActive(true);
            Time.timeScale = 0f;
            isOpen = true;
        }
        else
        {
            gridManager.InvokeSetAllPieceCanMove(true);
            EchapMenu.SetActive(false);
            Time.timeScale = 1f;
            isOpen = false;
        }
        
    }
    public void ResumeButton()
    {
        gridManager.InvokeSetAllPieceCanMove(true);
        Time.timeScale = 1.0f;
        EchapMenu.SetActive(false);
    }
    public void ResetButton()
    {
        gridManager.InvokeSetAllPieceCanMove(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitButton ()
    {
        gridManager.InvokeSetAllPieceCanMove(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    [SerializeField] private GameObject lostMessageGO;
    private void OpenLostMessage()
    {
        lostMessageGO.SetActive (true);
        canOpenEchapMenu = false;
        Time.timeScale = 0f;

    }

    [SerializeField] private GameObject WinMessage;
    private void OpenWinMessage()
    {
        WinMessage.SetActive(true);
        canOpenEchapMenu = false;
        Time.timeScale = 0f;
    }
}
