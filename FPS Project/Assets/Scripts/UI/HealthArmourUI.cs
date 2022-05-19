using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthArmourUI : MonoBehaviour
{
    const int maxHealth = 300, maxArmour = 300;

    [Range(0f, maxHealth)] public int health = 300;
    [Range(0f, maxArmour)] public int armour = 200;

    public Slider healthSlider;
    public Slider armourSlider;

    public TextMeshProUGUI healthArmourText;

    [SerializeField] int lerpSpeed;

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
        armourSlider.maxValue = maxArmour;

        healthSlider.value = health;
        armourSlider.value = armour;
    }

    void Update()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, health, lerpSpeed * Time.deltaTime);
        armourSlider.value = Mathf.Lerp(armourSlider.value, armour, lerpSpeed * Time.deltaTime);

        healthArmourText.text = $"<size=144><mspace=66>{health}</mspace><size=122> <size=100>/ <mspace=47>{armour}";
    }
}
