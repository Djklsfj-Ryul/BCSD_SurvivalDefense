using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //���ǵ� ���� ����
    [SerializeField] private float walkSpeed;
    //Inspectorâ���� ���� ������ �� �ִ�
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;


    private float applySpeed;

    [SerializeField] private float jumpForce;


    //���� ����
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    //��ŭ ������ ���� ����
    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;


    //�� ���� ����
    private CapsuleCollider capsuleCollider;

    //�ΰ���
    [SerializeField] private float lookSensitivity;

    //ī�޶� �Ѱ�
    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    //�ʿ��� ������Ʈ
    [SerializeField] private Camera theCamera;
    private Rigidbody myRigid;
    //Collider�� ������ ��Ҹ� ������

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        //theCamera = FindObjectOfType<Camera>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun(); // Move�Լ� ��
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while(_posY != applyCrouchPosY)
        {
            count += 1;
            _posY = Mathf.Lerp(_posY,applyCrouchPosY, 0.3f);
            //���� 
            theCamera.transform.localPosition = new Vector3(0,_posY ,0);
            if(count > 15)
            {
                break;
            }
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY,0f);
    }
    //����ó����

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        if (isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        //(1, 0, 0) * (1 or -1 or 0)
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        //(0, 0, 1)

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        // ������ �� (x,0,0) + (0,0,z)
        // normalized �� ���� 1�� ����� ���� �����Ѵ�
        // (1,0,1) = 2 
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        //
    }
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //���Ϸ� ���Ͱ��� ���ʹϾ����� �ٲ۴�
        //Debug.Log(myRigid.rotation);
        //Debug.Log(myRigid.rotation.eulerAngles);
    }
    private void CameraRotation()
    {
        //���� ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX += _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
