using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pistol : MonoBehaviour
{

    public PlayerBullet Bullet;

    public List<AudioClip> ShootSounds = new List<AudioClip>();

    public AudioSource ShootSoundPlayer;

    public void ShootDown(Vector2 direction)
    {

        var playerBullet = Instantiate(Bullet);
        playerBullet.transform.position = Bullet.transform.position;
        playerBullet.Direction = direction;
        playerBullet.gameObject.SetActive(true);

        var soundIndex = Random.Range(0, ShootSounds.Count);
        ShootSoundPlayer.clip = ShootSounds[soundIndex];
        ShootSoundPlayer.Play();

    }

    public void Shooting(Vector2 direction)
    {

    }
    public void ShootUp(Vector2 direction)
    {

    }
}
