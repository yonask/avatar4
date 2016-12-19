using UnityEngine;
using System.Collections;
//using Windows.Kinect;


public class movingavataty : MonoBehaviour
{
    [Tooltip("The Kinect joint we want to track.")]
    public KinectInterop.JointType trackedJoint = KinectInterop.JointType.SpineBase;

    [Tooltip("Whether the movement is relative to transform's initial position, or is in absolute coordinates.")]
    public bool relToInitialPos = false;

    [Tooltip("Whether the z-movement is inverted or not.")]
    public bool invertedZMovement = false;

    [Tooltip("Transform offset to the Kinect-reported position.")]
    public Vector3 transformOffset = Vector3.zero;

    [Tooltip("Whether the displayed position is in Kinect coordinates, or in world coordinates.")]
    public bool useKinectSpace = false;

    //public bool moveTransform = true;

    [Tooltip("Smooth factor used for the joint position smoothing.")]
    public float smoothFactor = 5f;

    [Tooltip("GUI-Text to display the current joint position.")]
    public GUIText debugText;
    [Tooltip("Rate at which the avatar will move through the scene.")]
    public float moveRate = 1f;

    [Tooltip("Whether the avatar is facing the player or not.")]
    public bool mirroredMovement = false;
    [Tooltip("If enabled, makes the avatar position relative to this camera to be the same as the player's position to the sensor.")]
    public Camera posRelativeToCamera;
    protected Quaternion initialRotation;
    [Tooltip("Game object this transform is relative to (optional).")]
    public GameObject offsetNode;

    protected Vector3 bodyRootPosition;


    protected Vector3 offsetPos = Vector3.zero;


    private Vector3 initialPosition = Vector3.zero;
    private long currentUserId = 0;
    private Vector3 initialUserOffset = Vector3.zero;

    private Vector3 vPosJoint = Vector3.zero;
    public Rigidbody rb;
    private Vector3 posHandL, posHandR, posHead, posShoulderL, posShoulderR, posSpineBase;
    void Start()
    {
        initialPosition = transform.position;
     /*   if (bodyRoot != null)
        {
            bodyRootPosition = bodyRoot.position;
        }
        else
        {
            bodyRootPosition = transform.position;
        }

        if (offsetNode != null)
        {
         //   bodyRootPosition = bodyRootPosition - offsetNodePos;
        }
        */
    }


