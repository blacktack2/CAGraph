using UnityEngine;
using TMPro;

public class StatsPanelText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _Display;

    [SerializeField]
    private CellGrid _CellGrid;

    public enum DisplayMode { FPS, MS }
    [SerializeField]
    private DisplayMode _DisplayMode = DisplayMode.FPS;

    [SerializeField, Range(0.1f, 2f)]
    private float _SampleDuration = 1f;

    private int _Frames, _MaxCount = 0;
    private float _Duration, _BestDuration = float.MaxValue, _WorstDuration;

    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        _Frames += 1;
        _Duration += frameDuration;

        if (frameDuration < _BestDuration)
            _BestDuration = frameDuration;
        if (frameDuration > _WorstDuration)
            _WorstDuration = frameDuration;

        if (_Duration > _SampleDuration)
        {
            _MaxCount = Mathf.Max(_MaxCount, _CellGrid.cellCount);
            string fpsText;
            if (_DisplayMode == DisplayMode.FPS)
                fpsText = string.Format("FPS\nBest:{0:0}\nAvrg:{1:0}\nWrst:{2:0}", 1f / _BestDuration, _Frames / _Duration, 1f / _WorstDuration);
            else
                fpsText = string.Format("MS\nBest{0:1}\nAvrg:{1:1}\nWrst{2:1}", 1000f * _BestDuration, 1000f * _Duration / _Frames, 1000f * _WorstDuration);
            string countText = string.Format("NumCells:{0}\nMaxCells:{1}", _CellGrid.cellCount, _MaxCount);
            _Display.SetText(fpsText + "\n\n" + countText);
            _Frames = 0;
            _Duration = 0f;
            _BestDuration = float.MaxValue;
            _WorstDuration = 0f;
        }
    }
}
