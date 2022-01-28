using com.sluggagames.keepUsAlive.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    [SerializeField]
    int _id;
    NavMeshAgent mover;
    public int Id
    {
        get
        {
            return _id;
        }
    }

    [SerializeField]
    Sprite characterIcon;
    public Sprite CharacterIcon
    {
        get
        {
            return characterIcon;
        }
    }

    protected virtual void Awake()
    {
        mover = GetComponent<NavMeshAgent>();
        int randomId = Random.Range(1, 100) * 1000;
        _id = randomId;

    }

    private void OnMouseDown()
    {
        AddToSelectedObjects(this);
        // add an effect
    }

    public void AddToSelectedObjects(Character _character)
    {
        if (_character.Id < 0) return;
        GameManager.Instance.AddCharacterToSelected(_character);
    }

    public void MoveCharacter(Vector3 _destination)
    {
        print("Moving character");
        mover.destination = _destination;
    }
}
