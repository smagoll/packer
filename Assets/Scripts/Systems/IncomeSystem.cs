using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;

sealed class IncomeSystem : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem
{
    private readonly EcsWorld _world = null;
    private readonly SceneData sceneData;
    
    private readonly EcsFilter<IncomeComponent> incomeFilter;
    private readonly EcsFilter<UpdateIncomeEvent> incomeEventFilter;

    private List<IncomeComponent> incomes = new();
    private WalletComponent wallet;

    private bool isRun;
    
    public void Init()
    {
        EcsEntity walletEntity = _world.NewEntity();
        wallet = walletEntity.Get<WalletComponent>();

        isRun = true;
        Income().Forget();
    }

    public void Run()
    {
        foreach (var i in incomeEventFilter)
        {
            UpdateIncomes();
            Debug.Log("update income");
        }
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
        incomes.Clear();
        foreach (var i in incomeFilter) incomes.Add(incomeFilter.Get1(i));
    }


    public void Destroy()
    {
        isRun = false;
    }
}