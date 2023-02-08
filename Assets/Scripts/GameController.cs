using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIHelper _uiHelper;
    [SerializeField] private Model _model;

    private IChunkBehaviour _chunkBehaviour;
    private Coroutine _checkChunkCoroutine = null;

    private void Start()
    {
        _chunkBehaviour = new ChunkBehaviour(_model.ChunkParent, Camera.main);
        _chunkBehaviour.SetDefault();
        _model.RocketEngine.SetDefault();

        _model.RocketCollision.OnThereIsContact += Stop;
        _uiHelper.OnPlayButtonPressed += Play;
        _uiHelper.OnChangeEngineBehaviour += ChangeRocket;

        _uiHelper.SetActive("Let's start");
    }

    private void ChangeRocket(BehaviourType type)
    {
        _model.RocketCollision.OnThereIsContact -= Stop;
        
        _model.Type = type;
        _model.RocketEngine.SetDefault();
        _model.RocketCollision.OnThereIsContact += Stop;
    }

    private void Play()
    {
        _model.Input.Lock = false;
        _checkChunkCoroutine = StartCoroutine(CheckChunk());
    }
        
    private void Stop()
    {
        _model.Input.Lock = true;
        StopCoroutine(_checkChunkCoroutine);
        _uiHelper.SetActive("Try again");
        _chunkBehaviour.SetDefault();
        _model.RocketEngine.SetDefault();
    }

    private void OnDestroy()
    {
        _uiHelper.OnPlayButtonPressed -= Play;
        _model.RocketCollision.OnThereIsContact -= Stop;
        _uiHelper.OnChangeEngineBehaviour -= ChangeRocket;
    }

    private IEnumerator CheckChunk()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            _chunkBehaviour.Check();
        }
    }
}