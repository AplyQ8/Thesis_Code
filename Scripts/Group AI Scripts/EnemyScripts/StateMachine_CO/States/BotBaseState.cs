using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс-абстракция состояний
public abstract class BotBaseState: MonoBehaviour
{
    //Поле для имени состояния
    public string StateName { get; protected set; }
    //Функция, срабатываюшая при захождении в состояние
    public abstract void EnterState(BotStateMachine botStateMachine);
    //Функция, характеризующая выполняемые действия состояния
    public abstract void UpdateAction();
    //Функция, проверяющая условие на выход из текущего состояния
    public abstract void UpdateState(BotStateMachine botStateMachine);
    

}
