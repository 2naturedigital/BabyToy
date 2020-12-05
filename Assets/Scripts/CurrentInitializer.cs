using UnityEngine;

public class CurrentInitializer : MonoBehaviour
{
    public WaterCurrent[] waterCurrent;
    public TankCurrent tankCurrent;
    public BoxCollider2D col;

    void Start() {
        foreach (var c in waterCurrent) {
            Debug.Log("DOING SOME STUFF PLEASE SEEEEE MEEEEEEEEEE");
            col = c.GetComponent<BoxCollider2D>();
            col.size.Set(1500, 200);
            col.size.Scale(new Vector2(2, 2));
        }
        //tankCurrent;
    }

    void Update() {
    }
}//end of CurrentInitializer
