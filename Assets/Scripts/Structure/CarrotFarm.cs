using System.Collections.Generic;
using UnityEngine;

namespace Structure
{
    public class CarrotFarm : MonoBehaviour
    {
        public void PickCarrot()
        {
            List<GameObject> activeChildren = new();

            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf && child.gameObject.name == "Carrot")
                    activeChildren.Add(child.gameObject);
            }

            if (activeChildren.Count == 0)
                return;

            int randomIndex = Random.Range(0, activeChildren.Count);
            activeChildren[randomIndex].SetActive(false);
        }
    }
}