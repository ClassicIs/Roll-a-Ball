using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    int xSize;
    int ySize;
    GameObject cell;
    BoxCollider colliderOfObject;
    Renderer rendererOfObject;
    Vector3 positionOfOrigin;

    public Wall(int xSize, int ySize, GameObject cell, Vector3 originPosition)
    {
        this.xSize = xSize;
        this.ySize = ySize;

        this.cell = cell;

        positionOfOrigin = originPosition;
        colliderOfObject = cell.GetComponent<BoxCollider>();
        rendererOfObject = cell.GetComponent<Renderer>(); 

    }

    public Vector3[] BuildAWall(out Vector3 sizeOfTheWholeWall)
    {
        //Debug.LogFormat("Collider is {0}\nCollider position is {1}", colliderOfObject.isTrigger, colliderOfObject.bounds.center);
        
        int numberOfPositions = xSize * ySize;
        Vector3[] positionsOfCells = new Vector3[numberOfPositions];
        Vector3 colliderSize = rendererOfObject.bounds.size;

        sizeOfTheWholeWall = new Vector3(xSize * colliderSize.x, ySize * colliderSize.y, colliderSize.z);

        float xValueOfCollider = colliderSize.x;
        float yValueOfCollider = colliderSize.y;

        //Debug.LogFormat("Size X of the piece is {0}\nSize Y of the piece is {1}\nSize Z of the piece is {2}\nName of the collider is {3}", colliderOfObject.bounds.size.x, colliderOfObject.bounds.size.y, colliderOfObject.bounds.size.z, colliderOfObject.name);
        
        float oldCellXPosition = 0f;
        float oldCellYPosition = 0f;

        float currentCellXPosition = oldCellXPosition;
        float currentCellYPosition = oldCellYPosition;

        int k = 0;

        for (int i = 0; i < ySize; i++)
        {
            /*float tmpYPos = currentCellYPosition;
            currentCellYPosition = oldCellYPosition + yValueOfCollider;
            oldCellYPosition = tmpYPos;*/

            currentCellYPosition = positionOfOrigin.y + yValueOfCollider * i;

            for (int j = 0; j < xSize; j++)
            {
                /*float tmpXPos = currentCellXPosition;
                currentCellXPosition = oldCellXPosition + xValueOfCollider;
                oldCellXPosition = tmpXPos;*/
                currentCellXPosition = positionOfOrigin.x + xValueOfCollider * j;
                positionsOfCells[k] = new Vector3(currentCellXPosition, currentCellYPosition, positionOfOrigin.z);
                k++;
            }
        }
        /*oldCellXPosition = 0f;
        currentCellXPosition = oldCellXPosition;*/

        return positionsOfCells;
    }
}
