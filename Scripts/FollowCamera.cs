using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainUtils;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float movingSpeed;

    void Awake()
    {
        
        this.transform.position = new Vector3()
        {
            x = this.player.position.x,
            y = this.player.position.y,
            z = this.player.position.z - 20
        };
    }
    void LateUpdate()
    {
        Vector3 target = new Vector3()
        {
            x = this.player.position.x,
            y = this.player.position.y,
            z = this.player.position.z - 20
        };
        Vector3 pos = Vector3.Lerp(transform.position, target, movingSpeed * Time.deltaTime);
        transform.position = pos;
    }
}
