using TMPro;
using UnityEngine;

public class UnitPanel : MonoBehaviour
{
    private TMP_Text unitNameText;
    private TMP_Text unitHealthText;

    private void Awake()
    {
        unitNameText = transform.Find("UnitName").GetComponent<TMP_Text>();
        unitHealthText = transform.Find("UnitHP").GetComponent<TMP_Text>();
    }

    public void Activate(UnitType type, int hp)
    {
        unitNameText.text = type.ToReadableString();
        unitHealthText.text = "Health: " + hp.ToString();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
