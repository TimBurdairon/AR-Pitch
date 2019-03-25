using System;
using System.Collections.Generic;
using System.Linq;
using Collections.Hybrid.Generic;
using UnityEngine.XR.iOS;

namespace UnityEngine.XR.iOS
{
    public class UnityARAnchorManager
    {
        public LinkedListDictionary<string, ARPlaneAnchorGameObject> planeAnchorMap;
        public List<ARPlaneAnchorGameObject> planeAnchorMapList;

        public string currentIdentifier;

        public UnityARAnchorManager()
        {
            planeAnchorMap = new LinkedListDictionary<string, ARPlaneAnchorGameObject>();
            planeAnchorMapList = new List<ARPlaneAnchorGameObject>();
            UnityARSessionNativeInterface.ARAnchorAddedEvent += AddAnchor;
            UnityARSessionNativeInterface.ARAnchorUpdatedEvent += UpdateAnchor;
            UnityARSessionNativeInterface.ARAnchorRemovedEvent += RemoveAnchor;
        }

        public void AddAnchor(ARPlaneAnchor arPlaneAnchor)
        {
            //planeAnchorMap.Values.
            GameObject go = UnityARUtility.CreatePlaneInScene(arPlaneAnchor);
            go.AddComponent<DontDestroyOnLoad>();  //this is so these GOs persist across scene loads
            ARPlaneAnchorGameObject arpag = new ARPlaneAnchorGameObject();
            arpag.planeAnchor = arPlaneAnchor;
            go.layer = 10;
            arpag.gameObject = go;
            if (ARObject.isMoving == false)
                go.SetActive(false);
            planeAnchorMap.Add(arPlaneAnchor.identifier, arpag);
            planeAnchorMapList.Add(arpag);
        }

        public void RemoveAnchor(ARPlaneAnchor arPlaneAnchor)
        {
            if (planeAnchorMap.ContainsKey(arPlaneAnchor.identifier))
            {
                ARPlaneAnchorGameObject arpag = planeAnchorMap[arPlaneAnchor.identifier];
                planeAnchorMapList.Remove(arpag);
                GameObject.Destroy(arpag.gameObject);
                planeAnchorMap.Remove(arPlaneAnchor.identifier);
            }
        }

        public void UpdateAnchor(ARPlaneAnchor arPlaneAnchor)
        {
            if (planeAnchorMap.ContainsKey(arPlaneAnchor.identifier))
            {
                ARPlaneAnchorGameObject arpag = planeAnchorMap[arPlaneAnchor.identifier];
                UnityARUtility.UpdatePlaneWithAnchorTransform(arpag.gameObject, arPlaneAnchor);
                arpag.planeAnchor = arPlaneAnchor;
                planeAnchorMap[arPlaneAnchor.identifier] = arpag;
                currentIdentifier = arPlaneAnchor.identifier;
                int i = planeAnchorMapList.FindIndex(myItem => myItem.gameObject.name == currentIdentifier);
                planeAnchorMapList[i] = arpag;
            }
            //planeAnchorMapList[i] = arpag;
        }



        public void UnsubscribeEvents()
        {
            UnityARSessionNativeInterface.ARAnchorAddedEvent -= AddAnchor;
            UnityARSessionNativeInterface.ARAnchorUpdatedEvent -= UpdateAnchor;
            UnityARSessionNativeInterface.ARAnchorRemovedEvent -= RemoveAnchor;
        }

        public void Destroy()
        {
            foreach (ARPlaneAnchorGameObject arpag in GetCurrentPlaneAnchors())
            {
                GameObject.Destroy(arpag.gameObject);
            }

            planeAnchorMap.Clear();
            UnsubscribeEvents();
        }

        public LinkedList<ARPlaneAnchorGameObject> GetCurrentPlaneAnchors()
        {
            return planeAnchorMap.Values;
        }
    }
}

