using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<PlacedObject> placedObjects { get; set; } = new List<PlacedObject>();
}
