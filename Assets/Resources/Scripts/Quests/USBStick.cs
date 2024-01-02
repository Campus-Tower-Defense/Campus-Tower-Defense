using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USBStick : MonoBehaviour
{
    public void OnCollect(){
        GameObject questManagerObject = GameObject.Find("QuestManager");
        if(questManagerObject == null){
            Debug.LogError("QuestManager not found");
            return;
        }
        QuestManager questManager = questManagerObject.GetComponent<QuestManager>();
        if(questManager == null){
            Debug.LogError("QuestManager not found");
            return;
        }
        questManager.UpdateQuestProgress(QuestManager.USB_STICK_QUEST_ID, 1);
    }
}
