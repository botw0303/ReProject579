using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Transform PlayerTransform;

	void Start()
    {
        PlayerTransform = GetComponent<Transform>();
    }
}
