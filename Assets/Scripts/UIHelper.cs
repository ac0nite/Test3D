using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IUIHelper
{
    event Action OnPlayButtonPressed;
    event Action<BehaviourType> OnChangeEngineBehaviour;
    void SetActive(string message);
}

public class UIHelper : MonoBehaviour, IUIHelper
{
    [SerializeField] private Button _playButton;
    [SerializeField] private TMP_Text _helperText;
    [SerializeField] private TMP_Dropdown _engineBrhaviourDropdown;

    public event Action OnPlayButtonPressed;
    public event Action<BehaviourType> OnChangeEngineBehaviour;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _playButton.onClick.AddListener(PlayButtonPressed);
        InitDropdown();
    }

    private void InitDropdown()
    {
        _engineBrhaviourDropdown.ClearOptions();
        _engineBrhaviourDropdown.AddOptions(Enum.GetNames(typeof(BehaviourType)).ToList());
        _engineBrhaviourDropdown.RefreshShownValue();
        _engineBrhaviourDropdown.value = (int) BehaviourType.SIMPLE;
        
        _engineBrhaviourDropdown.onValueChanged.AddListener(ChangedValueDropdown);
    }

    private void ChangedValueDropdown(int value)
    {
        OnChangeEngineBehaviour?.Invoke((BehaviourType) value);
    }

    private void PlayButtonPressed()
    {
        _canvas.enabled = false;
        OnPlayButtonPressed?.Invoke();
    }

    public void SetActive(string message)
    {
        _helperText.text = message;
        _canvas.enabled = true;
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(PlayButtonPressed);
        _engineBrhaviourDropdown.onValueChanged.RemoveListener(ChangedValueDropdown);
    }
}
