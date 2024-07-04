using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECSData/StaticData")]
public class StaticData : ScriptableObject
{
    public FurnitureIncomeData[] furnitures;
    public OfficeSizeData[] offices;
    
    [Header("Tiles")]
    public Tile tileFloor;
    public Tile tileHighlight;
}