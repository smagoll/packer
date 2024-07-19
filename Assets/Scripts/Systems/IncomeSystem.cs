using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using UnityEngine;
using YG;

sealed class IncomeSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    private readonly EcsWorld world;
    private readonly SceneData sceneData;

    private readonly EcsFilter<SpawnFurnitureEvent> spawnFurnitureEventFilter;
    private readonly EcsFilter<AddOfficeEvent> addOfficeEventFilter;
    private readonly EcsFilter<EndCreateOfficesEvent> endCreateOfficesFilter;
    private readonly EcsFilter<IncomeComponent> incomeFilter;
    private readonly EcsFilter<WalletComponent> walletFilter;
    private readonly EcsFilter<UpdateIncomeEvent> incomeEventFilter;

    private List<IncomeComponent> incomes = new();

    private bool isRun;
    private float lastTime;
    private readonly float intervalTime = 1f;
    private float localMoney;
    
    public void Init()
    {
        EcsEntity walletEntity = world.NewEntity();
        ref var walletComponent = ref walletEntity.Get<WalletComponent>();
        walletComponent.money = YandexGame.savesData.money;
        
        UpdateTextIncome();
        SaveCycle().Forget();
    }

    public void Run()
    {
        foreach (var i in incomeEventFilter) UpdateIncomes();
        foreach (var i in spawnFurnitureEventFilter) UpdateTextIncome();
        foreach (var i in addOfficeEventFilter) UpdateTextIncome();
        foreach (var i in endCreateOfficesFilter) IncomeAbsence();

        // income
        if (Time.time - lastTime > intervalTime)
        {
            ref var wallet = ref walletFilter.Get1(0);
            wallet.money += incomes.Sum(x => x.income);
            localMoney = wallet.money;
            sceneData.money.text = wallet.money.GetReduceMoney();
            
            lastTime = Time.time;
        }
    }

    private void UpdateIncomes()
    {
        incomes.Clear();
        foreach (var i in incomeFilter) incomes.Add(incomeFilter.Get1(i));
        sceneData.income.text = incomes.Sum(x => x.income).GetReduceMoney() + " е/с";
        UpdateTextIncome();
    }

    private void UpdateTextIncome()
    {
        var money = walletFilter.Get1(0).money;
        sceneData.money.text = money.GetReduceMoney();
    }

    private void IncomeAbsence()
    {
        if (YandexGame.savesData.lastDateEnter > 0)
        {
            var lastDateEnter = YandexGame.savesData.lastDateEnter;
            var passedTime = (DateTime.Now.Ticks - lastDateEnter);
            var dateTime = new TimeSpan(passedTime);
            var seconds = (int)Math.Round(dateTime.TotalSeconds);
            
            ref var wallet = ref walletFilter.Get1(0);
            wallet.money += incomes.Sum(x => x.income) * seconds;
            sceneData.money.text = wallet.money.GetReduceMoney();
        }
    }

    private void SaveMoney()
    {
        YandexGame.savesData.lastDateEnter = DateTime.Now.Ticks;
        YandexGame.savesData.money = localMoney;
        YandexGame.SaveProgress();
    }

    private async UniTaskVoid SaveCycle()
    {
        while (Application.isPlaying)
        {
            SaveMoney();
            await UniTask.Delay(60000);
        }
    }
    
    public void Destroy()
    {
        SaveMoney();
    }
}