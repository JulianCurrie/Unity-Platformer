﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDamageShoot : MonoBehaviour
{

  [SerializeField]
  private Transform playerBullet;

  private float distanceBetweenNewPlatforms = 120f;

  private LevelGenerator levelGenerator;

  [HideInInspector]
  public bool canShoot;

  void Awake()
  {
    levelGenerator = GameObject.Find(Tags.LEVEL_GENERATOR).GetComponent<LevelGenerator>();
  }

  void FixedUpdate()
  {
    Fire();
  }

  void Fire()
  {
    if (Input.GetKeyDown(KeyCode.K) && canShoot)
    {
      Vector3 bulletPos = transform.position;
      bulletPos.y += 1.5f;
      bulletPos.x += 1f;
      Transform newBullet = Instantiate(playerBullet, bulletPos, Quaternion.identity) as Transform;
      newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
      newBullet.parent = transform;

      print("shooting");
    }
  }

  void OnTriggerEnter(Collider target)
  {
    if (target.tag == Tags.MONSTER_BULLET_TAG)
    {
      Destroy(gameObject);
    }

    if (target.tag == Tags.HEALTH_TAG)
    {
      gameObject.SetActive(false);
    }

    if (target.tag == Tags.MORE_PLATFORMS)
    {
      Vector3 temp = target.transform.position;
      temp.x += distanceBetweenNewPlatforms;
      target.transform.position = temp;

      levelGenerator.GenerateLevel(false);
    }
  }
}
