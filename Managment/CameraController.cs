using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraController : MonoBehaviour
{
    Vehicle Player;

    [Header("Camera Parameters")]
    public CinemachineCameraOffset camOffset;
    public CinemachineRecomposer camRecomposer;
    public CinemachineVirtualCamera vCam;

    private float DefCamLens;
    bool stopLerping;

    // Lerping status true-Increase false-Decrease


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindObjectOfType<Vehicle>();
        DefCamLens = vCam.m_Lens.FieldOfView;
    }

    // Update is called once per frame
    void Update()
    {

    }



    #region Player Teleportation
    public void CamTeleportBehaviour()
    {
        camRecomposer.m_FollowAttachment = 0.7f;
        StartCoroutine(T_LerpCameraFollow());
    }

    // for player teleportation
    IEnumerator T_LerpCameraFollow()
    {
        while (camRecomposer.m_FollowAttachment < 1)
        {
            camRecomposer.m_FollowAttachment += 0.0005f;
            yield return null;
        }

        //Make sure CurrentSpeed = Speed
        camRecomposer.m_FollowAttachment = 1f;
        yield return null;
    }
    #endregion

  


    #region JumpBehabiour
    public void JumpTilt(bool status)
    {
        StartCoroutine(CameraOffsetLerp(status));
    }

    IEnumerator CameraOffsetLerp(bool status)
    {
        if (status && !Player.nearGround)
        {

            while (camOffset.m_Offset.y < 0.4)
            {
              //  camOffset.m_Offset.z += 0.0001f;
                camOffset.m_Offset.y += 0.0001f;
                yield return null;
            }

            //for sure
          //  camOffset.m_Offset.z = 0.4f;
            camOffset.m_Offset.y = 0.4f;
            yield return null;
        }

        else
        {
            while (camOffset.m_Offset.y > 0)
            {
              //  camOffset.m_Offset.z -= 0.0001f;
                camOffset.m_Offset.y -= 0.0001f;
                yield return null;
            }

            //fore sure
         //   camOffset.m_Offset.z = 0f;
            camOffset.m_Offset.y = 0f;
            yield return null;
        }

    }

   


    #endregion
}
