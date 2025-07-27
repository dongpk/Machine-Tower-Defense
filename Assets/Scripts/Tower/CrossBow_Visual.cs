using System.Collections;
using UnityEngine;

public class CrossBow_Visual : MonoBehaviour
{
  
    private Enemy myEnemy;
    [SerializeField] private LineRenderer attackLineVisuals;
    [SerializeField] private float attackVisualDuration = 0.1f;
    [Space]

    [Header("Glowing Visuals")]


    [Space]
    [SerializeField] private MeshRenderer meshRenderer;
    private float currentInsensity;
    [SerializeField] private float maxIntensity = 150;
    private Material material;
    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("rotor visuals")]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorUnloaded;
    [SerializeField] private Transform rotorLoaded;


    [Header("Front Glow String")]
    [SerializeField] private LineRenderer frontString_L;
    [SerializeField] private LineRenderer frontString_R;
    [Space]
    [SerializeField] private Transform frontStartPonint_L;
    [SerializeField] private Transform frontStartPonint_R;
    [SerializeField] private Transform frontEndPonint_L;
    [SerializeField] private Transform frontEndPonint_R;

    [Header("Back Glow String")]
    [SerializeField] private LineRenderer backString_L;
    [SerializeField] private LineRenderer backString_R;
    [Space]
    [SerializeField] private Transform backtStartPonint_L;
    [SerializeField] private Transform backStartPonint_R;
    [SerializeField] private Transform backEndtPonint_L;
    [SerializeField] private Transform backEndtPonint_R;

    [SerializeField] private LineRenderer[] lineRenderers; 




    void Awake()
    {
    
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
        UpdateMaterialLineRender();
        StartCoroutine(ChangeEmmision(1));
    }

    private void UpdateMaterialLineRender()
    {
        foreach (var l in lineRenderers)
        {
            l.material = material;
        }
    }

    void Update()
    {
        UpdateEmmissionColor();
        UpdateStrings();
        UpdateAttackVisualIfNeeded();
    }

    private void UpdateAttackVisualIfNeeded()
    {
        if (attackLineVisuals.enabled && myEnemy != null)
        {
            attackLineVisuals.SetPosition(1, myEnemy.CenterPoint());
        }
    }

    private void UpdateStrings()
    {
        UpdateStringVisuals(frontString_R, frontStartPonint_R, frontEndPonint_R);
        UpdateStringVisuals(frontString_L, frontStartPonint_L, frontEndPonint_L);
        UpdateStringVisuals(backString_R, backStartPonint_R, backEndtPonint_R);
        UpdateStringVisuals(backString_L, backtStartPonint_L, backEndtPonint_L);
    }

    public void PlayReloadVFX(float duration)
    {
        float newDuration = duration / 2;
        StartCoroutine(ChangeEmmision(newDuration));
        StartCoroutine(UpdateRotorPosition(newDuration));
    }
    private void UpdateEmmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentInsensity / maxIntensity);
        emissionColor *= Mathf.LinearToGammaSpace(currentInsensity); // Convert to gamma space for emission
        material.SetColor("_EmissionColor", emissionColor);
    }
    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint,newEnemy));
    }


    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint,Enemy newEnemy )
    {
       
        myEnemy = newEnemy;
        
        attackLineVisuals.enabled = true;
        attackLineVisuals.SetPosition(0, startPoint);
        attackLineVisuals.SetPosition(1, endPoint);
        yield return new WaitForSeconds(attackVisualDuration);
        attackLineVisuals.enabled = false;       
    }
    private IEnumerator ChangeEmmision(float duration)
    {
        float startTime = Time.time;
        float startIntensity = 0;

        while (Time.time - startTime < duration)
        {

            //tinh toan ti le thoi gian
            float tValue = (Time.time - startTime) / duration;
            currentInsensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null;
        }
        currentInsensity = maxIntensity;
    }
    private IEnumerator UpdateRotorPosition(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float tValue = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorUnloaded.position, rotorLoaded.position, tValue);
            yield return null;
        }

        rotor.position = rotorLoaded.position; // Ensure it ends at the loaded position
    }
    private void UpdateStringVisuals(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
   
}
