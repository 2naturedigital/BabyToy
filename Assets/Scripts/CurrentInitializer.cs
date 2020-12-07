using UnityEngine;

public class CurrentInitializer : MonoBehaviour
{
    public WaterCurrent[] waterCurrents;
    public TankCurrent tankCurrent;
    public BoxCollider2D[] colliders;
    public BoxCollider2D tankCollider;

    void Start() {
        foreach (var c in waterCurrents) {
        }
        foreach (var col in colliders) {
            Debug.Log("DOING SOME STUFF PLEASE SEEEEE MEEEEEEEEEE");
            //col.size = new Vector3(1500, 200);
            col.bounds.Expand(2.5f);
            //col.size.Scale(new Vector2(12, 12));
        }
        tankCollider.size = new Vector3(2000, 2500);
    }
    void Update() {
    }
}//end of CurrentInitializer
