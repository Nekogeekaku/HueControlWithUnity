using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DragAndDrop : MonoBehaviour {
    public Image selectIcon;
    public GameObject colorWheel;
    public Image selectedColor;
    public TMPro.TMP_InputField lightNumberImput;


    RectTransform _rt;
    
    private Image colorWheelImage;
    private RectTransform colorWheelTransform;
    private float colorWheelSize;
    float time;
    bool isAnchoredToPointer = false;
    // Use this for initialization
    bool first = true;

    private Canvas mainCanvas;

    void Awake()
    {
        time= Time.time;
        _rt = gameObject.GetComponent<RectTransform>();

        colorWheelImage = colorWheel.GetComponent<Image>();
        colorWheelTransform = colorWheel.GetComponent<RectTransform>();
        colorWheelSize = colorWheelTransform.sizeDelta.x / 2;


        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        // add the event triggers
        EventTrigger eTrigger = gameObject.AddComponent<EventTrigger>();

        // pointer down event
        EventTrigger.Entry downEvent = new EventTrigger.Entry();
        downEvent.eventID = EventTriggerType.PointerDown;
        downEvent.callback.AddListener(OnPointerDown);
        eTrigger.triggers.Add(downEvent);

        // point up event
        EventTrigger.Entry upEvent = new EventTrigger.Entry();
        upEvent.eventID = EventTriggerType.PointerUp;
        upEvent.callback.AddListener(OnPointerUp);
        eTrigger.triggers.Add(upEvent);
        Colorupdate();
    }
    private void OnPointerDown(BaseEventData data)
    {
        isAnchoredToPointer = true;
    }

	// Update is called once per frame
	void Update () {
        if (isAnchoredToPointer)
        {
            // find the pointers current position and move there
            var mousePos = mousePositionRelativeToCenterOfScreen();


            //Debug.Log(mousePos);
            //Debug.Log(Vector2.Distance(mousePos, Vector2.zero));
            if (Vector2.Distance(mousePos, Vector2.zero) < 350)
            {
                MoveIconToMousePosition();
                Colorupdate();

            }
           

        }

	}
    Vector2 mousePositionRelativeToCenterOfScreen()
    {
        var mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        return mousePos;
    }

    void MoveIconToMousePosition()
    {
        var mousePos= Input.mousePosition;
        _rt.position = Input.mousePosition + new Vector3(10, -20,0);
        //Debug.Log(_rt.sizeDelta);
        //Debug.Log(mainCanvas.scaleFactor);
        //Debug.Log(_rt.offsetMin);
    }

    private void OnPointerUp(BaseEventData data)
    {
        isAnchoredToPointer = false;

       

    }

    public void Colorupdate()
    {
        Vector2 localCursor;
        
        var pos1 = _rt.position;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(colorWheelTransform, pos1,
            null, out localCursor))
            return;

        int xpos = (int)(localCursor.x);
        int ypos = (int)(localCursor.y);

        if (xpos < 0) xpos = xpos + (int)colorWheelTransform.rect.width / 2;
        else xpos += (int)colorWheelTransform.rect.width / 2;

        if (ypos > 0) ypos = ypos + (int)colorWheelTransform.rect.height / 2;
        else ypos += (int)colorWheelTransform.rect.height / 2;

        Debug.Log("Correct Cursor Pos: " + xpos + " " + ypos);
        Color c = colorWheelImage.sprite.texture.GetPixel(xpos, ypos);
        selectedColor.color = c;
        float brightness, hue, saturation;
        Color.RGBToHSV(c, out hue, out saturation, out brightness);
        hue *= 65535f;
        saturation *= 254f;
        brightness *= 254f;
        brightness = Mathf.Max(brightness, 1f);

        if (Time.time - time > 0.5)
        {
            Debug.Log("Couleur: " + hue + " " + saturation + " " + brightness);

 
            time = Time.time;
            string data = "{\"on\":true, \"transitiontime\": 1,\"bri\":" + (int)brightness + ",\"hue\":" + (int)hue + ",\"sat\":" + (int)saturation + "}";
            StartCoroutine(Upload(data));
        }
        if (first)
        {
            first = false;
            Debug.Log("Couleur: " + hue + " " + saturation + " " + brightness);
            string data = "{\"on\":true, \"transitiontime\": 1,\"bri\":" + (int)brightness + ",\"hue\":" + (int)hue + ",\"sat\":" + (int)saturation + "}";
            StartCoroutine(Upload(data));
        }

 }

    public void ColorPicker(){
        Vector2 pos = Input.mousePosition; // Mouse position
        RaycastHit hit;
        Camera _cam = Camera.main; // Camera to use for raycasting
        Ray ray = _cam.ScreenPointToRay(pos);
        Physics.Raycast(_cam.transform.position, ray.direction, out hit, 10000.0f);

        Color c;
        if (hit.collider)
        {
            Debug.Log("Hit touché");
            Texture2D tex = (Texture2D)hit.collider.GetComponent<Renderer>().material.mainTexture; // Get texture of object under mouse pointer
            c = tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y); // Get color from texture
            selectIcon.color = c;
                
        }
    }


    IEnumerator Upload(string data)
    {
        yield return null;

        int lightNumber = 0;
        int.TryParse(lightNumberImput.text, out lightNumber);
        FindObjectOfType<HueChangeLightState>().Request(lightNumber, data, OnChangedState);


    }

    public void OnChangedState(ChangeLightStateResponse response)
    {
        if (response.IsSuccess)
        {
            Debug.Log( " Light changed state succesfully.");

        }
        else
        {
            if (response.Error == ChangeLightStateResponse.TypeOfError.known)
            {
                Debug.Log("Cannot change light state.\nReason is : " + response.ErrorDescription);
            }
            else
            {
                Debug.Log("Cannot  change light state.\n answer is : " + response.Unformatted + "\n Exception could be : " + response.exception);
            }
        }


    }


}
