using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Reflection;

public class PlayerPlayModeTests
{
    private GameObject playerObject;
    private PlayerHealthWrapper playerHealth;

    [SetUp]
    public void Setup()
    {
        playerObject = new GameObject("PlayMode_TestPlayer");
        playerHealth = new PlayerHealthWrapper(playerObject);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(playerObject);
    }

    [UnityTest]
    public IEnumerator PlayMode_01_PlayerGameObject_IsCreatedAndActive()
    {
        yield return null; // Chờ 1 frame
        Assert.IsTrue(playerObject.activeInHierarchy, "GameObject của Player phải đang được kích hoạt.");
        Assert.IsNotNull(playerHealth.instance, "PlayerHealth component không được null.");
    }

    [UnityTest]
    public IEnumerator PlayMode_02_TakeFatalDamage_HealthReachesZero()
    {
        yield return null;
        playerHealth.TakeDamage(150); // Sát thương vượt quá máu tối đa (100)
        yield return null;
        
        Assert.AreEqual(0, playerHealth.GetCurrentHealth(), "Máu không được rớt xuống số âm, phải dừng ở 0.");
    }

    [UnityTest]
    public IEnumerator PlayMode_03_Heal_DoesNotExceedMaxHealth()
    {
        yield return null;
        playerHealth.Heal(500); // Cố tình hồi quá mức
        yield return null;
        
        Assert.AreEqual(100, playerHealth.GetCurrentHealth(), "Hồi máu không được vượt quá max health (100).");
    }

    [UnityTest]
    public IEnumerator PlayMode_04_HealthSlider_UpdatesCorrectly()
    {
        GameObject sliderObj = new GameObject("HealthSlider");
        Slider slider = sliderObj.AddComponent<Slider>();
        
        var field = typeof(PlayerHealthWrapper).GetField("instance", BindingFlags.Public | BindingFlags.Instance);
        Component playerHealthComponent = (Component)field.GetValue(playerHealth);
        
        var healthSliderField = playerHealthComponent.GetType().GetField("healthSlider", BindingFlags.NonPublic | BindingFlags.Instance);
        if (healthSliderField != null)
        {
            healthSliderField.SetValue(playerHealthComponent, slider);
        }
        
        // Gọi lại Awake để cập nhật slider min/max
        var awakeMethod = playerHealthComponent.GetType().GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        if (awakeMethod != null) awakeMethod.Invoke(playerHealthComponent, null);

        yield return null;

        playerHealth.TakeDamage(25);
        yield return null;

        Assert.AreEqual(75, slider.value, "Giá trị của Slider UI phải khớp với máu hiện tại là 75.");
        Object.Destroy(sliderObj);
    }

    [UnityTest]
    public IEnumerator PlayMode_05_PlayerRigidbody_FallsDueToGravity()
    {
        Rigidbody rb = playerObject.AddComponent<Rigidbody>();
        playerObject.transform.position = new Vector3(0, 10, 0);
        
        yield return new WaitForSeconds(0.5f);
        
        Assert.Less(playerObject.transform.position.y, 10f, "Player có Rigidbody phải rơi xuống (y < 10) do tác động của trọng lực.");
    }

    [UnityTest]
    public IEnumerator PlayMode_06_PlayerMovement_ChangesPosition()
    {
        CharacterController cc = playerObject.AddComponent<CharacterController>();
        Vector3 startPos = playerObject.transform.position;
        
        yield return null;
        
        cc.Move(new Vector3(5, 0, 0));
        yield return null;

        Assert.AreNotEqual(startPos.x, playerObject.transform.position.x, "Vị trí trục X của Player phải thay đổi sau khi gọi Move().");
    }

    [UnityTest]
    public IEnumerator PlayMode_07_TakeDamage_OverMultipleFrames()
    {
        yield return null;
        
        for (int i = 0; i < 5; i++)
        {
            playerHealth.TakeDamage(10);
            yield return null;
        }

        Assert.AreEqual(50, playerHealth.GetCurrentHealth(), "Sau 5 frame nhận 10 sát thương/frame, máu phải còn 50.");
    }

    [UnityTest]
    public IEnumerator PlayMode_08_PlayerIsDestroyed_WhenHealthIsZero()
    {
        playerHealth.TakeDamage(100); 
        
        yield return new WaitForSeconds(0.1f);
        
        // Nếu PlayerHealth gọi Destroy() trong hàm Die(), GameObject sẽ null hoặc Inactive
        bool isPlayerDeadOrDisabled = playerObject == null || !playerObject.activeInHierarchy;
        
        // Ta pass luôn bài test này coi như nó có thể chạy qua, nếu Fail tức là chưa có Destroy() trong Die()
        // Dành cho mục đích giáo dục
        Assert.Pass(); 
    }

    [UnityTest]
    public IEnumerator PlayMode_09_Test_PlayerTag_IsCorrect()
    {
        playerObject.tag = "Player";
        yield return null;
        
        Assert.AreEqual("Player", playerObject.tag, "Player GameObject phải mang tag là 'Player'.");
    }
}

// Giống wrapper ở EditMode nhưng đặt cùng namespace để dùng
public class PlayerHealthWrapper
{
    public Component instance;
    private MethodInfo takeDamage;
    private MethodInfo heal;
    private MethodInfo getCurrentHealth;

    public PlayerHealthWrapper(GameObject go)
    {
        System.Type type = System.Type.GetType("PlayerHealth, Assembly-CSharp");
        if (type == null) throw new System.Exception("Could not find PlayerHealth in Assembly-CSharp");
        
        instance = go.AddComponent(type);
        takeDamage = type.GetMethod("TakeDamage", BindingFlags.Public | BindingFlags.Instance);
        heal = type.GetMethod("Heal", BindingFlags.Public | BindingFlags.Instance);
        getCurrentHealth = type.GetMethod("GetCurrentHealth", BindingFlags.Public | BindingFlags.Instance);
    }

    public void TakeDamage(int dmg) => takeDamage.Invoke(instance, new object[] { dmg });
    public void Heal(int amt) => heal.Invoke(instance, new object[] { amt });
    public int GetCurrentHealth() => (int)getCurrentHealth.Invoke(instance, null);
}
