
using UnityEngine;
using System.Reflection;



public class LoadGameAssets : MonoBehaviour
{
    private static LoadGameAssets i;

    public static LoadGameAssets instance
    {
        get
        {
            if (i == null)
                i = Instantiate(Resources.Load<LoadGameAssets>("GameAssets"));
            return i;
        }
    }

    public Transform damageNumbersPopup;
}



