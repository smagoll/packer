using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;

sealed class IncomeSystem : IEcsInitSystem, IEcsDestroySystem
{
    private readonly EcsWorld _world = null;
    private readonly SceneData sceneData;
    private EcsFilter<IncomeComponent> incomeFilter;

    private List<IncomeComponent> incomes = new();
    private WalletComponent wallet;

    private bool isRun;
    
    public void Init()
    {
        UpdateIncomes();
        
        EcsEntity walletEntity = _world.NewEntity();
        wallet = walletEntity.Get<WalletComponent>();

        isRun = true;
        Income().Forget();
    }

    private async UniTaskVoid Income()
    {
        while (isRun)
        {
            wallet.money += incomes.Sum(x => x.income);
            sceneData.money.text = wallet.money.ToString();
            await UniTask.Delay(1000);
        }
    }

    private void UpdateIncomes()
    {
        foreach (var i in incomeFilter) incomes.Add(incomeFilter.Get1(i));
    }


    public void Destroy()
    {
        isRun = false;
    }
}