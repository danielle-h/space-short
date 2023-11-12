using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehaviour : MonoBehaviour
{
    public Slider slider;//healthbar slider
    public Color low;
    public Color high;
    public Vector3 Offset;//offset as bosses have different heights

   public void SetHealth(float health, float maxHealth)
    {
        //make healthbar dissapear after death
        slider.gameObject.SetActive(health > 0);
        //update value
        slider.value = health;
        slider.maxValue = maxHealth;
        //Debug.Log("setting health to " + health + " out of " + maxHealth);

        //slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    void Update()
    {
        //follow boss around
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    }
}
