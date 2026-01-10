using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    public GameObject[] opponentCharacters;

    void Start()
    {
        if(opponentCharacters.Length == 0)
        {
            Debug.LogWarning("No opponent characters assigned in OpponentManager.");
            return;
        }
            ActivateRandomOpponent();
    }

    void ActivateRandomOpponent()
    {
        int randomIndex = Random.Range(0, opponentCharacters.Length);
        for (int i = 0; i < opponentCharacters.Length; i++)
        {
            if(i==randomIndex)
            {
                opponentCharacters[i].SetActive(true);
            }
            else
            {
                opponentCharacters[i].SetActive(false);
            }
        }
    }
}
