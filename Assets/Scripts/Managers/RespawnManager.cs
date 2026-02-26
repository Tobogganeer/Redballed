using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/*

=== After Die() called:
Reset movement properties (velocity, facing, etc)
- Add a facing to each checkpoint? not important tho
Disable visuals and controls
Make camera account for being dead
Wait a few seconds then respawn character
- Enable visuals
- Reset dead bool
- Move camera back (snap? move fast? fade to black?)


*/
