using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{

    public Transform playerTransform;

    private void LateUpdate()
    {
        Vector3 newPos = playerTransform.position;
        newPos.y = transform.position.y;
        transform.position = newPos;

        transform.rotation = Quaternion.Euler(90f, playerTransform.eulerAngles.y, 0f);
    }
}
