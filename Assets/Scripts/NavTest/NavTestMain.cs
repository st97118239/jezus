using UnityEngine;
using TMPro;

public class NavTestMain : MonoBehaviour
{
    public int health = 100;
    public TMP_Text healthText;
    public PawnSpawnerTest pst;

    public void ReceiveDmg(int dmg)
    {
        health -= dmg;

        healthText.text = "" + health.ToString();

        if (health <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        print("ded");
        pst.canSpawn = false;
    }
}
