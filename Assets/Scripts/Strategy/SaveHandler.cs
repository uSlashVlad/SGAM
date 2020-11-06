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
        "factory_money"
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
}