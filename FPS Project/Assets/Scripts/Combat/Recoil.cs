using UnityEngine;

public class Recoil : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;

    [SerializeField] private float hipRecoilX;
    [SerializeField] private float hipRecoilY;
    [SerializeField] private float hipRecoilZ;

    [SerializeField] private float ADSRecoilX;
    [SerializeField] private float ADSRecoilY;
    [SerializeField] private float ADSRecoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    public bool ADSing;


    public void UpdateRecoilData(Weapons weapon)
    {
        RecoilParameters parameters = Data.GetWeaponData(weapon).recoilData;

        hipRecoilX = -Mathf.Abs(parameters.hipfireRecoil.x);
        hipRecoilY = parameters.hipfireRecoil.y;
        hipRecoilZ = parameters.hipfireRecoil.z;

        ADSRecoilX = -Mathf.Abs(parameters.ADSRecoil.x);
        ADSRecoilY = parameters.ADSRecoil.y;
        ADSRecoilZ = parameters.ADSRecoil.z;

        snappiness = parameters.snappiness;
        returnSpeed = parameters.returnSpeed;
    }


    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }


    public void RecoilFire()
    {
        if (ADSing)
        {
            targetRotation += new Vector3(ADSRecoilX, RNG.Range(-ADSRecoilY, ADSRecoilY), RNG.Range(-ADSRecoilZ, ADSRecoilZ));
        }
        else
        {
            targetRotation += new Vector3(hipRecoilX, RNG.Range(-hipRecoilY, hipRecoilY), RNG.Range(-hipRecoilZ, hipRecoilZ));
        }
    }
}
