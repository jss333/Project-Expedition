using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightShadingEffect : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color normalcolor = Color.white;

    private List<Material> materials;

    private void Awake()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }


    public void FlashOnImapct()
    {
        StartCoroutine(FlashEntity());
    }


    private void ToggleHighlight(bool val)
    {
        if (val)
        {
            foreach (var material in materials)
            {
                //material.EnableKeyword("_EMISSION");
                material.SetColor("_Color", damageColor);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                material.SetColor("_Color", normalcolor);
            }
        }

    }

    IEnumerator FlashEntity()
    {
        ToggleHighlight(true);

        yield return new WaitForSeconds(0.1f);

        ToggleHighlight(false);
    }
}
