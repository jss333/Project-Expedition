using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DamageNumberTesting : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    [Header("Properties")]
    [SerializeField] private int fontSizeMin = 7;
    [SerializeField] private int fontSizeMax = 10;
    private float moveYSpeed = 1f;
    private float moveXSpeed = 1f;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void setUpNum(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textMesh.fontSize = Random.Range(fontSizeMin, fontSizeMax);
        textColor = textMesh.color;

        if (Random.Range(1, 3) == 1)
        {
            textColor = Color.red;
            textMesh.color = textColor; 
        }
        //UtilClass.GetColorFromString(""); hexvalue
        disappearTimer = 1f;
    }
    public void setUpString(string damageAmount)
    {
        textMesh.SetText(damageAmount);
        textMesh.fontSize = Random.Range(fontSizeMin, fontSizeMax);
        textColor = Color.cyan;
        textMesh.color = textColor;
        this.transform.localScale = new Vector3(.5f, .5f);
        
        //UtilClass.GetColorFromString(""); hexvalue
        disappearTimer = 1f;
    }
    private void Update()
    {
        
        transform.position += new Vector3(moveXSpeed, moveYSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 5f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
