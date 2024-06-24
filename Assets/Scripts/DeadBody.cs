using System.Collections;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 20f;
    [SerializeField] private SkinnedMeshRenderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
        //Destroy(gameObject,5);
    }


    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < myRenderer.materials.Length; i++)
        {
            while (myRenderer.materials[i].color.a > 0)
            {
                Color objColor = myRenderer.materials[i].color;
                float fadeAmount = objColor.a - (fadeSpeed * Time.deltaTime);

                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                myRenderer.materials[i].color = objColor;
                yield return null;
            }
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
