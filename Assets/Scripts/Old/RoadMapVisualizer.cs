using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadMapVisualizer : MonoBehaviour
{
    [SerializeField] FloorListSo listFloor;
    [SerializeField] SOEventFloor eventFloor;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] SpriteRenderer[] spriteWhiteBar;
    [SerializeField] List<SpriteWithFloor> FloorSprites;
    [SerializeField] SOEventFloorCreated floorCreated;
    private bool Isinitialized;

    private int currentFloor = 0;

    private void OnEnable()
    {
        eventFloor.NextFloor += NextFloor;
        floorCreated.FloorCreated += Initialized;
    }

    private void OnDisable()
    {
        eventFloor.NextFloor -= NextFloor;
        floorCreated.FloorCreated -= Initialized;
    }

    private void Awake()
    {
        //Initialized();
        Isinitialized = false;
    }

    private void Initialized()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = GetCorrespondingSprite(listFloor.list[i]) ;
        }
        Isinitialized = true;

    }
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.F))
    //    {
    //        NextFloor();
    //    }
    //}

    private void NextFloor()
    {
        if (!Isinitialized)
        {
            print("pas cool floor");
        }
        currentFloor++;
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            int index = i;

            if (index == 0)
            {
                Vector3 oldPos = spriteRenderers[index].transform.position;

                spriteRenderers[index].transform
                    .DOMoveY(spriteRenderers[i].transform.position.y -1f, 0.7f)
                    .OnComplete(() =>
                    {
                        spriteRenderers[index].transform.position = oldPos;
                        spriteRenderers[index].sprite = GetCorrespondingSprite(listFloor.list[index + currentFloor]);
                    });
            }
            else
            {
                Vector3 oldPos = spriteRenderers[index].transform.position;

                spriteRenderers[index].transform
                    .DOMoveY(spriteRenderers[index - 1].transform.position.y, 0.7f)
                    .OnComplete(() =>
                    {
                        spriteRenderers[index].transform.position = oldPos;
                        spriteRenderers[index].sprite = GetCorrespondingSprite(listFloor.list[index + currentFloor]);
                    });
            }
        }

        for (int i = 0; i < spriteWhiteBar.Length; i++)
        {
            int index = i;

            if (index == 0)
            {
                Vector3 oldPos = spriteWhiteBar[index].transform.position;

                spriteWhiteBar[index].transform
                    .DOMoveY(spriteWhiteBar[i].transform.position.y - 1f, 0.7f)
                    .OnComplete(() =>
                    {
                        spriteWhiteBar[index].transform.position = oldPos;
                        
                    });
            }
            else
            {
                Vector3 oldPos = spriteWhiteBar[index].transform.position;

                spriteWhiteBar[index].transform
                    .DOMoveY(spriteWhiteBar[index - 1].transform.position.y, 0.7f)
                    .OnComplete(() =>
                    {
                        spriteWhiteBar[index].transform.position = oldPos;
                        
                    });
            }
        }

    }



    private Sprite GetCorrespondingSprite(FloorEvent floorEvent)
    {
        for (int i = 0; i < FloorSprites.Count; i++)
        {
            if (floorEvent == FloorSprites[i].floorEvent)
            {
                return FloorSprites[i].sprite;
            }
        }

        return null;

    }
}

[Serializable]
public class SpriteWithFloor
{
    public Sprite sprite;
    public FloorEvent floorEvent;
}
