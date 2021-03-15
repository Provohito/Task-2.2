using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

	float visible = 8f;// Видимость бота

	private void Awake()
    {
		botAgent = GetComponent<NavMeshAgent>();
	}
    void Start()
	{
		startPointBot = transform.position;
		endPointBot = new Vector3(pointToMove, transform.position.y, transform.position.z);
	}

	void Update()
	{
		curTimeout += Time.deltaTime;
		if (!botAgent.hasPath & curTimeout > timeWait)
		{
			isTarget = !isTarget;
			curTimeout = 0;
		}
		

		float distance = Vector3.Distance(transform.position, player.transform.position);
		if (distance < 1f) //если дистанция меньше указанного значения
		{
			//animator.SetBool("attack", true); //тут я поставил, чтобы проигрывалась анимация атаки, но она проигрывается 1 раз без зацикливания, поэтому выключил
			//Player.SetHPPlayer(damage); //тогда наносим дамаг игроку, вызывая фукнцию в его скрипте (должен быть прикреплен к нему на инспекторе), название функции можете прописать свое, главное сделайте ее в скрипте игрока

		}
		else if (distance < visible) //если дистанция до игрока меньше радиуса видимости
		{
			//animator.SetBool("attack", false);//тогда выключаем анимацию атаки (если была включена), добавил я от себя
			botAgent.destination = player.transform.position; //и передаем агенту навигации координаты игрока, чтобы идти к нему
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


	}

	void Move(Vector3 moveVector)
    {
		botAgent.SetDestination(moveVector);
	}
}
