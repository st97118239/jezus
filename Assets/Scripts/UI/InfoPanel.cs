using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    private TMP_Text healthText;
    private TMP_Text coinText;
    private TMP_Text waveText;

    private void Start()
    {
        healthText = transform.Find("HPTextHolder").transform.Find("HPText").GetComponent<TMP_Text>();
        coinText = transform.Find("CoinTextHolder").transform.Find("CoinText").GetComponent<TMP_Text>();
        waveText = transform.Find("WaveTextHolder").transform.Find("WaveText").GetComponent<TMP_Text>();
    }

    public void RedrawHealthBar(int health)
    {
        healthText.text = "Health: " + health.ToString();
    }

    public void RedrawCoinText(int coins)
    {
        coinText.text = "Coins: " + coins.ToString();
    }

    public void RedrawWaveText(int wave, int enemyCount, int enemiesToSpawn)
    {
        enemyCount += enemiesToSpawn;
        waveText.text = "Wave: " + wave.ToString() + "\nEnemies Left: " + enemyCount.ToString();
    }
}
