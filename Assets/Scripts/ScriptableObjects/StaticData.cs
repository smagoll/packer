using UnityEngine;

[CreateAssetMenu(menuName = "ECSData/StaticData")]
public class StaticData : ScriptableObject
{
    public FurnitureIncomeData[] furnitures;
    public OfficeSizeData[] offices;
}