using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrownCat : CatBehavior
{
    private void Update()
    {
        HandleCatching();
        HandleAnimation();
        if (!isCatched)
        {
            HandleMovement();
        }
    }

    protected override IEnumerator OnCaught()
    {
        isCatched = true;
        Destroy(rb);
        navMeshAgent.enabled = false;
        Transform stackBase = PlayerController.instance.stackBase;
        int stackIndex = PlayerController.instance.BrownCats.Count;
        PlayerController.instance.catchingCat = true;
        BoxCollider box = GetComponent<BoxCollider>();
        float height = box != null ? box.bounds.size.y : 0.5f;
        yield return new WaitForSeconds(0.5f);
        Vector3 stackPosition = stackBase.position + Vector3.up * (stackIndex * height);
        transform.position = stackPosition;
        transform.SetParent(stackBase);
        transform.localRotation = Quaternion.identity;
        PlayerController.instance.BrownCats.Add(gameObject);
        PlayerController.instance.catchingCat = false;
    }
}
