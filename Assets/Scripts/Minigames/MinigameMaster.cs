using UnityEngine;

public abstract class MinigameMaster : MonoBehaviour
{
    protected void InitInputMaster()
    {
        InputMaster.minigameMaster = this;
    }

    public abstract void OnPlayerKeyDown(int playerId, KeyCode key);
    public abstract void OnPlayerKeyHold(int playerId, KeyCode key);
    public abstract void OnPlayerMouseClick(int playerId, bool rightClick);
    public abstract void OnPlayerMouseMove(int playerId, Vector2 pos);
}