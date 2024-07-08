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
    private readonly EcsFilter<SwitchMainStoreEvent> switchMainStoreEventFilter;
    private readonly EcsFilter<WalletComponent> walletFilter;
        
    public void Run()
    {
        foreach (var i in openStoreEventFilter)
        {
            SwitchMainStore(true);
            
            foreach (Transform office in sceneData.listOfficesStore)
            {
                Object.Destroy(office.gameObject);
            }
            
            foreach (var office in staticData.offices)
            {
                Spawn(office);
            }
        }

        foreach (var i in switchMainStoreEventFilter)
        {
            SwitchMainStore(switchMainStoreEventFilter.Get1(i).isSwitch);
        }
    }
    
    private void Spawn(OfficeSizeData office)
    {
        var officeObject = Object.Instantiate(staticData.prefabOfficeUI, sceneData.listOfficesStore);

        var storeCell = officeObject.GetComponent<StoreCell>();
        storeCell.textPrice.text = office.price.ToString();
        
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
            
            SwitchMainStore(false);
        }
        else
        {
            world.NewEntity().Get<FailBuyEvent>();
        }
    }

    private void SwitchMainStore(bool isSwitch)
    {
        if (isSwitch)
        {
            sceneData.storeWindow.gameObject.SetActive(true);
            sceneData.buttonBuyOffice.gameObject.SetActive(false);
        }
        else
        {
            sceneData.storeWindow.gameObject.SetActive(false);
            sceneData.buttonBuyOffice.gameObject.SetActive(true);
        }
    }
}