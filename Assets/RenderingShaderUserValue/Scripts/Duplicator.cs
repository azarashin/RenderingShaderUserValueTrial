using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Duplicator : MonoBehaviour
{
    [SerializeField]
    int _rows = 10;

    [SerializeField]
    int _columns = 10;

    [SerializeField]
    float _distance = 2.0f; 

    [SerializeField]
    GameObject _baseObject;

    GameObject[] _list;

    public GameObject[] Generate()
    {
        List<GameObject> ret = new List<GameObject>(); 
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                GameObject obj = Instantiate(_baseObject, new Vector3(transform.position.x + i * _distance, transform.position.y, transform.position.z + j * _distance), Quaternion.identity, transform);
                ret.Add(obj);
            }
        }
        _list = ret.ToArray(); 
        return _list; 
    }

    public void Clear()
    {
        if(_list == null)
        {
            return; 
        }
        foreach(GameObject obj in _list)
        {
            Destroy(obj); 
        }
    }



}
