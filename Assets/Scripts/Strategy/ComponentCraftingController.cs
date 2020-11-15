using UnityEngine;
using UnityEngine.UI;

public class ComponentCraftingController : MonoBehaviour
{
    public int currentComponent = -1;

    [SerializeField] private string[] componentNames;

    [SerializeField] private int[] componentAmounts;

    private readonly int[,] _componentMaterials =
    {
        {1, 3, 0, 0, 5},
        {5, 2, 0, 0, 7},
        {1, 4, 0, 0, 7},
        {4, 1, 3, 0, 10},
        {5, 1, 5, 1, 15},
        {4, 4, 4, 1, 20}
    };

    private readonly string[] _materialNames =
    {
        "energy", "metal", "crystals", "research", "money"
    };

    [SerializeField] private Text componentDescriptionElement;
    [SerializeField] private GameObject buttonGameObject;

    [SerializeField] private SaveHandler saveHandler;
    [SerializeField] private FactoriesController factoriesController;

    private void Start()
    {
        SelectComponent(-1);
        componentAmounts = saveHandler.LoadComponentAmounts();
    }

    public void SelectComponent(int number)
    {
        if (number == -1)
        {
            componentDescriptionElement.gameObject.SetActive(false);
            buttonGameObject.SetActive(false);
        }
        else
        {
            componentDescriptionElement.gameObject.SetActive(true);
            buttonGameObject.SetActive(true);

            var description = componentNames[number] + "\n\n";
            for (var i = 0; i < 5; i++)
            {
                var tempAmount = _componentMaterials[number, i];
                if (tempAmount != 0)
                {
                    description += tempAmount + " " + _materialNames[i];
                    if (i != 4) description += ", ";
                }
            }

            description += " required to craft this component\n\n";
            description += $"Now you have {componentAmounts[number]} of this components";

            componentDescriptionElement.text = description;
        }

        currentComponent = number;
    }

    public void TryUpgrade()
    {
        if (factoriesController.resEnergy >= _componentMaterials[currentComponent, 0]
            && factoriesController.resMetal >= _componentMaterials[currentComponent, 1]
            && factoriesController.resCrystals >= _componentMaterials[currentComponent, 2]
            && factoriesController.resResearches >= _componentMaterials[currentComponent, 3]
            && factoriesController.resMoney >= _componentMaterials[currentComponent, 4])
        {
            factoriesController.resEnergy -= _componentMaterials[currentComponent, 0];
            factoriesController.resMetal -= _componentMaterials[currentComponent, 1];
            factoriesController.resCrystals -= _componentMaterials[currentComponent, 2];
            factoriesController.resResearches -= _componentMaterials[currentComponent, 3];
            factoriesController.resMoney -= _componentMaterials[currentComponent, 4];
            factoriesController.StoreResources();
            factoriesController.UpdateResources();

            saveHandler.StoreComponentAmount(currentComponent, ++componentAmounts[currentComponent]);
            SelectComponent(currentComponent);
        }
    }
}