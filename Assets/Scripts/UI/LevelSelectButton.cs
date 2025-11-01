using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelSelectButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] string sceneName;

    [SerializeField] TextMeshProUGUI currentRecordText;

    [SerializeField] Image levelPreviewRenderer;

    [SerializeField] Sprite levelPreview;

    private Button button;

    public void OnSelect(BaseEventData eventData)
    {
        float savedRecordTime = SaveAndLoadMethods.LoadLevelTime(sceneName);

        //If the saved record time is -1, then there is no current record.
        if (savedRecordTime < 0)
            currentRecordText.text = "99:99.99";
        else
            currentRecordText.text = ConvertFloatTimeToString(savedRecordTime);

        levelPreviewRenderer.sprite = levelPreview;
    }

    void Awake()
    {
        button = GetComponent<Button>();
    }

    private string ConvertFloatTimeToString(float time)
    {
        return TimeSpan.FromSeconds(time).ToString("mm':'ss'.'ff");
    }
}
