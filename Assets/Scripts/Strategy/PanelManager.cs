using UnityEngine;

public class PanelManager : MonoBehaviour
{
    /// Contains image ui element with dark background.
    /// It needed for changing "blockRaycast" property when u open/close panel.
    /// I can't just .SetActive on this object because I use animation
    [SerializeField] private CanvasGroup panelsContainer;

    /// Contains Animator component of panelsContainer
    [SerializeField] private Animator panelsContainerAnimator;

    /// Contains panel objects. Function shows/hide only 1 element in it
    [SerializeField] private GameObject[] panelContents;

    // Just for animator. Rider created it, I don't really know why it is so important
    private static readonly int Visible = Animator.StringToHash("Visible");

    /// Contains number of last opened panel.
    /// It needed for closing only 1 panel instead of deactivating every panel in loop
    private static int _lastNum;

    [SerializeField] private FactoriesController factoriesController;

    public void OpenPanel(int number)
    {
        panelsContainerAnimator.SetBool(Visible, true); // Starts the animation
        panelsContainer.blocksRaycasts = true; // For interactions with panels, not with "buildings" cards in menu
        panelContents[number].SetActive(true); // Activating requested panel
        factoriesController.UpdatePanelStats(number); // Updating values of ui elements on this panel
        _lastNum = number; // Saving for future closing
    }

    public void ClosePanel()
    {
        panelsContainerAnimator.SetBool(Visible, false); // Starts the animation
        panelsContainer.blocksRaycasts = false; // For interactions with "buildings" cards in menu, not with panels
        panelContents[_lastNum].SetActive(false); // Deactivating last opened panel
    }
}