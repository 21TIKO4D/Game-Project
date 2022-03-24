using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
	[SerializeField]
	private Gradient gradient;
	[SerializeField]
	private Image fill;
	[SerializeField]
	private Slider slider;

	public void SetMaxFuel(int fuel)
	{
		slider.maxValue = fuel;
		slider.value = fuel;

		fill.color = gradient.Evaluate(1f);
	}

    public void DecreaseFuel(float amount)
	{
		Debug.Log("fuel: " + amount);
		slider.value -= amount;
		fill.color = gradient.Evaluate(slider.normalizedValue);
	}
}