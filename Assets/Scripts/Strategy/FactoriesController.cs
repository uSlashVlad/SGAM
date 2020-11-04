using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 ДЛЯ РАЗРАБОТЧИКОВ (FOR DEVELOPERS):
 Я знаю, что это отвратительный код, просто он в достаточной мере выполняет поставленную задачу,
 а она не самая ключевая в игре, ведь тема джема "moonshot", а не "strategy", хех
 Так что пока что сойдёт, если захочу сделать код лучше, сделаю, но не сейчас
 Я итак подобным не особо люблю заниматься, да и стратегии никогда не делал
*/
public class FactoriesController : MonoBehaviour
{
    /// Contains text ui elements for every factory
    [SerializeField] private Text[] statsText;

    /// Contains factories classes
    private readonly Factory[] _factories =
    {
        new Factory(0, 1, new[] {1, 2, 1, 1}, 15),
        new Factory(1, 2, new[] {2, 1, 1, 1}, 15),
        new Factory(2, 2, new[] {2, 1, 1, 1}, 15),
        new Factory(3, 3, new[] {2, 2, 2, 1}, 30),
        new Factory(4, 10, new[] {1, 1, 1, 1}, 50),
    };

    // Containers for resources values
    [SerializeField] private float resEnergy;
    [SerializeField] private float resMetal;
    [SerializeField] private float resCrystals;
    [SerializeField] private float resResearches;
    [SerializeField] private float resMoney;

    /// Contains text ui elements on bottom panel.
    /// Just for outputting resources amount to player
    [SerializeField] private Text[] resourceInds;

    private void Start()
    {
        // Initialization for indicators on bottom panel
        UpdateResources();
        // Starting coroutine for incrementing resources
        StartCoroutine(ResourcesCoroutine());
    }

    public void TryUpgrade(int upgradeNum)
    {
        var factory = _factories[upgradeNum];
        // Checking if player has enough resources 
        if (resMoney >= factory.GetCost &&
            resEnergy >= factory.BaseUpgradeRes[0] &&
            resMetal >= factory.BaseUpgradeRes[1] &&
            resCrystals >= factory.BaseUpgradeRes[2] &&
            resResearches >= factory.BaseUpgradeRes[3])
        {
            resMoney -= factory.GetCost;
            resEnergy -= factory.BaseUpgradeRes[0];
            resMetal -= factory.BaseUpgradeRes[1];
            resCrystals -= factory.BaseUpgradeRes[2];
            resResearches -= factory.BaseUpgradeRes[3];
            factory.Level++;

            UpdatePanelStats(upgradeNum);
            UpdateResources();
        }
    }

    public void UpdatePanelStats(int num)
    {
        var factory = _factories[num];
        // For smth like "1 energy/minute"
        var genType = (factory.TypeId == 0) ? "energy" :
            (factory.TypeId == 1) ? "metal" :
            (factory.TypeId == 2) ? "crystals" :
            (factory.TypeId == 3) ? "researches" : "money";
        statsText[num].text = $"{factory.GetGeneration} {genType}/minute\n\n" +
                              // For smth like "1 energy, 2 metal, 10000 money for upgrade
                              $"{factory.BaseUpgradeRes[0]} energy, {factory.BaseUpgradeRes[1]} metal, " +
                              $"{factory.BaseUpgradeRes[2]} crystals, {factory.BaseUpgradeRes[3]} researches, " +
                              $"{factory.GetCost} money for upgrade";
    }

    private void UpdateResources()
    {
        resourceInds[0].text = (int) resEnergy + "";
        resourceInds[1].text = (int) resMetal + "";
        resourceInds[2].text = (int) resCrystals + "";
        resourceInds[3].text = (int) resResearches + "";
        resourceInds[4].text = (int) resMoney + "";
    }

    private IEnumerator ResourcesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            // Because generation amount is amount per minute, every second I should add amount / 60
            // "f" at the end of "60" because I have to use float type and I shouldn't loose fractions
            resEnergy += _factories[0].GetGeneration / 60f;
            resMetal += _factories[1].GetGeneration / 60f;
            resCrystals += _factories[2].GetGeneration / 60f;
            resResearches += _factories[3].GetGeneration / 60f;
            resMoney += _factories[4].GetGeneration / 60f;
            UpdateResources();
        }
    }
}

internal class Factory
{
    public int Level = 1;
    public readonly int TypeId; // 0-energy, 1-metal, 2-crystals, 3-researches, 4-money
    private readonly int _baseGen;
    public readonly int[] BaseUpgradeRes;
    private readonly int _baseUpgradeMoney;

    public int GetGeneration => Level * _baseGen;
    public int GetCost => Level * _baseUpgradeMoney;

    // Just a constructor with initiating the values
    public Factory(int typeId, int baseGen, int[] baseUpgradeRes, int baseUpgradeMoney)
    {
        TypeId = typeId;
        _baseGen = baseGen;
        BaseUpgradeRes = baseUpgradeRes;
        _baseUpgradeMoney = baseUpgradeMoney;
    }
}