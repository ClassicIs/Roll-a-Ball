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
    WholeWallScript wholeWallScript;

    // Start is called before the first frame update
    void Start()
    {
        int length = transform.childCount;
        piecesOfWall = new GameObject[length - 1];
        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                wholeWallScript = transform.GetChild(i).gameObject.GetComponent<WholeWallScript>();
                wholeWallScript.OnPlayerHit += Break;
                continue;
            }
            piecesOfWall[i - 1] = transform.GetChild(i).gameObject;
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
            //Debug.Log("Player is NOT rushing");
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

                LeanTween.color(rigidBody.gameObject, tmpCol, timeToDisappear).destroyOnComplete = true;
                //Destroy(piece, 4f);
            }
            
            Destroy(gameObject, timeToDisappear);
        }
    }

}
