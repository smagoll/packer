using System;
using System.Collections.Generic;
using Leopotam.Ecs;

[Serializable]
public struct OfficeComponent
{
    public int id;
    public List<EcsEntity> furnitures;
    public int size;
}