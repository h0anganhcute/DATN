using NUnit.Framework;
using UnityEngine;
using System.Reflection;

public class PlayerEditModeTests
{
    private GameObject playerObject;
    private PlayerHealthWrapper playerHealth;

    [SetUp]
    public void Setup()
    {
        playerObject = new GameObject("TestPlayer");
        playerHealth = new PlayerHealthWrapper(playerObject);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerObject);
    }

    [Test]
    public void EditMode_01_PlayerStarts_WithMaxHealth()
    {
        Assert.AreEqual(100, playerHealth.GetCurrentHealth(), "Máu khởi đầu phải là 100.");
    }

    [Test]
    public void EditMode_02_TakeDamage_ReducesHealth()
    {
        playerHealth.TakeDamage(20);
        Assert.AreEqual(80, playerHealth.GetCurrentHealth(), "Máu phải giảm xuống 80 khi nhận 20 sát thương.");
    }

    [Test]
    public void EditMode_03_TakeDamage_NegativeValue_IsIgnored()
    {
        playerHealth.TakeDamage(-50);
        Assert.AreEqual(100, playerHealth.GetCurrentHealth(), "Sát thương âm không được làm tăng máu.");
    }

    [Test]
    public void EditMode_04_Heal_IncreasesHealth()
    {
        playerHealth.TakeDamage(50);
        playerHealth.Heal(30);
        Assert.AreEqual(80, playerHealth.GetCurrentHealth(), "Máu phải tăng lên 80 khi được hồi 30.");
    }

    [Test]
    public void EditMode_05_Heal_NegativeValue_IsIgnored()
    {
        playerHealth.TakeDamage(50);
        playerHealth.Heal(-20);
        Assert.AreEqual(50, playerHealth.GetCurrentHealth(), "Hồi máu âm phải bị bỏ qua.");
    }

    [Test]
    public void EditMode_06_ZeroDamageAndHeal_NoChange()
    {
        playerHealth.TakeDamage(0);
        Assert.AreEqual(100, playerHealth.GetCurrentHealth());
        
        playerHealth.TakeDamage(20);
        playerHealth.Heal(0);
        Assert.AreEqual(80, playerHealth.GetCurrentHealth(), "Damage hoặc Heal bằng 0 không được thay đổi máu.");
    }
}

// Wrapper dùng Reflection để tránh lỗi thiếu Assembly Definition reference trong Unity
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
        
        // Gọi Awake thủ công trong EditMode nếu cần (tránh lỗi slider null)
        var awakeMethod = type.GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        if (awakeMethod != null) awakeMethod.Invoke(instance, null);
    }

    public void TakeDamage(int dmg) => takeDamage.Invoke(instance, new object[] { dmg });
    public void Heal(int amt) => heal.Invoke(instance, new object[] { amt });
    public int GetCurrentHealth() => (int)getCurrentHealth.Invoke(instance, null);
}
