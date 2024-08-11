using UnityEngine;
using UnityEngine.UI;

[System.Serializable] public class GunInfo : WeaponInfo {
    public int maxBullets;
    public int currentBullets = 0;
    public int gunDamage;
    public ParticleSystem shootParticle;
    public ParticleSystem bulletParticle;
    public AudioSource shootSound;
    public AudioSource reloadSound;

    public void PlayGunParticles() {
        shootParticle.Play();
        bulletParticle.Play();
    }

    public void AddBullets() {
        currentBullets = maxBullets;
    }
    
    public void ReduceBullet() {
        currentBullets = currentBullets - 1;
    }

    // Audio
    public void Play_Shoot_Sound() {
        shootSound.Play();
    }

    public void Play_Reload_Sound() {
        reloadSound.Play();
    }

    public void Stop_Reload_Sound() {
        reloadSound.Stop();
    }
}