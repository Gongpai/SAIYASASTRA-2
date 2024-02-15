using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD.Spatial_Partition
{
    public class Grid
    {
        private int cellSize;
        private List<IPawn>[,] cells;
        private int numEnemies;

        public List<IPawn>[,] get_allCells
        {
            get => cells;
        }
        
        //Init the grid
        public Grid(int mapWidth, int cellSize, int numEnemies)
        {
            this.cellSize = cellSize;
            this.numEnemies = numEnemies;
            int numberOfCells = mapWidth / cellSize;

            cells = new List<IPawn>[numberOfCells, numberOfCells];
        }
        
        //Add a unity to the grid
        public void Add(IPawn pawn)
        {
            //Determine which grid cell the pawn is in
            int cellX = (int)(pawn.GetPawnTransform().position.x / cellSize);
            int cellZ = (int)(pawn.GetPawnTransform().position.z / cellSize);

            if (cells[cellX, cellZ] == null)
                cells[cellX, cellZ] = new List<IPawn>(numEnemies);
            
            int i_pawn = cells[cellX, cellZ].IndexOf(pawn);
            if (i_pawn < 0)
                cells[cellX, cellZ].Add(pawn);
            else
                cells[cellX, cellZ][i_pawn] = pawn;
            
            //Debug.Log("Pawn Count : " + cells[cellX, cellZ].Count);
            pawn.SetCellPosition(new Vector2Int(cellX, cellZ));

            //Add the pawn to the front of the list for the cell it's in
            pawn.SetPreviousPawn(null);
            pawn.SetNextPawn(cells[cellX, cellZ][cells[cellX, cellZ].Count - 1]);

            //Associate this cell with this pawn
            cells[cellX, cellZ][cells[cellX, cellZ].Count - 1] = pawn;

            if (pawn.GetNextPawn() != null)
            {
                //Set this pawn to be the previous pawn of the next pawn of this pawn (linked lists ftw)
                pawn.GetNextPawn().SetPreviousPawn(pawn);
            }
        }

        public void Remove(Vector2Int cell, IPawn pawn)
        {
            if (pawn != null)
            {
                UnlinkCell(pawn);
            }
            
            if(cells[cell.x, cell.y].Count > 0)
                cells[cell.x, cell.y].Remove(cells[cell.x, cell.y][cells[cell.x, cell.y].IndexOf(pawn)]);
        }
        
        //Get the closest enemy from the grid in player vision
        public IPawn FindClosestEnemy(IPawn playerPawn)
        {
            Vector2 vision = playerPawn.GetPawnVision() / 2;
            
            //Determine which grid cell the friendly pawn is in
            float cellX = playerPawn.GetPawnTransform().position.x;
            float cellZ = playerPawn.GetPawnTransform().position.z;
            int[,] cells_pos = new int[4, 2]
            {
                { (int)((cellX + vision.x) / cellSize), (int)((cellZ - vision.y) / cellSize)},
                { (int)((cellX + vision.x) / cellSize), (int)((cellZ + vision.y) / cellSize)},
                { (int)((cellX - vision.x) / cellSize), (int)((cellZ + vision.y) / cellSize)},
                { (int)((cellX - vision.x) / cellSize), (int)((cellZ - vision.y) / cellSize)}
            };

            /* Debug All Calls Pos
            string str = null;
            for (int i = 0; i < cells_pos.Length / 2; i++)
            {
                str += i + ". ";
                str += "X : " + cells_pos[i, 0];
                str += ", Y : " + cells_pos[i, 1] + Environment.NewLine;
            }
            Debug.Log("-----------------------------------------------");
            Debug.Log("Box Vision : " + str);
            */
            
            IPawn enemy = null;
            float distance = 0;
            //Get the first enemy in vision
            for (int i = 0; i < cells_pos.GetLength(0);)
            {
                //Debug.Log("INDEX : " + i + " :: Current : " + (cells_pos.Length / 2) + " :: Calls x : " + cells.GetLength(0)  + " :: Calls x : " + cells.GetLength(1));
                //Debug.Log("INDEX : " + i + " :: Calls x : " + cells_pos[i, 0]  + " :: Calls x : " + cells_pos[i, 1]);

                if (cells_pos[i, 0] < (cellSize / vision.x) && cells_pos[i, 1] < (cellSize / vision.y))
                {
                    if (cells[cells_pos[i, 0], cells_pos[i, 1]] != null)
                    {
                        for (int j = 0; j < cells[cells_pos[i, 0], cells_pos[i, 1]].Count; j++)
                        {
                            //Debug.Log(i + ". Count : " + cells[cells_pos[i, 0], cells_pos[i, 1]].Count);
                            IPawn next_enemy = cells[cells_pos[i, 0], cells_pos[i, 1]][j];
                            if (next_enemy != null)
                            { 
                                try
                                {
                                    float new__distance = Vector3.Distance(playerPawn.GetPawnTransform().position,
                                        next_enemy.GetPawnTransform().position);
                                }
                                catch (Exception e)
                                {
                                
                                }
                                
                                float new_distance = Vector3.Distance(playerPawn.GetPawnTransform().position,
                                    next_enemy.GetPawnTransform().position);

                                //Debug.Log("New Distance : " + new_distance);
                                //Debug.Log("Old Distance : " + distance);
                                if (new_distance < distance || distance == 0)
                                {
                                    enemy = next_enemy;

                                    distance = Vector3.Distance(playerPawn.GetPawnTransform().position,
                                        enemy.GetPawnTransform().position);
                                }
                            }
                            else
                            {
                                //Debug.Log(i + ". " + " Not Found : " + "Pos X : " + cells_pos[i, 0] + " Pos Z : " + cells_pos[i, 1]);
                            }
                        }
                    }
                }
                else
                {
                    //Debug.LogWarning("Out Of Enemy !!!!!!!!!!!!!!!!!!!!!");
                }

                i++;
            }
            
            //Find the closest pawn of all in the linked list
            //IPawn closestPawn = null;

            //float bestDistSqr = Mathf.Infinity;

            //Loop through the linked list
            /*
            while (enemy != null)
            {
                //The distance sqr between the pawn and this enemy
                float distSqr = (enemy.GetPawnTransform().position - playerPawn.GetPawnTransform().position).sqrMagnitude;

                //If this distance is better than the previous best distance, then we have found an enemy that's closer
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;

                    closestPawn = enemy;
                }

                //Get the next enemy in the list
                enemy = enemy.GetNextPawn();
            }*/

            return enemy;
        }
        
        //A pawn in the grid has moved, so see if we need to update in which grid the soldier is
        public void OnPawnMove(IPawn pawn, Vector3 oldPos)
        {
            //See which cell it was in 
            int oldCellX = (int)(oldPos.x / cellSize);
            int oldCellZ = (int)(oldPos.z / cellSize);

            //See which cell it is in now
            int cellX = (int)(pawn.GetPawnTransform().position.x / cellSize);
            int cellZ = (int)(pawn.GetPawnTransform().position.z / cellSize);

            //If it didn't change cell, we are done
            if (oldCellX == cellX && oldCellZ == cellZ)
            {
                return;
            }

            //Unlink it from the list of its old cell
            Remove(new Vector2Int(oldCellX, oldCellZ), pawn);
            //Debug.Log("Cell Pos : " + oldPos);

            //If it's the head of a list, remove it
            for (int i = 0; i < cells[oldCellX, oldCellZ].Count; i++)
            {
                if (cells[oldCellX, oldCellZ][i] == pawn)
                {
                    cells[oldCellX, oldCellZ][i] = pawn.GetNextPawn();
                    break;
                }
            }


            //Add it bacl to the grid at its new cell
            Add(pawn);
        }

        private void UnlinkCell(IPawn pawn)
        {
            if (pawn.GetPreviousPawn() != null)
            {
                pawn.GetPreviousPawn().SetNextPawn(pawn.GetNextPawn());
            }

            if (pawn.GetNextPawn() != null)
            {
                pawn.GetNextPawn().SetPreviousPawn(pawn.GetPreviousPawn());
            }
        }
    }
}