  /*  protected Vector3 Kinect2AvatarPos(Vector3 jointPosition, bool bMoveVertically)
    {
        float xPos = (jointPosition.x - offsetPos.x) * moveRate;
        float yPos = (jointPosition.y - offsetPos.y) * moveRate;
        float zPos = !mirroredMovement && !posRelativeToCamera ? (-jointPosition.z - offsetPos.z) * moveRate : (jointPosition.z - offsetPos.z) * moveRate;

        Vector3 newPosition = new Vector3(xPos, bMoveVertically ? yPos : 0f, zPos);

        Quaternion posRotation = mirroredMovement ? Quaternion.Euler(0f, 180f, 0f) * initialRotation : initialRotation;
        newPosition = posRotation * newPosition;

        if (offsetNode != null)
        {
            //newPosition += offsetNode.transform.position;
            newPosition = offsetNode.transform.position;
        }

        return newPosition;
    }
    */
    void Update()
    {
        KinectManager manager = KinectManager.Instance;
        rb = gameObject.GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(0, 176, 0);

        if (manager && manager.IsInitialized())
        {
         
            int iJointIndex = (int)trackedJoint;

            if (manager.IsUserDetected())
            {

               
                long userId = manager.GetPrimaryUserID();

                if (manager.IsJointTracked(userId, iJointIndex))
                {
                     posHandL = manager.GetJointPosition(userId, (int)KinectInterop.JointType.HandLeft);
                     posHandR = manager.GetJointPosition(userId, (int)KinectInterop.JointType.HandRight);
                     posHead = manager.GetJointPosition(userId, (int)KinectInterop.JointType.Head);
                     posShoulderL = manager.GetJointPosition(userId, (int)KinectInterop.JointType.ShoulderLeft);
                     posShoulderR = manager.GetJointPosition(userId, (int)KinectInterop.JointType.ShoulderRight);
                     posSpineBase = manager.GetJointPosition(userId, (int)KinectInterop.JointType.SpineBase);
                    Vector3 pos = gameObject.transform.position;
                    if (posHandL != Vector3.zero && posHandR != Vector3.zero && posHead != Vector3.zero && posHandL.y > posHead.y && posHandR.y > posHead.y)
                    {

                        // Vector3 targetPos = bodyRootPosition + Kinect2AvatarPos(trans, verticalMovement);
                                        gameObject.transform.position = new Vector3
                        (
                            Mathf.Clamp(pos.x, -58, 9),
                            posHandR.y * 10,
                            Mathf.Clamp(pos.z, -57, 0)
                        ); 
                        rb.isKinematic = false;
                         rb.AddRelativeForce(Vector3.forward * Time.deltaTime * 5);
                        //rb.AddRelativeForce(0,0,1 * 3);
                        //transform.position += transform.forward * Time.deltaTime * 10;
                        // Debug.Log("sdfsdf",pos.x);

                    }

                    else if (posHandL != Vector3.zero && posHandR != Vector3.zero && posShoulderL != Vector3.zero && posShoulderR != Vector3.zero && posHandL.y > posShoulderL.y && posHandR.y > posShoulderR.y)
                    {

                        //gameObject.transform.position = new Vector3(pos.x, posHandR.y * 7, pos.z);
                             gameObject.transform.position = new Vector3
                        (
                            Mathf.Clamp(pos.x, -58, 9),
                            posHandR.y * 8,
                            Mathf.Clamp(pos.z, -57, 0)
                        );
                       // transform.position += transform.forward * Time.deltaTime * 10;
                        rb.isKinematic = false;
                        rb.AddRelativeForce(Vector3.forward * Time.deltaTime * 4);
                       // rb.AddRelativeForce(0, 0, 1 * 2);



                    }

                    else if (posHandL != Vector3.zero && posHandR != Vector3.zero && posShoulderL != Vector3.zero && posShoulderR != Vector3.zero && posHandL.y == posShoulderL.y && posHandR.y == posShoulderR.y)
                    {

                        // gameObject.transform.position = new Vector3(pos.x, posHandR.y * 6, pos.z);
                            gameObject.transform.position = new Vector3
                             (
                                 Mathf.Clamp(pos.x, -58, 9),
                                 posHandR.y * 6,
                                 Mathf.Clamp(pos.z, -57, 0)
                             ); 
                      //  transform.position += transform.forward * Time.deltaTime * 10;
                        rb.isKinematic = false;
                       rb.AddRelativeForce(Vector3.forward * Time.deltaTime * 3);
                       // rb.AddRelativeForce(0, 0, 1 * 1.5f);


                    }
                    else if  (posHandL != Vector3.zero && posHandR != Vector3.zero && posSpineBase != Vector3.zero && posHandL.y > posSpineBase.y && posHandR.y > posSpineBase.y)
                    {

                        //gameObject.transform.position = new Vector3(pos.x, posHandR.y * 5, pos.z);
                          gameObject.transform.position = new Vector3
                          (
                              Mathf.Clamp(pos.x, -58, 9),
                              posHandR.y * 4,
                              Mathf.Clamp(pos.z, -57, 0)
                          ); 
                      //  transform.position += transform.forward * Time.deltaTime * 10;
                        rb.isKinematic = false;
                        rb.AddRelativeForce(Vector3.forward * Time.deltaTime * 2);
                      //  rb.AddRelativeForce(0, 0,1 * 1);

                    if (posHandL.y> posHandR.y)
                        {
                           rb.position -= Vector3.right * Time.deltaTime * 2;
                        }
                    else if (posHandL.y < posHandR.y)
                        {
                            rb.position += Vector3.right * Time.deltaTime * 2;
                        }


                    }

                    else if (posHandL != Vector3.zero && posHandR != Vector3.zero && posSpineBase != Vector3.zero && posHandL.y == posSpineBase.y && posHandR.y == posSpineBase.y)
                    {

                        //gameObject.transform.position = new Vector3(pos.x, posHandR.y * 4, pos.z);
                           gameObject.transform.position = new Vector3
                              (
                                  Mathf.Clamp(pos.x, -58, 9),
                                  posHandR.y * 2,
                                  Mathf.Clamp(pos.z, -57, 0)
                              );
                     //   transform.position += transform.forward * Time.deltaTime * 10;
                        rb.isKinematic = false;
                        rb.AddRelativeForce(Vector3.forward * Time.deltaTime * 1);



                    }
                    else
                    {
                        // gameObject.transform.position = new Vector3(pos.x, posHandR.y*2, pos.z);
                          gameObject.transform.position = new Vector3
                          (
                              Mathf.Clamp(pos.x, -58, 9),
                              posHandR.y * 1,
                              Mathf.Clamp(pos.z, -57, 0)
                          ); 
                       // transform.position += transform.forward * Time.deltaTime * 10;
                        rb.isKinematic = false;
                        rb.AddRelativeForce(Vector3.forward * Time.deltaTime * 0);
                        //rb.AddRelativeForce(0,0,0);


                    }


                    //if(moveTransform)

                }

            }

        }
    }

   /* void FixedUpdtae()
        {

        if (posHandL != Vector3.zero && posHandR != Vector3.zero && posHead != Vector3.zero && posHandL.y > posHead.y && posHandR.y > posHead.y)
        {
            rb = gameObject.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * 10);
        }

        else if (posHandL != Vector3.zero && posHandR != Vector3.zero && posShoulderL != Vector3.zero && posShoulderR != Vector3.zero && posHandL.y > posShoulderL.y && posHandR.y > posShoulderR.y)
        {
            rb.isKinematic = false;
            rb.AddRelativeForce(Vector3.forward * 2);
        }

        else if (posHandL != Vector3.zero && posHandR != Vector3.zero && posShoulderL != Vector3.zero && posShoulderR != Vector3.zero && posHandL.y == posShoulderL.y && posHandR.y == posShoulderR.y)
        {
            rb.isKinematic = false;
            rb.AddRelativeForce(Vector3.forward * 1.5f);
        }

        else if (posHandL != Vector3.zero && posHandR != Vector3.zero && posSpineBase != Vector3.zero && posHandL.y > posSpineBase.y && posHandR.y > posSpineBase.y)
        {
            rb.isKinematic = false;
            rb.AddRelativeForce(Vector3.forward * 1);
        }


        else if (posHandL != Vector3.zero && posHandR != Vector3.zero && posSpineBase != Vector3.zero && posHandL.y == posSpineBase.y && posHandR.y == posSpineBase.y)
        {
            rb.isKinematic = false;
            rb.AddRelativeForce(Vector3.forward * .5f);
        }
        else
        {
            rb.isKinematic = false;
            rb.AddRelativeForce(Vector3.forward * 0);
        }
        }
        */
}
