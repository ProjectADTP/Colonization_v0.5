using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceCountText;
    [SerializeField] private TextMeshProUGUI _unitsCountText;
    [SerializeField] private Button _scanButton;
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private Base _base;

    private void Awake()
    {
        _base.OnBaseClicked += ShowUI;
        _base.ResourceChanged += UpdateUI;

        HideUI();
    }

    private void OnEnable()
    {
        _scanButton.onClick.AddListener(Scan);
    }

    private void Update()
    {
        bool clickedOnUI = EventSystem.current.IsPointerOverGameObject();

        if (Input.GetMouseButtonDown(0) && !clickedOnUI)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit) ||!hit.collider.TryGetComponent<Base>(out _))
            {
                HideUI();
            }
        }
    }

    private void OnDisable()
    {
        _scanButton.onClick.RemoveListener(Scan);
    }

    private void OnDestroy()
    {
        _base.OnBaseClicked -= UpdateUI;
        _base.ResourceChanged -= UpdateUI;
    }

    private void Scan()
    {
        _base.ScanForResources();
    }

    private void UpdateUI()
    {
        _resourceCountText.text = _base.Resources.ToString();
        _unitsCountText.text = _base.Units.ToString();
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void ShowUI()
    {
        gameObject.SetActive(true);
        UpdateUI();
    }
}
