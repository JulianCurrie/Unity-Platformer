﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDamageShoot : MonoBehaviour
{
  [SerializeField]
  private Transform playerBullet;

  private float distanceBetweenNewPlatforms = 120f;

  private LevelGenerator levelGenerator;
  private LevelGeneratorPooling levelGeneratorPooling;

  [HideInInspector]
  public bool canShoot;

  private Button shootBtn;

  void Awake()
  {
    levelGenerator = GameObject.Find(Tags.LEVEL_GENERATOR).GetComponent<LevelGenerator>();
    levelGeneratorPooling = GameObject.Find(Tags.LEVEL_GENERATOR).GetComponent<LevelGeneratorPooling>();

    shootBtn = GameObject.Find(Tags.SHOOT_BUTTON).GetComponent<Button>();
    shootBtn.onClick.AddListener(() => Shoot());
  }

  void FixedUpdate()
  {
    if (Input.GetKeyDown(KeyCode.K))
    {
      Shoot();
    }
  }

  public void Shoot()
  {
    if (canShoot)
    {
      Vector3 bulletPos = transform.position;
      bulletPos.y += 1.5f;
      bulletPos.x += 1f;
      Transform newBullet = Instantiate(playerBullet, bulletPos, Quaternion.identity) as Transform;
      newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
      newBullet.parent = transform;
    }
  }

  void OnTriggerEnter(Collider target)
  {
    if (target.tag == Tags.MONSTER_BULLET_TAG || target.tag == Tags.BOUNDS_TAG)
    {
      GameplayController.instance.TakeDamage();
      Destroy(gameObject);
    }

    if (target.tag == Tags.HEALTH_TAG)
    {
      GameplayController.instance.IncrementHealth();
      target.gameObject.SetActive(false);
    }

    if (target.tag == Tags.MORE_PLATFORMS)
    {
      Vector3 temp = target.transform.position;
      temp.x += distanceBetweenNewPlatforms;
      target.transform.position = temp;

      levelGeneratorPooling.PoolingPlatforms();
    }
  }

  void OnCollisionEnter(Collision target)
  {
    if (target.gameObject.tag == Tags.MONSTER_TAG)
    {
      GameplayController.instance.TakeDamage();
      Destroy(gameObject);
    }
  }
}
