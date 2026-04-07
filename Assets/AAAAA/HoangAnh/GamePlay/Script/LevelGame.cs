using System.Collections;
using System.Collections.Generic; // Cần cái này để dùng List
using UnityEngine;

public class CaveSpawner : MonoBehaviour
{
    [Header("Setup Caves (Kéo 4 cái hang vào đây)")]
    public Transform[] caves;

    [Header("Setup Enemies (Kéo 3 loại quái vào đây)")]
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject enemyType3;

    [Header("Settings")]
    public float delayBetweenSpawns = 0.5f;
    public float timeBetweenLevels = 3f; // Thời gian chờ giữa các Level

    private int currentLevel = 0;
    private bool isSpawning = false;
    private List<GameObject> activeEnemies = new List<GameObject>(); // Danh sách quản lý quái đang sống

    void Start()
    {
        // Vừa vào game, đợi 3 giây rồi bắt đầu Level 1
        StartCoroutine(AutoGameFlow());
    }

    // Luồng xử lý chính của Game
    private IEnumerator AutoGameFlow()
    {
        while (currentLevel < 5)
        {
            yield return new WaitForSeconds(timeBetweenLevels);
            currentLevel++;

            isSpawning = true;
            yield return StartCoroutine(SpawnWaveLogic(currentLevel));
            isSpawning = false;

            // Sau khi đẻ xong, đợi cho đến khi danh sách quái trống rỗng
            Debug.Log("Đang đợi người chơi quét sạch quái Level " + currentLevel);
            yield return new WaitUntil(() => AllEnemiesDead());

            Debug.Log("Sạch bóng quân thù! Chuẩn bị sang Level tiếp theo...");
        }

        Debug.Log("CHÚC MỪNG! MÀY ĐÃ VƯỢT QUA TẤT CẢ LEVEL.");
    }

    private bool AllEnemiesDead()
    {
        // Dọn dẹp danh sách: xóa những con quái đã bị Destroy (null)
        activeEnemies.RemoveAll(item => item == null);

        // Nếu đẻ xong rồi và danh sách trống thì trả về true
        return !isSpawning && activeEnemies.Count == 0;
    }

    private IEnumerator SpawnWaveLogic(int level)
    {
        Debug.Log("== BẮT ĐẦU LEVEL " + level + " ==");

        switch (level)
        {
            case 1:
                yield return StartCoroutine(SpawnEnemies(enemyType1, 10));
                break;
            case 2:
                yield return StartCoroutine(SpawnEnemies(enemyType1, 5));
                yield return StartCoroutine(SpawnEnemies(enemyType2, 5));
                break;
            case 3:
                yield return StartCoroutine(SpawnEnemies(enemyType1, 5));
                yield return StartCoroutine(SpawnEnemies(enemyType2, 5));
                yield return new WaitForSeconds(10f);
                yield return StartCoroutine(SpawnEnemies(enemyType3, 5));
                break;
            case 4:
                yield return StartCoroutine(SpawnEnemies(enemyType1, 10));
                yield return StartCoroutine(SpawnEnemies(enemyType2, 5));
                yield return new WaitForSeconds(10f);
                yield return StartCoroutine(SpawnEnemies(enemyType3, 5));
                break;
            case 5:
                yield return StartCoroutine(SpawnEnemies(enemyType1, 10));
                yield return StartCoroutine(SpawnEnemies(enemyType2, 10));
                yield return new WaitForSeconds(10f);
                yield return StartCoroutine(SpawnEnemies(enemyType3, 10));
                break;
        }
    }

    private IEnumerator SpawnEnemies(GameObject enemyPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (caves.Length == 0) yield break;

            int randomCaveIndex = Random.Range(0, caves.Length);
            Transform spawnPoint = caves[randomCaveIndex];

            // Tạo quái và cho nó vào danh sách quản lý
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            activeEnemies.Add(newEnemy);

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}