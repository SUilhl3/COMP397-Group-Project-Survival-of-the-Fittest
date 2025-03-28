using UnityEngine;

public class Instance : MonoBehaviour
{

    public static Instance InstanceObject {  get; private set; }

    public jug j;
    public speed s;
    public deadshot d;
    public doubleTap dt;
    public quickRevive q;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        j = GetComponent<jug>();
        s = GetComponent<speed>();
        d = GetComponent<deadshot>();
        dt = GetComponent<doubleTap>();
        q = GetComponent<quickRevive>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
