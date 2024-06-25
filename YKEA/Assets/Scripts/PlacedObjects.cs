[System.Serializable]
public class PlacedObject
{
    public string ObjectKey { get; set; } = string.Empty;
    public float[] Color { get; set; } = new float[3];
    public float[] Position { get; set; } = new float[3];
    public float[] Rotation { get; set; } = new float[3];
}