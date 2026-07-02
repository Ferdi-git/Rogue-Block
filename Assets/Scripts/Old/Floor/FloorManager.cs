using System;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public SOEventFloor floorEvent;
    [SerializeField] private FloorListSo floorList;
    public int BossToutLescombienDeTour = 10;
    public SOEventFloorCreated floorCreated;
    public List<FloorStep> CustomFloors = new List<FloorStep>();

    private void OnEnable()
    {
        floorEvent.FirstFloorGeneration += GenerateFirstFloorList;
    }
    private void OnDisable()
    {
        floorEvent.FirstFloorGeneration -= GenerateFirstFloorList;
    }




    public void GenerateFirstFloorList () // j'ai mis "first" parce que il y a peut etre la possiblité de jouer à l'infini, la ça genere pour les 30premier floors
    {
        floorList.list.Clear();
        
        for (int i = 0; i < 60 ; i++)
        {
         if (i%2 == 0)
            {
                floorList.list.Add(FloorEvent.NormalFight);
            }
         else
            {
                floorList.list.Add(FloorEvent.Shop);
            }
         if (i % BossToutLescombienDeTour == 0 && i != 0)
            {
                floorList.list[i] = FloorEvent.BossFight;
            }
        }

        for (int i = 0; i < CustomFloors.Count; i++)
        {
            floorList.list[CustomFloors[i].NumeroDuPalier] = CustomFloors[i].Evenement;
        }

        floorCreated.InvokeFloorCreated();
    }

    


}













[Serializable]
public class FloorStep
{
    public FloorEvent Evenement;
    public int NumeroDuPalier;
}
public enum FloorEvent
{
    NormalFight,
    BossFight,
    Shop,
    Heal
}
