using System;
using UnityEngine;

namespace GDD.Spatial_Partition
{
    public abstract class Pawn : MonoBehaviour, IPawn
    {
        protected IPawn previousPawn;
        protected IPawn nextPawn;
        [SerializeField]protected Vector2Int cellPos = new Vector2Int();

        public virtual void OnEnable()
        {
            
        }

        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            
        }
        
        public virtual void OnDisable()
        {
            
        }

        public Vector2Int GetCellPosition()
        {
            return cellPos;
        }

        public void SetCellPosition(Vector2Int cell)
        {
            cellPos = cell;
        }

        public virtual Vector2 GetPawnVision()
        {
            return new Vector2();
        }
        public virtual void SetPawnVision(Vector2 vision)
        {
            
        }

        public abstract Transform GetPawnTransform();

        public IPawn GetPreviousPawn()
        {
            return previousPawn;
        }

        public void SetPreviousPawn(IPawn pawn)
        {
            previousPawn = pawn;
        }

        public IPawn GetNextPawn()
        {
            return nextPawn;
        }

        public void SetNextPawn(IPawn pawn)
        {
            nextPawn = pawn;
        }
    }
}