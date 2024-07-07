using System.Linq;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;
using YG;

sealed class StoreSystem : IEcsRunSystem
{
    private readonly EcsWorld world;
    private readonly SceneData sceneData;
    private readonly StaticData staticData;

    private readonly EcsFilter<OpenStoreEvent> openStoreEventFilter;
    private readonly EcsFilter<WalletComponent> walletFilter;
    
    public void Run()
    {
        foreach (var i in openStoreEventFilter)
        {
            foreach (Transform furniture in sceneData.listFurnitures)
            {
                Object.Destroy(furniture.gameObject);
            }
            
            foreach (var office in staticData.offices)
            {
                Spawn(office);
            }
        }
    }
    
    private void Spawn(OfficeSizeData office)
    {
        var officeObject = Object.Instantiate(office.prefabStore, sceneData.listOfficesStore);
        
        officeObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuyOffice(office);
        });
    }

    private void BuyOffice(OfficeSizeData office)
    {
        var officeData = staticData.offices.FirstOrDefault(x => x.officeType == office.officeType);
        ref var money = ref walletFilter.Get1(0).money;
        if (money >= officeData.price)
        {
            money -= officeData.price;
            ref var addOfficeEvent = ref world.NewEntity().Get<AddOfficeEvent>();
            addOfficeEvent.officeType = office.officeType;
            
            sceneData.storeWindow.SetActive(false);
        }
        else
        {
            world.NewEntity().Get<FailBuyEvent>();
            Debug.Log("no money");
        }
    }
}