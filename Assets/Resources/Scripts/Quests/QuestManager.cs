using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    private Dictionary<string, Quest> quests = new Dictionary<string, Quest>();

    public const string USB_STICK_QUEST_ID = "USBStickQuest";
    public const string KILLER_QUEST_ID = "KillerQuest";
    public const string IDENTITY_STONES_QUEST_ID = "IdentityStonesQuest";

    private void Start()
    {
        Quest usbStickQuest = new Quest(
            USB_STICK_QUEST_ID, "Fehlende USB-Sticks",
            "Du bist nicht der erste Studi, der sich in diesen Katakomben verirrt hat."
            + " Unter den vielen Raufbolden, die nach einem berüchtigten Abend im Partykeller"
            + " ihren Mut beweisen wollten, befanden sich auch wissbegierige Abenteurer, die"
            + " nach einer Lösung für diese schreckliche Katastrophe gesucht haben. Such nach"
            + " USB-Sticks, die die Abenteurer hinterlassen haben. Vielleicht findest du etwas"
            + " nützliches im Kampf gegen das unaufhaltbare Böse!",
            "3 USB Sticks im Dungeon aufheben.",
            "Neuer Turm freigeschaltet, den man jetzt platzieren kann",
            0, 3,
            CompleteUSBStickQuest);

        Quest killerQuest = new Quest(
            KILLER_QUEST_ID, "Killer-... ehh Betäubungsmaschine!",
            "Ich hab gehört, böse Geister sollen Angst vor Betäubungsspritzen haben? Betäube"
            + " 100 Geister und erhalte dafür die 100fache Entlohnung!",
            "100 Geister mit eigener Waffe betäuben bzw. töten",
            "100/1000 Gold (Geld)",
            0, 100, CompleteKillerQuest);

        Quest identityStonesQuest = new Quest(
            IDENTITY_STONES_QUEST_ID, "Identity Stones",
            "Minden war mal eine wunderschöne Stadt. Eine Stadt, die nicht nur eine schöne"
            + " Geschichte erzählt, sondern auch tolle Menschen beherbergt. Nach und nach hat "
            + "es aber die Menschen in die großen Städte verschlagen. Obwohl dieser Campus den "
            + "Menschen solch fabelhaftes Wissen und solch schöne Momente geboten hat, war Minden"
            + " irgendwann seelenverlassen und entwickelte sich zur Geisterstadt. Vielleicht erscheinen"
            + " deshalb die Geister... ? Setze ein Zeichen und hilf Minden in dieser Identitätskrise!",
            "Die Steine mit den Buchstaben 'C', 'A', 'M', 'P', 'U', 'S'  an den Platzhaltern über"
            + " dem Schriftzug \"Minden\" in der richtigen Reihenfolge platzieren. Die Steine findet man im Dungeon.",
            "Zugriff auf die fernsteuerbare Waffe \"Old Canon\", die an der Decke platziert ist.",
            // jedes mal, wenn ein stein an den richtigen platz gelegt wird, wird 1 progress hinzugefügt
            0, 6, CompleteIdentityStonesQuest);


        quests.Add(usbStickQuest.ID, usbStickQuest);
        quests.Add(killerQuest.ID, killerQuest);
        quests.Add(identityStonesQuest.ID, identityStonesQuest);
        
    }

    /// <summary>
    /// Updates the progress of a specific quest.
    /// </summary>
    /// <param name="questID">The ID of the quest to be updated.</param>
    /// <param name="amount">The amount to increase the progress by.</param>
    public void UpdateQuestProgress(string questID, int amount)
    {
        if (quests.ContainsKey(questID))
        {
            quests[questID].Progress += amount;
            if (quests[questID].Progress >= quests[questID].Target)
            {
                quests[questID].OnComplete?.Invoke();
            }
        }
    }

    /// <summary>
    /// Retrieves information about a specific quest.
    /// </summary>
    /// <param name="questID">The ID of the quest for which to get information.</param>
    /// <returns>A tuple containing the quest's description and progress.</returns>
    public (string Description, int Progress) GetQuestInfo(string questID)
    {
        if (quests.ContainsKey(questID))
        {
            return (quests[questID].Description, quests[questID].Progress);
        }
        return ("Quest not found", 0);
    }

    /// <summary>
    /// Specific completion logic for the USB Stick Quest.
    /// </summary>
    private void CompleteUSBStickQuest()
    {
        Debug.Log("USB Stick Quest completed! Reward unlocked.");
        //TODO Additional reward logic here
    }


    /// <summary>
    /// Specific completion logic for the Killer Quest.
    /// </summary>
    private void CompleteKillerQuest()
    {
        CurrencyManager.AddCurrency(1000);
        Debug.Log("Killer Quest completed! Reward unlocked.");
    }

    /// <summary>
    /// Specific completion logic for the Identity Stones Quest.
    /// </summary>
    private void CompleteIdentityStonesQuest()
    {
        Debug.Log("Identity Stones Quest completed! Reward unlocked.");
        //TODO Additional reward logic here
    }

    public string GetCurrentQuest()
    {
        foreach (KeyValuePair<string, Quest> entry in quests)
        {
            if (!entry.Value.IsDone)
            {
                return entry.Value.ID;
            }
        }
        return null;
    }
    internal class Quest
    {
        public string ID { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string TaskDescription { get; private set; }
        public string RewardDescription { get; private set; }
        public int Progress { get; set; }
        public int Target { get; private set; }
        public delegate void CompletionAction();
        public CompletionAction OnComplete;

        public bool IsDone
        {
            get
            {
                return Progress >= Target;
            }
        }
        public Quest(string id, string title, string description, string taskDescription, string rewardDescription, int progress, int target, CompletionAction onComplete)
        {
            ID = id;
            Title = title;
            Description = description;
            TaskDescription = taskDescription;
            RewardDescription = rewardDescription;
            Progress = progress;
            Target = target;
            OnComplete = onComplete;
        }
    }
}
