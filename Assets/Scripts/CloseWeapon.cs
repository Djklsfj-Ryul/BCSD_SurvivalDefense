using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon: MonoBehaviour
{
    public string CloseWeaponName;     //���� ���� �̸�

    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range;         //���ݹ���
    public int damage;          //���ݷ�
    public float workSpeed;     //�۾��ӵ�
    public float attackDelay;   //���� ������
    public float attackDelayA;  //���� Ȱ��ȭ ����
    public float attackDelayB;  //���� ��Ȱ��ȭ ����

    public Animator anim;       //�ִϸ��̼�
}