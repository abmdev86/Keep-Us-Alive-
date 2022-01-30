using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive
{
    public class BoxSelection : MonoBehaviour
    {
        /// <summary>
        /// Draggable inspector ref to the Image Gameobject's RectTransform
        /// </summary>
        public RectTransform selectionBox;

        /// <summary>
        /// store the location of wherever we first clicked before dragging.
        /// </summary>
        Vector2 initialClickPosition = Vector2.zero;

        /// <summary>
        /// Rectangle that the box has dragged, in screen space
        /// </summary>
        public Rect SelectionRect
        {
            get;
            private set;
        }

        /// <summary>
        /// if true the user is actively dragging a box
        /// </summary>
        public bool IsSelecting { get; private set; }

        /// <summary>
        /// Configure the visible box
        /// </summary>
        private void Start()
        {
            // setting the anchors to 0-0 = box size wont change as it parent size changes
            selectionBox.anchorMin = Vector2.zero;
            selectionBox.anchorMax = Vector2.zero;

            // pivot at 0 to pivot at left-lower corner
            selectionBox.pivot = Vector2.zero;

            // hide the box at start
            selectionBox.gameObject.SetActive(false);
        }

        private void Update()
        {
            // start dragging and record the mouse and start showing the box
            if (Input.GetMouseButtonDown(0))
            {
                initialClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                // show box
                selectionBox.gameObject.SetActive(true);
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                // figure out left and right corners
                float xMin = Mathf.Min(currentMousePosition.x, initialClickPosition.x);
                float xMax = Mathf.Max(currentMousePosition.x, initialClickPosition.x);
                float yMin = Mathf.Min(currentMousePosition.y, initialClickPosition.y);
                float yMax = Mathf.Max(currentMousePosition.y, initialClickPosition.y);

                // build rectangle
                Rect screenSpaceRect = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
                selectionBox.anchoredPosition = screenSpaceRect.position;
                selectionBox.sizeDelta = screenSpaceRect.size;

                //update our selection
                SelectionRect = screenSpaceRect;

            }

            if (Input.GetMouseButton(0))
            {
                SelectionComplete();

                selectionBox.gameObject.SetActive(false);

                IsSelecting = false;
            }
        }

        private void SelectionComplete()
        {
            Camera mainCam = GetComponent<Camera>();

            Vector3 min = mainCam.ScreenToViewportPoint(new Vector3(SelectionRect.xMin, SelectionRect.yMin));
            Vector3 max = mainCam.ScreenToViewportPoint(new Vector3(SelectionRect.xMax, SelectionRect.yMax));

            // create bounding box in viewport space. We have the x and y coords of the bottom-left
            // and top-right corners. now include z coords.
            min.z = mainCam.nearClipPlane;
            max.z = mainCam.farClipPlane;

            // Construct bounding box
            var viewPortBounds = new Bounds();
            viewPortBounds.SetMinMax(min, max);

            // check for selectable Interface
            foreach(BoxSelectable selectable in FindObjectsOfType<BoxSelectable>())
            {
                Vector3 selectedPosition = selectable.transform.position;
                Vector3 viewportPoint = mainCam.WorldToViewportPoint(selectedPosition);
                // is that point within our viewport? if so it is selected.
                bool selected = viewPortBounds.Contains(viewportPoint);
                if (selected)
                {
                    selectable.Selected();
                }
            }
        }
    }
}
