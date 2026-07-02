using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/KeepEnnemiSo")]
public class KeepEnnemiSo : ScriptableObject
{
    public Palier[] paliers = new Palier[4];
    public List<GeneratEnnemiSo> bossList;
}

[Serializable]
public class Palier
{
    public List<Sprite> Backgrounds;
    public List<GeneratEnnemiSo> ennemiList;
}
