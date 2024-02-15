using UnityEngine;

namespace GDD.Spatial_Partition
{
    public interface IPawn
    {
        public Vector2Int GetCellPosition();
        public void SetCellPosition(Vector2Int cell);
        public Vector2 GetPawnVision();
        public void SetPawnVision(Vector2 vision);
        
        public Transform GetPawnTransform();
        public IPawn GetPreviousPawn();
        public void SetPreviousPawn(IPawn pawn);
        public IPawn GetNextPawn();
        public void SetNextPawn(IPawn pawn);
    }
}