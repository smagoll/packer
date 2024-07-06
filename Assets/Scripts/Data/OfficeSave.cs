using UnityEngine;

public struct OfficeSave
{
    public int id;
    public FurnitureSave[] furnitures;

    public OfficeSave(int id, FurnitureSave[] furnitures)
    {
        this.id = id;
        this.furnitures = furnitures;
    }
}