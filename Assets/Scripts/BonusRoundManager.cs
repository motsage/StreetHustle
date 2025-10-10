using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusRoundManager : MonoBehaviour
{
    public static BonusRoundManager Instance { get; private set; }

    [Header("Round Settings")]
    public int roundDurationSeconds = 10;   
    public int coinsToSpawn = 20;          

    [Header("References")]
    public GameObject coinPrefab;          
    public TextMeshProUGUI timerText;       

    private List<GameObject> spawnedCoins = new List<GameObject>();
    public bool isActive { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartBonusRound()
    {
        if (isActive) return;
        StartCoroutine(RunBonusRound());
    }

    private IEnumerator RunBonusRound()
    {
        isActive = true;

        SpawnCoins();

        float t = roundDurationSeconds;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            if (timerText) timerText.text = Mathf.CeilToInt(t).ToString();
            yield return null;
        }

        EndBonusRound();
    }

    private void SpawnCoins()
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning("BonusRoundManager: coinPrefab not set.");
            return;
        }

        for (int i = 0; i < coinsToSpawn; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-327f, -156f),  
                1f,                      
                Random.Range(0f, 15f)    
            );

            GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
            spawnedCoins.Add(coin);
        }
    }

    public void EndBonusRound()
    {
        isActive = false;
        if (timerText) timerText.text = "";

        for (int i = spawnedCoins.Count - 1; i >= 0; i--)
        {
            if (spawnedCoins[i] != null) Destroy(spawnedCoins[i]);
        }
        spawnedCoins.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            StartBonusRound();
    }
}
