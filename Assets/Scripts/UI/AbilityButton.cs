using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public string abilityName;
    public bool isActive;
    public Image image;
    public Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        isActive = false;
    }
    public bool getActive()
    {
        return isActive;
    }
}
