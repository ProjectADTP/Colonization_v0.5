using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputReader : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    public event Action<Base> BaseClicked;

    private void Update()
    {
        bool clickedOnUI = EventSystem.current.IsPointerOverGameObject();

        if (Input.GetMouseButtonDown(0) && clickedOnUI == false)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit) || !hit.collider.TryGetComponent<Base>(out _))
            {
                BaseClicked?.Invoke(null);
            }
            else if (hit.collider.TryGetComponent(out Base chosenBase))
            {
                BaseClicked?.Invoke(chosenBase);
            }
        }
    }

}