using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
	[SerializeField]
	private Slider slider;
	[SerializeField]
	private Gradient gradient;
	[SerializeField]
	private Image fill;

	public void SetMaxFuel(int fuel)
	{
		slider.maxValue = fuel;
		slider.value = fuel;

		fill.color = gradient.Evaluate(1f);
	}

    public void DecreaseFuel(float amount)
	{
		slider.value -= amount;
		fill.color = gradient.Evaluate(slider.normalizedValue);
	}
}