using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ECSData/StaticData")]
public class StaticData : ScriptableObject
{
    public FurnitureIncomeData[] furnitures;
    public OfficeSizeData[] offices;
    public Vector2 offsetParallaxBackground;
    
    [Header("Tiles")]
    public Tile tileFloor;
    public Tile tileHighlight;
    
    [Header("Prefabs")]
    public GameObject prefabOfficeUI;
    public GameObject prefabOffice;
    public GameObject prefabFurnitureUI;
}