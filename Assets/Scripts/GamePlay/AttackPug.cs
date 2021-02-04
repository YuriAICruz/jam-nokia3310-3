using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

public class AttackPug : MonoBehaviour
{
    private bool _fistTime;
    private GameObject shot;
    private Player _player;
    private InputListener _inputListener;
    private Animator _animator;
    public GameObject FirePrefab;

    // Start is called before the first frame update
    void Start()
    {
        _inputListener = Bootstrap.Instance.inpListener;
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _inputListener.Attack += Attack;
    }

    private void Attack()
    {
        _animator.SetBool("attak", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        if (!_fistTime)
        {
           shot = Instantiate(FirePrefab,transform.position, quaternion.identity);
            _fistTime = true;
        }

        if (!shot.activeSelf)
        {
            shot.transform.position = transform.position;
            shot.SetActive(true);
        }

        _animator.SetBool("attak", false);
    }
}
