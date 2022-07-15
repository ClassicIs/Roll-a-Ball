using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall
{
    int xSize;
    int ySize;
    GameObject cell;
    Renderer rendererOfObject;
    Vector3 positionOfOrigin;
    Vector3 colliderSize;

    public Wall(int xSize, int ySize, GameObject cell, Vector3 originPosition)
    {
        this.xSize = xSize;
        this.ySize = ySize;

        this.cell = cell;
        rendererOfObject = cell.GetComponent<Renderer>();
        colliderSize = rendererOfObject.bounds.size;
        positionOfOrigin = new Vector3(originPosition.x + colliderSize.x / 2, originPosition.y + colliderSize.y / 2, originPosition.z);
    }

    public Vector3[] BuildAWall(out Vector3 sizeOfTheWholeWall, out Vector3 centerOfTheWall)
    {
        //Debug.LogFormat("Collider is {0}\nCollider position is {1}", colliderOfObject.isTrigger, colliderOfObject.bounds.center);
        
        int numberOfPositions = xSize * ySize;
        Vector3[] positionsOfCells = new Vector3[numberOfPositions];
         

        sizeOfTheWholeWall = new Vector3(xSize * colliderSize.x / 2, ySize * colliderSize.y / 2, colliderSize.z / 2);

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
        centerOfTheWall = new Vector3((positionOfOrigin.x + sizeOfTheWholeWall.x) - colliderSize.x / 2, (positionOfOrigin.y + sizeOfTheWholeWall.y) - colliderSize.x / 2, positionOfOrigin.z);
        return positionsOfCells;
    }
}
