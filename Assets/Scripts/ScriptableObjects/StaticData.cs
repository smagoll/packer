using UnityEngine;

[CreateAssetMenu(menuName = "ECSData/StaticData")]
public class StaticData : ScriptableObject
{
    public GameObject officePrefab;
    public FurnitureIncomeData[] furnitures;
}