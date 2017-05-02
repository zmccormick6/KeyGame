using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureKeyvilController : MonoBehaviour
{
    [SerializeField] private ParticleSystem Finished;
    [SerializeField] private ParticleSystem Finished2;

    [SerializeField] private AudioSource BossHealthGain;

    [SerializeField] private GameObject BossAttack;
    [SerializeField] private GameObject SecondBossAttack;
    [SerializeField] private GameObject ThirdBossAttack;
    [SerializeField] private GameObject PhaseOneAttack;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Keese;
    [SerializeField] private GameObject Mage;
    [SerializeField] private GameObject CurrentDoor;
    [SerializeField] private GameObject BossHealthBar;
    [SerializeField] private GameObject BossHealth;
    //[SerializeField] private GameObject Blink;
    [SerializeField] private Collider2D BossCollider;

    GameObject GameManager;

    AudioSource AttackSound;
    SpriteRenderer tempSprite;

    private Collider2D PlayerCollider;
    private Animator animator;
    public int Health = 40;

    Vector2 CurrentPosition;
    Vector2 PlayerPosition;

    Vector2 TopLeft = new Vector2(-6, 1);
    Vector2 TopRight = new Vector2(6, 1);
    Vector2 MiddleRight = new Vector2(6, -1.5f);
    Vector2 MiddleLeft = new Vector2(-6, -1.5f);
    Vector2 BottomRight = new Vector2(6, -4);
    Vector2 BottomLeft = new Vector2(-6, -4);
    Vector2 Middle = new Vector2(0, -1.5f);

    public bool attackThree = false, spawnHealth = false;
    public int temp = 7;

    bool phaseChangeCheck = false, sharedHealth = false, startTimer = false, once = false, phaseTwo = false, limit = true;
    int lastTemp, attackChoose, lastChoice;
    float attackSwitch = 1f, moveX = 0, moveY = 0, currentTime = 3f, bossHealth = 1;

    void Start()
    {
        Player = GameObject.Find("TempPlayer");

        PlayerCollider = Player.GetComponents<BoxCollider2D>()[1];
        animator = GetComponent<Animator>();
        tempSprite = gameObject.GetComponent<SpriteRenderer>();
        GameManager = GameObject.Find("Game Manager");

        AttackSound = GameObject.Find("Attack").GetComponent<AudioSource>();

        BossHealthBar = GameObject.Find("BossBar");
        BossHealth = GameObject.Find("BossHealth");
    }

    void FixedUpdate()
    {
        CurrentPosition = new Vector2(transform.position.x, transform.position.y);
        PlayerPosition = new Vector2(Player.transform.position.x, Player.transform.position.y);

        if (Health <= 0)
        {
            GameObject.Find("Game Manager").GetComponent<DoorSpawn>().EnemyCheck();
            Debug.Log("Dead");
        }

        if (temp == 0)
        {
            if (limit == true)
                transform.position = Vector2.MoveTowards(transform.position, TopLeft, 0.2f);
        }
        else if (temp == 1)
        {
            if (limit == true)
                transform.position = Vector2.MoveTowards(transform.position, TopRight, 0.2f);
        }
        else if (temp == 2)
        {
            if (limit == true)
                transform.position = Vector2.MoveTowards(transform.position, BottomRight, 0.2f);
        }
        else if (temp == 3)
        {
            if (limit == true)
                transform.position = Vector2.MoveTowards(transform.position, BottomLeft, 0.2f);
        }
        else if (temp == 4)
        {
            if (limit == true)
                transform.position = Vector2.MoveTowards(transform.position, Middle, 0.2f);
        }
        else if (temp == 5)
        {
            if (limit == true)
                transform.position = Vector2.MoveTowards(transform.position, MiddleLeft, 0.2f);
        }
        else if (temp == 6)
       {
            if (limit == true)
                transform.position = Vector2.MoveTowards(transform.position, MiddleRight, 0.2f);
        }

        if (startTimer == true)
        {
            Timer();
        }

        if (Health == 20)
        {
            if (once == false)
            {
                startTimer = false;
                StopAllCoroutines();
                gameObject.GetComponent<Collider2D>().enabled = false;
                tempSprite.enabled = true;
                StartCoroutine(PhaseChange());

                once = true;
            }
        }

        if (Health <= 0)
        {
            if (once == true)
            {
                StopAllCoroutines();
                StartCoroutine(Death());
                once = false;
            }
        }
    }

    private IEnumerator Death()
    {
        //StopAllCoroutines();

        GameObject[] Attacks = GameObject.FindGameObjectsWithTag("MageAttack");

        for (int i = 0; i < Attacks.Length; i++)
        {
            Destroy(Attacks[i]);
        }

        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        tempSprite.color = new Color(1f, 1f, 1f, 1f);
        temp = 4;
        StartCoroutine(Teleport());
        GameManager.GetComponent<LevelSwitch>().pause = true;
        StartCoroutine(MovePlayer());
        yield return new WaitForSeconds(1);
        animator.SetInteger("Death", 1);
        GameObject.Find("Main Camera").GetComponent<CameraShake>().ShakeCameraBoss();
        GameManager.GetComponent<SoundController>().BossMusicOff();
        gameObject.GetComponent<AudioSource>().Play();
        var finished = Instantiate(Finished, new Vector2(0, 6), transform.rotation);
        var finishedTwo = Instantiate(Finished2, new Vector2(0, 6), transform.rotation);
        finished.Play();
        finishedTwo.Play();

        GameObject[] Mages;

        Mages = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < Mages.Length; i++)
        {
            if (Mages[i].name == "Mage(Clone)")
                Destroy(Mages[i]);
            else if (Mages[i].name == "Keese(Clone)")
                Destroy(Mages[i]);
        }

        yield return new WaitForSeconds(5);
        finished.Stop();
        finishedTwo.Stop();
        BossHealthBar.SetActive(false);
        BossHealth.SetActive(false);
        GameManager.GetComponent<SoundController>().PlayEndMusic();
        Destroy(gameObject);
        GameManager.GetComponent<LevelSwitch>().pause = false;
        GameObject.Find("Keyvi").GetComponent<KeyviController>().dead = true;
        CurrentDoor.GetComponent<Animator>().SetInteger("DoorOpen", 1);
    }

    private IEnumerator OneMove()
    {
        yield return new WaitForSeconds(3);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Enemy" && other.tag != "Wall" && other.tag != "Keyvi" && other.tag != "Obstacle" && other.tag != "Water" && other.tag != "Right" && other.tag != "Left")
        {
            if (other.tag != "Hitbox")
            {
                if (other != PlayerCollider)
                {
                    StartCoroutine(Invincibility());

                    if (CurrentPosition.y > PlayerPosition.y)
                    {
                        transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y + 0.75f);
                    }
                    else if (CurrentPosition.y < PlayerPosition.y)
                    {
                        transform.position = new Vector2(CurrentPosition.x, CurrentPosition.y - 0.75f);
                    }

                    //StartCoroutine(Hit());
                    AttackSound.Play();
                    PureKeyvilHealth();
                }
            }
        }
    }

    private IEnumerator Teleport()
    {
        Vector2 Position = Middle;

        limit = false;
        animator.SetInteger("Teleport", 1);
        yield return new WaitForSeconds(0.7f);

        if (temp == 0)
        {
            Position = TopLeft;
        }
        else if (temp == 1)
        {
            Position = TopRight;
        }
        else if (temp == 2)
        {
            Position = BottomRight;
        }
        else if (temp == 3)
        {
            Position = BottomLeft;
        }
        else if (temp == 4)
        {
            Position = Middle;
        }
        else if (temp == 5)
        {
            Position = MiddleLeft;
        }
        else if (temp == 6)
        {
            Position = MiddleRight;
        }

        transform.position = Position;
        animator.SetInteger("Teleport", 0);
        limit = true;
        //yield return new WaitForSeconds(1f);
    }

    private IEnumerator Invincibility()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.35f);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    private IEnumerator Hit()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1.5f);
        temp = 7;
        moveX = Random.Range(-6.5f, 6.5f);
        moveY = Random.Range(-3.5f, 1f);

        transform.position = new Vector2(moveX, moveY);
        StartCoroutine(ChooseAttack());
    }

    public void MoveToCenter()
    {
        temp = 4;
    }

    public void PureKeyvilHealth()
    {
        if (Health >= 0)
        {
            Health--;
            bossHealth -= 0.05f;
            //BossHealth.GetComponent<RectTransform>().localScale = new Vector3(bossHealth, 1, 1);
            BossHealth.GetComponent<RectTransform>().localScale = new Vector3(1, bossHealth, 1);
            StartCoroutine(HitFlashing());
        }
    }

    public void Scale()
    {
        BossHealth.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 1);
        BossHealthBar.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 1);
    }

    private IEnumerator HitFlashing()
    {
        tempSprite.color = new Color(1f, 1f, 1f, 0.5f);

        int flashing = 0;

        for (int i = 0; i < 10; i++)
        {
            if (flashing % 2 == 0)
            {
                tempSprite.enabled = false;
            }

            else if (flashing % 2 == 1)
            {
                tempSprite.enabled = true;
            }

            flashing++;

            yield return new WaitForSeconds(0.075f);
        }
        tempSprite.color = new Color(1f, 1f, 1f, 1f);
    }

    public void ChangePositions()
    {
        temp = Random.Range(0, 7);

        if (lastTemp == temp)
        {
            if (temp == 0 || temp == 6)
            {
                temp = Random.Range(1, 5);
            }
            else
                temp += 1;
        }

        if (temp == 4)
        {
            temp = 5;
        }

        lastTemp = temp;
    }

    public void ChooseAttackPublic()
    {
        StartCoroutine(PhaseChange());
    }

    public void StartPhaseOne()
    {
        StartCoroutine(PhaseOne());
        spawnHealth = true;
    }

    public IEnumerator PhaseOne()
    {
        /*for (int i = 0; i < 8; i++)
        {
            Instantiate(ThirdBossAttack, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }*/

        //gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);

        //gameObject.GetComponent<Collider2D>().enabled = false;

        //ChangePositions();
        startTimer = true;

        GameManager.GetComponent<SoundController>().TurnOnMusic();
        //BossHealthBar.SetActive(true);
        //BossHealth.SetActive(true);

        Instantiate(Mage, new Vector2(-30, 0), Quaternion.identity);

        for (int i = 0; i < 200; i++)
        {
            Instantiate(Keese, new Vector2(-10, 0), Quaternion.identity);
            yield return new WaitForSeconds(5f);
            Instantiate(Keese, new Vector2(10, 0), Quaternion.identity);
            yield return new WaitForSeconds(5f);
            Instantiate(Mage, new Vector2(-30, 0), Quaternion.identity);
        }

        //Instantiate(Mage, new Vector2(-30, 0), Quaternion.identity);
    }

    public void ChangePlaces()
    {
        ChangePositions();
    }
    public void Timer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            ChangePositions();
            currentTime = 3f;
        }
    }

    private IEnumerator PhaseChange()
    {
        GameObject[] Mages;
        GameObject[] Attacks;

        Mages = GameObject.FindGameObjectsWithTag("Enemy");
        Attacks = GameObject.FindGameObjectsWithTag("MageAttack");

        for (int i = 0; i < Mages.Length; i++)
        {
            if (Mages[i].name == "Mage(Clone)")
                Destroy(Mages[i]);
            else if (Mages[i].name == "Keese(Clone)")
                Destroy(Mages[i]);
        }

        for (int i = 0; i < Attacks.Length; i++)
        {
            if (Attacks[i].name == "MageAttack(Clone)")
                Destroy(Attacks[i]);
        }

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        temp = 4;
        GetComponent<Animator>().SetInteger("PhaseChange", 2);
        GameManager.GetComponent<LevelSwitch>().pause = true;
        StartCoroutine(MovePlayer());
        GameObject.Find("Main Camera").GetComponent<CameraShake>().ShakeCameraBoss();
        var finished = Instantiate(Finished, new Vector2(0, 6), transform.rotation);
        var finishedTwo = Instantiate(Finished2, new Vector2(0, 6), transform.rotation);
        finished.Play();
        finishedTwo.Play();
        StartCoroutine(RefillHealth());
        Instantiate(BossHealthGain, transform.position, transform.rotation);
        BossHealthGain.Play();
        yield return new WaitForSeconds(5f);

        //bossHealth = 1;
        //BossHealth.GetComponent<RectTransform>().localScale = new Vector3(bossHealth, 1, 1);
        //BossHealth.GetComponent<RectTransform>().localScale = new Vector3(1, bossHealth, 1);
        BossHealthBar.GetComponent<Animator>().SetInteger("PhaseTwo", 1);
        GameManager.GetComponent<LevelSwitch>().pause = false;
        gameObject.GetComponent<Collider2D>().enabled = true;
        phaseTwo = true;
        finished.Stop();
        finishedTwo.Stop();
        StartCoroutine(PhaseTwoSpawn());
        StartCoroutine(ChooseAttack());
    }

    private IEnumerator RefillHealth()
    {
        for (float i = 0; i < 1.05; i += 0.05f)
        {
            bossHealth = i;
            BossHealth.GetComponent<RectTransform>().localScale = new Vector3(1, bossHealth, 1);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator MovePlayer()
    {
        Player.GetComponent<SpriteRenderer>().enabled = true;
        float elapsed = 0.0f;

        while (elapsed < 2)
        {
            elapsed += Time.deltaTime;

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, new Vector2(0, -1.5f), -0.015f);

            if (Player.transform.position.x > 6.5f)
                break;
            if (Player.transform.position.x < -6.5f)
                break;
            if (Player.transform.position.y > 0f)
                break;
            if (Player.transform.position.y < -4f)
                break;
        }

        yield return null;
    }

    private IEnumerator ChooseAttack()
    {
        yield return new WaitForSeconds(0f);

        //gameObject.GetComponent<Collider2D>().enabled = false;

        int attackChoose = Random.Range(1, 4);

        while (lastChoice == attackChoose)
        {
            attackChoose = Random.Range(1, 3);
        }
        lastChoice = attackChoose;
        //attackChoose = 3;

        if (attackChoose == 1)
        {
            StartCoroutine(Attack());
        }
        else if (attackChoose == 2)
        {
            StartCoroutine(SecondAttack());
        }
        else if (attackChoose == 3)
        {
            StartCoroutine(ThirdAttack());
        }
    }

    private IEnumerator PhaseTwoSpawn()
    {
        for (int i = 0; i < 200; i++)
        {
            Instantiate(Keese, new Vector2(-20, 0), Quaternion.identity);
            yield return new WaitForSeconds(7.5f);
        }
    }

    private IEnumerator Attack()
    {
        ChangePositions();
        StartCoroutine(Teleport());

        yield return new WaitForSeconds(attackSwitch);

        for (int i = 0; i < 24; i++)
        {
            //Blink.SetActive(true);

            var Missile = Instantiate(BossAttack, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            Missile.GetComponent<BossAttack>().AttackDirection(i % 8);
            yield return new WaitForSeconds(0.1f);
        }

        //Blink.SetActive(false);
        gameObject.GetComponent<Collider2D>().enabled = true;

        StartCoroutine(ChooseAttack());
    }

    private IEnumerator SecondAttack()
    {
        temp = Random.Range(5, 7);
        StartCoroutine(Teleport());

        yield return new WaitForSeconds(attackSwitch);

        GameObject Missile;

        for (int i = 0; i < 4; i++)
        {
            //Blink.SetActive(true);

            if (i % 2 == 0)
            {
                Missile = Instantiate(SecondBossAttack, new Vector3(transform.position.x, transform.position.y - 2.5f, 0), Quaternion.identity);
                Missile.GetComponent<SecondBossAttack>().yPosition(i);
            }
            else
            {
                Missile = Instantiate(SecondBossAttack, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                Missile.GetComponent<SecondBossAttack>().yPosition(i);
            }

            Missile.GetComponent<SecondBossAttack>().Direction(temp);
            yield return new WaitForSeconds(1f);

            //Blink.SetActive(false);
        }

        gameObject.GetComponent<Collider2D>().enabled = true;

        StartCoroutine(ChooseAttack());
    }

    private IEnumerator ThirdAttack()
    {
        //attackThree = false;

        temp = 4;
        StartCoroutine(Teleport());

        yield return new WaitForSeconds(attackSwitch);

        GameObject Missile;

        for (int i = 1; i < 8; i++)
        {
            //Blink.SetActive(true);

            Missile = Instantiate(ThirdBossAttack, new Vector3(transform.position.x, transform.position.y + i, 0), Quaternion.identity);
            Missile.GetComponent<ThirdBossAttack>().ChooseRadius(i);

            yield return new WaitForSeconds(0.25f);
            //Blink.SetActive(false);
        }

        attackThree = true;

        gameObject.GetComponent<Collider2D>().enabled = true;
        //yield return new WaitForSeconds(2f);

        StartCoroutine(ChooseAttack());
    }
}
