using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DamageNumberTesting : MonoBehaviour
{
    
    public static DamageNumberTesting Create(Vector2 position, int damageAmount)
    {
        Transform popupTransform = Instantiate(LoadGameAssets.instance.damageNumbersPopup, position, Quaternion.identity);
        DamageNumberTesting damagePopup = popupTransform.GetComponent<DamageNumberTesting>();
        damagePopup.setUp(damageAmount);

        return damagePopup;
    }
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    [Header("Properties")]
    [SerializeField] private int fontSizeMin = 30;
    [SerializeField] private int fontSizeMax = 36;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void setUp(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textMesh.fontSize = Random.Range(fontSizeMin, fontSizeMax);
        textColor = textMesh.color;
        //UtilClass.GetColorFromString(""); hexvalue
        disappearTimer = 1f;
    }
    private void Update()
    {
        float moveYSpeed = 20f;
        float moveXSpeed = 10f;
        transform.position = new Vector2(moveXSpeed, moveYSpeed) * Time.deltaTime;


        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
