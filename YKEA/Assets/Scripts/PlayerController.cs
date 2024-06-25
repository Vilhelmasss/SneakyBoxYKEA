using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public enum PlayerState
    {
        PlaceObject,
        FreeSelect,
    }

    [Serializable]
    public class KeyPlacedObjectPair
    {
        public string key;
        public GameObject objectToPlace;
    }
    public List<KeyPlacedObjectPair> ObjectList = new List<KeyPlacedObjectPair>();
    private Dictionary<string, GameObject> stringToPlacedObject = new Dictionary<string, GameObject>();
    private Dictionary<GameObject, PlacedObject> placedObjectsKeyValuePair = new Dictionary<GameObject, PlacedObject>();
    private List<PlacedObject> placedObjects = new List<PlacedObject>();
    private float gridSize = 1f;
    private float snapThreshold = 0.5f;
    public GameObject CanvasUI;
    public GameObject selectedGameObject;
    public string currentKey;
    public PlayerState currentState = PlayerState.FreeSelect;
    public bool isGridEnabled = false;
    public GameObject previewPrefab;
    private GameObject previewObject;

    void Awake()
    {
        foreach (var kvp in ObjectList)
        {
            stringToPlacedObject[kvp.key] = kvp.objectToPlace;
        }
    }

    void Start()
    {
        currentState = PlayerState.FreeSelect;
    }

    void Update()
    {
        switch (currentState)
        {
            case PlayerState.PlaceObject:
                PlaceObjectState();
                break;
            case PlayerState.FreeSelect:
                FreeSelectState();
                break;
            default:
                return;
        }
    }

    void PlaceObjectState()
    {
        UpdatePreviewPosition();
        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }
    }

    public void SwitchToPlaceObjectState()
    {
        currentState = PlayerState.PlaceObject;
        previewObject = Instantiate(stringToPlacedObject[GetKey()]);
        previewObject.GetComponent<Collider>().enabled = false;
    }

    public void MoveObjectButton()
    {
        // TO DO - ATTACH COLOR TO THE NEW OBJECT
        PlacedObject placedObject = placedObjectsKeyValuePair[selectedGameObject];
        ObjectDelete();
        currentKey = placedObject.ObjectKey;
        SwitchToPlaceObjectState();
    }

    public void ChangeColor(Color color)
    {
        if (selectedGameObject is null)
            return;

        var material = selectedGameObject.GetComponent<Renderer>().material;
        material.color = color;

        placedObjectsKeyValuePair[selectedGameObject].Color[0] = color.r;
        placedObjectsKeyValuePair[selectedGameObject].Color[1] = color.g;
        placedObjectsKeyValuePair[selectedGameObject].Color[2] = color.b;
    }

    public void SwtichToFreeSelectState()
    {
        if (previewObject is not null)
            Destroy(previewObject);

        currentState = PlayerState.FreeSelect;
    }

    private void UpdatePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                Renderer renderer = previewObject.GetComponent<Renderer>();

                if (renderer != null)
                {
                    Vector3 offset = new Vector3(0, renderer.bounds.extents.y, 0);
                    Vector3 position = hit.point + offset;

                    if (isGridEnabled)
                        position = SnapToGrid(position);

                    previewObject.transform.position = position;
                }
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlacedObject"))
            {
                
            }
        }
    }

    private void PlaceObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                var objectToPlace = stringToPlacedObject[GetKey()];
                Renderer renderer = objectToPlace.GetComponent<Renderer>();

                if (renderer != null)
                {
                    Vector3 offset = new Vector3(0, renderer.bounds.extents.y, 0);
                    Vector3 position = hit.point + offset;

                    if (isGridEnabled)
                        position = SnapToGrid(position);

                    GameObject placedObject = Instantiate(objectToPlace, position, Quaternion.identity);
                    ObjectSave(placedObject);
                }
            }
        }
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        // TO DO - ADD SNAPPING TO OBJECTS IF Physics.Overlap HITS A gameObject WITH LAYER "PlacedObject".
        position.x = Mathf.Round(position.x) * gridSize;
        position.z = Mathf.Round(position.z) * gridSize;

        return position;
    }

    void FreeSelectState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PlacedObject"))
                    ObjectSelected(hit.collider.gameObject);
            }
        }
    }

    void ObjectSelected(GameObject placedObject)
    {
        selectedGameObject = placedObject;
        CanvasUI.GetComponent<UIManager>().ObjectSelected();
    }

    private string GetKey()
    {
        switch (currentKey)
        {
            case "Cube":
                currentKey = "Cube";
                return currentKey;
            case "Sphere":
                currentKey = "Sphere";
                return currentKey;
            case "Cylinder":
                currentKey = "Cylinder";
                return currentKey;
            default:
                currentKey = "Cube";
                return currentKey;
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.placedObjects = this.placedObjects;
    }

    public void LoadData(GameData gameData)
    {
        foreach (var go in gameData.placedObjects)
        {
            GameObject placedObject = Instantiate(stringToPlacedObject[go.ObjectKey], FloatsToVector3(go.Position), FloatsToQuaternion(go.Rotation));
            var material = placedObject.GetComponent<Renderer>().material;
            material.color = new Color(go.Color[0], go.Color[1], go.Color[2]);

            ObjectSave(placedObject, go.ObjectKey);
        }
    }

    private void ObjectSave(GameObject placedObject, string objectKey = null)
    {
        if (objectKey is null)
        {
            GetKey();
            objectKey = currentKey;
        }

        var material = placedObject.GetComponent<Renderer>().material;

        float[] colorFloats = VariablesToFloatArray(material.color.r, material.color.g, material.color.b);
        float[] posFloats = VariablesToFloatArray(placedObject.transform.position.x, placedObject.transform.position.y, placedObject.transform.position.z);
        float[] rotFloats = VariablesToFloatArray(placedObject.transform.localRotation.x, placedObject.transform.localRotation.y, placedObject.transform.localRotation.z);

        PlacedObject _placedObject = new PlacedObject()
        {
            ObjectKey = objectKey,
            Color = colorFloats,
            Position = posFloats,
            Rotation = rotFloats,
        };

        placedObjectsKeyValuePair.Add(placedObject, _placedObject);
        placedObjects.Add(_placedObject);
    }

    public void ObjectDelete()
    {
        try
        {
            placedObjects.Remove(placedObjectsKeyValuePair[selectedGameObject]);
            placedObjectsKeyValuePair.Remove(selectedGameObject);
            Destroy(selectedGameObject);
        }
        catch (Exception e)
        {
            throw new Exception($"{e} exception");
        }
    }

    private Vector3 FloatsToVector3(float[] floats)
    {
        if (floats is null || floats.Length != 3)
        {
            throw new ArgumentException("Input array must have exactly 3 elements.");
        }

        return new Vector3(floats[0], floats[1], floats[2]);
    }

    private Quaternion FloatsToQuaternion(float[] floats)
    {
        if (floats is null || floats.Length != 3)
        {
            throw new ArgumentException("Input array must have exactly 3 elements.");
        }

        Quaternion quat = Quaternion.Euler(floats[0], floats[1], floats[2]);
        return quat;
    }

    private float[] VariablesToFloatArray(float x, float y, float z)
    {
        float[] floats = { x, y, z };

        return floats;
    }
}