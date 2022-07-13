using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinSpawnerScript : MonoBehaviour
{
    [SerializeField]
    AnimationCurve spawnCurve;
    [SerializeField]
    //bool vertOrHor;
    enum Spacing
    {
        Vertical,
        Horizontal,
        Z_Space
    }
    [SerializeField]
    Spacing curSpacing;

    [SerializeField]
    bool inReverse;
    [SerializeField]
    GameObject objToSpawn;    
    [SerializeField]
    Transform endOfCoinSpawn;
    [SerializeField]    
    [Range(3, 20)]
    uint howManyObjects;
    [SerializeField]
    [Range(0, 25)]
    float archForObject;
    [SerializeField]
    float steep;
    Action<Vector3> spawnAction;
    
    enum TypeOfMesh
    {
        Cube,
        Sphere,
        Mesh
    }
    [SerializeField]
    TypeOfMesh thisMesh;
    [SerializeField]
    float radiusOfMesh;
    [SerializeField]
    Color gizmosColor;
    [SerializeField]
    Mesh spawnerMesh;
    [SerializeField]
    Vector3 sizeOfTheObject;

    
    // Start is called before the first frame update
    void Start()
    {
        //drawGizmos = false;
        Vector3 startPos = transform.position;
        spawnAction += InstantiateObject;        
        Spawner(howManyObjects - 1, startPos, endOfCoinSpawn.position, archForObject, steep, inReverse, spawnAction);
        
    }

    void InstantiateObject(Vector3 positionOfTheObject)
    {
        InstantiateObject(objToSpawn, positionOfTheObject);
    }

    void InstantiateObject(GameObject objectToInstantiate, Vector3 positionOfTheObject)
    {
        Instantiate(objectToInstantiate, positionOfTheObject, Quaternion.identity, transform);
    }

    void Spawner(uint howMany, Vector3 start, Vector3 end, float arch, float steepsess, bool reverse, Action<Vector3> toSpawn)
    {
        Vector3 posToSpawn;
        float archChanging;
        
        float howManyInOneScale = 1f/howMany;
        int reverseDir;
        if (reverse)
        {
            reverseDir = -1;
        }
        else
        {
            reverseDir = 1;
        }

        for (float i = 0; i <= 1; i += howManyInOneScale)
        {
            posToSpawn = Vector3.Lerp(start, end, i);
            if (arch != 1)
            {
                /*float iModified;
                if (i <= 0.5f)
                {
                    iModified = i * 2;
                }
                else
                {
                    iModified = (1 - i) * 2;
                }
                //Debug.LogFormat("i modified = {0}", iModified);

                archChanging = Mathf.Lerp(transform.position.y, transform.position.y + arch, steepsess * Mathf.Pow(iModified, 2)) - transform.position.y;

                posToSpawn.y += archChanging;*/
                archChanging = spawnCurve.Evaluate(i);
                switch (curSpacing)
                {
                    case Spacing.Vertical:
                        posToSpawn.y += archChanging * arch * reverseDir;
                        break;
                    case Spacing.Horizontal:
                        posToSpawn.x += archChanging * arch * reverseDir;
                        break;
                    case Spacing.Z_Space:
                        posToSpawn.z += archChanging * arch * reverseDir;
                        break;
                }
            }
            if(toSpawn != null)
            {
                toSpawn(posToSpawn);
            }
        }       
    }

    void DrawWire(Vector3 positionOfSphere)
    {
        Gizmos.color = gizmosColor;
        switch (thisMesh)
        {
            case TypeOfMesh.Sphere:
                Gizmos.DrawWireSphere(positionOfSphere, radiusOfMesh);
                break;
            case TypeOfMesh.Cube:
                Gizmos.DrawWireCube(positionOfSphere, new Vector3(radiusOfMesh, radiusOfMesh, radiusOfMesh));
                break;
            case TypeOfMesh.Mesh:
                Gizmos.DrawWireMesh(spawnerMesh, positionOfSphere, Quaternion.identity, sizeOfTheObject);
                break;
        }        
    }

    Action<Vector3> drawAction;
    
    bool drawGizmos = true;
    private void OnDrawGizmos()
    {
        /*Vector3 startOfSpawn = transform.position;
        Vector3 endOfSpawn = endOfCoinSpawn.position;
        float arch = archForObject;
        bool reverse = inReverse;*/        

        if (drawGizmos)
        {
            drawAction = DrawWire;
            Spawner(howManyObjects - 1, transform.position, endOfCoinSpawn.position, archForObject, steep, inReverse, drawAction);
        }
    }
}
