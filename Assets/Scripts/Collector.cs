using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Collector : MonoBehaviour
{
    [Header("Everything")]
    [SerializeField] private GameObject everything;
    
    [Header("Values")]
    [SerializeField] private int collectedValue = 0;
    [SerializeField] private int maxValue = 50;
    [SerializeField] private float durationToTurnRight = 5;

    [Header("Slider")]
    public Slider slider;
    public Image fill;

    [Header("Prefabs")] 
    [SerializeField] private GameObject goodPrefab;
    [SerializeField] private GameObject sondakiParticlePrefab;
    [SerializeField] private GameObject badPrefab;
    
    [Header("Text")]
    public TextMeshProUGUI textBox;
    [SerializeField] private GameObject unemployed;
    [SerializeField] private GameObject engineer;
    [SerializeField] private GameObject techLover;
    [SerializeField] private GameObject technician;
    [SerializeField] private GameObject winCanvas;
    
    [Header("Gate / Player Object")] 
    [SerializeField] private GameObject gate0Object;
    [SerializeField] private GameObject gate1Object;
    [SerializeField] private GameObject gate2Object;
    [SerializeField] private GameObject gate3Object;


    private GameObject currentPlayer;

    [SerializeField] private GameObject pukeParticlePrefab;
    private void Start()
    {
        UpdateSlider();
        currentPlayer = gate0Object;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("good"))
        {
            collectedValue += 5;
            UpdateSlider();
            Instantiate(goodPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("bad"))
        {
            collectedValue -= 5;
            UpdateSlider();
            Instantiate(badPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Gate1"))
        {
            gate0Object.SetActive(false);
            gate1Object.SetActive(true);
            StartCoroutine(spin(gate1Object));
            currentPlayer = gate1Object;
        }
        else if (other.CompareTag("Gate2"))
        {
            gate0Object.SetActive(false);
            gate1Object.SetActive(false);
            gate2Object.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(spin(gate2Object));
            currentPlayer = gate2Object;
        }
        else if (other.CompareTag("Gate3"))
        {
            gate0Object.SetActive(false);
            gate1Object.SetActive(false);
            gate2Object.SetActive(false);
            gate3Object.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(spin(gate3Object));
            currentPlayer = gate3Object;
        }
        else if (other.CompareTag("canKiss"))
        {
            StartCoroutine(kiss(other.gameObject));
            Instantiate(pukeParticlePrefab, gameObject.transform.position, Quaternion.identity);
            //todo animasyonu oynat
        }
        else if (other.CompareTag("cantKiss"))
        {
            Instantiate(sondakiParticlePrefab, gameObject.transform.position, Quaternion.identity);
            StartCoroutine(kiss(other.gameObject));
        }
        else if (other.CompareTag("turnRight"))
        {
            everything.transform.DORotate(new Vector3(0, -90, 0), durationToTurnRight);
            //gameObject.transform.DOMoveX(transform.position.x-8,5);
        }
        else if (other.CompareTag("FinishLine"))
        {
            FinishLine();
        }
        else if (other.CompareTag("picture"))
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator kiss(GameObject other)
    {
        other.gameObject.GetComponent<Animator>().SetBool("kiss", true);
        gameObject.GetComponent<Movement>().enabled = false;
        currentPlayer.GetComponent<Animator>().SetBool("kiss", true);
        yield return new WaitForSeconds(6);
        currentPlayer.GetComponent<Animator>().SetBool("kiss", false);
        other.gameObject.GetComponent<Animator>().SetBool("kiss", false);
        gameObject.GetComponent<Movement>().enabled = true;
        

        collectedValue -= 15;
        UpdateSlider();
    }
    IEnumerator spin(GameObject character)
    {
        Debug.Log("worked");
        character.GetComponent<Animator>().SetBool("spin",true);
        yield return new WaitForSeconds(1.10f);
        character.GetComponent<Animator>().SetBool("spin",false);
    }
    private IEnumerator FinishLine()
    {
        gameObject.GetComponent<Movement>().enabled = false;
        //todo animasyonu oynat
        
        var animationtime = 0;
        yield return new WaitForSeconds(animationtime);
        winCanvas.SetActive(true);
    }
    

    private void UpdateSlider()
    {
        slider.maxValue = maxValue;
        slider.value = collectedValue;
        fill.fillAmount = slider.value;
    }
    
}