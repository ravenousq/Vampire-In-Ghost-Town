using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    private float crosshairSpeed;
    private float crosshairResistance;

    private Vector2 initialPosition;

    private Vector2 bounds;

    private float aimTimer;
    private CinemachineCamera cinemachine;

    private List<Transform> enemiesToAdd = new List<Transform>();
    private List<Transform> targets = new List<Transform>();


    public void SetUp(float maxAimDuration, float ashenRainDuration, float crosshairSpeed, float crosshairResistance, CinemachineCamera cinemachine)
    {
        aimTimer = maxAimDuration;
        this.crosshairSpeed = crosshairSpeed;
        this.crosshairResistance = crosshairResistance;
        this.cinemachine = cinemachine;

        initialPosition = transform.position;
        

        Camera cam = Camera.main;

        Vector3 screenCenter = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 screenTopRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
;

        bounds = new Vector2(screenTopRight.x - screenCenter.x,screenTopRight.y - screenCenter.y);
        this.cinemachine.Follow = gameObject.transform;

        PlayerManager.instance.player.SetCrosshair(gameObject);
    }

    private void Update()
    {
        aimTimer -= Time.deltaTime;
        if (aimTimer <= 0 || Input.GetKeyDown(KeyCode.F))
            Execute();


        MovementLogic();

        GatherTargets();
    }

    private void Execute()
    {
        cinemachine.Follow = PlayerManager.instance.player.transform;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

        StartCoroutine(DamageTargets());
    }

    private IEnumerator DamageTargets()
    {
        if(targets.Count > 0)
        {
            Enemy enemy = targets[Random.Range(0, targets.Count)].GetComponent<Enemy>();
            enemy.Damage();
            enemy.Knockback(new Vector2(2, 2), enemy.gameObject.transform.position.x + (Random.Range(1, 10) > 5 ? -1 : 1), 2);
            enemy.mark.SetActive(false);
            targets.Remove(enemy.gameObject.transform);
        }

        yield return new WaitForSeconds(.2f);

        if(targets.Count > 0)
            StartCoroutine(DamageTargets());
        else
            Destroy(gameObject);
    }

    private void GatherTargets()
    {

        if (enemiesToAdd.Count > 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            targets.Add(enemiesToAdd[^1]);

            if(!SkillManager.instance.isSkillUnlocked("Ashen Rain"))
            {
                Execute();
                return;
            }

            enemiesToAdd[^1].GetComponent<Enemy>().mark.SetActive(true);
        }
    }

    private void MovementLogic()
    {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Vector2 distance = new Vector2(Mathf.Abs(initialPosition.x - transform.position.x) - 5, Mathf.Abs(initialPosition.y - transform.position.y) -5);

        Vector2 clamped = new Vector2(Mathf.InverseLerp(0, bounds.x, distance.x), Mathf.InverseLerp(0, bounds.y, distance.y));

        Vector2 finalSpeed = new Vector2(Mathf.Lerp(0, crosshairSpeed, 1 - clamped.x), Mathf.Lerp(0, crosshairSpeed, 1 - clamped.y));

        Vector2 adjustedSpeed = new Vector2(finalSpeed.x, finalSpeed.y);

        transform.Translate(mouseMovement * adjustedSpeed * Time.deltaTime);

        if(mouseMovement == Vector2.zero)
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, crosshairResistance * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!other.GetComponent<Enemy>())
            return;   
        
        if(!targets.Contains(other.gameObject.transform))
            enemiesToAdd.Add(other.gameObject.transform);
    }
    
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(!other.GetComponent<Enemy>())
            return;   

        if(enemiesToAdd.Contains(other.gameObject.transform))
            enemiesToAdd.Remove(other.gameObject.transform);
    }

}
