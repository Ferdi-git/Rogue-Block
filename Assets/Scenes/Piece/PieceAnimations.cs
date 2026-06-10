using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieceAnimations : MonoBehaviour
{
    AudioSource audioSource;
    private BoardPiece boardPiece;

    private SinglePieceSquare[] squares;

    [SerializeField] StatsEnnemi statsEnnemi;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioClip shieldBreak;
    [SerializeField] Color FullHealthColor;
    [SerializeField] Color HealthColor;
    [SerializeField] Color LowHealthColor;
    [SerializeField] Color TextShieldShield;
    [SerializeField] Color TextShieldDamage;
    [SerializeField] ParticleSystem deathParticle;

    [Header("---Speed")]
    [SerializeField, Range(0.1f, 3f)] private float animSpeed = 1f;

    [Header("---Glow")]
    [SerializeField] private float glowIntensity = 2f;
    [SerializeField] private float glowDuration = 0.25f;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    [Tooltip("Normal,Repeat,Atk,Defend,Heal")]
    [SerializeField, ColorUsage(true, true)] private Color[] glowColors;


    [Header("---Health Display")]
    [SerializeField] TextMeshPro textHealth;
    [SerializeField] TextMeshPro textShield;
    [SerializeField] SpriteRenderer spriteBackground;


    [Header("---Events")]
    [SerializeField] SOEventPieceHealth eventPieceHealth;
    [SerializeField] SOEventTrail eventTrail;
    [SerializeField] SOEventVisualNumber eventVisualNumber;
    [SerializeField] SOEventVisuelEffect visualEffect;


    // Scales a duration by animSpeed (higher = faster)
    private float S(float duration) => duration / animSpeed;
    private int lastShield = 0;


    private void OnEnable()
    {
        eventPieceHealth.PieceTakeDamage += PieceTakeDamage;
        eventPieceHealth.PieceShieldBreak += PieceLooseShield;
    }
    private void OnDisable()
    {
        eventPieceHealth.PieceTakeDamage -= PieceTakeDamage;
        eventPieceHealth.PieceShieldBreak -= PieceLooseShield;
    }

    private void Start()
    {
        boardPiece = gameObject.GetComponent<PieceInfo>().currentBoardPiece;
        squares = gameObject.GetComponent<PieceInfo>().GetSelfPoints();
        for (int i = 0; i < squares.Length; i++)
        {
            spriteRenderers.Add(gameObject.GetComponent<PieceInfo>().GetSelfPoints()[i].spriteRenderer);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RefreshHealth(boardPiece);
    }

    public IEnumerator PlayAnimations(int number, TypeAnim typeAnim, BoardPiece declencheur)
    {
        Color glowColor = GetGlowColor(typeAnim);

        yield return Parabole(typeAnim, glowColor, number, declencheur);

        EffetPiece(typeAnim);

        transform.DOKill();

        transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);

        transform.DOScale(1.05f + 0.005f * number, S(0.1f)).OnComplete(() =>
        {
            int intClip = Mathf.Clamp(number, 0, audioClips.Length - 1);
            audioSource.pitch = 1f;

            audioSource.clip = audioClips[intClip];
            audioSource.Play();

            transform.DOScale(1f, S(0.1f));
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        });

        yield return Glow(glowColor, number);
    }

    private IEnumerator Glow(Color glowColor, int numberSpeed)
    {
        Color baseColor = glowColors[0];

        float glowIn = Mathf.Max(S(0.07f), S(glowDuration * 0.3f - 0.01f * numberSpeed));
        float glowOut = Mathf.Max(S(0.13f), S(glowDuration - 0.01f * numberSpeed));

        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            Material mat = spriteRenderers[i].material;

            mat.DOKill();
            float capturedIn = glowIn;
            float capturedOut = glowOut;

            mat.DOColor(glowColor, "_GlowColor", capturedIn)
               .OnComplete(() =>
               {
                   mat.DOColor(baseColor, "_GlowColor", capturedOut);
               });
        }
        yield return new WaitForSeconds(glowIn + glowOut);
    }

    private Color GetGlowColor(TypeAnim typeAnim)
    {
        Color glowColor = glowColors[0];
        float intensityMultiplier = Mathf.Pow(2f, glowIntensity);

        switch (typeAnim)
        {
            case TypeAnim.classic:
                glowColor = glowColors[0] * intensityMultiplier;
                break;
            case TypeAnim.repeat:
                glowColor = glowColors[1] * intensityMultiplier;
                break;
            case TypeAnim.atk:
                glowColor = glowColors[2] * intensityMultiplier;
                break;
            case TypeAnim.shield:
                glowColor = glowColors[3] * intensityMultiplier;
                break;
            case TypeAnim.heal:
                glowColor = glowColors[4] * intensityMultiplier;
                break;
            case TypeAnim.takeDamage:
                glowColor = glowColors[2] * intensityMultiplier;
                break;
            case TypeAnim.loseShield:
                glowColor = glowColors[2] * intensityMultiplier;
                break;
            case TypeAnim.generateMana:
                glowColor = glowColors[6] * intensityMultiplier;
                break;
        }
        return glowColor;
    }

    private IEnumerator Parabole(TypeAnim typeAnim, Color glowColor, int number, BoardPiece declencheur)
    {
        if (typeAnim == TypeAnim.takeDamage)
        {
            bool ended = false;
            Action trailEvent = () => ended = true;

            visualEffect.InvokeEffectEnemyDealAtk(new VisuelAttakData()
            {
                posAttacker = transform.position,
                eventEndVisuel = trailEvent,
            });
            yield return new WaitUntil(() => ended);
        }
        else
        {
            Color repeatColor = glowColors[1] * Mathf.Pow(2f, glowIntensity);

            if (declencheur != null && typeAnim == TypeAnim.atk)
            {
                bool ended = false;
                Action trailEvent = () => ended = true;
                eventTrail.InvokeCreateTrail(new EventTrailData()
                {
                    pos1 = declencheur.pieceInfo.transform.position,
                    pos2 = transform.position,
                    height = 1,
                    trailTime = S(0.15f - 0.005f * number),
                    glowColor = repeatColor,
                    eventEndTrail = trailEvent,
                });
                yield return new WaitUntil(() => ended);
            }
            else if (declencheur != null)
            {
                bool ended = false;
                Action trailEvent = () => ended = true;
                eventTrail.InvokeCreateTrail(new EventTrailData()
                {
                    pos1 = declencheur.pieceInfo.transform.position,
                    pos2 = transform.position,
                    height = 1,
                    trailTime = S(0.15f - 0.005f * number),
                    glowColor = glowColor,
                    eventEndTrail = trailEvent,
                });
                yield return new WaitUntil(() => ended);
            }

            if (typeAnim == TypeAnim.atk)
            {
                bool ended = false;
                Action trailEvent = () => ended = true;
                visualEffect.InvokeEffectAtkEnemy(new VisuelAttakData()
                {
                    posAttacker = transform.position,
                    eventEndVisuel = trailEvent,
                });
                yield return new WaitUntil(() => ended);
            }

            if (typeAnim == TypeAnim.heal || typeAnim == TypeAnim.shield || typeAnim == TypeAnim.loseShield)
                RefreshHealth(null);
        }
    }

    private void EffetPiece(TypeAnim typeAnim)
    {
        switch (typeAnim)
        {
            case TypeAnim.classic: break;
            case TypeAnim.repeat: break;
            case TypeAnim.atk: break;
            case TypeAnim.shield: PlayShieldAnim(); break;
            case TypeAnim.heal: PlayHealAnim(); break;
            case TypeAnim.takeDamage: break;
            case TypeAnim.loseShield: break;
            case TypeAnim.generateMana: break;
        }
    }

    public void DestroyPieceAnim()
    {
        gameObject.GetComponent<PieceInfo>().Unfill();
        StartCoroutine(DieAnim());
    }

    private IEnumerator DieAnim()
    {
        deathParticle.Play();
        yield return new WaitForSeconds(S(0.3f));
        Destroy(gameObject);
    }


    public enum TypeAnim
    {
        classic,
        repeat,
        atk,
        shield,
        heal,
        failed,
        takeDamage,
        loseShield,
        generateMana,
    }


    public void PlayHealAnim()
    {
        foreach (SinglePieceSquare s in squares) s.healParticule.Play();
    }

    public void PlayShieldAnim()
    {
        foreach (SinglePieceSquare s in squares)
        {
            s.shieldGO.transform.localScale = Vector3.zero;
            s.shieldGO.transform.DOScale(Vector3.one, S(0.2f)).SetEase(Ease.InOutSine);
        }
    }

    public void PieceTakeDamage(BoardPiece piece, int nbr)
    {
        if (piece != boardPiece) return;

        EventVisualNbrData visualNbrData = new EventVisualNbrData();
        visualNbrData.nbr = nbr;
        visualNbrData.color = Color.red;
        visualNbrData.isPositive = false;

        float randX = UnityEngine.Random.Range(0f, 1f);
        visualNbrData.spawnPoint = new Vector2(transform.position.x + randX, transform.position.y + randX);

        eventVisualNumber.InvokeCreateVisualNumber(visualNbrData);
        RefreshHealth(piece);
    }

    public void PieceLooseShield(BoardPiece piece, int nbr)
    {
        if (piece != boardPiece) return;

        EventVisualNbrData visualNbrData = new EventVisualNbrData();
        visualNbrData.nbr = nbr;
        visualNbrData.color = Color.cyan;
        visualNbrData.isPositive = false;

        audioSource.clip = shieldBreak;
        audioSource.pitch = 1;
        audioSource.Play();

        float randX = UnityEngine.Random.Range(0f, 1f);
        visualNbrData.spawnPoint = new Vector2(transform.position.x + randX, transform.position.y + randX + 0.75f);

        eventVisualNumber.InvokeCreateVisualNumber(visualNbrData);
        RefreshHealth(piece);
    }

    public void RefreshHealth(BoardPiece piece)
    {
        if (boardPiece.healthPoint == boardPiece.maxHealthPoint)
        {
            textHealth.text = boardPiece.healthPoint.ToString();
            textHealth.color = FullHealthColor;
        }
        else if (boardPiece.healthPoint < 6)
        {
            textHealth.text = boardPiece.healthPoint.ToString();
            textHealth.color = LowHealthColor;
        }
        else
        {
            textHealth.text = boardPiece.healthPoint.ToString();
            textHealth.color = HealthColor;
        }


        int nbrAttacked = boardPiece.context.NbrCaseAtk * statsEnnemi.actualAtkDamage;

        textShield.gameObject.SetActive(false);

        if (boardPiece.shield <= 0 && lastShield > 0)
        {
            foreach (SinglePieceSquare s in squares)
            {
                s.shieldGO.transform.DOKill();
                s.shieldGO.transform.DOScale(Vector3.zero, S(0.2f)).SetEase(Ease.InOutSine);
            }
        }

        if (boardPiece.shield > 0)
        {
            textShield.gameObject.SetActive(true);
            textShield.color = TextShieldShield;
            textShield.text = boardPiece.shield.ToString();
        }
        else if (nbrAttacked > 0)
        {
            textShield.gameObject.SetActive(true);
            textShield.color = TextShieldDamage;

            textShield.text = "-" + nbrAttacked.ToString();
        }

        lastShield = boardPiece.shield;

    }

    public void ShowOnTop()
    {
        for (int i = 0; i < spriteRenderers.Count; i++) spriteRenderers[i].sortingOrder = 7;
        spriteBackground.sortingOrder = 8;
        textHealth.sortingOrder = 9;
        textShield.sortingOrder = 10;
    }

    public void ShowNormal()
    {
        for (int i = 0; i < spriteRenderers.Count; i++) spriteRenderers[i].sortingOrder = 3;
        spriteBackground.sortingOrder = 5;
        textHealth.sortingOrder = 5;
        textShield.sortingOrder = 6;
    }
}