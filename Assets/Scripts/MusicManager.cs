using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
  public AudioClip[] songs;
  public AudioMixerGroup mixerGroup;
  public float delayBetweenSongs = 1.0f;

  private AudioSource audioSource;
  private int currentSongIndex = 0;
  public Slider volumeSlider;

  void Start()
  {
    // Метод встановлює значення гучності слайдером в меню
    float volume = PlayerPrefs.GetFloat("VolumeSetting", 0.3f);
    Scene activeScene = SceneManager.GetActiveScene();
    if (activeScene.name == "MainScene")
    {
      volumeSlider = FindObjectOfType<Slider>();
      volumeSlider.value = volume;
    }
    audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.volume = volume;
    audioSource.outputAudioMixerGroup = mixerGroup;
    songs = Shuffle(songs); // перемішує список пісень
    PlaySong(currentSongIndex);
  }

  AudioClip[] Shuffle(AudioClip[] clips)
  {
    // Створює новий масив в якому утримує перемішані аудіокліпи
    AudioClip[] shuffledClips = new AudioClip[clips.Length];

    // копіює кліпи в цей масив
    for (int i = 0; i < clips.Length; i++)
    {
      shuffledClips[i] = clips[i];
    }

    // Перемішує масив за метдом Fisher-Yates
    for (int i = shuffledClips.Length - 1; i > 0; i--)
    {
      int j = Random.Range(0, i + 1);
      AudioClip temp = shuffledClips[i];
      shuffledClips[i] = shuffledClips[j];
      shuffledClips[j] = temp;
    }

    // повертає його
    return shuffledClips;
  }


  void Update() // Якщо музика закінчилась, включає наступний кліп
  {
    if (!audioSource.isPlaying)
    {
      currentSongIndex = (currentSongIndex + 1) % songs.Length;
      if (currentSongIndex == 0)
      {
        audioSource.Stop();
      }
      else
      {
        Invoke("PlaySong", delayBetweenSongs);
      }
    }
  }

  void PlaySong()// визначає наступний кліп і запускає його
    {
    PlaySong(currentSongIndex);
  }

  void PlaySong(int songIndex)
  {
    audioSource.clip = songs[songIndex];
    audioSource.Play();
  }
  public void SetVolume(float volume)
  {
    audioSource.volume = volumeSlider.value;
    PlayerPrefs.SetFloat("VolumeSetting", volumeSlider.value);
  }
}
