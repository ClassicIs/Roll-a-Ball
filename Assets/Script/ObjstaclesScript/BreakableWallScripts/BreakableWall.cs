using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField]
    float explosionForce;
    [SerializeField]
    float radiusOfExplosion;

    GameObject[] piecesOfWall;
    [Header("One Piece")]
    [SerializeField]
    GameObject breakPiece;
    [SerializeField]
    WholeWallScript wholeWallScript;
    [SerializeField]
    [Range(1, 50)]
    int XSize;
    [SerializeField]
    [Range(1, 50)]
    int ySize;
    [SerializeField]
    GameObject onePieceOfWall;
    bool gizmosActive = true;

    private void Awake()
    {
        gizmosActive = false;
        wholeWallScript.OnPlayerHit += Break;
    }

    void Start()
    {
        Wall thisWall = new Wall(XSize, ySize, onePieceOfWall, transform.position);
        Vector3 sizeOfColliderWall;
        Vector3 centerOfWall;
        Vector3[] wallPositions = thisWall.BuildAWall(out sizeOfColliderWall, out centerOfWall);

        piecesOfWall = new GameObject[wallPositions.Length];

        //Debug.LogFormat("Wall size is {0}\nCenter of Wall is {1}", sizeOfColliderWall, centerOfWall);
        wholeWallScript.SetWallSize(sizeOfColliderWall, centerOfWall);
        for (int i = 0; i < wallPositions.Length; i++)
        {
            piecesOfWall[i] = Instantiate(onePieceOfWall, wallPositions[i], Quaternion.identity, transform);
        }
    }

    void DestroyOld()
    {
        foreach (GameObject piece in piecesOfWall)
        {
            Destroy(piece);
        }
    }
    
    public void Break(playerController player)
    {
        if (!player.Rushing())
        {            
            return;
        }

        wholeWallScript.WallOff();
        //Debug.Log("Player is rushing");
        GameObject[] newPieces = new GameObject[piecesOfWall.Length];
        Vector3 newPosition;
        for (int i = 0; i < piecesOfWall.Length; i++)
        {
            newPosition = piecesOfWall[i].transform.position;
            newPieces[i] = Instantiate(breakPiece, newPosition, Quaternion.identity, gameObject.transform);
        }

        DestroyOld();
        Color tmpCol = newPieces[0].GetComponentInChildren<MeshRenderer>().material.color;
        tmpCol.a = 0f;
        float timeToDisappear = 6;
        foreach (GameObject piece in newPieces)
        {
            Rigidbody[] rb = piece.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidBody in rb)
            {
                rigidBody.AddExplosionForce(explosionForce, player.transform.position, radiusOfExplosion);

                //LeanTween.color(rigidBody.gameObject, tmpCol, timeToDisappear).destroyOnComplete = true;
                //Destroy(piece, 4f);
            }
            
            Destroy(gameObject, timeToDisappear);
        }
    }

    private delegate void CreateCubes(Vector3 positions);

    private event CreateCubes EventToMakeCubes;

    private void OnDrawGizmos()
    {
        if(!gizmosActive)
        {
            return;
        }
        Wall thisWall = new Wall(XSize, ySize, onePieceOfWall, transform.position);
        Vector3 sizeOfColliderWall;
        Vector3 centerOfWall;
        Vector3[] wallPositions = thisWall.BuildAWall(out sizeOfColliderWall, out centerOfWall);
        Vector3 sizeOfColider = onePieceOfWall.GetComponent<Renderer>().bounds.size;

        Gizmos.color = Color.red;
        //Debug.LogFormat("Center of wall is {0}\nSize of Collider Wall is {1}", centerOfWall, sizeOfColliderWall);
        Gizmos.DrawCube(new Vector3(centerOfWall.x, centerOfWall.y, centerOfWall.z), new Vector3(sizeOfColliderWall.x * 2, sizeOfColliderWall.y * 2, sizeOfColliderWall.z * 2));
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(centerOfWall, sizeOfColider);
        Gizmos.color = Color.green;
        for (int i = 0; i < wallPositions.Length; i++)
        {
            //Debug.LogFormat("Cube {0} is {1}", i, sizeOfColider);
            Gizmos.DrawWireCube(wallPositions[i], sizeOfColider);
        }

        
    }
}
