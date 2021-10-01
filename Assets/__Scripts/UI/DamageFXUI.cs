using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DamageFXUI : MonoBehaviour
{
    /// <summary>
    /// Alpha chanel of image
    /// </summary>
    [SerializeField] private float alphaMax = .25f;

    [SerializeField] private float fadingSpeed = 1f;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Color imgColor = _image.color;
        imgColor.a = alphaMax;
        _image.color = imgColor;
    }

    // Update is called once per frame
    void Update()
    {
        Color imgColor = _image.color;
        imgColor.a -= fadingSpeed * Time.deltaTime;
        imgColor.a = Mathf.Clamp(imgColor.a, 0, 1);
        _image.color = imgColor;

        if (imgColor.a == 0)
            gameObject.SetActive(false);
    }
}
