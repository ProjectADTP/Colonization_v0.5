using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseStatsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceCountText;
    [SerializeField] private TextMeshProUGUI _unitsCountText;
    [SerializeField] private Button _scanButton;
    [SerializeField] private InputReader _inputReader;

    private Base _base;
    private Storage _storage;

    private void Awake()
    {
        _inputReader.BaseClicked += Show;

        Hide();
    }

    private void OnEnable()
    {
        _scanButton.onClick.AddListener(Scan);
    }

    private void OnDisable()
    {
        _scanButton.onClick.RemoveListener(Scan);
    }

    private void OnDestroy()
    {
        _inputReader.BaseClicked -= Show;
    }

    private void Scan()
    {
        _base.StartScan();
    }

    private void UpdateInfo()
    {
        _resourceCountText.text = _storage.Resources.ToString();
        _unitsCountText.text = _base.Units.ToString();
    }

    private void Hide()
    {
        if (_storage != null) 
        {
            _storage.ResourceChanged -= UpdateInfo;
        }
        
        gameObject.SetActive(false);

        _storage = null;
        _base = null;
    }

    private void Show(Base chosenBase)
    {
        if (chosenBase == null) 
        {
            Hide();

            return;
        }

        _base = chosenBase;

        if (_base.TryGetComponent(out Storage storage))
        {
            _storage = storage;
        }

        if (gameObject.activeSelf == false && _base != null)
        {
            gameObject.SetActive(true);

            _storage.ResourceChanged += UpdateInfo;

            UpdateInfo();
        }
    }
}
