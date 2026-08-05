using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class GunManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onGunGrabbed;
    [SerializeField]
    private UnityEvent onGunDropped;
    [SerializeField]
    private Transform gunPosition;
    [SerializeField]
    private Text ammoText;
    [SerializeField]
    private InputManager inputManager;
    private Gun currentGun;
    private List<Gun> guns = new List<Gun>();
    private int currentGunIntex = 0;
    private void Awake()
    {
        onGunDropped?.Invoke();
    }
    public void GrabGun(Gun gun)
    {
        Gun existingGun = guns.Find(listGun => listGun.GunData == gun.GunData);
        if ( existingGun != null)
        {
            if( existingGun.IsGunFull)
            {
                return;
            }
            existingGun.ChargeTotalBullets();
            Destroy(gun.gameObject);
            return;
        }
        guns.Add(gun);
        currentGun?.gameObject.SetActive(false);
        currentGun = gun;
        currentGun.GrabGun(gunPosition, ammoText);
        currentGun.OnGunEmpty.AddListener(DropGun);
        onGunGrabbed?.Invoke();
        currentGunIntex = guns.IndexOf(currentGun);
    }
    public void SwitchUpGun()
    {
        currentGunIntex++;
        if (currentGunIntex >= guns.Count)
        {
            currentGunIntex = 0;
        }
        SwitchGun();
    }
    public void SwitchDownGun()
    {
        currentGunIntex--;
        if (currentGunIntex < 00)
        {
            currentGunIntex = guns.Count -1;
        }
        SwitchGun();
    }
    private void SwitchGun()
    {
        if (guns.Count <= 1) return;
        currentGun.gameObject.SetActive(false);
        currentGun = guns[currentGunIntex];
        currentGun.gameObject.SetActive(true);
        currentGun.GrabGun(gunPosition, ammoText, false);
    }
    public void DropAllGuns()
    {
        foreach (Gun gun in guns)
        {
            Destroy(gun.gameObject);
        }
        guns.Clear();
        currentGun = null;
        onGunDropped?.Invoke();
    }
    public void DropGun()
    {
        currentGun.OnGunEmpty.RemoveListener(DropGun);
        guns.Remove(currentGun);
        Destroy(currentGun.gameObject);
        if(guns.Count > 0)
        {
            currentGunIntex = guns.Count - 1;
            currentGun = guns[currentGunIntex];
            currentGun.gameObject.SetActive(true);
            currentGun.GrabGun(gunPosition, ammoText, false);
        }
        else
        {
            onGunDropped?.Invoke();
            currentGun = null;
        }
    }
    private void Update()
    {
        if(currentGun == null) return;
        currentGun.HandleFire(inputManager.LeftButtonPressed, inputManager.LeftButtonHeld);
        if(inputManager.RightButtonPressed)
        {
            currentGun.ChargeGun();
        }
    }

}
