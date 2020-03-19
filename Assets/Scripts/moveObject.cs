using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObject : MonoBehaviour {

    private GameManager gm;
    private bool onGround;
    private bool HoldingObject;
    private float groundY = 0;                  //A altura do chão


    //Variáveis para o Chão
    [SerializeField]
    private GameObject groundGameObject;        //Para definir o objeto chão
    private float groundSpeed = 0.01f;          //Para definir a velocidade da esteira

    //Variáveis de Queda
    private float fallingSpeed = 0;             //Variável para controlar velocidade de queda
    private float gravityAcc = 1.05f;           //Variável para definir aceleração da gravidade
    private float maxFallingSpeed = 0.2f;       //Variável para definir velocidade maxima de queda

    //Variaveis para desativar código
    private bool useDeltaMovement = false;      //Para o Objeto se mover até o lugar tocado
    private bool useInstantMovement = false;    //Para o Objeto se teleportar até o lugar tocado
    private bool useDragMovement = true;        //Para o Objeto ser arrastado após tocar nele

    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        groundGameObject = GameObject.Find("Ground");
        //Pega a posição do objeto que foi definido como chão e define o groundY nessa altura.
        if (groundGameObject != null)
            groundY = groundGameObject.transform.position.y;
    }
	
	void Update () {

        if (!gm.GetIsRunning())
            Destroy(this.gameObject);

        //Se o Objeto estiver abaixo do nível da esteira, ele volta para o nível da esteira
        if (this.gameObject.transform.position.y <= groundY)
            gameObject.transform.position = new Vector3(this.transform.position.x, groundY, 0);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Pega o primeiro toque na tela

            // Pega a posição do toque em relação àquela posição no mundo
            Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

            //Para não passar do limite do chão
            if (touchedPos.y < groundY)
                touchedPos.y = groundY;
            
            //Se o seu dedo tocou o objeto, ele é guiado pelo seu dedo
            if(useDragMovement)
            {
                //Checa se o toque está na posição da Sprite
                bool overSprite = this.GetComponent<SpriteRenderer>().bounds.Contains(touchedPos);

                //Se o toque estava na posição da Sprite no começo do toque, o Objeto está sendo segurado até que toque pare.
                if (overSprite && touch.phase == TouchPhase.Began)
                {
                    HoldingObject = true;
                }
                if(HoldingObject && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Ended))
                {
                    HoldingObject = false;
                }

                if(HoldingObject)
                {
                    fallingSpeed = 0;
                    transform.position = touchedPos;
                }

            }

            //Teleporta o Objeto para onde seu dedo tocou
            if (useInstantMovement)
            {
                transform.position = touchedPos;
            }

            //O Objeto caminha até a direção onde seu dedo tocou
            if (useDeltaMovement)
            {
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    // Leva o Objeto até a posição onde está o seu dedo.
                    transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
                }
            }
            
        }
        
        if (!HoldingObject)
        {
            if(fallingSpeed == 0)
            fallingSpeed += 0.05f;
            fallingSpeed *= gravityAcc;
            if (fallingSpeed > maxFallingSpeed)
                fallingSpeed = maxFallingSpeed;

            if (transform.position.y - fallingSpeed < groundY)
                fallingSpeed = transform.position.y - groundY;

            this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - fallingSpeed, 0);

            if (gameObject.transform.position.y <= groundY + 0.1f)
            {
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x - groundSpeed, groundY, 0);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!HoldingObject)
        {
            if(this.gameObject.tag == collision.gameObject.tag)
            {
                //Adiciona pontos
                gm.IncreasePoints();
                Destroy(this.gameObject);
            }
            else
            {
                //Tira vida, não adiciona ponto
                gm.DecreaseLife();
                Destroy(this.gameObject);
            }
            
        }
    }
}
