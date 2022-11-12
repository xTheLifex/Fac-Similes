using UnityEngine;

namespace FacSimiles.GUI
{
    public class UIRotating : MonoBehaviour
    {
        public Vector3 rotation; // Measured in degrees per second
        RectTransform rt;
        public bool rotating = true;
        private void Start() 
        {
            rt = GetComponent<RectTransform>();
        }

        private void Update() 
        {
            if (rt && rotating)
            {
                rt.rotation = Quaternion.Euler(rt.rotation.eulerAngles + rotation * Time.deltaTime);
            }
        }

        public void SetRotating(bool val)
        {
            rotating = val;
        }
    }
}