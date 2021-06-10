using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    GameObject player;
    Vector2 default_dir;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        default_dir = new Vector2(1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 player_dir = new Vector2(player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y);
        player_dir.Normalize();
        Debug.Log(Vector2.Dot(Vector2.up,player_dir));
        this.gameObject.transform.rotation = Quaternion.Euler(this.gameObject.transform.rotation.x, this.gameObject.transform.rotation.y, 
        Vector2.Dot(Vector2.up,player_dir) >= 0 ? Vector2.Angle(default_dir,player_dir) : -Vector2.Angle(default_dir,player_dir));
    }
}
