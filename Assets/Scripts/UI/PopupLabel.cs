using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PopupLabel : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int fontSizeMin = 7;
    [SerializeField] private int fontSizeMax = 10;
    [SerializeField] private float alternateColorProbability = 0.5f;
    [SerializeField] private Color alternateColor = Color.red;
    [SerializeField] private Vector2 moveSpeed = Vector2.one;
    [SerializeField] private float startFadingTimerSec = 1f;
    [SerializeField] private float fadeSpeed = 5f;

    private TextMeshPro textMesh;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        textMesh.fontSize = Random.Range(fontSizeMin, fontSizeMax);
        if (Random.value < alternateColorProbability)
        {
            textMesh.color = alternateColor;
        }
        textColor = textMesh.color;
    }

    public void UpdateLabel(string strLabel)
    {
        textMesh.SetText(strLabel);
    }

    private void Update()
    {
        transform.position += new Vector3(moveSpeed.x, moveSpeed.y) * Time.deltaTime;

        startFadingTimerSec -= Time.deltaTime;
        if(startFadingTimerSec < 0)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
