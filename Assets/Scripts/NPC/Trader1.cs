using UnityEngine;

public class Trader1 : NPC, ITalkable
{
    [SerializeField]
    private DialogueText dialogueText;

    [SerializeField]
    private DialogueController dialogueController;

    public override void Interact()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        //Start Convertation
        dialogueController.DisplayNextParagraph(dialogueText);
    }
}
