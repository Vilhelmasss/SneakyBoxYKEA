using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<PlacedObject> placedObjects { get; set; } = new List<PlacedObject>();
}
