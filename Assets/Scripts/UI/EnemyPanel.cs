using TMPro;
using UnityEngine;

public class EnemyPanel : MonoBehaviour
{
    private TMP_Text enemyNameText;
    private TMP_Text enemyHealthText;

    private void Start()
    {
        enemyNameText = transform.Find("EnemyName").GetComponent<TMP_Text>();
        enemyHealthText = transform.Find("EnemyHP").GetComponent<TMP_Text>();
    }

    public void Activate(EnemyType type, int hp)
    {
        enemyNameText.text = type.ToReadableString();
        enemyHealthText.text = "Health: " + hp.ToString();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
