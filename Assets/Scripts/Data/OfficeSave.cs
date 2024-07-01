using UnityEngine;

public struct OfficeSave
{
    public Vector2 position;
    public FurnitureSave[] furnitures;

    public OfficeSave(Vector2 position, FurnitureSave[] furnitures)
    {
        this.position = position;
        this.furnitures = furnitures;
    }
}