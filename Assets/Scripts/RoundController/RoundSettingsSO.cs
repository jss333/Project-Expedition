using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RoundSettings", menuName = "Round System/Round Settings")]
public class RoundSettingsSO : ScriptableObject
{
    [Header("Boss")]
    [SerializeField] private int bossHealth;

    [Range(0, 100)]
    [SerializeField] private int bossStunBulletChance;

    [SerializeField] private int numMinionsToSpawn;


    [Header("Minions")]
    [Range(0, 100)]
    [SerializeField] private int minionStunBulletChance;


    [Header("Player")]
    [SerializeField] private int playerHealth;


    [Header("Level")]
    [SerializeField] private string currentLevel;
    [SerializeField] private string nextLevel;

    public int BossHealth => bossHealth;
    public int BossStunBulletChance => bossStunBulletChance;
    public int NumMinionsToSpawn => numMinionsToSpawn;
    public int MinionStunBulletChance => minionStunBulletChance;
    public int PlayerHealth => playerHealth;
    public string CurrentLevel => currentLevel;
    public string NextLevel => nextLevel;


    public void SetCurrentLevelName(string currentSceneName)
    {
        currentLevel = currentSceneName;
    }
}
