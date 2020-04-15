using NaughtyAttributes;
using UnityEngine;

namespace VHS
{    
    public class CameraController : MonoBehaviour
    {
        #region Variables
            #region Data
                [BoxGroup("Input Data")] public CameraInputData camInputData;

                [BoxGroup("Custom Classes")] public CameraZoom cameraZoom;
                [BoxGroup("Custom Classes")] public CameraSwaying cameraSway;

            #endregion

            #region Settings
                [Space]
                [BoxGroup("Settings")] public Vector2 sensitivity;
                [BoxGroup("Settings")] public Vector2 smoothAmount;
                [BoxGroup("Settings")] [MinMaxSlider(-90f,90f)] public Vector2 lookAngleMinMax;
                [BoxGroup("Settings")] [MinMaxSlider(-180f,180f)] public Vector2 lockedLookAngleMinMax;
            #endregion

            #region Private
                float m_yaw;
                float m_pitch;

                float m_desiredYaw;
                float m_desiredPitch;
                [BoxGroup("DEBUG")][ReadOnly] public bool isLockedCamera = false;

                #region Components                    
                    Transform m_pitchTranform;
                    Camera m_cam;
                #endregion
            #endregion
            
        #endregion

        #region BuiltIn Methods     
            void Start()
            {
                GetComponents();
                InitComponents();
                ChangeCursorState();
            }

            void LateUpdate()
            {
                CalculateRotation();
                SmoothRotation();
                ApplyRotation();
                HandleZoom();
            }
        #endregion

        #region Custom Methods
            void GetComponents()
            {
                m_pitchTranform = transform.GetChild(0).transform;
                m_cam = GetComponentInChildren<Camera>();
            }

            void InitComponents()
            {
                cameraZoom.Init(m_cam, camInputData);
                cameraSway.Init(m_cam.transform);
            }

            void CalculateRotation()
            {
                m_desiredYaw += camInputData.InputVector.x * sensitivity.x * Time.deltaTime;
                m_desiredPitch -= camInputData.InputVector.y * sensitivity.y * Time.deltaTime;
                if(m_desiredYaw < 0) {
                    m_desiredYaw += 360;
                } else if(m_desiredYaw > 360) {
                    m_desiredYaw -= 360;
                }
                m_desiredYaw = Mathf.Clamp(m_desiredYaw, 0, 360);

                m_desiredPitch = Mathf.Clamp(m_desiredPitch,lookAngleMinMax.x,lookAngleMinMax.y);
                    float upperLimit = 0, lowerLimit = 0;
                
                if(isLockedCamera) {
                    if(transform.parent.eulerAngles.y + lockedLookAngleMinMax.y > 360) {
                        //print("A");
                        upperLimit = transform.parent.eulerAngles.y + lockedLookAngleMinMax.x;
                        lowerLimit = (transform.parent.eulerAngles.y + lockedLookAngleMinMax.y) % 360;
                        if(m_desiredYaw > 180) {
                            m_desiredYaw = Mathf.Clamp(m_desiredYaw,upperLimit,360);
                        } else {
                            m_desiredYaw = Mathf.Clamp(m_desiredYaw,0,lowerLimit);
                        }

                    } else if(transform.parent.eulerAngles.y + lockedLookAngleMinMax.x < 0) {
                        //print("B");
                        upperLimit = ((transform.parent.eulerAngles.y + lockedLookAngleMinMax.x) % 360) + 360;
                        lowerLimit = transform.parent.eulerAngles.y + lockedLookAngleMinMax.y;
                        if(m_desiredYaw > 180) {
                            m_desiredYaw = Mathf.Clamp(m_desiredYaw,upperLimit,360);
                        } else {
                            m_desiredYaw = Mathf.Clamp(m_desiredYaw,0,lowerLimit);
                        }

                    } else {
                        //print("C");
                        upperLimit = transform.parent.eulerAngles.y + lockedLookAngleMinMax.y;
                        lowerLimit = transform.parent.eulerAngles.y + lockedLookAngleMinMax.x;
                        m_desiredYaw = Mathf.Clamp(m_desiredYaw,lowerLimit,upperLimit);

                    }

                    //m_desiderYaw goes from -360 to 360
                    // upperLimit = transform.parent.eulerAngles.y + lockedLookAngleMinMax.y;
                    // lowerLimit = transform.parent.eulerAngles.y + lockedLookAngleMinMax.x;
                    // m_desiredYaw = Mathf.Clamp(m_desiredYaw,lowerLimit,upperLimit);

                    // float lowerLimit = transform.parent.eulerAngles.y + lockedLookAngleMinMax.y > 360 ? (transform.parent.eulerAngles.y + lockedLookAngleMinMax.y) % 360 : transform.parent.eulerAngles.y + lockedLookAngleMinMax.x;
                    // float upperLimit = transform.parent.eulerAngles.y + lockedLookAngleMinMax.x < 0 ? (transform.parent.eulerAngles.y + lockedLookAngleMinMax.x) % 360 : transform.parent.eulerAngles.y + lockedLookAngleMinMax.y;
                    //m_desiredYaw = Mathf.Clamp(m_desiredYaw,lowerLimit,upperLimit);
                }
                    //print("Desired: " + m_desiredYaw + " Upper Limit: " + upperLimit + " Lower Limit: " + lowerLimit);

            }

            void SmoothRotation()
            {
                m_yaw = Mathf.Lerp(m_yaw,m_desiredYaw, smoothAmount.x * Time.deltaTime);
                m_pitch = Mathf.Lerp(m_pitch,m_desiredPitch, smoothAmount.y * Time.deltaTime);
            }

            void ApplyRotation()
            {
                transform.eulerAngles = new Vector3(0f,m_yaw,0f);
                m_pitchTranform.localEulerAngles = new Vector3(m_pitch,0f,0f);
            }

            public void HandleSway(Vector3 _inputVector,float _rawXInput)
            {
                cameraSway.SwayPlayer(_inputVector,_rawXInput);
            }

            void HandleZoom()
            {
                if(camInputData.ZoomClicked || camInputData.ZoomReleased)
                    cameraZoom.ChangeFOV(this);

            }

            public void ChangeRunFOV(bool _returning)
            {
                cameraZoom.ChangeRunFOV(_returning,this);
            }

            void ChangeCursorState()
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        #endregion
    }
}
