using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityToggleUI : MonoBehaviour
{
    private List<AbilityButton> buttonList = new List<AbilityButton>();
    private DebugMenuUI menuUI;
    private bool shieldAbilityRed = true;
    private bool shieldAbilityBlue = true;
    private bool doubleJump = true;

    public bool ShieldAbilityRed { get => shieldAbilityRed; set => shieldAbilityRed = value; }
    public bool ShieldAbilityBlue { get => shieldAbilityBlue; set => shieldAbilityBlue = value; }
    public bool DoubleJump { get => doubleJump; set => doubleJump = value; }

    private void Start()
    {
        menuUI = this.GetComponent<DebugMenuUI>();
        buttonList.Clear();
        buttonList = menuUI.abilityButtons;
    }

    public void manageVariableChanges()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            //Debug.Log(buttonList[i].abilityName);
            switch(buttonList[i].abilityName)
            {
                case "ReflectShieldBlue":
                    shieldAbilityBlue = buttonList[i].getActive();
                    //Debug.Log(buttonList[i].getActive());
                    break;
                case "ReflectShieldRed":
                    shieldAbilityRed = buttonList[i].getActive();
                    //Debug.Log(buttonList[i].getActive());
                    break;
                case "DoubleJump":
                    doubleJump = buttonList[i].getActive();
                    //Debug.Log(buttonList[i].getActive());
                    break;
            }
        }
    }
}
