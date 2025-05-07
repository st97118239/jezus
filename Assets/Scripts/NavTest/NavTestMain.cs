using UnityEngine;
using TMPro;

public class NavTestMain : MonoBehaviour
{
    public int health = 100;
    public TMP_Text healthText;
    public PawnSpawnerTest pst;
    public bool isDead = false;

    public void ReceiveDmg(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            health = 0;
            GameOver();
        }

        healthText.text = "" + health.ToString();
    }

    private void GameOver()
    {
        print("ded");
        isDead = true;
        pst.canSpawn = false;
        foreach (GameObject pawn in pst.activePawns)
        {
            pawn.GetComponent<NavigationTest>().canMove = false;
        }
    }
}
