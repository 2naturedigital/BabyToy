using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    private Vector3 dir;
    private Vector3 changes;
    private Vector3 prevreading;
    public int bounds;
    void onStart () {
       Debug.Log("initialized");
       dir = Vector3.zero;
       dir.x = Input.acceleration.x;
       dir.y = Input.acceleration.y;
       dir.z = Input.acceleration.z;

       changes = Vector3.zero;
       prevreading = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
       // grab changes since last update
       changes.x = Mathf.Abs(prevreading.x - dir.x);
       changes.y = Mathf.Abs(prevreading.y - dir.y);
       changes.z = Mathf.Abs(prevreading.z - dir.z);

       Debug.Log("Changes x: " + changes.x);
       Debug.Log("Changes y: " + changes.y);
       Debug.Log("Changes z: " + changes.z);

       // update previousreading with current
       prevreading = dir;

      // shake when bounds out
       if ((changes.x > bounds && changes.y > bounds) || (changes.x > bounds && changes.z > bounds) || (changes.y > bounds && changes.z > bounds)) {
            Debug.Log("Shake Happened!");
       }
    }
}
