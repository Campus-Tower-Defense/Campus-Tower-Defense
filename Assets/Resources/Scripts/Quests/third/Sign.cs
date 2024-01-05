using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private GameObject C;
    private GameObject A;
    private GameObject M;
    private GameObject P;
    private GameObject U;
    private GameObject S;

    private QuestManager questManager;
    void Start()
    {

        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();

        C = gameObject.transform.Find("C").gameObject;
        if (C == null)
        {
            Debug.LogError("C child not found under sign.");
        }
        else
        {
            C.SetActive(false);
        }

        A = gameObject.transform.Find("A").gameObject;
        if (A == null)
        {
            Debug.LogError("A child not found under sign.");
        }
        else
        {
            A.SetActive(false);
        }

        M = gameObject.transform.Find("M").gameObject;
        if (M == null)
        {
            Debug.LogError("M child not found under sign.");
        }
        else
        {
            M.SetActive(false);
        }

        P = gameObject.transform.Find("P").gameObject;
        if (P == null)
        {
            Debug.LogError("P child not found under sign.");
        }
        else
        {
            P.SetActive(false);
        }

        U = gameObject.transform.Find("U").gameObject;
        if (U == null)
        {
            Debug.LogError("U child not found under sign.");
        }
        else
        {
            U.SetActive(false);
        }

        S = gameObject.transform.Find("S").gameObject;
        if (S == null)
        {
            Debug.LogError("S child not found under sign.");
        }
        else
        {
            S.SetActive(false);
        }
    }

    public void SetLetter(char letter)
    {
        bool isFound = true;
        switch (letter)
        {
            case 'C':
                C.SetActive(true);
                break;
            case 'A':
                A.SetActive(true);
                break;
            case 'M':
                M.SetActive(true);
                break;
            case 'P':
                P.SetActive(true);
                break;
            case 'U':
                U.SetActive(true);
                break;
            case 'S':
                S.SetActive(true);
                break;
            default:
                isFound = false;
                Debug.LogError("Invalid letter.");
                break;
        }

        if (isFound)
        {
            questManager.UpdateQuestProgress(QuestManager.IDENTITY_STONES_QUEST_ID, 1);
        }
    }




}
