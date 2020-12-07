using UnityEngine;

public class CurrentInitializer : MonoBehaviour
{
    public WaterCurrent[] waterCurrents;
    public TankCurrent tankCurrent;

    void Start() {
        //Debug.Log("Adjusting the currents");
        foreach (var c in waterCurrents) {
            c.GetComponent<BoxCollider2D>().size = new Vector3(1500, 200);
        }
        tankCurrent.GetComponent<BoxCollider2D>().size = new Vector3(2000, 2500);
    }
    void Update() {
    }
}//end of CurrentInitializer
