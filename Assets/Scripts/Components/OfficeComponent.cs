using System;
using System.Collections.Generic;
using Leopotam.Ecs;

[Serializable]
public struct OfficeComponent
{
    public int id;
    public List<EcsEntity> Furnitures;
    public int size;
}