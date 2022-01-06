using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
	[SerializeField]
	Slider refreshRateSlider;
	[SerializeField]
	TMP_Text refreshRateValueText;
	[SerializeField]
	TMP_Dropdown fullscreenDropdown;

	bool limitFps;
    int maxRefreshRate;
	int oldLimit;

    void Start()
	{
		fullscreenDropdown.value = (int)Screen.fullScreenMode;
		GetMaxRefreshRate();
		limitFps = true;
		refreshRateSlider.value = maxRefreshRate;
		gameObject.SetActive(false);
	}

	public void ToggleFullscreen(int index)
	{
        Screen.fullScreenMode = (FullScreenMode)index;
		GetMaxRefreshRate();
	}

	private void GetMaxRefreshRate()
	{
		foreach (var item in Screen.resolutions)
		{
			if (item.refreshRate > maxRefreshRate)
			{
				maxRefreshRate = item.refreshRate;
			}
		}
		if (maxRefreshRate < 60)
			maxRefreshRate = 60;
		refreshRateSlider.maxValue = maxRefreshRate;
		if (refreshRateSlider.value > maxRefreshRate)
		{
			refreshRateSlider.value = maxRefreshRate;
		}
	}

	public void ToggleVSync(bool value)
	{
		limitFps = value;
		if (value && oldLimit != 0)
		{
			LimitFramerate(oldLimit);
		}
		else
		{
			if (Application.targetFrameRate > 0)
			{
				oldLimit = Application.targetFrameRate; 
			}
			LimitFramerate(0);
		}
	}

	public void LimitFramerate(float limit)
	{
		int value = Mathf.RoundToInt(limit);
		if (limitFps)
		{
			if (value > maxRefreshRate)
			{
				value = maxRefreshRate;
				refreshRateSlider.value = value;
				return;
			}
		}
		else value = 0;
		refreshRateValueText.text = value.ToString();
		Application.targetFrameRate = value;
	}
}
