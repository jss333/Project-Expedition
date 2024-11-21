using AbilitySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDebugButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilityTextName;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private bool isActive;

    private int abilityIndex;

    private void Start()
    {
        isActive = false;
    }

    public void Initialize(int index)
    {
        abilityIndex = index;

        abilityTextName.text = AbilityManager.singleton.AvailableAbilities[abilityIndex].name;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            OnButtonClick();
            AbilityManager.singleton.OnAbilityEnabledAndDisabled?.Invoke(index, isActive);
        });
    }

    private void OnButtonClick()
    {
        isActive = !isActive;

        if(isActive)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = Color.red;
        }
    }
}
