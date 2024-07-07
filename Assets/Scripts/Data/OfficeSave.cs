using System.Collections.Generic;

public struct OfficeSave
{
    public int id;
    public OfficeType officeType;
    public List<FurnitureSave> furnitures;

    public OfficeSave(int id, OfficeType officeType)
    {
        this.id = id;
        this.officeType = officeType;
        furnitures = new();
    }
}