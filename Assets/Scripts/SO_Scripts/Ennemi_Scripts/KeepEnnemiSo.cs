using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
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
