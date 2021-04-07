using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

enum BotParams : int
{
	First = 1,
	Second = 2,
	Third = 3,
	Fourth = 4,
	Fifth = 5
}

public class BotAgentScript : MonoBehaviour
{
    //  Напишите позицию, куда должен идти бот(Брал только по х)
    [SerializeField]
    int pointToMove;
	private NavMeshAgent botAgent;
	[SerializeField]
	GameObject player;

	Vector3 startPointBot;
	Vector3 endPointBot;
	int timeWait = 3;
	float curTimeout = 0;

	bool isTarget;

	[SerializeField]
	BotParams botNumber;
	Botparams paramsBot = new Botparams();

	float visible = 5f;// Видимость бота
	[Header("Работа с Hp персонажа")]
	[SerializeField] private Text currentHPText;
	[SerializeField] private Slider sliderHP;


	[Space(10)]
	[SerializeField] private int maxHP;
	[SerializeField] private int minHP;

	public int currentHP;

	[SerializeField]
	GameObject uiController;
	[SerializeField]
	GameObject diePanel;

	int health;
	int damage;
	int speedDamage;


	private void Awake()
    {
		botAgent = GetComponent<NavMeshAgent>();
		SetParams();
	}
	void SetParams()
    {
        switch ((int)botNumber)
        {
			case 1:
				health = paramsBot.First[0];
				damage = paramsBot.First[1];
				speedDamage = paramsBot.First[2];
				break;
			case 2:
				health = paramsBot.Second[0];
				damage = paramsBot.Second[1];
				speedDamage = paramsBot.Second[2];
				break;
			case 3:
				health = paramsBot.Third[0];
				damage = paramsBot.Third[1];
				speedDamage = paramsBot.Third[2];
				break;
			case 4:
				health = paramsBot.Fourth[0];
				damage = paramsBot.Fourth[1];
				speedDamage = paramsBot.Fourth[2];
				break;
			case 5:
				health = paramsBot.Fifth[0];
				damage = paramsBot.Fifth[1];
				speedDamage = paramsBot.Fifth[2];
				break;

		}
    }
    void Start()
	{
		startPointBot = transform.position;
		endPointBot = new Vector3(pointToMove, transform.position.y, transform.position.z);

		sliderHP.maxValue = maxHP;
		sliderHP.minValue = minHP;

		currentHP = maxHP;

	}

	void Update()
	{
		HPCheck();
		BotDeath();

		curTimeout += Time.deltaTime;
		if (!botAgent.hasPath & curTimeout > timeWait)
		{
			isTarget = !isTarget;
			curTimeout = 0;
			
		}

		
		float distance = Vector3.Distance(transform.position, player.transform.position);
		if (distance < 2f) //если дистанция меньше указанного значения
		{
            if (player.GetComponent<AgentController>().currentHP != 0)
			{ 
				//StartCoroutine(DamageTake());
			}
		}
		else if (distance < visible) //если дистанция до игрока меньше радиуса видимости
		{
			
			botAgent.destination = player.transform.position + new Vector3(2, 0, 2); //и передаем агенту навигации координаты игрока, чтобы идти к нему + расстояние, между ботом и игроком
			SetRotation(player.transform.position);
		}
		else
		{
			if (isTarget)
			{
				Move(endPointBot);
			}
			else
			{
				Move(startPointBot);
			}
		}
		
		sliderHP.value = currentHP;
		currentHPText.GetComponent<Text>().text = string.Format("{0:0}", currentHP);



	}

	void Move(Vector3 moveVector)
    {
		botAgent.SetDestination(moveVector);
	}

	void SetRotation(Vector3 lookAt)
	{
		Vector3 targetPoint = player.transform.position;
		Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.deltaTime);
	}
	
	IEnumerator DamageTake(int _damage, int _damageSpeed)
    {
		yield return new WaitForSeconds(1f);
		player.GetComponent<AgentController>().currentHP -= UnityEngine.Random.Range(1,15);
		currentHP -= UnityEngine.Random.Range(20, 50);
		StopAllCoroutines();
    }
	private void HPCheck()
	{
		if (currentHP >= maxHP)
			currentHP = maxHP;
	}

	private void BotDeath()
	{
		if (currentHP <= minHP)
		{
			currentHP = minHP;
			StopAllCoroutines();
			StartCoroutine(DieBot());
			

		}
	}
	
	IEnumerator DieBot()
	{
		Debug.Log("Die");
		Destroy(this.gameObject);
		uiController.GetComponent<UiController>().OpenDiePanel(diePanel);
		yield return new WaitForSeconds(0.5f);
	}

	
}
