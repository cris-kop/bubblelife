using UnityEngine;

public class Simplemover_Straight : SimpleMover_Base
{
    // Update is called once per frame
    protected override void Move()
    {
        transform.position += _dir * speed * gameController.GetCurrentLevel() * Time.deltaTime;
    }
}
