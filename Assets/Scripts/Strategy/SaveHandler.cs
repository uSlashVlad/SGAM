using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] private string[] saveKeys =
    {
        "res_energy",
        "res_metal",
        "res_crystals",
        "res_researches",
        "res_money",
        "factory_energy",
        "factory_metal",
        "factory_crystals",
        "factory_researches",
        "factory_money",
        "component_gears",
        "component_batteries",
        "component_tools",
        "component_lasercores",
        "component_opteyes",
        "component_unicomp"
    };

    public float[] LoadResValue()
    {
        var result = new float[5];
        for (var i = 0; i < 5; i++)
        {
            result[i] = PlayerPrefs.GetFloat(saveKeys[i]);
        }

        return result;
    }

    public void StoreResValues(float[] values)
    {
        for (var i = 0; i < values.Length; i++)
        {
            PlayerPrefs.SetFloat(saveKeys[i], values[i]);
        }
    }

    public int[] LoadFactoryLevels()
    {
        var result = new int[5];
        for (var i = 0; i < 5; i++)
        {
            result[i] = PlayerPrefs.GetInt(saveKeys[i + 5]);
        }

        return result;
    }

    public void StoreFactoryLevel(int number, int amount)
    {
        PlayerPrefs.SetInt(saveKeys[5 + number], amount);
    }

    public int[] LoadComponentAmounts()
    {
        var result = new int[6];
        for (var i = 0; i < 6; i++)
        {
            result[i] = PlayerPrefs.GetInt(saveKeys[i + 10]);
        }

        return result;
    }

    public void StoreComponentAmount(int number, int amount)
    {
        PlayerPrefs.SetInt(saveKeys[10 + number], amount);
    }
}