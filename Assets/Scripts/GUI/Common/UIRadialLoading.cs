using UnityEngine;
using UnityEngine.UI;

namespace FacSimiles.GUI
{
    public class UIRadialLoading : MonoBehaviour
    {
        public GameObject[] radials;
        private int activeBar = 0;
        private float nextTick = 0f;
        public float tickTime = 1f;
        public Texture2D unselectedImage;
        public Texture2D selectedImage;
        private void Update() 
        {
            if (radials.Length > 0)
            {
                if (nextTick < Time.time)
                {
                    nextTick = Time.time + tickTime;
                    SetActiveBar(activeBar + 1);
                }
            }
        }

        public void SetActiveBar(int index)
        {
            if (!(radials.Length > 0))
                return;

            if (index >= radials.Length)
            {
                // Set to 0
                activeBar = 0;
            } else
            {
                // Set to index
                activeBar = index;
            }

            for(int i=0; i < radials.Length; i++)
            {
                RawImage img = radials[i].GetComponent<RawImage>();
                if (!img)
                    continue;
                
                if (i == activeBar)
                {
                    img.texture = selectedImage;
                } else
                {
                    img.texture = unselectedImage;
                }
            }

        }


    }
}