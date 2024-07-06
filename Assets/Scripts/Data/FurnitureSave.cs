using UnityEngine;

public struct FurnitureSave
{
    public int id;
    public Vector2 position;

    public FurnitureSave(int id, Vector2 position)
    {
        this.id = id;
        this.position = position;
    }
}