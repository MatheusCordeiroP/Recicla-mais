using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Controle de jogo
    private int points;
    private int life;
    private bool isRunning;

    //Coisas para Salvar o jogo
    private int maxScore;
    private string gameData;

    //Controle de HUD inGame
    [SerializeField]
    private Text pointsText;
    [SerializeField]
    private Image[] lifeImage;

    //Controle de HUD GameOver
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private Text textHighScore;
    [SerializeField]
    private Text textLastScore;
    [SerializeField]
    private Text textNewHighScore;

    //Controle de HUD Menu
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private Text mainMenuHighScore;
    [SerializeField] private GameObject[] tutorial;

    //Controle de Sons
    [SerializeField] AudioClip[] FezPonto = null;
    [SerializeField] AudioClip[] Perdeu = null;
    [SerializeField] AudioClip[] Errou = null;
    [SerializeField] AudioClip[] NovoJogo = null;
    AudioSource audioSource;

    //Controle de Tutorial
    private bool canSkip = false;
    private int i;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
        LoadGame();
        mainMenuHighScore.text = "Recorde: " + maxScore.ToString();
        //if (maxScore > 3) {
        //    mainMenuScreen.SetActive(true);
        //}
        //else
        //{
            i = 0;
            ShowTutorial();
        //}
    }

    private void Update()
    {
        //Se puder pular e o Toque for maior que 0
        if(Input.touchCount > 0 && canSkip)
        {
            ShowTutorial();
        }
    }

    public void IncreasePoints()
    {
        points++;
        pointsText.text = PadronizarScore(points);
        audioSource.clip = FezPonto[0];
        audioSource.Play();

    }

    public void DecreaseLife()
    {
        life--;

        if (life == 3)
        {
            lifeImage[0].enabled = true;
            lifeImage[1].enabled = true;
            lifeImage[2].enabled = true;
        }
        else if (life == 2)
        {
            lifeImage[2].enabled = false;
            audioSource.clip = Errou[0];
            audioSource.Play();
        }
        else if (life == 1)
        {
            lifeImage[1].enabled = false;
            audioSource.clip = Errou[0];
            audioSource.Play();
        }
        else
        {
            lifeImage[0].enabled = false;
            Invoke("EndGame", 1f);
            audioSource.clip = Errou[0];
            audioSource.Play();
        }
    }

    public void EndGame()
    {
        isRunning = false;
        if(points > maxScore)
        {
            maxScore = points;
            SaveGame();
            textNewHighScore.enabled = true;
            audioSource.clip = Perdeu[0];
            audioSource.Play();
        }
        else
        {
            textNewHighScore.enabled = false;
            audioSource.clip = Perdeu[0];
            audioSource.Play();
        }
        pointsText.enabled = false;
        gameOverScreen.SetActive(true);
        textHighScore.text = PadronizarScore(maxScore);
        textLastScore.text = PadronizarScore(points);
        
    }

    public void NewGame()
    {
        life = 4;
        DecreaseLife();
        gameOverScreen.SetActive(false);
        points = 0;
        pointsText.text = PadronizarScore(points);
        pointsText.enabled = true;
        isRunning = true;
        audioSource.clip = NovoJogo[0];
        audioSource.Play();
    }

    public void ShowTutorial()
    {
        canSkip = false;
        CloseTutorials();
        if (i >= tutorial.Length)
        {
            mainMenuScreen.SetActive(true);
            return;
        }
        tutorial[i].SetActive(true);
        i++;
        Invoke("ChangeTutorial", 2f);
    }

    private void CloseTutorials()
    {
        for(int j = 0; j < tutorial.Length; j++)
        {
            tutorial[j].SetActive(false);
        }
    }

    private void ChangeTutorial()
    {
        canSkip = true;
    }

    public void BeginGame()
    {
        mainMenuScreen.SetActive(false);
        NewGame();
    }

    public void ReturnToMenu()
    {
        mainMenuScreen.SetActive(true);
        mainMenuHighScore.text = "Recorde: " + maxScore.ToString();
        gameOverScreen.SetActive(false);
    }

    //Guarda os dados do jogo em formato Binário
    public void SaveGame()
    {
        //Limpa o gameData
        gameData = "";

        //Deixa o MaxScore sempre com 5 digitos
        gameData = PadronizarScore(maxScore);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, gameData);
        file.Close();
    }

    //Carrega os dados do jogo se existir um jogo salvo, e inicializa as informações como "aaa" e "0" se não houver jogo salvo
    public void LoadGame()
    {
        //Se não tem jogo salvo, define maxScore = 0
        if (!File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            maxScore = 0;
        }
        //Se existe jogo salvo, carrega o maxScore dele
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            gameData = (string)bf.Deserialize(file);
            maxScore = int.Parse(gameData);
            file.Close();
        }
    }

    public string PadronizarScore(int score)
    {
        string returnScore = "";

        if (score < 10) returnScore = "0000" + score.ToString();
        else if (score < 100) returnScore = "000" + score.ToString();
        else if (score < 1000) returnScore = "00" + score.ToString();
        else if (score < 10000) returnScore = "0" + score.ToString();
        else returnScore = score.ToString();

        return returnScore;
    }

    public bool GetIsRunning()
    {
        return isRunning;
    }
}
