using UnityEngine.Events;
public static class GoldManager
{
    static float _currentGold;
    public static UnityEvent goldChanged = new UnityEvent();

    public static float GetCurrentGold()
    {
        return _currentGold;
    }

    public static void AddGold(float amount)
    {
        _currentGold += amount;
        LogChanges();
    }

    public static bool SpendGold(float amount)
    {
        if (_currentGold - amount < 0)
        {
            return false;
        }
        else
        {
            _currentGold -= amount;
            LogChanges();
            return true;
        }
    }

    static void LogChanges()
    {
        goldChanged.Invoke();
    }
}
