using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth singleton;
    public float currentHealth;
    public float maxHealth = 100f;
    public bool isDead = false;
    public Slider healthSlider;
    public Text healthCounter;

    [Header("Damage Screen")]
    public Color damageColor;
    public Image damegeImage;
    float colorSmoothing = 6;
    bool isTakingDamage = false;

    private void Awake()
    {
        singleton = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.value = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        if (isTakingDamage)
        {
            damegeImage.color = damageColor; 
        }
        else
        {
            damegeImage.color = Color.Lerp(damegeImage.color, Color.clear, colorSmoothing * Time.deltaTime);
        }
        isTakingDamage = false;
    }

    public void DamgePlayer(float damage)
    {
        if (currentHealth > 0) {
            if (damage >= currentHealth)
            {
                isTakingDamage = true;
                PlayerDead();
            }
            else
            {
                isTakingDamage = true;
                currentHealth -= damage;
               
            }
            UpdateHealthUI();
        }
    }

    public void AddHealth(float healthAmmount)
    {
        currentHealth += healthAmmount;
        UpdateHealthUI();
    }

    void PlayerDead()
    {
        isDead = true;
        healthSlider.value = 0;
        UpdateHealthUI();
        currentHealth = 0;
    }

    public void UpdateHealthUI()
    {
        healthCounter.text = currentHealth.ToString();
        healthSlider.value -= currentHealth;
    }
}
