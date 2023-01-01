using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgessBar : MonoBehaviour, IDataPersistence
{
    private static Slider slider;
    private int fillSpeed = 1;
    private static int targetProgress = 0;
    public TextMeshProUGUI fishingSkillText;
    [SerializeField] private ParticleSystem dingPS;
    private bool areParticlesPlaying;
    private GameObject player;
    private static float progressBarValue;
    private int childObjCount;

    public void LoadData(GameData data) => progressBarValue = data.progressBarValue;
    public void SaveData(GameData data) => data.progressBarValue = (int)progressBarValue;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        slider.value = progressBarValue;
        slider.maxValue = 100;
        fishingSkillText.text = DisplayFishSkillText();
        childObjCount = transform.childCount;
    }

    private void Update()
    {
        if (InputManager.Instance.fishingControlsEnabled)
        {
            for (int i = 0; i < childObjCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < childObjCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            return;
        }

        if (slider.value >= 100) return;

        if (slider.value < targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
            fishingSkillText.text = DisplayFishSkillText();
            progressBarValue = slider.value;
        }

        if (slider.value == 100 && !areParticlesPlaying) 
        {
            Debug.Log("Play particles");
            if (player != null)
            {
                dingPS.transform.position = player.transform.position;
            }
            
            dingPS.Play();
            areParticlesPlaying = true;
        }
    }

    public static void IncrementFishingSkillBar(int newProgress)
    {
        targetProgress = (int)(slider.value + newProgress);
    }

    public string DisplayFishSkillText() => $"Fishing Skill {slider.value:0} / 100";
}
