using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    Vector3 SizeLoss;
    // Start is called before the first frame update
    private void Awake()
    {
        SizeLoss = new Vector3(0.00001f, 0.00001f, 0);
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, 0, 0.02f);
        transform.localScale -= SizeLoss;

    }





}
