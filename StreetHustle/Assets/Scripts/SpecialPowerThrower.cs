using UnityEngine;
using UnityEngine.UI;

public class SpecialPowerThrower : MonoBehaviour
{
    public Slider powerSlider;           
    public GameObject projectilePrefab;   
    public Transform throwPoint;          

    [Header("Special Settings")]
    public int maxPower = 10;
    public float throwForce = 10f;
    public float rapidFireRate = 5f;    
    public int projectilesPerShot = 5;    
    public float spreadAngle = 10f;       

    [Header("Effects")]
    public Image sliderFill;             
    public Color normalColor = Color.white;
    public Color readyColor = Color.green;
    public AudioSource audioSource;
    public AudioClip readySound;         

    private float currentCharge = 1f;
    private bool isRapidFiring = false;
    private float fireTimer = 0f;
    private bool playedReadySound = false;

    [SerializeField] private Shooting _Shooting;


    [Header("Weather Effects")]
    public Material rainySkybox;
    public Material normalSkybox;
    public GameObject rainParticleSystem;
    public GameObject thunderParticleSystem;
    public GameObject lightningParticleSystem1;
    public GameObject lightningParticleSystem2;

    [Header("Weather Sounds")]
    public AudioSource rainAudioSource;
    public AudioSource lightningAudioSource1;
    public AudioSource lightningAudioSource2;
    public AudioSource thunderAudioSource;

    [Header("Lightning & Thunder Timing")]
    public float minLightningDelay = 2f;
    public float maxLightningDelay = 5f;
    public float minThunderDelay = 3f;
    public float maxThunderDelay = 6f;


    private float lightningTimer = 0f;
    private float nextLightningTime = 0f;
    private float thunderTimer = 0f;
    private float nextThunderTime = 0f;

    void Start()
    {
        if (powerSlider != null)
        {
            powerSlider.minValue = 1f;
            powerSlider.maxValue = maxPower;
            powerSlider.value = currentCharge;
        }

        if (sliderFill != null)
            sliderFill.color = normalColor;

        DeactivateWeather();
    }

    void Update()
    {
        if (currentCharge < maxPower)
        {
            currentCharge += Time.deltaTime / 2f;  
            currentCharge = Mathf.Min(currentCharge, maxPower);

            if (powerSlider != null)
                powerSlider.value = currentCharge;
        }

        if (currentCharge >= maxPower)
        {
            if (sliderFill != null)
                sliderFill.color = readyColor;

            if (!playedReadySound && audioSource != null && readySound != null)
            {
                audioSource.PlayOneShot(readySound);
                playedReadySound = true;
            }
        }

        if (currentCharge >= maxPower &&  Input.GetKey(KeyCode.Space))
        {
            isRapidFiring = true;
            ActivateWeather();
            HandleLightningAndThunder();
        }
      

        if (isRapidFiring)
        {
            fireTimer += Time.deltaTime;
            powerSlider.value -= Time.deltaTime;
            if (fireTimer >= rapidFireRate)
            {
                _Shooting.ShootBullet();
                // ThrowProjectiles();
                fireTimer = 0f;
            }else if (powerSlider.value == 1)
            {
                isRapidFiring = false;
                currentCharge = 1f;
                if (powerSlider != null)
                    powerSlider.value = currentCharge;

                if (sliderFill != null)
                   // sliderFill.color = normalColor;

                playedReadySound = false;
                DeactivateWeather();
            }

        }
    }

    void ThrowProjectiles()
    {
        if (projectilePrefab != null && throwPoint != null)
        {
            for (int i = 0; i < projectilesPerShot; i++)
            {
                GameObject obj = Instantiate(projectilePrefab, throwPoint.position, throwPoint.rotation);
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    float angle = (i - (projectilesPerShot - 1) / 2f) * spreadAngle;
                    Quaternion spreadRotation = Quaternion.Euler(0, angle, 0);
                    Vector3 direction = spreadRotation * throwPoint.forward;

                    rb.AddForce(direction * throwForce, ForceMode.Impulse);
                }
                Destroy(obj, 3f); 
            }
        }
    }
    void ActivateWeather()
    {
        if (rainySkybox != null)
            RenderSettings.skybox = rainySkybox;

        if (rainParticleSystem) rainParticleSystem.SetActive(true);
        if (thunderParticleSystem) thunderParticleSystem.SetActive(true);
        if (lightningParticleSystem1) lightningParticleSystem1.SetActive(true);
        if (lightningParticleSystem2) lightningParticleSystem2.SetActive(true);

        if (rainAudioSource && !rainAudioSource.isPlaying) rainAudioSource.Play();

        lightningTimer = 0f;
        nextLightningTime = Random.Range(minLightningDelay, maxLightningDelay);
        thunderTimer = 0f;
        nextThunderTime = Random.Range(minThunderDelay, maxThunderDelay);
    }

    void DeactivateWeather()
    {
        if (normalSkybox != null)
            RenderSettings.skybox = normalSkybox;

        if (rainParticleSystem) rainParticleSystem.SetActive(false);
        if (thunderParticleSystem) thunderParticleSystem.SetActive(false);
        if (lightningParticleSystem1) lightningParticleSystem1.SetActive(false);
        if (lightningParticleSystem2) lightningParticleSystem2.SetActive(false);

        if (rainAudioSource && rainAudioSource.isPlaying) rainAudioSource.Stop();
        if (lightningAudioSource1 && lightningAudioSource1.isPlaying) lightningAudioSource1.Stop();
        if (lightningAudioSource2 && lightningAudioSource2.isPlaying) lightningAudioSource2.Stop();
        if (thunderAudioSource && thunderAudioSource.isPlaying) thunderAudioSource.Stop();
    }

    void HandleLightningAndThunder()
    {
        lightningTimer += Time.deltaTime;
        thunderTimer += Time.deltaTime;

        if (lightningTimer >= nextLightningTime)
        {
            if (lightningAudioSource1) lightningAudioSource1.Play();
            if (lightningAudioSource2) lightningAudioSource2.Play();
            lightningTimer = 0f;
            nextLightningTime = Random.Range(minLightningDelay, maxLightningDelay);
        }

        if (thunderTimer >= nextThunderTime)
        {
            if (thunderAudioSource) thunderAudioSource.Play();
            thunderTimer = 0f;
            nextThunderTime = Random.Range(minThunderDelay, maxThunderDelay);
        }
    }
}
