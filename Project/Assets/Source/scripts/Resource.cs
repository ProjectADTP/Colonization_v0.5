using UnityEngine;

public class Resource: MonoBehaviour
{
    private bool _isReserved;

    public bool IsAvailable => !_isReserved;

    public void Reserve()
    {
        _isReserved = true;
    }
}