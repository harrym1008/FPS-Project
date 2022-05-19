using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletBehaviour : MonoBehaviour
{
    public class PelletImpact
    {
        public Vector3 spawnLocation;
        public Vector3 impactLocation;
        public Collider impactCollider;
    }

    Vector3 spawnPos;
    float effectiveRange;
    float velocity;

    public ShellBehaviour parentShell;

    [SerializeField] LayerMask layerMask;

    bool frame1 = true;

    public GameObject bulletTrail;
    public GameObject bulletHole;
    public GameObject smokeEffect;

    BulletTrailFade trailLine;


    public void SetData(float _velocity, float _effectiveRange)
    {
        effectiveRange = _effectiveRange;
        velocity = _velocity;
    }


    private void Start()
    {
        spawnPos = transform.position;
        trailLine = Instantiate(bulletTrail, transform.position, Quaternion.identity, null).GetComponent<BulletTrailFade>();
        trailLine.Assign();
        trailLine.lr.SetPosition(0, transform.position);
        trailLine.lr.SetPosition(1, transform.position);
    }

    private void LateUpdate()
    {
        if (frame1)
        {
            frame1 = false;
            return;
        }

        if (effectiveRange < 0f)
        {
            Destroy(gameObject);
        }

        float distanceThisFrame = velocity * Time.deltaTime;
        effectiveRange -= distanceThisFrame;

        Obstruction obstruction = CheckForObstruction(distanceThisFrame);

        if (trailLine != null)
        {
            trailLine.lr.SetPosition(1, obstruction.advanceTo);
        }

        if (obstruction.hitSomething)
        {
            PelletImpact pelletImpact = new PelletImpact
            {
                spawnLocation = spawnPos,
                impactLocation = obstruction.advanceTo,
                impactCollider = obstruction.collider
            };

            parentShell.ReportDamage(pelletImpact);

            GameObject smoke = Instantiate(smokeEffect, obstruction.raycastHit.point, Quaternion.LookRotation(obstruction.raycastHit.normal));
            Destroy(smoke, 1.5f);

            try
            {
                RaycastHit hit = obstruction.raycastHit;
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (meshCollider != null || meshCollider.sharedMesh != null)
                {
                    Mesh mesh = meshCollider.sharedMesh;
                    Renderer renderer = hit.collider.GetComponent<MeshRenderer>();

                    int[] hitTriangle = new int[]
                    {
                            mesh.triangles[hit.triangleIndex * 3],
                            mesh.triangles[hit.triangleIndex * 3 + 1],
                            mesh.triangles[hit.triangleIndex * 3 + 2]
                    };
                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        int[] subMeshTris = mesh.GetTriangles(i);
                        for (int j = 0; j < subMeshTris.Length; j += 3)
                        {
                            if (subMeshTris[j] == hitTriangle[0] &&
                                subMeshTris[j + 1] == hitTriangle[1] &&
                                subMeshTris[j + 2] == hitTriangle[2])
                            {
                                Material mat = renderer.materials[i];
                                smoke.GetComponent<BulletDebrisManager>().myDebrisCode = mat.name.Substring(0, 4);
                                //print(smoke.GetComponent<BulletDebrisManager>().myDebrisCode);
                            }
                        }
                    }

                }

                Destroy(Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(RNG.RandomVector3(0f, 0f, 180f))), 120);

            }
            catch { }

            Destroy(gameObject);
        }
        else
        {
            transform.position = obstruction.advanceTo;
        }
    }




    private Obstruction CheckForObstruction(float distanceToAdvance)
    {
        Vector3 endingPosition = transform.position + transform.forward * distanceToAdvance;
        bool didIHit = Physics.Linecast(transform.position, endingPosition, out RaycastHit hit, layerMask);

        if (didIHit)
        {
            return new Obstruction
            {
                hitSomething = true,
                raycastHit = hit,
                collider = hit.collider,
                advanceTo = hit.point
            };
        }
        else
        {
            return new Obstruction
            {
                hitSomething = false,
                raycastHit = hit,
                collider = null,
                advanceTo = endingPosition
            };
        }
    }
}
