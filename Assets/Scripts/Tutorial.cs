using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    #region Singleton

    private static Tutorial _singleton;
    public static Tutorial Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.LogWarning($"{nameof(value)} already exists in the current scene. Deleting clone");
                Destroy(value.gameObject);
            }
        }
    }
    private void OnEnable()
    {
        Singleton = this;
    }
    #endregion
    //if the player has played any of these cards
    public bool hasPlayedJumpCard, hasPlayedProjectileCard = false;
    [SerializeField] private float firstStopPosition;
    [SerializeField] private float SecondStopPosition;
    [SerializeField] private float slowDistance;
    [SerializeField] private float endDistance;
    [SerializeField] private Card[] tutorialCards;
    [SerializeField] private GameObject secondBarrier;
    private void FixedUpdate()
    {
        UpdateCardsBasedOnDistance();
        if (!hasPlayedJumpCard)
        {
            SlowTime(firstStopPosition);
            CardHandler.Singleton.cards[0] = tutorialCards[0];
        }
        else if (!hasPlayedProjectileCard)
        {
            SlowTime(SecondStopPosition);
            CardHandler.Singleton.cards[0] = tutorialCards[1];
        }
        else if (Player.Singleton.transform.position.z > endDistance)
        {
            SlowTime(1000f);
            MenuManager.ReturnToMainMenu();
        }
    }
    private void SlowTime(float position)
    {
        if (Player.Singleton.transform.position.z < position - slowDistance)//if the player is not in the distance to slow down
        {
            GameManager.Singleton.GameSpeed = 1;
            return;//don't bother slowing down
        }
        //distance (in z axis) to the stop position
        float distanceToStopPosition = position - Player.Singleton.transform.position.z;
        //ratio that the player is through in the 'slow time' zone
        // 1 being at the start of it
        // 0 being at the end of it, where the game stop time to save the player
        float ratioToStopInSlowDistance = distanceToStopPosition / slowDistance;
        GameManager.Singleton.GameSpeed = ratioToStopInSlowDistance;
    }
    private void UpdateCardsBasedOnDistance()
    {
        if (hasPlayedJumpCard)
        {
            if (Player.Singleton.transform.position.z < firstStopPosition)//if the player is not in the distance to slow down
            {
                if (PlayerMovement.Singleton.Velocity.y < 1)
                {
                    hasPlayedJumpCard = false;
                }
            }
        }
        hasPlayedProjectileCard = secondBarrier == null;
    }
}
