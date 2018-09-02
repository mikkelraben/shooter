using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Shoot : NetworkBehaviour
{

    RaycastHit Hit;
    bool Aiming, CanIHasBullet, PrimaryHeld;
    public float Damage = 25, RateOfFire = 10;
    public GameObject hole;
    public bool Auto, Single, Burst, Shotgun;
    public int AmmoInMagazine = 25, StartingAmmo = 225, MagazineSize = 25, NumberOfshotInshell;
    float TimeBulletShot;
    public Text AmmoText, ModeText;
    bool ShotsBurst;
    int NumberShots;
    public string[] ModeNames;
    public Vector3 VectorOverride;
    // Mode 0:Safety 1:Single 2:Burst 3:Auto  4: Shotgunfire
    public int Mode = 3;
    private void Start()
    {
        ModeText = GameObject.Find("Mode").GetComponent<Text>();
        AmmoText = GameObject.Find("Ammo").GetComponent<Text>();
        ModeText.text = "Mode: " + ModeNames[Mode];
        Debug.Log("Mode: " + ModeNames[Mode]);
        AmmoText.text = (AmmoInMagazine + " / " + (StartingAmmo + AmmoInMagazine));
    }
    // Update is called once per frame
    void Update()
    {
        if (hasAuthority == false)
        {
            return;
        }

        //reload mechanics
        if (Input.GetKeyDown("r") && StartingAmmo != 0)
        {
            StartingAmmo = StartingAmmo - 25 + AmmoInMagazine;
            AmmoInMagazine += MagazineSize - AmmoInMagazine;
            if (StartingAmmo < 0)
            {
                AmmoInMagazine = AmmoInMagazine + StartingAmmo;
                StartingAmmo = 0;
            }
            AmmoText.text = (AmmoInMagazine + " / " + (StartingAmmo + AmmoInMagazine));
        }

        //moves and detects if gun is aiming
        if (Input.GetButtonDown("Fire2"))
        {
            transform.GetChild(0).GetChild(0).transform.Translate(new Vector3(0.25f, 0.05f, 0));
            Aiming = true;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            transform.GetChild(0).GetChild(0).transform.Translate(new Vector3(-0.25f, -0.05f, 0));
            Aiming = false;
        }

        //Calls Fire when Primary button pressed
        if (Input.GetButton("Fire1"))
        {
            PrimaryHeld = true;
        }
        else
        {
            PrimaryHeld = false;
        }

        //burst shoooting
        if (ShotsBurst && AmmoInMagazine != 0 && TimeBulletShot < Time.time && NumberShots < 3)
        {
            NumberShots++;
            if (NumberShots == 3)
            {
                ShotsBurst = false;
                NumberShots = 0;
            }
            fire(Aiming, false);
        }

        //switch modes
        if (Input.GetKeyDown("v"))
        {
            Mode++;
            if (Single == false && Mode == 1)
            {
                Mode = 2;
            }
            if (Burst == false && Mode == 2)
            {
                Mode = 3;
            }
            if (Auto == false && Mode == 3)
            {
                Mode = 4;
            }
            if (Shotgun == false && Mode > 3)
            {
                Mode = 0;
            }


            if (Mode > 4)
            {
                Mode = 0;
                ModeText.text = "Mode: " + ModeNames[Mode];
            }
            ModeText.text = "Mode: " + ModeNames[Mode];
        }

        if (Mode == 3 && AmmoInMagazine != 0 && TimeBulletShot < Time.time && PrimaryHeld)
        {
            CanIHasBullet = true;
        }
        else if (Mode == 2 && AmmoInMagazine != 0 && TimeBulletShot < Time.time && Input.GetButtonDown("Fire1"))
        {
            ShotsBurst = true;
        }
        else if (Mode == 1 && AmmoInMagazine != 0 && TimeBulletShot < Time.time && Input.GetButtonDown("Fire1"))
        {
            CanIHasBullet = true;
        }
        else if (Mode == 4 && AmmoInMagazine != 0 && TimeBulletShot < Time.time && Input.GetButtonDown("Fire1"))
        {
            fire(false, false);
            for (int i = 0; i < NumberOfshotInshell - 1; i++)
            {
                fire(false, true);
            }
        }
        else
        {
            CanIHasBullet = false;
        }

        if (CanIHasBullet)
        {
            fire(Aiming, false);
        }
    }
    //fires weapon 
    protected void fire(bool Aim, bool Shotgun)
    {
        if (Shotgun == false)
        {
            AmmoInMagazine--;
            AmmoText.text = (AmmoInMagazine + " / " + (StartingAmmo + AmmoInMagazine));
        }
        TimeBulletShot = 1 / RateOfFire + Time.time;
        Vector3 RandomDot;
        if (Aim == false)
        {
            RandomDot = Random.insideUnitSphere * 2;
        }
        else
        {
            RandomDot = Random.insideUnitSphere;
        }
        if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward * 100 + VectorOverride + RandomDot, out Hit, 1000f))
        {
            CmdSpawnBulletHole();
            Debug.DrawRay(transform.GetChild(0).position, transform.GetChild(0).forward * 100 + VectorOverride + RandomDot);
            Enemy enemy = Hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }


        }

    }
    [Command]
    public void CmdSpawnBulletHole()
    {
        Debug.Log("Here?");
        GameObject BulletHole = Instantiate(hole, Hit.point, Quaternion.LookRotation(Hit.normal, Vector3.up) * Quaternion.Euler(new Vector3(180, 0, 0)));
        Destroy(BulletHole, 20f);
        Debug.Log("got this far");
    }
}