//This script allows a camera to follow the player smoothly and without rotation

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	
    public bool FreeCamera_Lock = true;
    public float moveSpeed = 1.0f;

    public float zoomSpeed;
    float zoomOffset;

	private Survivor[] survivors;
    public Transform currentTarget;
    
	int index;

    bool freeCamera;

    public float mouseH = 2.0f;
    public float mouseV = 2.0f;
    

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    [SerializeField] float smoothing = 5f;                          //Amount of smoothing to apply to the cameras movement
    [SerializeField] Vector3 offset = new Vector3(0f, 15f, -22f);  //The offset of the camera from the player (how far back and above the player the camera should be)


    void Start()
	{
        survivors = GameObject.FindObjectsOfType<Survivor>();

		index = 0;
		currentTarget = survivors[index].transform;
	}

	void Update()
	{
        if (FreeCamera_Lock == false)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                index++;
                if (index > survivors.Length)
                {
                    index = 0;
                }

                if (index == survivors.Length)
                {
                    index++;
                    freeCamera = true;

                    transform.position = new Vector3(0, 30, 0);
                    transform.eulerAngles = new Vector3(-90, 0, 0);
                }
                else
                {
                    currentTarget = survivors[index].transform;
                    freeCamera = false;
                    transform.eulerAngles = new Vector3(55, 0, 0);
                }
            }
        }
        else
        {
            freeCamera = true;
        }
	}

	//FixedUpdate is used to handle physics based code. No physics code exists in this FixedUpdate, but since the player's movement code
	//is handled in FixedUpdate, we are moving the camera in FixedUpdate as well so that they stay in sync

	void LateUpdate()
	{
        if (freeCamera == false)
        {
            float m = Input.GetAxis("Mouse ScrollWheel");

            
            offset += offset * -m * zoomSpeed;

            //Use the player's position and offset to determine where the camera should be
            Vector3 targetCamPos = currentTarget.transform.position + offset;
            //Smoothly move from the current position to the desired position using a Lerp, which is short
            //for linear interpolation. Basically, it takes where you are, where you want to be, and an amount of time
            //and then tells you where you will be along that line
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

            transform.LookAt(currentTarget.transform.position);
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float d = Input.GetAxis("Depth");

            Vector3 movement = (transform.forward * v) + (transform.right * h) + (Vector3.up * d);
            movement *= moveSpeed * Time.deltaTime;

            transform.position += movement;

            yaw += mouseH * Input.GetAxis("Mouse X");
            pitch -= mouseV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

    }

    
}

