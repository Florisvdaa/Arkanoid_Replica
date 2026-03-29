using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoaderProgress : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loaderText;

    private void Update()
    {
        float progress = Loader.GetLoadingProgress();
        int percent = Mathf.Clamp(Mathf.RoundToInt(progress * 100f), 0, 100);
        loaderText.text = "Loading..." + percent + "%";
    }
}
