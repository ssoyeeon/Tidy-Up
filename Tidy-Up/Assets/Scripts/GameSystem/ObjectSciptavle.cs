using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TidyUp
{
    [CreateAssetMenu(fileName = "Object", menuName = "ScriptableObjects/ScriptableObjects", order = 1)]
    public class ObjectSciptavle : ScriptableObject
    {
        public string objectName;
        public GameObject objectPrefab;
    }
}
