using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Slider slider;

    private void Start()
    {
    }
    private void Update()
    {
        slider.maxValue = playerHealth.startingHealth;
        slider.value = playerHealth.currentHealth;
    }
}
