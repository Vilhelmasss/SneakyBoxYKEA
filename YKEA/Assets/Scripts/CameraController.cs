using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    private class WallAndDistance
    {
        public GameObject wall;
        public float distanceToWall;
    }
    public GameObject cameraHolder;
    public List<GameObject> visibleWalls;
    public int maxSeenWalls;
    private float horizontal = 0f;
    private float vertical = 0f;

    void Start()
    {
        UpdateVisibleWalls();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal != 0f)
        {
            cameraHolder.transform.localRotation *= Quaternion.Euler(0, horizontal * Time.deltaTime * 120f, 0);
            UpdateVisibleWalls();
        }
        if (vertical != 0f)
        {
            gameObject.transform.Translate(Vector3.forward * vertical * Time.deltaTime * 10f);
        }
    }

    void UpdateVisibleWalls()
    {
        List<WallAndDistance> wallsAndDistance = new List<WallAndDistance>();

        for (int i = 0; i < visibleWalls.Count; i++)
        {
            float distance = Vector3.Distance(visibleWalls[i].transform.position, gameObject.transform.position);

            WallAndDistance wallAndDistance = new WallAndDistance()
            {
                wall = visibleWalls[i],
                distanceToWall = distance,
            };

            wallsAndDistance.Add(wallAndDistance);
        }

        wallsAndDistance.Sort((b, a) => a.distanceToWall.CompareTo(b.distanceToWall));

        for (int i = 0; i < wallsAndDistance.Count; i++)
        {
            if (i < maxSeenWalls)
                wallsAndDistance[i].wall.SetActive(true);
            else
                wallsAndDistance[i].wall.SetActive(false);
        }
    }
}
