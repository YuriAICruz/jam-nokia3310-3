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
    public Animator animator;
    public GameObject FirePrefab;
    public float animationTime;
    private bool _executing;

    // Start is called before the first frame update
    void Start()
    {
        _inputListener = Bootstrap.Instance.inpListener;
        _player = FindObjectOfType<Player>();
        _inputListener.Attack += Attack;
    }

    private void Attack()
    {
        if (_executing) return;
        
        _executing = true;

        animator.SetLayerWeight(2, 1);
        animator.SetLayerWeight(1, 0);

        Fire();
    }

    public void Fire()
    {
        if (!_fistTime)
        {
            shot = Instantiate(FirePrefab, transform.position, quaternion.identity);
            _fistTime = true;
        }

        if (!shot.activeSelf)
        {
            shot.transform.position = transform.position;
            shot.SetActive(true);
        }

        Invoke(nameof(ResetAnimator), animationTime);
    }

    void ResetAnimator()
    {
        animator.SetLayerWeight(2, 0);
        animator.SetLayerWeight(1, 1);
        _executing = false;
    }
}