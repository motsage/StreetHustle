using UnityEngine;

public class SimpleDelegate : MonoBehaviour
{

    //Define delegate type
    public delegate void myDelegate(string message);

    // create a delegate instance
    public myDelegate msgDelegate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Assign our method to the delegate
        msgDelegate = PrintMessage;
        msgDelegate += PrintSecondMessage;
   
        msgDelegate.Invoke("ctu training");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintMessage(string msg)
    {
        Debug.Log("Print message function: " + msg);
    }
    void PrintSecondMessage(string msg)
    {
        Debug.Log("Print message function: " + msg.ToUpper());
    }

}
