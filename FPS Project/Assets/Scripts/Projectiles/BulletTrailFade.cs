using UnityEngine;
using System.Collections;

public class BulletTrailFade : MonoBehaviour
{
	[SerializeField] private Color color;
    public Vector2 speed = new Vector2(10, 20);
	public LineRenderer lr = null;

    float speedChose;

    private void Start()
    {
        speedChose = RNG.RangeBetweenVector2(speed);

        if (lr == null)
        {
            Assign();
        }
    }


    public void Assign ()
    {
        lr = GetComponent<LineRenderer>();
    }

	void Update ()
	{
		color.a = Mathf.Lerp (color.a, 0, Time.deltaTime * speedChose);
		lr.startColor = color;
		lr.endColor = color;

        if (color.a <= 0.04f)
        {
            Destroy(gameObject);
        }
	}
}
