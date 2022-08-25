using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class Planet : SerializedMonoBehaviour
{
    public GameObject landPref;
    public ELandData[,,] planetData;

    public float cameraSize;
    public float rotationSpeed;
    public float revolutionSpeed;

    public void Awake()
    {
        
        GenPlanet();
    }

    [Button("Init")]
    public void Init()
    {
        planetData = new ELandData[16, 16, 16];
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (Mathf.Pow(x - 7.5f, 2) + Mathf.Pow(y - 7.5f, 2) + Mathf.Pow(z - 7.5f, 2) > 6 * 6)
                        planetData[x, y, z] = ELandData.empty;
                    else
                        planetData[x, y, z] = ELandData.grass;
                }
            }
        }
    }

    public void GenPlanet()
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (planetData[x, y, z]!=ELandData.underground&&planetData[x,y,z]!=ELandData.empty)
                    {
                        Vector3 pos = transform.position + new Vector3(x - 7.5f, y - 7.5f, z - 7.5f) * landPref.transform.localScale.x;
                        var land= Instantiate(landPref, pos, transform.rotation);
                        land.transform.parent = transform;
                    }
                }
            }
        }
    }

    [Button("MoveCamera")]
    public void CameraFollow()
    {
        CameraControl.instance.SetTarget(new CameraData(transform, cameraSize));
    }

    public void FixedUpdate()
    {
        if(GameManager.instance.currentState!=GameState.Editor)
        {
            transform.RotateAround(Sun.instance.transform.position, Vector3.forward, Time.deltaTime * revolutionSpeed);
            transform.Rotate(transform.up, Time.deltaTime * rotationSpeed,Space.Self);
        }
    }
}
public enum ELandData
{
    empty,
    underground,
    grass
}