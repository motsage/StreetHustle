using UnityEngine;

public class Delegate : MonoBehaviour
{
    public delegate void MessageDelegate(string message);
    public MessageDelegate onMessage;

    void Start()
    {
        // Subscribe multiple methods
        onMessage += ShowMessage;
        onMessage += LogMessage;

        // Invoke only the first method
        if (onMessage != null)
        {
            onMessage.GetInvocationList()[0].DynamicInvoke("Hello, first method!");
        }

        // Invoke only the second method
        onMessage.GetInvocationList()[1].DynamicInvoke("Hello, second method!");
    }

    void ShowMessage(string msg)
    {
        Debug.Log("ShowMessage: " + msg);
    }

    void LogMessage(string msg)
    {
        Debug.Log("LogMessage: " + msg);
    }
}
