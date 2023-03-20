using UnityEngine;

[CreateAssetMenu(fileName = "Desc", menuName = "LevelDescriptor", order = 0)]
public class LevelDescriptor : ScriptableObject {
    [SerializeField] private int enemiesCount;
    [SerializeField] private int enemiesLevel;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemySpawnInterval;

    public int EnemiesCount => enemiesCount;
    public int EnemiesLevel => Random.Range(1, enemiesLevel + 1);
    public float EnemySpeed => enemySpeed;
    public float EnemySpawnInterval => enemySpawnInterval;
}