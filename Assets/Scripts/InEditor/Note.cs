using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class Note : MonoBehaviour
    {

        [SerializeField] [TextArea(10, 20)] private string _notes;

    }
}
