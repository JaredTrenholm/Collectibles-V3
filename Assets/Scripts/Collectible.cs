using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float timeToRespawn;
    public float shrinkSpeed;
    public Vector3 rotateSpeeds;
    public Vector3 hoverSpeeds;
    public float hoverDistance;
    private Vector3 originalSize;
    private Vector3 originalPosition;
    private float timePassed = 0;
    private bool collected = false;
    private bool vanished = false;
    private bool respawning = false;
    private Collider collider;

    private void Start()
    {
        collider = this.gameObject.GetComponent<Collider>();
        originalSize = this.gameObject.transform.localScale;
        originalPosition = this.gameObject.transform.position;
    }
    private void Update()
    {
        RunStates();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (collected)
                return;
            collected = true;
            collider.enabled = false;
        }
    }

    private void SetVisible(bool isVisible)
    {
        collider.enabled = isVisible;
    }

    private void RunStates()
    {
        switch (collected)
        {
            case true:
                if(vanished == false)
                {
                    this.gameObject.transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime);
                    CheckShrink();
                } else
                {
                    if (timePassed < timeToRespawn)
                    {
                        timePassed += Time.deltaTime;
                        return;
                    }
                    else
                    {
                        collected = false;
                        vanished = false;
                        respawning = true;
                        collider.enabled = true;
                        timePassed = 0f;
                    }
                }
                break;
            case false:
                if (respawning)
                {
                    this.gameObject.transform.localScale += new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime);
                    CheckGrowth();
                }
                break;
        }
        Hover();
        Rotate();
    }

    private void CheckShrink()
    {
        if (this.gameObject.transform.localScale.x <= 0)
        {
            this.gameObject.transform.localScale = new Vector3(0, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
        if (this.gameObject.transform.localScale.y <= 0)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, 0, this.gameObject.transform.localScale.z);
        }
        if (this.gameObject.transform.localScale.z <= 0)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, 0);
        }
        if (this.gameObject.transform.localScale == Vector3.zero)
        {
            vanished = true;
        }
    }
    
    private void CheckGrowth()
    {
        if (this.gameObject.transform.localScale.x >= originalSize.x)
        {
            this.gameObject.transform.localScale = new Vector3(originalSize.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
        if (this.gameObject.transform.localScale.y >= originalSize.y)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, originalSize.y, this.gameObject.transform.localScale.z);
        }
        if (this.gameObject.transform.localScale.z >= originalSize.z)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, originalSize.z);
        }
        if (this.gameObject.transform.localScale == originalSize)
        {
            respawning = false;
        }
    }
    
    private void Hover()
    {
        if(Vector3.Distance(this.transform.position, originalPosition) < hoverDistance)
        {
            this.transform.position += hoverSpeeds*Time.deltaTime;
        } else {
            hoverSpeeds *= -1;
            this.transform.position += hoverSpeeds*Time.deltaTime;
        }
    }

    private void Rotate()
    {
        this.transform.Rotate(rotateSpeeds * Time.deltaTime, Space.Self);
    }
}
